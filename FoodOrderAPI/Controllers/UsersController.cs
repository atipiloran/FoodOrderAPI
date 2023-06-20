using FoodOrderAPI.Models.Dto;
using FoodOrderAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using System.IdentityModel.Tokens.Jwt;

namespace FoodOrderAPI.Controllers
{
    [EnableCors("Cors")]
    [Route("api/auth")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var LoginResponse = await _userRepository.Login(loginRequestDTO);
            if (LoginResponse.User == null || string.IsNullOrEmpty(LoginResponse.Token))
            {
                // If the login response is invalid, return a BadRequest response
                return BadRequest();
            }
            return LoginResponse;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            //bool userNameIsUnique = _userRepository.IsUniqueUser(registrationRequestDTO.UserName);
            //if(!userNameIsUnique)
            //{
            //    return BadRequest();
            //}

            var user = await _userRepository.Register(registrationRequestDTO);
            if (user == null)
            {
                // If the user registration is unsuccessful, return a BadRequest response
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/auth/check
        [HttpPost("check")]
        [AllowAnonymous] // Allow unauthenticated access to this action
        public async Task<ActionResult<AnyType>> Check()
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var bearerToken = HttpContext.Request.Headers.Authorization.ToString();
                bearerToken = bearerToken.Substring(bearerToken.LastIndexOf(" ") + 1);
                var jwt = new JwtSecurityToken(bearerToken);
                var claimlist = jwt.Claims.ToList();
                var nameid = jwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
                var role = jwt.Claims.FirstOrDefault(c => c.Type == "role").Value;

                var resp = new
                {
                    role = role,
                    claimlist = claimlist,
                };

                return Ok(resp);
            }
            catch (Exception ex)
            {
                // If an exception occurs, return an Unauthorized response
                return Unauthorized();
            }
        }
    }
}
