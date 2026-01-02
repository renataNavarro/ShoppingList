using SmartGrocery.App.Factories;
using SmartGrocery.App.ViewModels;
using SmartGrocery.App.Views.ListLayouts;
using SmartGrocery.Core.Models;

namespace SmartGrocery.App.Views;

public partial class ShoppingListPage : ContentPage
{
    private ShoppingListViewModel _vm;

    public ShoppingListPage(ShoppingListViewModel vm, ShoppingListViewFactory factory)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        Content = factory.Create(vm);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
    
}
