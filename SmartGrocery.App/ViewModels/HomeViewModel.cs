using SmartGrocery.Core.Interfaces;
using SmartGrocery.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace SmartGrocery.App.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly IRepository _repository;

    public ObservableCollection<ShoppingList> Lists { get; } = new();
    public ICommand AddListCommand { get; }
    public ICommand DeleteListCommand { get; }
    public HomeViewModel(IRepository repository)
    {
        _repository = repository;
        AddListCommand = new Command(async () => await AddListAsync());
        DeleteListCommand = new Command<ShoppingList>(async list => await DeleteListAsync(list));
    }

    public async Task LoadListsAsync()
    {
        var lists = await _repository.GetAllListsAsync();
        Lists.Clear();
        foreach (var list in lists)
            Lists.Add(list);
    }
    private async Task AddListAsync()
{
    var name = await Application.Current.MainPage.DisplayPromptAsync(
        "New List",
        "Enter the name of your new shopping list:",
        "Create",
        "Cancel");

    if (string.IsNullOrWhiteSpace(name))
        return;

    var newList = new ShoppingList { ListName = name };
    await _repository.AddListAsync(newList);
    await LoadListsAsync();
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
    Lists.Remove(list);
}
}
