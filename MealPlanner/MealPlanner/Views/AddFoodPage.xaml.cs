using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Controls;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFoodPage : ContentPage
    {
        public AddFoodPage()
        {
            InitializeComponent();
        }

        public AddFoodPage(Meal currentMeal)
        {
            InitializeComponent();
            (BindingContext as AddFoodViewModel).CurrentMeal = currentMeal; 
            (BindingContext as AddFoodViewModel).MealSwitchVisibility = false;
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

        private void CollectionView_SelectionChanged(object sender, EventArgs e)
        {
            IAliment existingAliment = (sender as Grid).BindingContext as IAliment;
            //bool answer = await Application.Current.MainPage.DisplayAlert(aliment.Name, aliment.Proteins + " p" + " " + aliment.Calories + " cal", "Add", "Close");

            RSPopup rSPopup = new RSPopup();
            rSPopup.SetTitle(existingAliment.Name);

            this.RSPopupCustomView2.BindingContext = new AlimentPopUpViewModel(existingAliment);
            rSPopup.SetCustomView(this.RSPopupCustomView2);
            rSPopup.AddAction("Cancel", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive);
            rSPopup.AddAction("Add", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive, new Command(async () => 
            {
                var vm = this.BindingContext as AddFoodViewModel;

                if (vm.MealSwitchVisibility)
                {
                    IAliment aliment = vm.RefData.CreateAndCopyAlimentProperties(existingAliment);
                    aliment.ServingSize = (this.RSPopupCustomView2.BindingContext as AlimentPopUpViewModel).AlimentServingSize;

                    vm.SelectedMealFood.Aliments.Add(aliment);
                    vm.RefData.DaylyProteins += aliment.Proteins;
                    vm.RefData.DaylyCarbs += aliment.Carbs;
                    vm.RefData.DaylyFats += aliment.Fats;
                    vm.RefData.DaylyCalories += aliment.Calories;

                    DayMealAliment dayMealAliment = new DayMealAliment();
                    dayMealAliment.DayMealId = vm.SelectedMealFood.Id;
                    dayMealAliment.AlimentId = aliment.Id;
                    dayMealAliment.ServingSize = (this.RSPopupCustomView2.BindingContext as AlimentPopUpViewModel).AlimentServingSize;
                    dayMealAliment.AlimentType = aliment.AlimentType;

                    await App.DataBaseRepo.AddDayMealAlimentAsync(dayMealAliment);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    IAliment aliment = vm.RefData.CreateAndCopyAlimentProperties(existingAliment);
                    aliment.ServingSize = (this.RSPopupCustomView2.BindingContext as AlimentPopUpViewModel).AlimentServingSize;

                    MealFood mealFood = new MealFood();
                    mealFood.MealId = vm.CurrentMeal.Id;
                    mealFood.FoodId = aliment.Id;
                    mealFood.ServingSize = aliment.ServingSize;

                    vm.CurrentMeal.Foods.Add(aliment as Food);
                    vm.CurrentMeal.Calories += aliment.Calories;
                    vm.CurrentMeal.Proteins += aliment.Proteins;
                    vm.CurrentMeal.Carbs += aliment.Carbs;
                    vm.CurrentMeal.Fats += aliment.Fats;

                    await App.DataBaseRepo.AddMealFoodAsync(mealFood);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }

            }));
            rSPopup.Show();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if((BindingContext as AddFoodViewModel).FilteredAliments != null)
                (BindingContext as AddFoodViewModel).FilteredAlimentsRefresh();
        }
    }
}