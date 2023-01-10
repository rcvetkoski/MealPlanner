using MealPlanner.ViewModels;
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
    public partial class ObjectifsPage : ContentPage
    {
        public ObjectifsPage()
        {
            InitializeComponent();
        }

        private async void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1);

            var slider = (Slider)sender;
            slider.Value = newStep * 1;

            var vm = (BindingContext as ObjectifsViewModel);

            var originalTDEE = Math.Round((vm.RefData.User.BMR * vm.RefData.User.SelectedPhysicalActivityLevel.Ratio * vm.RefData.User.SelectedObjectif.Ratio), 0);


            vm.RefData.User.AdjustedCalories = originalTDEE * (slider.Value / 100);
            vm.RefData.User.TDEE = originalTDEE + vm.RefData.User.AdjustedCalories;
            await App.DataBaseRepo.UpdateUserAsync(vm.RefData.User);
        }
    }
}