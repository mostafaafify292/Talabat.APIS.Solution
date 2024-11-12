using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager ,
            SignInManager<AppUser> signInManager ,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }



        [HttpPost("login")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user =await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password , false);
            if (result.Succeeded is false) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDTO() 
            {
              DisplayName = user.DisplayName,
              Email = user.Email,
              Token =await _authService.CreateTokenAsync(user , _userManager)
            });
            
        }


        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register (RegisterDTO model)
        {
            
            if (CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] {"this email is already exist!"} });
            }
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,

            };
            var result = await _userManager.CreateAsync(user , model.Password);
            if (result.Succeeded is false) return BadRequest(new ApiResponse(400));
            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await _authService.CreateTokenAsync(user , _userManager)
            });  
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }




        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("address")]
        public async Task<ActionResult<AddressIdentityDTO>> GetUserAddress()
        {           
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);
            var mappedAddress = _mapper.Map<Address , AddressIdentityDTO>(user.Address);

            return Ok(mappedAddress);
        }




        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("address")]
        public async Task<ActionResult<AddressIdentityDTO>> UpdateAddressForUser(AddressIdentityDTO address)
        {
            var updatedAddress = _mapper.Map< AddressIdentityDTO , Address>(address);
            var user =await _userManager.FindUserWithAddressByEmailAsync(User);
            updatedAddress.Id = user.Address.Id;
            user.Address = updatedAddress;
            var result =await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var returnAddressToDto = _mapper.Map<Address, AddressIdentityDTO>(user.Address);
            return Ok(returnAddressToDto);
        }



    }
}
