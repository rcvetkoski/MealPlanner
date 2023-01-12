using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp.Views.Forms;
using MealPlanner.ViewModels;
using static MealPlanner.Models.User;
using Xamarin.Essentials;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MacrosPage : ContentPage
    {
        public MacrosPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as MacrosViewModel;
            vm.InitProperties();
            InitChart();
        }

        public void InitChart()
        {
            var vm = BindingContext as MacrosViewModel;

            var entries = new[]
            {
                new ChartEntry((float)vm.RefData.User.SelectedTypeOfRegime.ProteinPercentage * 100)
                {
                    Label = "Proteins",
                    ValueLabel = $"{vm.RefData.User.SelectedTypeOfRegime.ProteinPercentage * 100} %",
                    ValueLabelColor = SKColor.Parse("#29aae3"),
                    TextColor = SKColor.Parse("#29aae3"),
                    Color = SKColor.Parse("#29aae3")
                },
                new ChartEntry((float)vm.RefData.User.SelectedTypeOfRegime.CarbsPercentage * 100)
                {
                    Label = "Carbs",
                    ValueLabel = $"{vm.RefData.User.SelectedTypeOfRegime.CarbsPercentage * 100} %",
                    ValueLabelColor = SKColor.Parse("#77d065"),
                    TextColor = SKColor.Parse("#77d065"),
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry((float)vm.RefData.User.SelectedTypeOfRegime.FatsPercentage * 100)
                {
                    Label = "Fats",
                    ValueLabelColor = SKColor.Parse("#b455b6"),
                    TextColor = SKColor.Parse("#b455b6"),
                    ValueLabel = $"{vm.RefData.User.SelectedTypeOfRegime.FatsPercentage * 100} %",
                    Color = SKColor.Parse("#b455b6")
                }
            };

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Screen density
            var density = mainDisplayInfo.Density;
            var size = Device.GetNamedSize(NamedSize.Small, typeof(Label), useOldSizes: false) * density;

            var chart = new DonutChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelMode = LabelMode.LeftAndRight,
                LabelTextSize = (float)size
            };
            chartView.Chart = chart;
        }
    }
}