using SmartGrocery.App.ViewModels;

namespace SmartGrocery.App.Views;

public partial class ShoppingListPage : ContentPage
{
    private ShoppingListViewModel _vm;

    public ShoppingListPage(ShoppingListViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}
