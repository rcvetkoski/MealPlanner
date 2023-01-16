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
    public partial class FoodPage : ContentPage
    {
        public FoodPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as FoodViewModel;
            if (vm.AlimentToUpdate != null)
                vm.InitProperties(vm.AlimentToUpdate);
            else
                vm.InitProperties(vm.CurrentAliment);
        }
    }
}