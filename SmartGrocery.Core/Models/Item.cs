using SQLite;

namespace SmartGrocery.Core.Models;

public class Item
{
     [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ShoppingListId { get; set; } // foreign key

        public string ItemName { get; set; }

        public bool IsChecked { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime? LastBought { get; set; }

        public double Frequency { get; set; } = 0; // dias entre compras
}