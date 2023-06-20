using System.Collections.Generic;
using System.Threading.Tasks;
using FoodOrderAPI.Models;

namespace FoodOrderAPI.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItems();
        Task<Item> GetItem(int id);
        Task<Item> CreateItem(Item item);
        Task<bool> UpdateItem(Item item);
        Task<bool> DeleteItem(int id);
    }
}
