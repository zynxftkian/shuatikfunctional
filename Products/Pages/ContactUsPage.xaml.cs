namespace Products.Pages;

public partial class ContactUsPage : ContentPage
{
	public ContactUsPage()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductsPage());
    }
}