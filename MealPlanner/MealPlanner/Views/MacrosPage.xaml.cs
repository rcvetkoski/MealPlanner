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
                new ChartEntry((float)vm.RefData.User.SelectedTypeOfRegime.ProteinPercentage * 100)
                {
                    Label = "Proteins",
                    ValueLabel = (vm.RefData.User.SelectedTypeOfRegime.ProteinPercentage * 100).ToString(),
                    ValueLabelColor = SKColor.Parse("#2c3e50"),
                    TextColor = SKColor.Parse("#2c3e50"),
                    Color = SKColor.Parse("#2c3e50")
                },
                new ChartEntry((float)vm.RefData.User.SelectedTypeOfRegime.CarbsPercentage * 100)
                {
                    Label = "Carbs",
                    ValueLabel = (vm.RefData.User.SelectedTypeOfRegime.CarbsPercentage * 100).ToString(),
                    ValueLabelColor = SKColor.Parse("#77d065"),
                    TextColor = SKColor.Parse("#77d065"),
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry((float)vm.RefData.User.SelectedTypeOfRegime.FatsPercentage * 100)
                {
                    Label = "Fats",
                    ValueLabelColor = SKColor.Parse("#b455b6"),
                    TextColor = SKColor.Parse("#b455b6"),
                    ValueLabel = (vm.RefData.User.SelectedTypeOfRegime.FatsPercentage * 100).ToString(),
                    Color = SKColor.Parse("#b455b6")
                }
            };
            var chart = new DonutChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelMode = LabelMode.RightOnly,
                LabelTextSize = 38

            };
            chartView.Chart = chart;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            InitChart();
        }
    }
}