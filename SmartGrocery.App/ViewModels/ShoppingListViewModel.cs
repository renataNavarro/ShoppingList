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
    public ICommand ClearCheckedCommand { get; }

    public ICommand RefreshCommand => new Command(async () => await LoadItemsAsync());

    private ObservableCollection<ItemViewModel> _allItems = new();

    public ObservableCollection<ItemViewModel> Items { get; set;}
    public bool HasItems => _allItems.Any();

    public string ListName => _shoppingList.ListName;
    public ShoppingListType ListType => _shoppingList.Type;
    public bool IsChecked { get; set; }



    public ShoppingListViewModel(IRepository repository, ShoppingList list)
    {
        _repository = repository;
        _shoppingList = list;
        LoadItemsAsync();
        AddItemCommand = new Command(async () => await AddItemAsync());

        ClearCheckedCommand = new Command(async () => await ClearCheckedAsync());

        DeleteItemCommand = new Command<ItemViewModel>(async item =>
        {
            await DeleteItemAsync(item);
        });
        Items.CollectionChanged += (_, __) =>
    {
        OnPropertyChanged(nameof(HasItems));
    };
    }

    private void ApplyFilter()
    {
        Items.Clear();
        foreach (var item in _allItems.Where(i => string.IsNullOrEmpty(SearchQuery) || i.ItemName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)))
            Items.Add(item);
    }
    public async Task LoadItemsAsync()
    {
        System.Console.WriteLine("LoadItems");
        _allItems.Clear();
        var itemsFromRepo = await _repository.GetItemsByListIdAsync(_shoppingList.Id);
        _allItems = new ObservableCollection<ItemViewModel>(
            itemsFromRepo.Select(i => new ItemViewModel(i)));
        

        ApplyFilter();  // atualiza a coleção visível
    }
    public async Task AddItemAsync()
    {
        var page = new AddItemPage(_shoppingList.Id);
        await Application.Current.MainPage.Navigation.PushModalAsync(page);

        // Espera até o modal fechar
        while (page.Navigation.ModalStack.Contains(page))
            await Task.Delay(100);

        if (page.ResultItems.Any())
        {
            foreach (var item in page.ResultItems)
            {
                await _repository.AddItemAsync(item);
            }
            await LoadItemsAsync();
        }

    }


    private async Task ClearCheckedAsync()
    {
        var checkedItems = Items.Where(i => i.IsChecked).ToList();

        foreach (var item in checkedItems)
            await _repository.DeleteItemAsync(item.Id);

        await LoadItemsAsync();
    }
   public async Task UpdateItemAsync(ItemViewModel itemVm)
{
    await _repository.UpdateItemAsync(itemVm.Model);
}


    public async Task DeleteItemAsync(ItemViewModel itemVm)
{
    await _repository.DeleteItemAsync(itemVm.Model.Id);

    _allItems.Remove(itemVm);
    ApplyFilter();
}
private async Task OnItemChanged(ItemViewModel item)
{
    await _repository.UpdateItemAsync(item.Model);
}

}
