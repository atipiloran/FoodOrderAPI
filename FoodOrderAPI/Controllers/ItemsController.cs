using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using FoodOrderAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace FoodOrderAPI.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        // Constructor
        public ItemsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/items
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AnyType>> GetItems()
        {
            // Retrieve items and their corresponding categories from the database
            var items = (
                from it in _db.Items
                join ct in _db.Categories on it.CategoryId equals ct.Id
                select new
                {
                    id = it.Id,
                    name = it.Name,
                    price = it.Price,
                    description = it.Description,
                    category = ct.Name
                }
            );

            // Return the items
            return Ok(items);
        }

        // GET: api/items/{id}
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            if (id < 1)
            {
                // If the id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Retrieve the item with the specified id from the database
            var item = await _db.Items.FirstOrDefaultAsync(u => u.Id == id);

            if (item == null)
            {
                // If the item is not found, return a NotFound response
                return NotFound();
            }

            // Return the retrieved item
            return Ok(item);
        }

        // POST: api/items
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem([FromBody] AddItemDTO item)
        {
            if (item == null)
            {
                // If the item is null, return a BadRequest response
                return BadRequest();
            }

            // Create a new item with the properties from the DTO
            var newItem = new Item
            {
                CategoryId = item.CategoryId,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
            };

            // Add the new item to the database
            await _db.Items.AddAsync(newItem);
            await _db.SaveChangesAsync();

            // Return the created item
            return Ok(item);
        }

        // DELETE: api/items/{id}
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (id < 1)
            {
                // If the id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Find the item with the specified id in the database
            var item = _db.Items.FirstOrDefault(u => u.Id == id);

            if (item == null)
            {
                // If the item is not found, return a NotFound response
                return NotFound();
            }

            // Remove the item from the database
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();

            // Return a NoContent response to indicate successful deletion
            return NoContent();
        }

        // PUT: api/items/{id}
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item itemBody)
        {
            if (itemBody == null || id != itemBody.Id)
            {
                // If the item is null or the id doesn't match, return a BadRequest response
                return BadRequest();
            }

            // Find the item with the specified id in the database
            var item = _db.Items.FirstOrDefault(u => u.Id == id);

            if (item == null)
            {
                // If the item is not found, return a NotFound response
                return NotFound();
            }

            // Update the item properties with the values from the request body
            item.Name = itemBody.Name;
            item.Description = itemBody.Description;
            item.Price = itemBody.Price;

            // Update the item in the database
            _db.Items.Update(item);
            await _db.SaveChangesAsync();

            // Return a NoContent response to indicate successful update
            return NoContent();
        }
    }
}
