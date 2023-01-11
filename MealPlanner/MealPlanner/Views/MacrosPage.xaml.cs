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

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MacrosPage : ContentPage
    {
        public MacrosPage()
        {
            InitializeComponent();
            InitChart();
        }

        public void InitChart()
        {
            var vm = BindingContext as MacrosViewModel;

            var entries = new[]
            {
                new ChartEntry((float)vm.ProtsPercentage)
                {
                    Label = "Proteins",
                    ValueLabel = $"{vm.RefData.User.SelectedTypeOfRegime.ProteinPercentage * 100} %",
                    ValueLabelColor = SKColor.Parse("#29aae3"),
                    TextColor = SKColor.Parse("#29aae3"),
                    Color = SKColor.Parse("#29aae3")
                },
                new ChartEntry((float)vm.CarbsPercentage)
                {
                    Label = "Carbs",
                    ValueLabel = $"{vm.RefData.User.SelectedTypeOfRegime.CarbsPercentage * 100} %",
                    ValueLabelColor = SKColor.Parse("#77d065"),
                    TextColor = SKColor.Parse("#77d065"),
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry((float)vm.FatsPercentage)
                {
                    Label = "Fats",
                    ValueLabelColor = SKColor.Parse("#b455b6"),
                    TextColor = SKColor.Parse("#b455b6"),
                    ValueLabel = $"{vm.RefData.User.SelectedTypeOfRegime.FatsPercentage * 100} %",
                    Color = SKColor.Parse("#b455b6")
                }
            };
            var chart = new DonutChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelMode = LabelMode.LeftAndRight,
                LabelTextSize = 40

            };
            chartView.Chart = chart;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            InitChart();
        }

        private async void RSNumericEntry_Completed(object sender, EventArgs e)
        {
            var vm = BindingContext as MacrosViewModel;
            vm.OnPropertyChangedPercentageSum100();

            if (vm.IsMacroPercentageSum100)
            {
                vm.RefData.User.SelectedTypeOfRegime.ProteinPercentage = vm.ProtsPercentage / 100;
                vm.RefData.User.SelectedTypeOfRegime.CarbsPercentage = vm.CarbsPercentage / 100;
                vm.RefData.User.SelectedTypeOfRegime.FatsPercentage = vm.FatsPercentage / 100;
                vm.RefData.UpdateDailyValues();

                //await App.DataBaseRepo.UpdateUserAsync(vm.RefData.User);
                await App.DataBaseRepo.UpdateTypeOfRegimeItemAsync(vm.RefData.User.SelectedTypeOfRegime);
            }

            InitChart();
        }
    }
}