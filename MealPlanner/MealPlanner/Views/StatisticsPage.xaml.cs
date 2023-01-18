using MealPlanner.Helpers;
using MealPlanner.Models;
using MealPlanner.ViewModels;
using Microcharts;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsPage : ContentPage
    {
        public StatisticsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitChart();
        }

        public void InitChart()
        {
            var vm = BindingContext as StatisticsViewModel;

            var proteinColor = Color.LightGray;

            List<Log> logs = new List<Log>();

            foreach (Log log in vm.RefData.Logs.Where(x=> x.Date.Date <= DateTime.Now.Date).OrderByDescending(x => x.Date))
            {
                if (logs.Count >= 7)
                    break;

                logs.Add(log);
            }

            var maxSize = logs.Count() <= 7 ? logs.Count : 7;
            ChartEntry[] chartEntries = new ChartEntry[maxSize];
            ChartEntry[] chartCaloriesEntries = new ChartEntry[maxSize];
            int i = 0;
            foreach(Log log in logs.OrderBy(x=> x.Date))
            {
                if (i >= 7)
                    break;

                // Weight
                ChartEntry chartEntry = new ChartEntry((float)log.UserWeight)
                {
                    Label = log.Date.ToString("dd/MM"),
                    ValueLabel = $"{(float)log.UserWeight} kg",
                    ValueLabelColor = proteinColor.ToSKColor(),
                    TextColor = proteinColor.ToSKColor(),
                    Color = proteinColor.ToSKColor()
                };
                chartEntries[i] = chartEntry;


                // Calories
                double calories = 0;

                //Populate meals
                foreach (Meal meal in log.Meals)
                {
                    vm.RefData.PopulateMeal(meal);
                }

                calories = Math.Round(log.Meals.Sum(x => x.Calories), 0);
                ChartEntry chartCaloriesEntry = new ChartEntry((float)calories)
                {
                    Label = log.Date.ToString("dd/MM"),
                    ValueLabel = $"{(float)calories} {vm.RefData.User.EnergyUnit}",
                    ValueLabelColor = proteinColor.ToSKColor(),
                    TextColor = proteinColor.ToSKColor(),
                    Color = proteinColor.ToSKColor()
                };
                chartCaloriesEntries[i] = chartCaloriesEntry;

                i++;
            }

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Screen density
            var density = mainDisplayInfo.Density;
            var size = (Device.GetNamedSize(NamedSize.Small, typeof(Label), useOldSizes: false)) * density;


            // Weight
            var chart = new LineChart()
            {
                Entries = chartEntries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelTextSize = (float)size,
                LabelOrientation = Orientation.Horizontal
            };
            chartView.Chart = chart;


            // Calories
            var chartCalories = new BarChart()
            {
                Entries = chartCaloriesEntries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelTextSize = (float)size,
                LabelOrientation = Orientation.Horizontal
            };
            chartViewCalories.Chart = chartCalories;
        }

    }
}