using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    }
}
