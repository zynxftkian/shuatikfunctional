namespace Products.Pages
{
    public partial class MyAccountPage : ContentPage
    {
        private bool isSidebarOpen = false;
        public MyAccountPage()
        {
            InitializeComponent();
            CheckScreenSize();
            SizeChanged += (s, e) => CheckScreenSize();
            base.OnAppearing();
            LoadUserDetails();
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
                UserInformation.IsVisible = false; // Hide My Account
                await Sidebar.TranslateTo(0, 0, 200); // Slide in
            }
            else
            {
                await Sidebar.TranslateTo(-250, 0, 200); // Slide out
                Sidebar.IsVisible = false;
                UserInformation.IsVisible = true; // Show My Account
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
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            CloseSidebar();
            await Navigation.PushAsync(new ProductsPage());
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
            await Navigation.PushAsync(new OrdersPage());
        }
        private async void OnLogOutClicked(object sender, EventArgs e)
        {
            MyAccountPage.ClearUserDetails();
            Preferences.Remove("IsLoggedIn");
            Preferences.Remove("LoggedInUser");
            Preferences.Remove("LoggedInEmail");
            Preferences.Remove("LoggedInPhone");
            UpdateUI();
            await DisplayAlert("Logged Out", "You have been Logged Out Successfully", "Ok");
        }
        private async void OnMyAccountClicked(object sender, EventArgs e)
        {
            CloseSidebar();
        }
        private void LoadUserDetails()
        {
            if (!Preferences.Get("IsLoggedIn", false))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    usernameLabel.Text = "";
                    emailLabel.Text = "";
                    phoneNumberLabel.Text = "";
                });
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                usernameLabel.Text = Preferences.Get("LoggedInUser", "DefaultUser");
                emailLabel.Text = Preferences.Get("LoggedInEmail", "default@example.com");
                phoneNumberLabel.Text = Preferences.Get("LoggedInPhone", "0000000000");
            });
        }

        private void UpdateUI()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            MobileLogInLogOutButton.IsVisible = !isLoggedIn;
            MobileLogOutButton.IsVisible = isLoggedIn;

            DesktopLogInLogOutButton.IsVisible = !isLoggedIn;
            DesktopLogOutButton.IsVisible = isLoggedIn;
        }
        public static void ClearUserDetails()
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                var currentPage = Application.Current.MainPage.Navigation.NavigationStack[^1] as MyAccountPage;
                if (currentPage != null)
                {
                    currentPage.usernameLabel.Text = "";
                    currentPage.emailLabel.Text = "";
                    currentPage.phoneNumberLabel.Text = "";
                }
            });
        }
    }
}

