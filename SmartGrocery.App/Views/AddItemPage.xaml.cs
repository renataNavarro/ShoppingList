using Microsoft.Maui.Controls;
using SmartGrocery.Core.Models;

namespace SmartGrocery.App.Views;

public partial class AddItemPage : ContentPage
{
    public Item ResultItem { get; private set; }
    private readonly int _shoppingListId;

    public AddItemPage(int shoppingListId)
    {
        InitializeComponent();
        _shoppingListId = shoppingListId;

        // Atualiza o Label sempre que o Stepper mudar
        QuantityStepper.ValueChanged += (s, e) =>
        {
            QuantityLabel.Text = ((int)e.NewValue).ToString();
        };
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        var name = ItemNameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Error", "Item name cannot be empty.", "OK");
            return;
        }

        var quantity = (int)QuantityStepper.Value;

        ResultItem = new Item
        {
            ItemName = name,
            Quantity = quantity,
            ShoppingListId = _shoppingListId
        };

        await Navigation.PopModalAsync(); // fecha o modal
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        ResultItem = null;
        await Navigation.PopModalAsync();
    }
}
