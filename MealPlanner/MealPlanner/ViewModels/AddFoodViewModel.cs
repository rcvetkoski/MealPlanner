using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class AddFoodViewModel : BaseViewModel
    {
        public AddFoodViewModel()
        {
            Title = "AddFood";
            MealSwitchVisibility = true;
            IsFoodChecked = true;
            searchResults = new List<Food>();
            foreach (var item in RefData.Foods)
                searchResults.Add(item);
        }

        private bool mealSwitchVisibility;
        public bool MealSwitchVisibility { get { return mealSwitchVisibility; } set { mealSwitchVisibility = value; OnPropertyChanged("MealSwitchVisibility"); } }

        public DayMeal SelectedMealFood { get; set; }
        public Meal CurrentMeal { get; set; }

        private bool isFoodChecked;
        public bool IsFoodChecked { get { return isFoodChecked; } set { isFoodChecked = value; OnPropertyChanged("IsFoodChecked"); } }


        private bool isMealChecked;
        public bool IsMealChecked { get { return isMealChecked; } set { isMealChecked = value; OnPropertyChanged("IsMealChecked"); } }


        public bool isLibraryChecked;
        public bool IsLibraryChecked { get { return isLibraryChecked; } set { isLibraryChecked = value; OnPropertyChanged("IsLibraryChecked"); } }


        public ICommand PerformSearch => new Command<string>((string query) =>
        {
            if(string.IsNullOrEmpty(query))
            {
                foreach (var item in RefData.Foods)
                    searchResults.Add(item);
            }
            else
                SearchResults = RefData.Foods.Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();
        });


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


        public ICommand SelectedAlimentCommand => new Command(async(object parameter) =>
        {
            IAliment aliment = parameter as IAliment;
            bool answer = await Application.Current.MainPage.DisplayAlert(aliment.Name, aliment.Proteins + " p" + " " + aliment.Calories + " cal", "Add", "Close");

            if(answer && MealSwitchVisibility)
            {
                SelectedMealFood.Aliments.Add(aliment);
                RefData.DaylyCalories += aliment.Calories;
                RefData.DaylyProteins += aliment.Proteins;
                RefData.DaylyCarbs += aliment.Carbs;
                RefData.DaylyFats += aliment.Fats;

                DayMealAliment dayMealAliment = new DayMealAliment();
                dayMealAliment.DayMealId = SelectedMealFood.Id;
                dayMealAliment.AlimentId = aliment.Id;

                await App.DataBaseRepo.AddDayMealAlimentAsync(dayMealAliment);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else if(answer)
            {
                CurrentMeal.Foods.Add(aliment as Food);
                CurrentMeal.Calories += aliment.Calories;
                CurrentMeal.Proteins += aliment.Proteins;
                CurrentMeal.Carbs += aliment.Carbs;
                CurrentMeal.Fats += aliment.Fats;

                await Application.Current.MainPage.Navigation.PopAsync();
            }
        });

        private IAliment selectedAliment;
        public IAliment SelectedAliment { get { return selectedAliment; } set { selectedAliment = value; OnPropertyChanged("SelectedAliment"); } }
    }
}
