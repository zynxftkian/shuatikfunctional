using static Products.Pages.LogInLogOutPage;

namespace Products.Pages;

public partial class TermsAndConditionsPage : ContentPage
{
	public TermsAndConditionsPage(string fromPage)
	{
		InitializeComponent();
        AppState.CameFromPage = fromPage;
        TermsCheckbox.IsChecked = AppState.TermsAccepted;

        CheckScreenSize();
        SizeChanged += (s, e) => CheckScreenSize();
    }

    private void CheckScreenSize()
    {
        if (Width < 800) // Mobile View
        {
            tacpmobile.IsVisible = true;
            tacpdesktop.IsVisible = false;
        }
        else // Desktop View
        {
            tacpdesktop.IsVisible = true;
            tacpmobile.IsVisible = false;
        }
    }

    private async void OnViewTermsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermsAndConditionsPage("LoginPage"));
    }


    private void OnCheckboxChanged(object sender, CheckedChangedEventArgs e)
    {
        AppState.TermsAccepted = e.Value;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
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