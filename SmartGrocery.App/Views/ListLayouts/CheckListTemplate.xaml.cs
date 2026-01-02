using SmartGrocery.App.ViewModels;
using SmartGrocery.Core.Models;

namespace SmartGrocery.App.Views.ListLayouts
{
    public partial class CheckListTemplate : ContentView
    {
        public CheckListTemplate()
        {
            InitializeComponent();
        }
        private async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkbox &&
            checkbox.BindingContext is Item item &&
            BindingContext is ShoppingListViewModel vm)
        {
            item.IsChecked = e.Value;
            await vm.UpdateItemAsync(item);
        }
    }
    }
}
