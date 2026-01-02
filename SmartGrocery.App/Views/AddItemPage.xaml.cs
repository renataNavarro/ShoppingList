using Microsoft.Maui.Controls;
using SmartGrocery.Core.Models;
using System.Collections.Generic;

namespace SmartGrocery.App.Views;

public partial class AddItemPage : ContentPage
{
    public List<Item> ResultItems { get; private set; } = new();
    private readonly int _shoppingListId;
    private int _quantity = 1;
    private bool _isUpdatingText = false;

    public AddItemPage(int shoppingListId)
    {
        InitializeComponent();
        _shoppingListId = shoppingListId;
        
        QuantityEntry.Text = "1";
        
        QuantityEntry.TextChanged += (_, e) =>
        {
            if (_isUpdatingText) return;
            
            var text = e.NewTextValue;
            
            if (string.IsNullOrEmpty(text))
            {
                _quantity = 1;
                return;
            }
            
            if (!int.TryParse(text, out var value) || value < 1)
            {
                _isUpdatingText = true;
                QuantityEntry.Text = _quantity.ToString();
                _isUpdatingText = false;
            }
            else
            {
                _quantity = value;
            }
        };
        
        QuantityEntry.Unfocused += (_, __) =>
        {
            if (string.IsNullOrEmpty(QuantityEntry.Text) || _quantity < 1)
            {
                _quantity = 1;
                _isUpdatingText = true;
                QuantityEntry.Text = "1";
                _isUpdatingText = false;
            }
        };
        
        // Auto-focus on load
        Loaded += async (s, e) => 
        {
            await Task.Delay(100);
            ItemNameEntry.Focus();
        };
    }

    private void Increase_Clicked(object sender, EventArgs e)
    {
        _quantity++;
        _isUpdatingText = true;
        QuantityEntry.Text = _quantity.ToString();
        _isUpdatingText = false;
    }

    private void Decrease_Clicked(object sender, EventArgs e)
    {
        if (_quantity > 1) _quantity--;
        _isUpdatingText = true;
        QuantityEntry.Text = _quantity.ToString();
        _isUpdatingText = false;
    }

    private void Add_Clicked(object sender, EventArgs e)
    {
        var name = ItemNameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            DisplayAlert("Error", "Item name cannot be empty.", "OK");
            return;
        }

        var newItem = new Item
        {
            ItemName = name,
            Quantity = _quantity,
            ShoppingListId = _shoppingListId
        };
        
        ResultItems.Add(newItem);
        
        // Add to UI list
        var itemFrame = new Frame
        {
            Padding = 12,
            Margin = new Thickness(0, 4),
            BackgroundColor = Color.FromArgb("#2D2D2D"),
            CornerRadius = 8,
            HasShadow = false
        };
        
        var itemLabel = new Label
        {
            Text = $"{name} ({_quantity})",
            TextColor = Colors.White,
            FontSize = 16
        };
        
        itemFrame.Content = itemLabel;
        AddedItemsContainer.Children.Add(itemFrame);
        
        // Show the "Added Items" section and Done button
        AddedItemsLabel.IsVisible = true;
        DoneButton.IsVisible = true;
        
        // Clear form for next item
        ItemNameEntry.Text = string.Empty;
        _quantity = 1;
        _isUpdatingText = true;
        QuantityEntry.Text = "1";
        _isUpdatingText = false;
        
        // Refocus for quick entry
        ItemNameEntry.Focus();
    }

    private async void Done_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        ResultItems.Clear();
        await Navigation.PopModalAsync();
    }
}