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
    public partial class WorkoutJournalPage : ContentPage
    {
        public WorkoutJournalPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = (BindingContext as WorkoutJournalViewModel);
            vm.SetTitle();
            datePicker.DateSelected += DateSelected;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            datePicker.DateSelected -= DateSelected;
        }

        private void DateSelected(object sender, DateChangedEventArgs e)
        {
            var vm = (BindingContext as WorkoutJournalViewModel);
            vm.SetTitle();
            vm.RefData.GetMealsAtDate(e.NewDate);
            vm.RefData.GetWorkoutAtDay(e.NewDate);
            vm.RefData.UpdateDailyValues();
        }
    }
}