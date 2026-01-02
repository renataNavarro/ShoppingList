using SmartGrocery.App.ViewModels;
using SmartGrocery.Core.Models;
using Microsoft.Maui.Controls;
using SmartGrocery.App.Views.ListLayouts;

namespace SmartGrocery.App.Factories;

public class ShoppingListViewFactory
{
    public View Create(ShoppingListViewModel vm)
    {
        return vm.ListType switch
        {
            ShoppingListType.Quantity => new SimpleQuantity { BindingContext = vm },
            ShoppingListType.Checklist => new CheckListTemplate { BindingContext = vm },
            _ => new SimpleListTemplate { BindingContext = vm }
        };
    }
}


