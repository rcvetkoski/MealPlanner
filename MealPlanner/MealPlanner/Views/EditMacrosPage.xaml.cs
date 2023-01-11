using MealPlanner.ViewModels;
using Microcharts;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMacrosPage : ContentPage
    {
        public EditMacrosPage()
        {
            InitializeComponent();
            InitChart();
        }

        public void InitChart()
        {
            var vm = BindingContext as EditMacrosViewModel;

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
            var chart = new DonutChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelMode = LabelMode.LeftAndRight,
                LabelTextSize = 40

            };
            chartView.Chart = chart;
        }

        private async void RSNumericEntry_Completed(object sender, EventArgs e)
        {
            UpdateMacros();
        }

        private void RSNumericEntry_Unfocused(object sender, FocusEventArgs e)
        {
            UpdateMacros();
        }

        private async void UpdateMacros()
        {
            var vm = BindingContext as EditMacrosViewModel;
            vm.OnPropertyChangedPercentageSum100();

            if (vm.IsMacroPercentageSum100)
            {
                vm.RefData.User.SelectedTypeOfRegime.ProteinPercentage = vm.ProtsPercentage / 100;
                vm.RefData.User.SelectedTypeOfRegime.CarbsPercentage = vm.CarbsPercentage / 100;
                vm.RefData.User.SelectedTypeOfRegime.FatsPercentage = vm.FatsPercentage / 100;
                vm.RefData.User.NotifyProgressBars();
                vm.RefData.User.NotifyTargetValues();
                //vm.InitProperties();
                InitChart();

                await App.DataBaseRepo.UpdateTypeOfRegimeItemAsync(vm.RefData.User.SelectedTypeOfRegime);
            }
        }
    }
}
