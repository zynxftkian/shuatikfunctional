using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Products;
using Microsoft.Maui.Platform;



namespace Products
{
    public partial class ProductsPage : ContentPage
    {
        public ObservableCollection<Product> Products { get; set; }

        private bool isSidebarOpen = false;

        public ProductsPage()
        {
            InitializeComponent();
            CheckScreenSize();
            SizeChanged += (s, e) => CheckScreenSize();

            Products = new ObservableCollection<Product>
    {
        new Product { Name = "SARTIK", Image = "sartik.jpg", Description = "Sartik is a premium version of Shuatik made of Spanish sardines, carefully crafted enhanced by slightly salty umami flavor profile with a blend of herbs and spice, it can be paired with atchara." },
        new Product { Name = "SAUSATIK", Image = "sausatik.jpg", Description = "Sausatik is made of Chinese sausage with versatility that balances sweetness, salty, and creaminess, making it the perfect complement to pasta dish." },
        new Product { Name = "SHUATIK", Image = "shuatik.jpg", Description = "The pasta uses a squash sauce with coconut milk providing a creamy, slightly sweet, and savory flavors that in a way depicts a traditional Filipino dish \"Ginataang Kalabasa\". Topped with latik that is made by simmering coconut milk until the solids separate into curds and caramelized into golden toasted flavor." }
    };

            BindingContext = this;
            UpdateUI();
        }

        // Check screen size and adjust navbar/sidebar visibility
        private void CheckScreenSize()
        {
            if (Width < 800) // Mobile View
            {
                NavbarButtons.IsVisible = false;
                HamburgerButton.IsVisible = true;
                MobileProductCarousel.IsVisible = true;
                DesktopProductLayout.IsVisible = false;
                HomeSectionDesktop.IsVisible = false;
                HomeSectionMobile.IsVisible = true;
                MobileFooter.IsVisible = true;
                DesktopFooter.IsVisible = false;
            }
            else // Desktop View
            {
                NavbarButtons.IsVisible = true;
                HamburgerButton.IsVisible = false;
                Sidebar.IsVisible = false;
                Sidebar.TranslationX = -250;

                MobileProductCarousel.IsVisible = false;
                DesktopProductLayout.IsVisible = true;

                HomeSectionDesktop.IsVisible = true;
                HomeSectionMobile.IsVisible = false;

                MobileFooter.IsVisible = false;
                DesktopFooter.IsVisible = true;
            }
        }

        // ✅ Open/Close Sidebar (Improved)
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

        // ✅ Close Sidebar When Clicking a Menu Item
        private async void CloseSidebar()
        {
            if (isSidebarOpen)
            {
                await Sidebar.TranslateTo(-250, 0, 200);
                Sidebar.IsVisible = false;
                isSidebarOpen = false;
            }
        }

        //Navbar buttons

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            CloseSidebar();
            HighlightNavbarButton("Home");
            await ProductsScrollView.ScrollToAsync(0, 0, true);
        }

        public async void OnProductsClicked(object sender, EventArgs e)
        {
            CloseSidebar();
            HighlightNavbarButton("Products");
            if (ProductsContainer != null)
            {
                await ProductsScrollView.ScrollToAsync(ProductsContainer, ScrollToPosition.Start, true);
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

        private async void OnOrdersClicked(object sender, EventArgs e)
        {
            CloseSidebar();
            await Navigation.PushAsync(new OrdersPage());
        }

        // Hover effect: Show description on pointer enter
        private void OnPointerEntered2(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.Content is Grid grid && grid.Children[1] is Label descLabel)
            {
                descLabel.IsVisible = true;
            }
        }

        // Hide description on pointer exit
        private void OnPointerExited2(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.Content is Grid grid && grid.Children[1] is Label descLabel)
            {
                descLabel.IsVisible = false;
            }
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

        // Order button click event
        private async void OnOrderClicked(object sender, EventArgs e)
        {
            // Check if user is logged in (Stored in Preferences)
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

            if (!isLoggedIn)
            {
                // Redirect to Login Page if not logged in
                await Navigation.PushAsync(new LogInLogOutPage());
                return;
            }

            // Proceed with order placement if logged in
            Button button = (Button)sender;
            string productName = "";
            double price = 0;

            if (button.Parent is VerticalStackLayout productLayout)
            {
                Label nameLabel = productLayout.Children[0] as Label;
                productName = nameLabel.Text;

                switch (productName)
                {
                    case "SARTIK":
                    case "SAUSATIK":
                    case "SHUATIK":
                        price = 80.00;
                        break;
                }
            }

            var existingOrder = App.Orders.FirstOrDefault(o => o.ProductName == productName);
            if (existingOrder != null)
            {
                existingOrder.Quantity++;
            }
            else
            {
                App.Orders.Add(new OrderItem { ProductName = productName, Price = price });
            }

            await DisplayAlert("Order Placed", "Your order has been added to the Cart page.", "OK");
        }

        private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
        {

            backtotop.IsVisible = e.ScrollY > 100;

            double productsPosition = ProductsContainer.Y;
            double scrollY = e.ScrollY;

            if (scrollY >= productsPosition - 50) // 50px buffer for smooth transition
            {
                HighlightNavbarButton("Products");
            }
            else
            {
                HighlightNavbarButton("Home");
            }
        }

        private void HighlightNavbarButton(string section)
        {
            HomeButton.BorderColor = section == "Home" ? Colors.Black : Colors.Transparent;
            ProductsButton.BorderColor = section == "Products" ? Colors.Black : Colors.Transparent;
        }
        public class Product
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public string Description { get; set; }
            public string PriceText => $"ORDER     --     80 Pesos";
        }
        private void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Microsoft.Maui.Controls.Image image && image.Parent is Grid grid)
            {
                var descriptionView = grid.Children.OfType<ScrollView>().FirstOrDefault();
                if (descriptionView != null)
                {
                    descriptionView.IsVisible = true; // Show the description
                }
            }
        }

        private void OnDescriptionTapped(object sender, EventArgs e)
        {
            if (sender is ScrollView scrollView)
            {
                scrollView.IsVisible = false; // Hide the description and show the image again
            }
        }
        private async void OnLogOutClicked(object sender, EventArgs e)
        {

            AppState.TermsAccepted = false;
            await Navigation.PushAsync(new LogInLogOutPage());

            MyAccountPage.ClearUserDetails();
            Preferences.Remove("IsLoggedIn");
            Preferences.Remove("LoggedInUser");
            Preferences.Remove("LoggedInEmail");
            Preferences.Remove("LoggedInPhone");
            UpdateUI();
            await DisplayAlert("Logged Out", "You have been Logged Out Successfully", "Ok");
        }

        private void UpdateUI()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            MobileLogInLogOutButton.IsVisible = !isLoggedIn;
            MobileLogOutButton.IsVisible = isLoggedIn;

            DesktopLogInLogOutButton.IsVisible = !isLoggedIn;
            DesktopLogOutButton.IsVisible = isLoggedIn;
        }

        private async void OnContactUsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactUsPage());
        }
        private async void OnPrivacyClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PrivacyPage());
        }
        private async void OnTermsAndConditionsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermsAndConditionsPage("HomePage"));
        }
        private async void OnAccessibilityClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccessibilityPage());
        }
        private async void OnOurTeamClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OurTeamPage());
        }
        private async void OnMyAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyAccountPage());
        }
        private async void OnBackToTopClicked(object sender, EventArgs e)
        {
            // Scroll to top smoothly
            await ProductsScrollView.ScrollToAsync(0, 0, true);
        }
    }
}