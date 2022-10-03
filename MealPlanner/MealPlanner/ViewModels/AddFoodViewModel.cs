using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;

namespace MealPlanner.ViewModels
{
    public class AddAlimentViewModel : BaseViewModel
    {
        public AddAlimentViewModel()
        {
            Title = "AddFood";
            MealSwitchVisibility = true;
            IsFoodChecked = true;
            searchResults = new List<Food>();
            foreach (var item in RefData.Foods)
                searchResults.Add(item);

            CreateFoodCommand = new Command(CreateFood);
            SelectAlimentCommand = new Command<Aliment>(SelectAliment);
            CreateMealCommand = new Command(CreateMeal);

            FilteredAliments = new ObservableCollection<Aliment>();
            FilteredAlimentsRefresh();
        }


        public ObservableCollection<Aliment> FilteredAliments { get; set; } 
        public void FilteredAlimentsRefresh()
        {
            FilteredAliments.Clear();

            foreach (Aliment aliment in RefData.Aliments)
            {
                if(IsMealChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Meal)
                    FilteredAliments.Add(aliment);
                else if(!IsMealChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                    FilteredAliments.Add(aliment);
            }
        }

        private bool mealSwitchVisibility;
        public bool MealSwitchVisibility { get { return mealSwitchVisibility; } set { mealSwitchVisibility = value; OnPropertyChanged("MealSwitchVisibility"); } }

        public DayMeal SelectedMealFood { get; set; }
        public Meal CurrentMeal { get; set; }

        private bool isFoodChecked;
        public bool IsFoodChecked { get { return isFoodChecked; } set { isFoodChecked = value; OnPropertyChanged("IsFoodChecked"); } }

        private bool isMealChecked;
        public bool IsMealChecked { get { return isMealChecked; } set { isMealChecked = value; OnPropertyChanged("IsMealChecked"); FilteredAlimentsRefresh(); } }

        public bool isLibraryChecked;
        public bool IsLibraryChecked { get { return isLibraryChecked; } set { isLibraryChecked = value; OnPropertyChanged("IsLibraryChecked"); } }



        private List<Food> searchResults;
        public List<Food> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }


        public ICommand SelectAlimentCommand { get;set; }
        private void SelectAliment(Aliment existingAliment)
        {
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
                if (MealSwitchVisibility)
                {
                    var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                    Aliment aliment = RefData.CreateAndCopyAlimentProperties(existingAliment, ratio);

                    var lastDayMealId = RefData.DayMealAliments.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (lastDayMealId != null)
                        aliment.DayMealAlimentID = lastDayMealId.Id + 1;
                    else
                        aliment.DayMealAlimentID = 1;

                    aliment.ServingSize = rsPopupBindingContext.AlimentServingSize;


                    SelectedMealFood.Aliments.Add(aliment);
                    RefData.DaylyProteins += rsPopupBindingContext.AlimentProteins;
                    RefData.DaylyCarbs += rsPopupBindingContext.AlimentCarbs;
                    RefData.DaylyFats += rsPopupBindingContext.AlimentFats;
                    RefData.DaylyCalories += rsPopupBindingContext.AlimentCalories;

                    DayMealAliment dayMealAliment = new DayMealAliment();
                    dayMealAliment.DayMealId = SelectedMealFood.Id;
                    dayMealAliment.AlimentId = aliment.Id;
                    dayMealAliment.ServingSize = rsPopupBindingContext.AlimentServingSize;
                    dayMealAliment.AlimentType = aliment.AlimentType;

                    await App.DataBaseRepo.AddDayMealAlimentAsync(dayMealAliment);
                }
                else
                {
                    var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                    Aliment aliment = RefData.CreateAndCopyAlimentProperties(existingAliment, ratio);
                    aliment.ServingSize = rsPopupBindingContext.AlimentServingSize;

                    MealFood mealFood = new MealFood();
                    mealFood.MealId = CurrentMeal.Id;
                    mealFood.FoodId = aliment.Id;
                    mealFood.ServingSize = aliment.ServingSize;

                    CurrentMeal.Foods.Add(aliment as Food);
                    CurrentMeal.Calories += aliment.Calories;
                    CurrentMeal.Proteins += aliment.Proteins;
                    CurrentMeal.Carbs += aliment.Carbs;
                    CurrentMeal.Fats += aliment.Fats;

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


        public ICommand CreateFoodCommand { get; set; }
        private void CreateFood()
        {
            App.Current.MainPage.Navigation.PushAsync(new FoodPage());
        }

        public ICommand CreateMealCommand { get; set; }
        private void CreateMeal()
        {
            App.Current.MainPage.Navigation.PushAsync(new MealPage());
        }
    }
}
