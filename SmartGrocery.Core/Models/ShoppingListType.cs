namespace SmartGrocery.Core.Models;

public enum ShoppingListType
{
    Simple = 0,      // só nome
    Quantity = 1,    // nome + quantidade
    Checklist = 2,   // marcar comprado
    Smart = 3        // futuro: sugestões, recorrência etc
}
