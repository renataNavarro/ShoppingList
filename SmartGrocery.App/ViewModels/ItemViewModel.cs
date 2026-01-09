using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmartGrocery.Core.Models;

public class ItemViewModel : INotifyPropertyChanged
{
    public Item Model { get; }

    public ItemViewModel(Item model)
    {
        Model = model;

        _quantity = model.Quantity;
        _unitPrice = model.UnitPrice;
        _isChecked = model.IsChecked;
    }

    // Nome (somente leitura)
    public string ItemName => Model.ItemName;
    public int Id => Model.Id;

    // CHECK
    private bool _isChecked;
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            if (_isChecked == value)
                return;

            _isChecked = value;
            Model.IsChecked = value;

            OnPropertyChanged();
        }
    }

    // QUANTIDADE
    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity == value)
                return;

            _quantity = value;
            Model.Quantity = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalPrice));
        }
    }

    // PREÇO UNITÁRIO
    private decimal _unitPrice;
    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            if (_unitPrice == value)
                return;

            _unitPrice = value;
            Model.UnitPrice = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalPrice));
        }
    }

    // TOTAL (DERIVADO)
    public decimal TotalPrice => Quantity * UnitPrice;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
