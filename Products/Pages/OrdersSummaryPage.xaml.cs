using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Pages;

public partial class OrdersSummaryPage : ContentPage
{
    public ObservableCollection<OrderItem> Orders { get; set; }

    private string _paymentMethod;

    private bool isSidebarOpen = false;

    public OrdersSummaryPage(string paymentMethod)
    {
        InitializeComponent();
        _paymentMethod = paymentMethod;
        Orders = new ObservableCollection<OrderItem>(App.Orders);

        // Calculate total price
        double totalPrice = Orders.Sum(order => order.Price * order.Quantity);
        TotalPriceLabel.Text = $"{totalPrice}"; // No currency symbol

        BindingContext = this;

        CheckScreenSize();
        SizeChanged += (s, e) => CheckScreenSize();

        UpdateUI();
        LoadUserDetails();
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

    // Home Button Click Event
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        CloseSidebar();
        await Navigation.PushAsync(new ProductsPage());
    }

    // Orders Button Click Event
    private async void OnOrdersClicked(object sender, EventArgs e)
    {
        CloseSidebar();
        await Navigation.PushAsync(new OrdersPage());
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
    private async void OnLogOutClicked(object sender, EventArgs e)
    {
        App.Orders.Clear();

        // Update the total price after clearing orders
        double totalPrice = Orders.Sum(order => order.Price * order.Quantity);
        TotalPriceLabel.Text = $"{totalPrice}"; // Update the total label

        MyAccountPage.ClearUserDetails();
        Preferences.Remove("IsLoggedIn");
        Preferences.Remove("LoggedInUser");
        Preferences.Remove("LoggedInEmail");
        Preferences.Remove("LoggedInPhone");

        UpdateUI();

        await Navigation.PushAsync(new ProductsPage());

        await DisplayAlert("Logged Out", "You have been Logged Out Successfully", "Ok");
    }

    private void LoadUserDetails()
    {
        if (!Preferences.Get("IsLoggedIn", false))
        {
            usernameLabel.Text = "";
            return;
        }

        usernameLabel.Text = Preferences.Get("LoggedInUser", "");
    }
}
