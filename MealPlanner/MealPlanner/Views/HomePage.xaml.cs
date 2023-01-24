using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Controls;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            var vm = (BindingContext as HomeViewModel);
            vm.SelectedJournalTemplateDayOfWeek = -1;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (BindingContext as HomeViewModel);

            datePicker.DateSelected += DateSelected;
            vm.SetTitle();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            datePicker.DateSelected -= DateSelected;
        }

        private void DateSelected(object sender, DateChangedEventArgs e)
        {
            var vm = (BindingContext as HomeViewModel);
            vm.SetTitle();
            vm.RefData.GetMealsAtDate(e.NewDate);
            vm.RefData.GetWorkoutAtDay(e.NewDate);
            vm.RefData.UpdateDailyValues();
        }
    }
}