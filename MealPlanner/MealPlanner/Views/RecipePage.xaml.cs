﻿using MealPlanner.Models;
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

            var proteinColor = (Color)Application.Current.Resources["ProteinColor"];
            var carbsColor = (Color)Application.Current.Resources["CarbsColor"];
            var fatsColor = (Color)Application.Current.Resources["FatsColor"];

            var entries = new[]
            {
                new ChartEntry((float)Math.Round(vm.AlimentProteinProgress * 100, 0))
                {
                    Label = "Proteins",
                    ValueLabel = $"{Math.Round(vm.AlimentProteinProgress * 100, 0)} %",
                    ValueLabelColor = proteinColor.ToSKColor(),
                    TextColor = proteinColor.ToSKColor(),
                    Color = proteinColor.ToSKColor()
                },
                new ChartEntry((float)Math.Round(vm.AlimentCarbsProgress * 100, 0))
                {
                    Label = "Carbs",
                    ValueLabel = $"{Math.Round(vm.AlimentCarbsProgress * 100, 0)} %",
                    ValueLabelColor = carbsColor.ToSKColor(),
                    TextColor = carbsColor.ToSKColor(),
                    Color = carbsColor.ToSKColor()
                },
                new ChartEntry((float)Math.Round(vm.AlimentFatsProgress * 100, 0))
                {
                    Label = "Fats",
                    ValueLabelColor = fatsColor.ToSKColor(),
                    TextColor = fatsColor.ToSKColor(),
                    ValueLabel = $"{Math.Round(vm.AlimentFatsProgress * 100, 0)} %",
                    Color = fatsColor.ToSKColor()
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