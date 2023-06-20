using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using FoodOrderAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodOrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetOrders()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem> GetOrderById(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem> CreateOrder(OrderItem order)
        {
            _context.OrderItems.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.OrderItems.FindAsync(id);
            if (order == null)
                return false;

            _context.OrderItems.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOrder(OrderItem order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
