namespace Products.Pages;

public partial class AccessibilityPage : ContentPage
{
	public AccessibilityPage()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void OnContactUsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ContactUsPage());
    }
    private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {

        backtotop.IsVisible = e.ScrollY > 10;
    }
    private async void OnBackToTopClicked(object sender, EventArgs e)
    {
        // Scroll to top smoothly
        await ProductsScrollView.ScrollToAsync(0, 0, true);
    }
}