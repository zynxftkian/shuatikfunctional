namespace Products.Pages;

public partial class PrivacyPage : ContentPage
{
	public PrivacyPage()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductsPage());
    }
    private async void OnContactUsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ContactUsPage());
    }
}