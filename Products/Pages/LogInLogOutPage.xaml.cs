using System;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Products.Pages
{
    public partial class LogInLogOutPage : ContentPage
    {
        public LogInLogOutPage()
        {
            InitializeComponent();
            AppState.TermsAccepted = false;
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

        // ✅ LOGIN FUNCTION
        private async void OnLogin1Clicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;  // Use capital U
            string password = PasswordEntry.Text;  // Use capital P

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Login Failed", "Please enter both username and password.", "OK");
                return;
            }

            string storedPassword = Preferences.Get($"User_{username}_Password", null);

            if (storedPassword != null && storedPassword == password)
            {
                string email = Preferences.Get($"User_{username}_Email", "");
                string phone = Preferences.Get($"User_{username}_Phone", "");

                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("LoggedInUser", username);
                Preferences.Set("LoggedInEmail", email);
                Preferences.Set("LoggedInPhone", phone);

                await DisplayAlert("Login Successful", $"Welcome, {username}!", "OK");
                await Navigation.PushAsync(new ProductsPage());
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid Username or Password", "Try again");
            }
        }

        // ✅ SIGN-UP FUNCTION
        private async void OnLogin2Clicked(object sender, EventArgs e)
        {
            if (!AppState.TermsAccepted)
            {
                await DisplayAlert("Terms Required", "You must accept the terms and conditions.", "OK");
                return;
            }

            string username = usernameEntry.Text;
            string password = passwordEntry.Text;
            string email = emailEntry.Text;
            string phone = phoneNumberEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d+$"))
            {
                await DisplayAlert("Error", "Phone number must contain only numbers.", "OK");
                return;
            }

            // Check if user already exists
            string existingUser = Preferences.Get($"User_{username}_Password", null);
            if (existingUser != null)
            {
                await DisplayAlert("Error", "Username already exists. Try a different one.", "OK");
                return;
            }

            // Save user data
            Preferences.Set($"User_{username}_Password", password);
            Preferences.Set($"User_{username}_Email", email);
            Preferences.Set($"User_{username}_Phone", phone);

            await DisplayAlert("Sign Up Successful", "You can now log in.", "OK");

            LogIn.IsVisible = true;
            SignIn.IsVisible = false;
        }

        // Switch to sign-up view
        private void OnSignUpClicked(object sender, EventArgs e)
        {
            LogIn.IsVisible = false;
            SignIn.IsVisible = true;
            SignIn.ForceLayout();
        }

        // Switch back to login view
        private void OnBackToLoginClicked(object sender, EventArgs e)
        {
            LogIn.IsVisible = true;
            SignIn.IsVisible = false;
            LogIn.ForceLayout();
        }

        // Navigate to Terms and Conditions Page
        private async void OnViewTermsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermsAndConditionsPage("LogInLogOutPage"));
        }
    }
}
