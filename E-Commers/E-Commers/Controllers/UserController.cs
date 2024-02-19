using E_Commers.Application.Abstraction;
using E_Commers.Application.Extensions;
using E_Commers.Application.Interfaces;
using E_Commers.Domain.Entity;
using E_Commers.Domain.Entity.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController:ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("/add")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody]User user)
        {
            var result = await _userRepository.CreateAsync(user);
            if(result != null)
            {
                return Ok(new Response<Permission>()
                {
                    Result = result,
                });
            }
            return BadRequest();
        }
        [HttpGet("/get")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(Guid Id)
        {
            var result = await _userRepository.GetAsync(x=>x.Id == Id);
            if (result != null)
            {
                return Ok(new Response<Permission>()
                {
                    Result = result,
                });
            }
            return NotFound(new Response<Permission>()
            {
                StatusCode = 404,
                Message = "User Not Found",
                IsSuccess = true,
                Result = null
            });
        }

        [HttpGet("/getall")]
        [Authorize(/*Roles = "delete"*/)]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userRepository.GetAllAsync();
            if (result != null)
            {
                return Ok(new Response<Permission>()
                {
                    Result = result,
                });
            }
            return NotFound(new Response<Permission>()
            {
                StatusCode = 404,
                Message = "User Not Found",
                IsSuccess = true,
                Result = null
            });
        }
        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]UserCredential userCredential)
        {
            string password = userCredential.Password.ComputeHash();

            var result = await _userRepository.GetAsync(x=>x.Username == userCredential.UserName && x.Password == password);
            if (result != null)
            {
                Tokens tokens = await _tokenService.CreateTokensAsync(result);

                return Ok(tokens);
            }
            return NotFound(new Response<Permission>()
            {
                StatusCode = 404,
                Message = "User Not Found",
                IsSuccess = true,
                Result = null
            });
        }
        [HttpPost("/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCredentialRegister userregister)
        {
            string passowrd = userregister.Password.ComputeHash();
            var result = await _userRepository.GetAsync(x=>x.Username == userregister.UserName || x.Email == userregister.email);
            if (result != null)
            {
                return BadRequest(new Response<User>(){
                    StatusCode = 409,
                    Message = "User already exixsts",
                    IsSuccess = true,
                    Result = null
                });
            }
            userregister.Password = passowrd;
            User user = new()
            {
                Password = userregister.Password,
                Name = userregister.FullName,
                Username = userregister.UserName,
                Email = userregister.email,
                Phone = userregister.Phone
            };

            var res = await _userRepository.CreateAsync(user);
            if (res == null)
            {
                return BadRequest();
            }
            return Ok(res);

        }

        public class UserCredential
        {
            public string UserName { get; set; }
            public string Password { get; set; }

        }
        public class UserCredentialRegister
        {
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string email { get; set; }
            public string Phone { get; set; }

        }

    }
    
}
