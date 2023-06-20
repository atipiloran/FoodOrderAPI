using FoodOrderAPI.Controllers;
using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FoodOrderAPI.Tests
{
    public class ItemsControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ItemsController _controller;

        public ItemsControllerTests()
        {
            // Set up an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _controller = new ItemsController(_dbContext);
        }

        public void Dispose()
        {
            // Clean up the in-memory database after each test
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetItems_ReturnsOkResult()
        {
            // Arrange
            // Add some sample items to the in-memory database
            await _dbContext.Items.AddRangeAsync(new List<Item>
            {
                new Item { Id = 1, Name = "Item 1", Price = 9.99, Description = "Description 1", CategoryId = 1 },
                new Item { Id = 2, Name = "Item 2", Price = 14.99, Description = "Description 2", CategoryId = 2 },
                new Item { Id = 3, Name = "Item 3", Price = 19.99, Description = "Description 3", CategoryId = 1 },
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.NotEmpty(items);
        }
    }
}
