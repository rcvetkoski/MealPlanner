using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.ComponentModel;
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

        private void EditMeal_Clicked(object sender, EventArgs e)
        {
            var aliment = (sender as Xamarin.Forms.ImageButton).BindingContext as IAliment;

            if(aliment is Meal)
            {
                RSPopup rSPopup = new RSPopup();
                rSPopup.SetTitle(aliment.Name);
                this.RSPopupCustomView.BindingContext = aliment;
                rSPopup.SetCustomView(this.RSPopupCustomView);
                rSPopup.AddAction("Update", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral);
                rSPopup.AddAction("Modify", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive);
                rSPopup.Show();

                //Navigation.PushAsync(new MealPage());
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