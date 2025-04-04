using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Products.Pages;

public partial class OrdersPage : ContentPage, INotifyPropertyChanged
{
    public ObservableCollection<OrderItem> Orders => App.Orders;

    public string TotalAmount => $"{Orders.Sum(o => o.TotalPrice):N2}";

    private bool isSidebarOpen = false;

    public OrdersPage()
    {
        InitializeComponent();
        BindingContext = this;
        Orders.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalAmount));

        CheckScreenSize();
        SizeChanged += (s, e) => CheckScreenSize();

        UpdateUI();
    }

    private void CheckScreenSize()
    {
        if (Width < 800) // Mobile View
        {
            NavbarButtons.IsVisible = false;
            HamburgerButton.IsVisible = true;
        }
        else // Desktop View
        {
            NavbarButtons.IsVisible = true;
            HamburgerButton.IsVisible = false;
            Sidebar.IsVisible = false;
            Sidebar.TranslationX = -250;
        }
    }

    private async void ToggleSidebar(object sender, EventArgs e)
    {
        isSidebarOpen = !isSidebarOpen;

        if (isSidebarOpen)
        {
            Sidebar.IsVisible = true;
            await Sidebar.TranslateTo(0, 0, 200); // Slide in
        }
        else
        {
            await Sidebar.TranslateTo(-250, 0, 200); // Slide out
            Sidebar.IsVisible = false;
        }
    }

    private async void CloseSidebar()
    {
        if (isSidebarOpen)
        {
            await Sidebar.TranslateTo(-250, 0, 200);
            Sidebar.IsVisible = false;
            isSidebarOpen = false;
        }
    }

    private void OnMinusClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OrderItem order)
        {
            if (order.Quantity > 1)
            {
                order.Quantity--;
            }
            else
            {
                App.Orders.Remove(order);
            }
            OnPropertyChanged(nameof(TotalAmount)); // Update total amount when an item is removed
        }
    }

    private void OnPlusClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OrderItem order)
        {
            order.Quantity++;
            OnPropertyChanged(nameof(TotalAmount)); // Update total amount
        }
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        CloseSidebar();
        await Navigation.PushAsync(new ProductsPage());
    }

    public new event PropertyChangedEventHandler PropertyChanged;
    protected new void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private async void OnCheckOutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OrdersSummaryPage());
    }
    private void OnPointerEntered(object sender, PointerEventArgs e)
    {
        if (sender is Button button)
        {
            button.BackgroundColor = Colors.White;
        }
    }

    private void OnPointerExited(object sender, PointerEventArgs e)
    {
        if (sender is Button button)
        {
            button.BackgroundColor = Colors.Transparent;
        }
    }
    private void OnPointerEntered3(object sender, PointerEventArgs e)
    {
        if (sender is Button button)
        {
            button.BackgroundColor = Colors.PeachPuff;
        }
    }
    private void OnPointerExited3(object sender, PointerEventArgs e)
    {
        if (sender is Button button)
        {
            button.BackgroundColor = Colors.Orange;
        }
    }
    private async void OnAboutUsClicked(object sender, EventArgs e)
    {
        CloseSidebar();
        await Navigation.PushAsync(new AboutUsPage());
    }
    private async void OnLogInOrSignUpClicked(object sender, EventArgs e)
    {
        CloseSidebar();
        await Navigation.PushAsync(new LogInLogOutPage());
    }
    private async void OnProductsClicked(object sender, EventArgs e)
    {
        CloseSidebar();
        // Navigate to the ProductsPage
        var productsPage = new ProductsPage();
        await Navigation.PushAsync(productsPage);

        await Task.Delay(600);

        // Scroll to the Products section once the page is displayed
        productsPage.OnProductsClicked(sender, e);

    }
    private async void OnOrdersClicked(object sender, EventArgs e)
    {
        CloseSidebar();
    }
    private async void OnLogOutClicked(object sender, EventArgs e)
    {
        App.Orders.Clear();
        OnPropertyChanged(nameof(TotalAmount));

        MyAccountPage.ClearUserDetails();
        Preferences.Remove("IsLoggedIn");
        Preferences.Remove("LoggedInUser");
        Preferences.Remove("LoggedInEmail");
        Preferences.Remove("LoggedInPhone");

        UpdateUI();

        await Navigation.PushAsync(new ProductsPage());

        await DisplayAlert("Logged Out", "You have been Logged Out Successfully", "Ok");
    }

    private async void OnMyAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyAccountPage());
    }
    private void UpdateUI()
    {
        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
        MobileLogInLogOutButton.IsVisible = !isLoggedIn;
        MobileLogOutButton.IsVisible = isLoggedIn;

        DesktopLogInLogOutButton.IsVisible = !isLoggedIn;
        DesktopLogOutButton.IsVisible = isLoggedIn;
    }
}
