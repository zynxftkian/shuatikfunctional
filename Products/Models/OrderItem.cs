using System.ComponentModel;

public class OrderItem : INotifyPropertyChanged
{
    public string ProductName { get; set; }
    public double Price { get; set; }

    private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(TotalPrice)); // Notify UI
            }
        }
    }

    public double TotalPrice => Price * Quantity;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
