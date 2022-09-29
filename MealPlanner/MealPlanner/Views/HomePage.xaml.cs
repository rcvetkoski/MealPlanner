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

        private void AddFood_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddFoodPage((e as TappedEventArgs).Parameter as DayMeal));
        }
    }
}