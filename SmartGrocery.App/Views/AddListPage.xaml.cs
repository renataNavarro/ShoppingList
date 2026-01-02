using SmartGrocery.Core.Models;

namespace SmartGrocery.App.Views;

public partial class AddListPage : ContentPage
{
    public ShoppingList ResultList { get; private set; }
    private ShoppingListType _selectedType;


    public AddListPage()
    {
        InitializeComponent();

      _selectedType = ShoppingListType.Simple; // default
    UpdateSelectionUI();
    Loaded += async (s, e) => 
    {
        await Task.Delay(100);
         ListNameEntry.Focus();
        System.Console.WriteLine(ListNameEntry.IsFocused);  
    };
    }
   
private void OnPageLoaded(object sender, EventArgs e)
{
     ListNameEntry.Focus();
    System.Console.WriteLine(ListNameEntry.IsFocused);  
}
    private void UpdateSelectionUI()
{
    // Purple border when selected, transparent when not
    SimpleFrame.BorderColor = 
        _selectedType == ShoppingListType.Simple ? Color.FromArgb("#512bd4") : Colors.Transparent;
    
    QuantityFrame.BorderColor = 
        _selectedType == ShoppingListType.Quantity ? Color.FromArgb("#512bd4") : Colors.Transparent;
    
    ChecklistFrame.BorderColor = 
        _selectedType == ShoppingListType.Checklist ? Color.FromArgb("#512bd4") : Colors.Transparent;
}


    private async void Create_Clicked(object sender, EventArgs e)
    {
        var name = ListNameEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Error", "List name cannot be empty.", "OK");
            return;
        }

        

        ResultList = new ShoppingList
        {
            ListName = name,
            Type = _selectedType
        };

        await Navigation.PopModalAsync();
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        ResultList = null;
        await Navigation.PopModalAsync();
    }

private void OnSimpleTapped(object sender, EventArgs e)
{
    _selectedType = ShoppingListType.Simple;
    UpdateSelectionUI();
}

private void OnQuantityTapped(object sender, EventArgs e)
{
    _selectedType = ShoppingListType.Quantity;
    UpdateSelectionUI();
}

private void OnChecklistTapped(object sender, EventArgs e)
{
    _selectedType = ShoppingListType.Checklist;
    UpdateSelectionUI();
}

}
