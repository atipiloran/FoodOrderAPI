using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        // Constructor
        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/categories
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            // Retrieve all categories from the database
            return Ok(await _db.Categories.ToListAsync());
        }

        // GET: api/categories/{id}
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (id < 1)
            {
                // If the id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Retrieve the category with the specified id from the database
            var category = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);

            if (category == null)
            {
                // If the category is not found, return a NotFound response
                return NotFound();
            }

            // Return the retrieved category
            return Ok(category);
        }

        // POST: api/categories
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                // If the category is not valid, return a BadRequest response
                return BadRequest(ModelState);
            }

            if (category == null)
            {
                // If the category is null, return a BadRequest response
                return BadRequest();
            }

            if (category.Id > 0)
            {
                // If the category has an id greater than 0 (indicating it already exists), return a StatusCode 500 response
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Add the new category to the database
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();

            // Return the created category
            return Ok(category);
        }

        // DELETE: api/categories/{id}
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id < 1)
            {
                // If the id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Find the category with the specified id in the database
            var category = _db.Categories.FirstOrDefault(u => u.Id == id);

            if (category == null)
            {
                // If the category is not found, return a NotFound response
                return NotFound();
            }

            // Remove the category from the database
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            // Return a NoContent response to indicate successful deletion
            return NoContent();
        }

        // PUT: api/categories/{id}
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category categoryBody)
        {
            if (categoryBody == null || id != categoryBody.Id)
            {
                // If the category is null or the id does not match, return a BadRequest response
                return BadRequest();
            }

            // Find the category with the specified id in the database
            var category = _db.Categories.FirstOrDefault(u => u.Id == id);

            if (category == null)
            {
                // If the category is not found, return a NotFound response
                return NotFound();
            }

            // Update the category properties with the values from the request body
            category.Name = categoryBody.Name;
            category.Description = categoryBody.Description;

            // Update the category in the database
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();

            // Return a NoContent response to indicate successful update
            return NoContent();
        }
    }
}
