using MealPlanner.Models;
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
    public partial class AddFoodPage : ContentPage
    {
        public AddFoodPage()
        {
            InitializeComponent();
        }

        public AddFoodPage(DayMeal dayMeal)
        {
            InitializeComponent();
            (BindingContext as AddFoodViewModel).SelectedMealFood = dayMeal;
        }

        private void CreateFood_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FoodPage());
        }

        private void CreateMeal_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MealPage());
        }
    }
}