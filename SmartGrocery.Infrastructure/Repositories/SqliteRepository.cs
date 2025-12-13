using SmartGrocery.Core.Interfaces;
using SmartGrocery.Core.Models;
using SmartGrocery.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartGrocery.Infrastructure.Services;

public class SqliteRepository : IRepository
{
    private readonly LocalDatabase _db;

    public SqliteRepository(LocalDatabase db)
    {
        _db = db;
    }

    // --- ShoppingLists ---

    public async Task<List<ShoppingList>> GetAllListsAsync()
    {
        var lists = await _db.GetListsAsync();

        // Populate items for each list
        foreach (var list in lists)
        {
            list.Items = (await _db.GetItemsAsync())
                .Where(i => i.ShoppingListId == list.Id)
                .ToList();
        }

        return lists;
    }

    public async Task<ShoppingList> GetListByIdAsync(int id)
    {
        var list = await _db.GetListAsync(id);
        if (list != null)
        {
            list.Items = (await _db.GetItemsAsync())
                .Where(i => i.ShoppingListId == id)
                .ToList();
        }
        return list;
    }

    public async Task AddListAsync(ShoppingList list)
    {
        await _db.SaveListAsync(list);
    }

    public async Task UpdateListAsync(ShoppingList list)
    {
        await _db.SaveListAsync(list);
    }

    public async Task DeleteListAsync(int id)
    {
        // Delete items first
        var items = (await _db.GetItemsAsync()).Where(i => i.ShoppingListId == id).ToList();
        foreach (var item in items)
        {
            await _db.DeleteItemAsync(item.Id);
        }

        // Delete the list
        await _db.DeleteListAsync(id);
    }

    // --- Items ---

    public async Task<List<Item>> GetItemsByListIdAsync(int listId)
    {
        var items = (await _db.GetItemsAsync())
            .Where(i => i.ShoppingListId == listId)
            .ToList();

        return items;
    }

    public async Task AddItemAsync(Item item)
    {
        await _db.SaveItemAsync(item);
    }

    public async Task UpdateItemAsync(Item item)
    {
        await _db.SaveItemAsync(item);
    }

    public async Task DeleteItemAsync(int id)
    {
        await _db.DeleteItemAsync(id);
    }
}