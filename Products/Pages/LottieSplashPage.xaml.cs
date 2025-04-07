namespace Products.Pages;

public partial class LottieSplashPage : ContentPage
{
	public LottieSplashPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Wait for the duration of the Lottie animation
        await Task.Delay(9000);  // Adjust the delay to match the Lottie's animation duration

        // Navigate to the next page after the animation
        Application.Current.MainPage = new NavigationPage(new ProductsPage()); // or MainPage
    }
}