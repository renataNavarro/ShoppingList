using SmartGrocery.App.ViewModels;
using SmartGrocery.Core.Interfaces;
using SmartGrocery.Core.Models;
namespace SmartGrocery.App.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _vm;
    private readonly IRepository _repository;

    public HomePage()//HomeViewModel vm)
    {
        InitializeComponent();
        _vm = Helpers.ServiceProvider.GetService<HomeViewModel>();
        _repository = Helpers.ServiceProvider.GetService<IRepository>();
        BindingContext = _vm;
        System.Diagnostics.Debug.WriteLine("HomePage initialized");
    }

     protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadListsAsync();
        System.Diagnostics.Debug.WriteLine($"Loaded {_vm.Lists.Count} lists");
    } 
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (e.CurrentSelection.FirstOrDefault() is ShoppingList selectedList)
    {
        // Passa o objeto da lista para a página de detalhes
        var page = new ShoppingListPage(new ShoppingListViewModel(_repository, selectedList));
        await Navigation.PushAsync(page);
    }

    // Deseleciona o item para não ficar destacado
    ((CollectionView)sender).SelectedItem = null;
}

}
