using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void EditMeal_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MealPage());
        }

        private void AddFood_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddFoodPage((e as TappedEventArgs).Parameter as DayMeal));
        }
    }
}