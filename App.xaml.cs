namespace Calendar
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SplashPage(); // Começa com Splash
        }
    }
}