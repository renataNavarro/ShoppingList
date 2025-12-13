using System.Collections.Generic;
using System.Threading.Tasks;
using SmartGrocery.Core.Models;

namespace SmartGrocery.Core.Interfaces
{
    public interface IRepository
    {
        Task<List<ShoppingList>> GetAllListsAsync();
        Task<ShoppingList> GetListByIdAsync(int id);
        Task AddListAsync(ShoppingList list);
        Task UpdateListAsync(ShoppingList list);
        Task DeleteListAsync(int id);

        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int id);
        Task<List<Item>> GetItemsByListIdAsync(int listId);
    }
}
