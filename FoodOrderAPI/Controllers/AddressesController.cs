using System.Net;
using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using FoodOrderAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        // Constructor
        public AddressesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/addresses
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            // Retrieve all addresses from the database
            return Ok(await _db.Addresses.ToListAsync());
        }

        // GET: api/addresses/{id}
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            if (id < 1)
            {
                // If the id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Retrieve the address with the specified id from the database
            var item = await _db.Addresses.FirstOrDefaultAsync(u => u.Id == id);

            if (item == null)
            {
                // If the address is not found, return a NotFound response
                return NotFound();
            }

            // Return the retrieved address
            return Ok(item);
        }

        // POST: api/addresses
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Address>> CreateAddress([FromBody] Address item)
        {
            if (!ModelState.IsValid)
            {
                // If the item is not valid, return a BadRequest response
                return BadRequest(ModelState);
            }

            if (item == null)
            {
                // If the item is null, return a BadRequest response
                return BadRequest();
            }

            if (item.Id > 0)
            {
                // If the item has an id greater than 0 (indicating it already exists), return a StatusCode 500 response
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Add the new address to the database
            await _db.Addresses.AddAsync(item);
            await _db.SaveChangesAsync();

            // Return the created address
            return Ok(item);
        }

        // DELETE: api/addresses/{id}
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (id < 1)
            {
                // If the id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Find the address with the specified id in the database
            var item = _db.Addresses.FirstOrDefault(u => u.Id == id);

            if (item == null)
            {
                // If the address is not found, return a NotFound response
                return NotFound();
            }

            // Remove the address from the database
            _db.Addresses.Remove(item);
            await _db.SaveChangesAsync();

            // Return a NoContent response to indicate successful deletion
            return NoContent();
        }

        // PUT: api/addresses/{id}
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address itemBody)
        {
            if (itemBody == null || id != itemBody.Id)
            {
                // If the item is null or the id does not match, return a BadRequest response
                return BadRequest();
            }

            // Find the address with the specified id in the database
            var item = _db.Addresses.FirstOrDefault(u => u.Id == id);

            if (item == null)
            {
                // If the address is not found, return a NotFound response
                return NotFound();
            }

            // Update the address properties with the values from the request body
            item.Name = itemBody.Name;
            item.Details = itemBody.Details;

            // Update the address in the database
            _db.Addresses.Update(item);
            await _db.SaveChangesAsync();

            // Return a NoContent response to indicate successful update
            return NoContent();
        }
    }
}
