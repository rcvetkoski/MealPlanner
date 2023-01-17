using MealPlanner.Models;
using MealPlanner.ViewModels;
using Microcharts.Forms;
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
using Xamarin.Essentials;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePage : ContentPage
    {
        public RecipePage()
        {
            InitializeComponent();
        }

        public bool CheckFields()
        { 
            return (nameEntry.CheckIsValid() || servingSizeEntry.CheckIsValid()) == false ? false : true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as RecipeViewModel;
            vm.InitProperties(vm.CurrentAliment);
            InitChart();
        }

        public void InitChart()
        {
            var vm = BindingContext as RecipeViewModel;

            var entries = new[]
            {
                new ChartEntry((float)Math.Round(vm.AlimentProteinProgress * 100, 0))
                {
                    Label = "Proteins",
                    ValueLabel = $"{Math.Round(vm.AlimentProteinProgress * 100, 0)} %",
                    ValueLabelColor = SKColor.Parse("#29aae3"),
                    TextColor = SKColor.Parse("#29aae3"),
                    Color = SKColor.Parse("#29aae3")
                },
                new ChartEntry((float)Math.Round(vm.AlimentCarbsProgress * 100, 0))
                {
                    Label = "Carbs",
                    ValueLabel = $"{Math.Round(vm.AlimentCarbsProgress * 100, 0)} %",
                    ValueLabelColor = SKColor.Parse("#77d065"),
                    TextColor = SKColor.Parse("#77d065"),
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry((float)Math.Round(vm.AlimentFatsProgress * 100, 0))
                {
                    Label = "Fats",
                    ValueLabelColor = SKColor.Parse("#b455b6"),
                    TextColor = SKColor.Parse("#b455b6"),
                    ValueLabel = $"{Math.Round(vm.AlimentFatsProgress * 100, 0)} %",
                    Color = SKColor.Parse("#b455b6")
                }
            };

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Screen density
            var density = mainDisplayInfo.Density;
            var size = (Device.GetNamedSize(NamedSize.Micro, typeof(Label), useOldSizes: false)) * density;

            var chart = new DonutChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelMode = LabelMode.RightOnly,
                LabelTextSize = (float)size
            };
            chartView.Chart = chart;
        }

    }
}