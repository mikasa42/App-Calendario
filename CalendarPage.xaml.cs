using Calendar.ViewModels;
namespace Calendar;

public partial class CalendarPage : ContentPage
{
	public CalendarPage()
	{
        InitializeComponent();
        BindingContext = new MonthViewModel();
    }
}