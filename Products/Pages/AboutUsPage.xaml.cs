using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Products.Pages
{
    public partial class AboutUsPage : ContentPage
    {
        public AboutUsPage()
        {
            InitializeComponent();
            CheckScreenSize();
            SizeChanged += (s, e) => CheckScreenSize();
        }
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductsPage());
        }
        private void CheckScreenSize()
        {
            if (Width < 800) // Mobile View
            {
                mobileVMC.IsVisible = true;
                desktopVMC.IsVisible = false;
            }
            else // Desktop View
            {
                desktopVMC.IsVisible = true;
                mobileVMC.IsVisible = false;
            }
        }
    }
}
