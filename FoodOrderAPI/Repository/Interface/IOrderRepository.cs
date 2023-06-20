using FoodOrderAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodOrderAPI.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderItem>> GetOrders();
        Task<OrderItem> GetOrderById(int id);
        Task<OrderItem> CreateOrder(OrderItem order);
        Task<bool> DeleteOrder(int id);
        Task<bool> UpdateOrder(OrderItem order);
    }
}
