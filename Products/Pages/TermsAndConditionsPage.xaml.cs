namespace Products.Pages;

public partial class TermsAndConditionsPage : ContentPage
{
	public TermsAndConditionsPage()
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
    private async void OnPrivacyClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrivacyPage());
    }
}