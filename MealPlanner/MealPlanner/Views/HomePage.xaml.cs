using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Controls;

namespace MealPlanner.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            datePicker.DateSelected += DateSelected;

            //collectionView.ItemsSource = null;
            //collectionView.ItemsSource = (BindingContext as HomeViewModel).RefData.Meals;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            datePicker.DateSelected -= DateSelected;
        }


        private void DateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as HomeViewModel).SetTitle();
            (BindingContext as HomeViewModel).RefData.GetMealsAtDate(e.NewDate, e.NewDate.DayOfWeek);
        }
    }
}