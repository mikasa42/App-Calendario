namespace Calendar;

public partial class SplashPage : ContentPage
{
    private bool _hasAnimated = false;

    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Garante que a animação só roda uma vez
        if (_hasAnimated) return;
        _hasAnimated = true;

        await MainLayout.FadeTo(1, 1500);
        await Task.Delay(2000);

        Application.Current.MainPage = new NavigationPage(new CalendarPage());
    }
}
