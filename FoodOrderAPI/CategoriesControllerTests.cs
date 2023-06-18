using FoodOrderAPI.Controllers;
using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FoodOrderAPI.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly CategoriesController _controller;
        private readonly Mock<ApplicationDbContext> _mockContext;

        public CategoriesControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new CategoriesController(_mockContext.Object);
        }

        [Fact]
        public async Task GetCategories_ReturnsOkResult()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1", Description = "Description 1" },
                new Category { Id = 2, Name = "Category 2", Description = "Description 2" }
            };

            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(categories.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(categories.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(categories.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(categories.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Categories).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.GetCategories();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetCategory_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1", Description = "Description 1" };

            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(category);

            _mockContext.Setup(c => c.Categories).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.GetCategory(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetCategory_WithInvalidId_ReturnsBadRequestResult()
        {
            // Act
            var result = await _controller.GetCategory(0);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetCategory_WithNonExistentId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Category)null);

            _mockContext.Setup(c => c.Categories).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.GetCategory(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_WithValidCategory_ReturnsOkResult()
        {
            // Arrange
            var category = new Category { Name = "New Category", Description = "New Description" };

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_WithInvalidCategory_ReturnsBadRequestResult()
        {
            // Arrange
            var category = new Category { Name = null, Description = "New Description" };

            // Add model state error
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_WithNullCategory_ReturnsBadRequestResult()
        {
            // Act
            var result = await _controller.CreateCategory(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_WithExistingCategoryId_ReturnsStatusCode500Result()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1", Description = "Description 1" };

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as StatusCodeResult).StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1", Description = "Description 1" };

            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(category);

            _mockContext.Setup(c => c.Categories).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_WithInvalidId_ReturnsBadRequestResult()
        {
            // Act
            var result = await _controller.DeleteCategory(0);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_WithNonExistentId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Category)null);

            _mockContext.Setup(c => c.Categories).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_WithValidIdAndCategory_ReturnsNoContentResult()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "Category 1", Description = "Description 1" };
            var updatedCategory = new Category { Id = 1, Name = "Updated Category", Description = "Updated Description" };

            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(existingCategory);

            _mockContext.Setup(c => c.Categories).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.UpdateCategory(1, updatedCategory);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_WithInvalidIdAndCategory_ReturnsBadRequestResult()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Updated Category", Description = "Updated Description" };

            // Act
            var result = await _controller.UpdateCategory(2, category);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_WithNullCategory_ReturnsBadRequestResult()
        {
            // Act
            var result = await _controller.UpdateCategory(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_WithNonExistentId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Category)null);

            _mockContext.Setup(c => c.Categories).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.UpdateCategory(1, new Category());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
