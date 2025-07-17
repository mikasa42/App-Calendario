using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Calendar.Models;
using Calendar.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using SQLite;

namespace Calendar.ViewModels
{
    public partial class MonthViewModel : ObservableObject
    {
        private MonthData _monthData;

        public ObservableCollection<DayItem> Days { get; } = new();

        public string selectedMonth { get; set; }
        public string selectedYear { get; set; }

        MonthsDatabase database;

        public MonthViewModel()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "months.db3");
            database = new MonthsDatabase(dbPath);

            Debug.WriteLine(FileSystem.AppDataDirectory);

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            LoadMonthData(currentMonth, currentYear);
        }

        private async void LoadMonthData(int month, int year)
        {
            _monthData = await database.GetMonthAsync((uint)month, (uint)year);

            if (_monthData == null)
                throw new InvalidOperationException($"No entry for {month}/{year}");

            Days.Clear();

            // Primeiro dia do mês (ex: 1 de julho de 2025)
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // 0 = Domingo, 1 = Segunda, ...

            // Adiciona espaços vazios antes do dia 1
            for (int i = 0; i < firstDayOfWeek; i++)
            {
                Days.Add(new DayItem
                {
                    Index = -1,
                    IsSelected = false,
                    IsSpacer = true // novo campo
                });
            }

            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int i = 0; i < daysInMonth; i++)
            {
                Days.Add(new DayItem
                {
                    Index = i,
                    IsSelected = _monthData.GetDayStateAt(i),
                    IsSpacer = false
                });
            }

            UpdateMonthAndYearView(month, year);
        }

        private void UpdateMonthAndYearView(int month, int year)
        {
            selectedMonth = new DateTime(year, month, 1).ToString("MMMM", CultureInfo.CurrentCulture).ToUpper();
            selectedYear = $"Calendário  {year.ToString()}";
            OnPropertyChanged(nameof(selectedMonth));
            OnPropertyChanged(nameof(selectedYear));
        }

        [RelayCommand]
        private async Task ToggleDay(DayItem item)
        {
            item.IsSelected = !item.IsSelected;
            _monthData.SetDayStateAt(item.Index, item.IsSelected);
            Debug.WriteLine("Day Toggled");
            await database.SaveMonthAsync(_monthData);
        }

        [RelayCommand]
        private void PreviousMonth()
        {
            _monthData.Month -= 1;
            if (_monthData.Month <= 0)
            {
                _monthData.Month = 12;
                _monthData.Year -= 1;
            }
            Debug.WriteLine("Previous month button pressed");
            LoadMonthData((int)_monthData.Month, (int)_monthData.Year);
        }

        [RelayCommand]
        private void NextMonth()
        {
            _monthData.Month += 1;
            if (_monthData.Month >= 13)
            {
                _monthData.Month = 1;
                _monthData.Year += 1;
            }
            Debug.WriteLine("Next month button pressed");
            LoadMonthData((int)_monthData.Month, (int)_monthData.Year);
        }
    }

    public partial class DayItem : ObservableObject
    {
        public int Index { get; set; }

        [ObservableProperty]
        private bool isSelected;
        public bool IsSpacer { get; set; }

        public bool IsPlaceholder { get; set; } = false;

        public string DayText => IsSpacer ? string.Empty : (Index + 1).ToString();

        public Color BackgroundColor => IsSpacer
            ? Colors.Transparent: isSelected ? Colors.MediumPurple : Colors.Lavender;
       
    }
}
