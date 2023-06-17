using System;
using FoodOrderAPI.Models;

namespace FoodOrderAPI.Models.Dto
{
    public class LoginResponseDTO
    {
        // Represents the user information
        public User User { get; set; }

        // Represents the authentication token
        public string Token { get; set; }
    }
}

