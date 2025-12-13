using SmartGrocery.Core.Interfaces;
using SmartGrocery.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using SmartGrocery.App.Views;


namespace SmartGrocery.App.ViewModels;

public class ShoppingListViewModel : BaseViewModel
{
    private readonly IRepository _repository;
    private readonly ShoppingList _shoppingList;
    private string _searchQuery;
public string SearchQuery
{
    get => _searchQuery;
    set
    {
        SetProperty(ref _searchQuery, value);
        ApplyFilter();
    }
}
public ICommand AddItemCommand { get; }
public ICommand DeleteItemCommand { get; }

public ICommand RefreshCommand => new Command(async () => await LoadItemsAsync());

private ObservableCollection<Item> _allItems = new();
private void ApplyFilter()
{
    Items.Clear();
    foreach(var item in _allItems.Where(i => string.IsNullOrEmpty(SearchQuery) || i.ItemName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)))
        Items.Add(item);
}


    public ObservableCollection<Item> Items { get; } = new();

    public string ListName => _shoppingList.ListName;

    public ShoppingListViewModel(IRepository repository, ShoppingList list)
    {
        _repository = repository;
        _shoppingList = list;
        LoadItemsAsync();
         AddItemCommand = new Command(async () => await AddItemAsync());
         DeleteItemCommand = new Command<Item>(async item =>
         {
          await DeleteItemAsync(item);   
         });
    }

    public async Task LoadItemsAsync()
{
    System.Console.WriteLine("LoadItems");
    var itemsFromRepo = await _repository.GetItemsByListIdAsync(_shoppingList.Id);

    _allItems.Clear();
    foreach (var item in itemsFromRepo)
        _allItems.Add(item);

    ApplyFilter();  // atualiza a coleção visível
}
public async Task AddItemAsync()
{
    var page = new AddItemPage(_shoppingList.Id);
    await Application.Current.MainPage.Navigation.PushModalAsync(page);

    // Espera até o modal fechar
    while (page.Navigation.ModalStack.Contains(page))
        await Task.Delay(100);

    if (page.ResultItem != null)
    {
        await _repository.AddItemAsync(page.ResultItem);
        await LoadItemsAsync();
    }
}


  

public async Task ToggleItemAsync(Item item)
    {
        item.IsChecked = !item.IsChecked;
        await _repository.UpdateItemAsync(item);
    }

    public async Task DeleteItemAsync(Item item)
    {
        await _repository.DeleteItemAsync(item.Id);
        Items.Remove(item);
    }
}
