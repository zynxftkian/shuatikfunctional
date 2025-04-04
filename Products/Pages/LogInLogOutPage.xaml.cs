using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;

namespace Products.Pages
{
    public partial class LogInLogOutPage : ContentPage
    {
        // Dictionary to store registered users (temporary storage)
        private Dictionary<string, (string Password, string Email, string PhoneNumber)> registeredUsers = new();

        public LogInLogOutPage()
        {
            InitializeComponent();
        }

        // Button hover effect (optional UI improvement)
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

        // Login function
        private async void OnLogin1Clicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Login Failed", "Please enter both username and password", "OK");
                return;
            }

            if (registeredUsers.ContainsKey(username) && registeredUsers[username].Password == password)
            {
                string email = registeredUsers[username].Email;
                string phoneNumber = registeredUsers[username].PhoneNumber;

                // Save login state and user details
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("LoggedInUser", username);
                Preferences.Set("LoggedInEmail", email);
                Preferences.Set("LoggedInPhone", phoneNumber);

                await DisplayAlert("Login Successful", $"Welcome, {username}!", "OK");

                // Navigate to MyAccountPage
                await Navigation.PushAsync(new ProductsPage());
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid Username or Password", "Try again");
            }
        }

        // Sign up function
        private async void OnLogin2Clicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;
            string email = emailEntry.Text;
            string phoneNumber = phoneNumberEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                await DisplayAlert("Error", "Please fill all fields", "OK");
                return;
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            // Validate phone number (digits only)
            if (!Regex.IsMatch(phoneNumber, @"^\d+$"))
            {
                await DisplayAlert("Error", "Phone number must contain only numbers.", "OK");
                return;
            }

            if (registeredUsers.ContainsKey(username))
            {
                await DisplayAlert("Error", "Username already exists. Try another one.", "OK");
                return;
            }

            // Store user details
            registeredUsers[username] = (password, email, phoneNumber);
            await DisplayAlert("Sign Up Successful", "You can now log in.", "OK");

            // Redirect user to login page
            LogIn.IsVisible = true;
            SignIn.IsVisible = false;
        }



        // Switch to sign-up page
        private void OnSignUpClicked(object sender, EventArgs e)
        {
            LogIn.IsVisible = false;
            SignIn.IsVisible = true;
            SignIn.ForceLayout();
        }

        // Switch back to login page
        private void OnBackToLoginClicked(object sender, EventArgs e)
        {
            LogIn.IsVisible = true;
            SignIn.IsVisible = false;
            LogIn.ForceLayout();
        }
    }
}
