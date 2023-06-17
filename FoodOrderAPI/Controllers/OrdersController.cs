using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using FoodOrderAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodOrderAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        // Constructor
        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/orders
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AnyType>> GetOrders()
        {
            // Extract user information from the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var bearerToken = HttpContext.Request.Headers.Authorization.ToString();
            bearerToken = bearerToken.Substring(bearerToken.LastIndexOf(" ") + 1);
            var jwt = new JwtSecurityToken(bearerToken);
            var claimlist = jwt.Claims.ToList();
            var nameid = jwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var role = jwt.Claims.FirstOrDefault(c => c.Type == "role").Value;

            if (nameid.IsNullOrEmpty())
            {
                // If the user information is missing, return a BadRequest response
                return BadRequest();
            }

            if (role == "admin")
            {
                // Retrieve all order items with their associated items for admin role
                var allOrderItems = (
                    from oi in _db.OrderItems
                    join it in _db.Items on oi.ItemId equals it.Id
                    select new
                    {
                        itemName = it.Name,
                        quantity = oi.Quantity,
                        itemPrice = it.Price,
                        orderHeader = oi.OrderHeaderId,
                    }
                );

                // Retrieve all order header details for admin role
                var allOrderHeaderDetails = (
                    from oh in _db.OrderHeaders
                    join ad in _db.Addresses on oh.AddressId equals ad.Id
                    join us in _db.Users on oh.UserId equals us.Id
                    orderby oh.DateTime descending
                    select new
                    {
                        id = oh.Id,
                        address = ad.Details,
                        user = us.Name,
                        date = oh.DateTime,
                        notes = oh.Notes,
                        status = oh.Status,
                    }
                );

                var resAdmin = new
                {
                    orderHeaderDetails = allOrderHeaderDetails,
                    orderItems = allOrderItems,
                };

                return Ok(resAdmin);
            }

            // Retrieve order items with their associated items for the user
            var orderItems = (
                from oi in _db.OrderItems
                join it in _db.Items on oi.ItemId equals it.Id
                where oi.OrderHeader.UserId == int.Parse(nameid)
                select new
                {
                    itemName = it.Name,
                    quantity = oi.Quantity,
                    itemPrice = it.Price,
                    orderHeader = oi.OrderHeaderId,
                }
            );

            // Retrieve order header details for the user
            var orderHeaderDetails = (
                from oh in _db.OrderHeaders
                join ad in _db.Addresses on oh.AddressId equals ad.Id
                join us in _db.Users on oh.UserId equals us.Id
                where oh.UserId == int.Parse(nameid)
                orderby oh.DateTime descending
                select new
                {
                    id = oh.Id,
                    address = ad.Details,
                    user = us.Name,
                    date = oh.DateTime,
                    notes = oh.Notes,
                    status = oh.Status,
                }
            );

            var res = new
            {
                orderHeaderDetails = orderHeaderDetails,
                orderItems = orderItems,
            };

            return Ok(res);
        }

        // GET: api/orders/{id}
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnyType>> GetOrder(int id)
        {
            if (id < 1)
            {
                // If the provided id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Retrieve the order header with the specified id
            var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id);

            if (orderHeader == null)
            {
                // If the order header is not found, return a NotFound response
                return NotFound();
            }

            // Retrieve the order items with their associated items for the order header
            var orderItems = (
                from oi in _db.OrderItems
                join it in _db.Items on oi.ItemId equals it.Id
                where oi.OrderHeaderId == id
                select new
                {
                    itemName = it.Name,
                    quantity = oi.Quantity,
                    itemPrice = it.Price,
                }
            );

            // Retrieve the order header details for the order header
            var orderHeaderDetails = (
                from oh in _db.OrderHeaders
                join ad in _db.Addresses on oh.AddressId equals ad.Id
                join us in _db.Users on oh.UserId equals us.Id
                where oh.Id == id
                select new
                {
                    address = ad.Details,
                    user = us.Name,
                    date = oh.DateTime,
                    notes = oh.Notes,
                }
            );

            var res = new
            {
                orderHeaderDetails = orderHeaderDetails,
                orderItems = orderItems,
            };

            return Ok(res);
        }

        // POST: api/orders
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AnyType>> CreateOrder([FromBody] NewOrderDTO req)
        {
            // Extract user information from the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var bearerToken = HttpContext.Request.Headers.Authorization.ToString();
            bearerToken = bearerToken.Substring(bearerToken.LastIndexOf(" ") + 1);
            var jwt = new JwtSecurityToken(bearerToken);
            var claimlist = jwt.Claims.ToList();
            var nameid = jwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var role = jwt.Claims.FirstOrDefault(c => c.Type == "role").Value;

            // Create a new order header
            var newHeader = new OrderHeader
            {
                UserId = int.Parse(nameid),
                AddressId = req.AddressId,
                Notes = req.Notes,
                DateTime = DateTime.Now,
                Status = 1
            };

            await _db.OrderHeaders.AddAsync(newHeader);
            _db.SaveChanges();

            // Add each order item to the database
            foreach (OrderItemRequestDTO item in req.OrderItems)
            {
                var newOrderItem = new OrderItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    OrderHeaderId = newHeader.Id,
                };

                await _db.AddAsync(newOrderItem);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/orders/{id}
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (id < 1)
            {
                // If the provided id is invalid, return a BadRequest response
                return BadRequest();
            }

            // Retrieve the order header with the specified id
            var category = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

            if (category == null)
            {
                // If the order header is not found, return a NotFound response
                return NotFound();
            }

            // Remove the order header from the database
            _db.OrderHeaders.Remove(category);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/orders/{id}
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderStatusChangeDTO orderStatusChangeDTO)
        {
            // Retrieve the order header with the specified id
            var orderHeader = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

            if (orderHeader == null)
            {
                // If the order header is not found, return a NotFound response
                return NotFound();
            }

            // Update the order status
            orderHeader.Status = orderStatusChangeDTO.status;

            _db.OrderHeaders.Update(orderHeader);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
