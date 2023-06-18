using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using FoodOrderAPI.Models.Dto;
using FoodOrderAPI.Repository;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; 
using NUnit.Framework;
using System.Threading.Tasks;
//using Xunit;

namespace FoodOrderAPI.Tests.Repository
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Seed test data
            _dbContext.Users.AddRange(new[]
            {
                new User { Id = 1, UserName = "testuser1", Password = "password1", Name = "Test User 1", Email = "testuser1@example.com", Role = "User" },
                new User { Id = 2, UserName = "testuser2", Password = "password2", Name = "Test User 2", Email = "testuser2@example.com", Role = "User" }
            });

            _dbContext.SaveChanges();

            // Create UserRepository instance with the test DbContext
            var configuration = new ConfigurationBuilder().Build(); // You can customize the configuration if needed
            _userRepository = new UserRepository(_dbContext, configuration);
        }

        [TearDown]
        public void Cleanup()
        {
            // Dispose the DbContext and release the in-memory database
            _dbContext.Dispose();
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsLoginResponseDTO()
        {
            // Arrange
            var loginRequestDTO = new LoginRequestDTO
            {
                UserName = "testuser1",
                Password = "password1"
            };

            // Act
            var result = await _userRepository.Login(loginRequestDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Token);
            Assert.IsNotNull(result.User);
            Assert.AreEqual("testuser1", result.User.UserName);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsEmptyLoginResponseDTO()
        {
            // Arrange
            var loginRequestDTO = new LoginRequestDTO
            {
                UserName = "testuser1",
                Password = "incorrectpassword"
            };

            // Act
            var result = await _userRepository.Login(loginRequestDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Token);
            Assert.IsNull(result.User);
        }

        [Test]
        public async Task Register_ValidRegistrationRequestDTO_ReturnsRegisteredUser()
        {
            // Arrange
            var registrationRequestDTO = new RegistrationRequestDTO
            {
                UserName = "newuser",
                Password = "newpassword",
                Name = "New User",
                Email = "newuser@example.com",
                Role = "User"
            };

            // Act
            var result = await _userRepository.Register(registrationRequestDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("newuser", result.UserName);
        }

        [Test]
        public async Task Register_DuplicateUserName_ThrowsException()
        {
            // Arrange
            var registrationRequestDTO = new RegistrationRequestDTO
            {
                UserName = "testuser1", // Existing username
                Password = "newpassword",
                Name = "New User",
                Email = "newuser@example.com",
                Role = "User"
            };

            // Act and Assert
            Assert.ThrowsAsync<Exception>(async () => await _userRepository.Register(registrationRequestDTO));
        }
    }
}
