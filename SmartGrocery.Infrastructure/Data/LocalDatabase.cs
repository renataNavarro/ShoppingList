using SQLite;
using SmartGrocery.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmartGrocery.Infrastructure.Data
{
    public class LocalDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        /* public LocalDatabase(string dbPath = null)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dbPath = Path.Combine(folder, "SmartGrocery.db3");
            }

            _db = new SQLiteAsyncConnection(dbPath);
            InitializeAsync().Wait();
        } */
        public LocalDatabase(string dbPath)
        {
            try
            {
                Console.WriteLine("DB: Entrando no construtor");
                Console.WriteLine($"DB: Caminho = {dbPath}");

                _database = new SQLite.SQLiteAsyncConnection(dbPath);
                Console.WriteLine("DB: Conexão criada");

                _database.CreateTableAsync<ShoppingList>();
                Console.WriteLine("DB: Tabela criada");
                _database.CreateTableAsync<Item>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB: ERRO");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private async Task InitializeAsync()
        {
            await _database.CreateTableAsync<ShoppingList>();
            await _database.CreateTableAsync<Item>();
        }

        // --- ShoppingLists ---

        public Task<List<ShoppingList>> GetListsAsync()
        {
            return _database.Table<ShoppingList>().ToListAsync();
        }

        public Task<ShoppingList> GetListAsync(int id)
        {
            return _database.Table<ShoppingList>().Where(l => l.Id == id).FirstOrDefaultAsync();
        }

        public Task SaveListAsync(ShoppingList list)
        {
            if (list.Id != 0)
                return _database.UpdateAsync(list);
            else
                return _database.InsertAsync(list);
        }

        public async Task DeleteListAsync(int id)
        {
            await _database.ExecuteAsync(
                    "DELETE FROM Item WHERE ShoppingListId = ?", id);
                    /*Test orphan items removed 
                    var list = GetItemsAsync();
                    System.Console.WriteLine("items: "+list.Result.Count); */
            await _database.DeleteAsync<ShoppingList>(id);
        }

        // --- Items ---

        public Task<List<Item>> GetItemsAsync()
        {
            return _database.Table<Item>().ToListAsync();
        }

        public Task SaveItemAsync(Item item)
        {
            if (item.Id != 0)
                return _database.UpdateAsync(item);
            else
                return _database.InsertAsync(item);
        }

        public Task DeleteItemAsync(int id)
        {
            return _database.DeleteAsync<Item>(id);
        }

        // Optional: Get items by list id
        public async Task<List<Item>> GetItemsByListIdAsync(int listId)
        {
            return await _database.Table<Item>().Where(i => i.ShoppingListId == listId).ToListAsync();
        }
    }
}
