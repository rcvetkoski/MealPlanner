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
        private HomeViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            viewModel = BindingContext as HomeViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //collectionView.ItemsSource = null;
            //collectionView.ItemsSource = (BindingContext as HomeViewModel).RefData.Meals;
        }

        private void DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.SetTitle();
            viewModel.RefData.GetMealsAtDate(e.NewDate);
        }
    }
}