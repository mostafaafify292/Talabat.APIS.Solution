
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.APIS.Helpers;
using Talabat.APIS.Middleware;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Data.Identity;


namespace Talabat.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        { 
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSwaggerServices();
            //DataBase For product 
            builder.Services.AddDbContext<StoreContext>(options=>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });
            //DataBase For Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider)=>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            //DataBase For Identity
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(options=>
            {
                //options.Password.RequiredUniqueChars = 2;
                //options.Password.RequiredLength = 2;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.Services.AddApplicationServices();

            builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssure"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty))
                };

            });



            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbcontext = services.GetRequiredService<StoreContext>();
            var _IdentityDbcontext = services.GetRequiredService<AppIdentityDbContext>();
            var _userManager = services.GetRequiredService<UserManager<AppUser>>();
            var LoggerFactory= services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _IdentityDbcontext.Database.MigrateAsync(); //Update DataBase For Identity
                await _dbcontext.Database.MigrateAsync(); //Update DataBase
                await StoreContextSeeding.SeedAsync(_dbcontext); //Data Seeding
                await AppIdentityDbContextSeeding.SeedUserAsync(_userManager); //Data Seeding For UserIdentity
            }
            catch (Exception ex  )
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during migration");
            }

            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
