using FoodOrderAPI.Models;
using FoodOrderAPI.Models.Dto;
using FoodOrderAPI.Models;

namespace FoodOrderAPI.Repository.Interface
{
    public interface IUserRepository
    {
        // Checks if the given username is unique in the system
        bool IsUniqueUser(string username);

        // Attempts to log in a user based on the provided login request data
        // Returns a LoginResponseDTO containing the result of the login attempt
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        // Registers a new user based on the provided registration request data
        // Returns the registered User object upon successful registration
        Task<User> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
