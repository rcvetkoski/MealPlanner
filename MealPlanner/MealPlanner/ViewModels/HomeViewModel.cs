using MealPlanner.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace MealPlanner.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Title = "Home";

            //User = new User() { Age = 32, Height = 180, Weight = 69, TargetCalories = 2986, TargetProteins = 300, TargetCarbs = 323, TargetFats = 89 };


            RefData.DayMeals = new ObservableCollection<DayMeal>();
            RefData.DayMeals.CollectionChanged += DayMeals_CollectionChanged;

            // Breakfast
            var breakfast = new DayMeal() { Name = "Breakfast", Order = 1 };
            breakfast.Aliments.Add(new Meal() { Name = "Tortilla Wraps", Calories = 756, Proteins = 53, Carbs = 198, Fats = 26 });
            breakfast.Aliments.Add(new Food() { Name = "Skim milk", Calories = 136, Proteins = 12, Carbs = 0, Fats = 4 });
            RefData.DayMeals.Add(breakfast);

            // Lunch
            var lunch = new DayMeal() { Name = "Lunch", Order = 2 };
            lunch.Aliments.Add(new Food() { Name = "Egg", Calories = 116, Proteins = 6, Carbs = 1, Fats = 5 });
            RefData.DayMeals.Add(lunch);

            // Other
            RefData.DayMeals.Add(new DayMeal() { Name = "Dinner", Order = 2 });
            RefData.DayMeals.Add(new DayMeal() { Name = "Snack", Order = 3 });
        }

        private void DayMeals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefData.DaylyProteins += (e.NewItems[0] as DayMeal).Proteins;
            RefData.DaylyCarbs += (e.NewItems[0] as DayMeal).Carbs;
            RefData.DaylyFats += (e.NewItems[0] as DayMeal).Fats;
            RefData.DaylyCalories += (e.NewItems[0] as DayMeal).Calories;
        }
    }
}