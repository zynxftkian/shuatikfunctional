namespace Products.Pages;

public partial class OurTeamPage : ContentPage
{
	public OurTeamPage()
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
            desktopUS.IsVisible = false;
            mobileUS.IsVisible = true;
        }
        else // Desktop View
        {
            desktopUS.IsVisible = true;
            mobileUS.IsVisible = false;
        }
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