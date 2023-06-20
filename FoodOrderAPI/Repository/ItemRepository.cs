using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _db;

        public ItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            return await _db.Items.ToListAsync();
        }

        public async Task<Item> GetItem(int id)
        {
            return await _db.Items.FindAsync(id);
        }

        public async Task<Item> CreateItem(Item item)
        {
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateItem(Item item)
        {
            _db.Items.Update(item);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
                return false;

            _db.Items.Remove(item);
            return await _db.SaveChangesAsync() > 0;
        }
    }

}
