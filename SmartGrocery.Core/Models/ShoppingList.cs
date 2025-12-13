using System;
using System.Collections.Generic;
using SQLite;

namespace SmartGrocery.Core.Models
{
    public class ShoppingList
    {
         [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // chave primária local
        public string ListName { get; set; } // exemplo: "Compras Semanais"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastBought { get; set; } // quando foi comprada pela última vez
         [Ignore]
        public List<Item> Items { get; set; } = new List<Item>();
        // Quantidade total de itens
         [Ignore]
        public int TotalItems => Items.Count;

        // Frequência média de compra dos itens da lista
         [Ignore]
        public double AverageFrequency => Items.Count == 0 ? 0 : Items.Average(i => i.Frequency);
    }
}
