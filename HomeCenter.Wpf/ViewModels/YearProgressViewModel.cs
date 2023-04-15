using System;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeCenter.Wpf.ViewModels
{
    public partial class YearProgressViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _yearProgress = "0";

        public YearProgressViewModel()
        {
            SetYearProgress();
        }

        private void SetYearProgress()
            => YearProgress = double.Round((double)DateTime.Now.DayOfYear / CultureInfo.InvariantCulture.Calendar.GetDaysInYear(DateTime.Now.Year) * 100, 0).ToString();
    }
}
