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
            await Navigation.PopAsync();
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
        private async void OnBackToTopClicked(object sender, EventArgs e)
        {
            // Scroll to top smoothly
            await ProductsScrollView.ScrollToAsync(0, 0, true);
        }
        private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
        {

            backtotop.IsVisible = e.ScrollY > 100;
        }
    }
}
