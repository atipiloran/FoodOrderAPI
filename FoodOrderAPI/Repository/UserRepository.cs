using Azure.Identity;
using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using FoodOrderAPI.Models.Dto;
using FoodOrderAPI.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secret;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secret = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.Users.FirstOrDefault(u =>
                u.UserName.ToLower() == loginRequestDTO.userName.ToLower() &&
                u.Password.ToLower() == loginRequestDTO.password.ToLower()
            );

            if (user == null)
            {
                // User not found, return an empty LoginResponseDTO
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writtenToken = tokenHandler.WriteToken(token);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = writtenToken,
                User = user,
            };

            return loginResponseDTO;
        }

        public async Task<User> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            User user = new User()
            {
                UserName = registrationRequestDTO.UserName,
                Password = registrationRequestDTO.Password,
                Name = registrationRequestDTO.Name,
                Email = registrationRequestDTO.Email,
                Role = registrationRequestDTO.Role,
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Clear the password before returning the user
            user.Password = "";

            return user;
        }
    }
}
