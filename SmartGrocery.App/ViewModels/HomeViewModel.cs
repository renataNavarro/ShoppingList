using SmartGrocery.Core.Interfaces;
using SmartGrocery.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using SmartGrocery.App.Views;

namespace SmartGrocery.App.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly IRepository _repository;

    public ObservableCollection<ShoppingList> Lists { get; } = new();
    private List<ShoppingList> _allLists = new();
    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (_searchQuery == value)
                return;

            _searchQuery = value;
            OnPropertyChanged();
            ApplyFilter();
        }
    }

    public bool HasLists => _allLists.Any();

    public ICommand AddListCommand { get; }
    public ICommand DeleteListCommand { get; }
    public HomeViewModel(IRepository repository)
    {
        System.Console.WriteLine("1");
        _repository = repository;
        AddListCommand = new Command(async () => await AddListAsync());
        System.Console.WriteLine("2");
        DeleteListCommand = new Command<ShoppingList>(async list => await DeleteListAsync(list));
        System.Console.WriteLine("3");
        Lists.CollectionChanged += (_, __) =>
    {
        OnPropertyChanged(nameof(HasLists));
    };
    }

    /* public async Task LoadListsAsync()
    {
        var lists = await _repository.GetAllListsAsync();
        Lists.Clear();
        foreach (var list in lists)
            Lists.Add(list);

        System.Console.WriteLine("loadlistend");
    } */
    public async Task LoadListsAsync()
    {
        var lists = await _repository.GetAllListsAsync();

        _allLists = lists.ToList();

        ApplyFilter();
    }

    private async Task AddListAsync()
    {
        System.Console.WriteLine("add after load1");
        var page = new AddListPage();
        await Application.Current.MainPage.Navigation.PushModalAsync(page);
        // Espera até o modal fechar
        while (page.Navigation.ModalStack.Contains(page))
            await Task.Delay(100);
        System.Console.WriteLine("add after load2");
        if (page.ResultList == null)
        {
            System.Console.WriteLine("Add list  null");
            return;
        }
        await _repository.AddListAsync(page.ResultList);
        System.Console.WriteLine("add after rep");
        await LoadListsAsync();
        System.Console.WriteLine("add after load");
    }
    private void ApplyFilter()
    {
        Lists.Clear();

        IEnumerable<ShoppingList> filtered = _allLists;

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            filtered = filtered.Where(l =>
                l.ListName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var list in filtered)
            Lists.Add(list);
    }

    private async Task DeleteListAsync(ShoppingList list)
    {
        if (list == null) return;

        // opcional: confirmar antes de deletar
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete List",
            $"Are you sure you want to delete '{list.ListName}'?",
            "Yes",
            "Cancel");

        if (!confirm) return;

        await _repository.DeleteListAsync(list.Id);
        _allLists.Remove(list);
        ApplyFilter();
    }
}
