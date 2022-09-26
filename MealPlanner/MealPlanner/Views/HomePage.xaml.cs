using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
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
            var aliment = (sender as Xamarin.Forms.ImageButton).BindingContext as IAliment;

            if(aliment is Meal)
            {
                Navigation.PushAsync(new MealPage());
            }
            else if(aliment is Food)
                Navigation.PushAsync(new FoodPage());
        }

        private void AddFood_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddFoodPage((e as TappedEventArgs).Parameter as DayMeal));
        }
    }
}