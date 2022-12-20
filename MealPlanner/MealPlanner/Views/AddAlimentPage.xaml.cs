using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Controls;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAlimentPage : ContentPage
    {
        public AddAlimentPage()
        {
            InitializeComponent();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            (BindingContext as AddAlimentViewModel).Search(); 

            if(!string.IsNullOrEmpty(e.NewTextValue))
            {
                scanBarCodeButton.IsVisible = false;
                cancelButton.IsVisible = true;
            }
            else
            {
                scanBarCodeButton.IsVisible = true;
                cancelButton.IsVisible = false;
            }
        }


        private async void FilterSwitch(object sender, EventArgs e)
        {
            double x = 0;
            var vm = (BindingContext as AddAlimentViewModel);
            var parent = (sender as VisualElement).Parent as Grid;
            var foodButton = parent.FindByName("foodButton");
            var recipeButton = parent.FindByName("recipeButton");
            //var slider = parent.FindByName("slider") as BoxView;

            if (sender == foodButton)
            {
                x = 0;
                vm.IsRecipeChecked = false;
                vm.IsFoodChecked = true;
                vm.FoodButtonAtributtes = FontAttributes.Bold;
                vm.RecipeButtonAtributtes = FontAttributes.None;
            }
            else
            {
                x = (sender as View).Width;
                vm.IsFoodChecked = false;
                vm.IsRecipeChecked = true;
                vm.FoodButtonAtributtes = FontAttributes.None;
                vm.RecipeButtonAtributtes = FontAttributes.Bold;
            }

            vm.SetTitle();
            //Animation slideAnimation = new Animation((v) => { slider.TranslationX = v; }, slider.TranslationX, x);
            //slideAnimation.Commit(this, "slide", 16, 250, Easing.Linear, (v, c) => { }, () => false);


            //await slider.TranslateTo(x, 0);
            vm.SliderX = x;
        }
    }
}