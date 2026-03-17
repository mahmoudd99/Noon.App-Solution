using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noon.App.Dtos;
using Noon.App.Errors;
using Noon.App.Extentions;
using Noon.Core.Entities.IdentityModule;
using Noon.Core.Services;
using System.Security.Claims;

namespace Noon.App.Controllers
{
   
    public class AccountsController :BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _token;
        private readonly IMapper _mapper;

        public AccountsController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager ,
            ITokenServices token,
            IMapper mapper
            )
           
          
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
            _mapper = mapper;
        }

        [HttpPost("login")] //POST : /api/Accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await _token.CreateTokenAsync(user , _userManager),
            });

        }

        [HttpPost("register")]  //POST : /api/Accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            if (ChechEmailExist(model.Email).Result.Value)
                return BadRequest();
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0],

            };

            var result= await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _token.CreateTokenAsync(user, _userManager)
            });

        }

        [Authorize]
        [HttpGet]  // GET : /api/Accounts
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var email= User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _token.CreateTokenAsync(user, _userManager)
            });

        }

        [Authorize]
        [HttpGet("address")] // GET : /api/Accounts/address
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var user = await _userManager.FindUserWithAddress(User);
            return Ok(_mapper.Map<Address,AddressDto>(user.Address));


        }

        [Authorize]
        [HttpPut("address")] //PUT : /api/accounts/address
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {

            var address = _mapper.Map<AddressDto, Address>(addressDto);

            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindUserWithAddressAsync(User);

            address.Id = user.Address.Id;

             user.Address = address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(addressDto);

            
        }

        [HttpGet("emailexists")]  //GET : /api/Accounts/emailexists
        public async Task<ActionResult<bool>> ChechEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }


    }
}
