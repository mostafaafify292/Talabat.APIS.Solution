
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Talabat.APIS.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;


namespace Talabat.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        { 
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options=>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });
           // builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
           // builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
           // builder.Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();
            builder.Services.AddScoped(typeof(IGenericRepository<>) , typeof(GenericRepository<>));
            builder.Services.AddAutoMapper(typeof(MapperProfile));
           


            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbcontext = services.GetRequiredService<StoreContext>();
            var LoggerFactory= services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbcontext.Database.MigrateAsync(); //Update DataBase
                await StoreContextSeeding.SeedAsync(_dbcontext); //Data Seeding
            }
            catch (Exception ex  )
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during migration");
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
