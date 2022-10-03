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


            AlimentPopUpViewModel rsPopupBindingContext;
            RSPopupAlimentDetailPage rSPopupAlimentDetailPage = new RSPopupAlimentDetailPage();
            rSPopupAlimentDetailPage.BindingContext = new AlimentPopUpViewModel(existingAliment);
            rsPopupBindingContext = rSPopupAlimentDetailPage.BindingContext as AlimentPopUpViewModel;

            rSPopup.SetCustomView(rSPopupAlimentDetailPage);

            // Add
            rSPopup.AddAction("Add", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral, new Command(async () => 
            {
                var vm = this.BindingContext as AddFoodViewModel;

                if (vm.MealSwitchVisibility)
                {
                    var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                    IAliment aliment = vm.RefData.CreateAndCopyAlimentProperties(existingAliment, ratio);

                    var lastDayMealId = (BindingContext as AddFoodViewModel).RefData.DayMealAliments.OrderByDescending(x=> x.Id).FirstOrDefault();
                    if (lastDayMealId != null)
                        aliment.DayMealAlimentID = lastDayMealId.Id + 1;
                    else
                        aliment.DayMealAlimentID = 1;

                    aliment.ServingSize = rsPopupBindingContext.AlimentServingSize;


                    vm.SelectedMealFood.Aliments.Add(aliment);
                    vm.RefData.DaylyProteins += rsPopupBindingContext.AlimentProteins;
                    vm.RefData.DaylyCarbs += rsPopupBindingContext.AlimentCarbs;
                    vm.RefData.DaylyFats += rsPopupBindingContext.AlimentFats;
                    vm.RefData.DaylyCalories += rsPopupBindingContext.AlimentCalories;

                    DayMealAliment dayMealAliment = new DayMealAliment();
                    dayMealAliment.DayMealId = vm.SelectedMealFood.Id;
                    dayMealAliment.AlimentId = aliment.Id;
                    dayMealAliment.ServingSize = rsPopupBindingContext.AlimentServingSize;
                    dayMealAliment.AlimentType = aliment.AlimentType;

                    await App.DataBaseRepo.AddDayMealAlimentAsync(dayMealAliment);
                }
                else
                {
                    var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                    IAliment aliment = vm.RefData.CreateAndCopyAlimentProperties(existingAliment, ratio);
                    aliment.ServingSize = rsPopupBindingContext.AlimentServingSize;

                    MealFood mealFood = new MealFood();
                    mealFood.MealId = vm.CurrentMeal.Id;
                    mealFood.FoodId = aliment.Id;
                    mealFood.ServingSize = aliment.ServingSize;

                    vm.CurrentMeal.Foods.Add(aliment as Food);
                    vm.CurrentMeal.Calories += aliment.Calories;
                    vm.CurrentMeal.Proteins += aliment.Proteins;
                    vm.CurrentMeal.Carbs += aliment.Carbs;
                    vm.CurrentMeal.Fats += aliment.Fats;

                    // TODO
                    await App.DataBaseRepo.AddMealFoodAsync(mealFood);
                }

                rSPopup.Close();
                await Application.Current.MainPage.Navigation.PopAsync();
            }));

            // Edit
            rSPopup.AddAction("Edit", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive, new Command(() =>
            {
                if (existingAliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                {
                    FoodPage foodPage = new FoodPage();
                    var foodPageBindingContext = foodPage.BindingContext as FoodViewModel;

                    // Fill informations
                    foodPageBindingContext.IsNew = false;
                    foodPageBindingContext.Id = existingAliment.Id;
                    foodPageBindingContext.Name = existingAliment.Name;
                    foodPageBindingContext.ServingSize = existingAliment.ServingSize;
                    foodPageBindingContext.Unit = existingAliment.Unit;
                    foodPageBindingContext.Proteins = existingAliment.Proteins;
                    foodPageBindingContext.Carbs = existingAliment.Carbs;
                    foodPageBindingContext.Fats = existingAliment.Fats;
                    foodPageBindingContext.Calories = existingAliment.Calories;


                    App.Current.MainPage.Navigation.PushAsync(foodPage);
                }
                else
                {
                    MealPage mealPage = new MealPage();
                    var mealPageBindingContext = mealPage.BindingContext as MealViewModel;

                    // Fill informations
                    mealPageBindingContext.IsNew = false;
                    mealPageBindingContext.CurrentMeal = existingAliment as Meal;

                    App.Current.MainPage.Navigation.PushAsync(mealPage);
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