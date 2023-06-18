using Xunit;
using FoodOrderAPI.Models;

namespace FoodOrderAPI.Tests.Models
{
    public class AddressTests
    {
        [Fact]
        public void Address_Initialization_Success()
        {
            // Arrange
            var address = new Address
            {
                Name = "Home",
                Details = "123 Main St",
                UserId = 1
            };

            // Act & Assert
            Assert.Equal("Home", address.Name);
            Assert.Equal("123 Main St", address.Details);
            Assert.Equal(1, address.UserId);
        }

        [Fact]
        public void Address_Validation_Success()
        {
            // Arrange
            var address = new Address
            {
                Name = "Office",
                Details = "456 Elm St",
                UserId = 2
            };

            // Act
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(address, null, null);
            var validationResults = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(address, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Address_Validation_Failure_InvalidName()
        {
            // Arrange
            var address = new Address
            {
                Name = "",
                Details = "789 Park Ave",
                UserId = 3
            };

            // Act
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(address, null, null);
            var validationResults = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(address, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.MemberNames.Contains("Name"));
        }

        [Fact]
        public void Address_Validation_Failure_InvalidDetails()
        {
            // Arrange
            var address = new Address
            {
                Name = "Home",
                Details = "",
                UserId = 4
            };

            // Act
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(address, null, null);
            var validationResults = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(address, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.MemberNames.Contains("Details"));
        }

        [Fact]
        public void Address_Validation_Failure_InvalidUserId()
        {
            // Arrange
            var address = new Address
            {
                Name = "Office",
                Details = "456 Elm St",
                UserId = 0
            };

            // Act
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(address, null, null);
            var validationResults = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(address, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.MemberNames.Contains("UserId"));
        }
    }
}
