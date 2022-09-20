using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void EditMeal_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FoodPage());
        }
    }
}