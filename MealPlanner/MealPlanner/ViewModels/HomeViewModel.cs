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

            User = new User() { Age = 32, Height = 180, Weight = 69, TargetCalories = 2986, TargetProteins = 300, TargetCarbs = 323, TargetFats = 89 };


            DayMeals = new ObservableCollection<DayMeal>();
            DayMeals.CollectionChanged += DayMeals_CollectionChanged;

            // Breakfast
            var breakfast = new DayMeal() { Name = "Breakfast", Order = 1, Calories = 756, Proteins = 53, Carbs = 198, Fats = 26 };
            breakfast.Aliments.Add(new Meal() { Name = "Tortilla Wraps", Calories = 756, Proteins = 53, Carbs = 198, Fats = 26 });
            breakfast.Aliments.Add(new Food() { Name = "Skim milk", Calories = 136, Proteins = 12, Carbs = 0, Fats = 4 });
            DayMeals.Add(breakfast);

            // Lunch
            var lunch = new DayMeal() { Name = "Lunch", Order = 2, Calories = 756, Proteins = 53, Carbs = 198, Fats = 26 };
            lunch.Aliments.Add(new Food() { Name = "Egg", Calories = 116, Proteins = 6, Carbs = 0, Fats = 5 });
            DayMeals.Add(lunch);

            // Other
            DayMeals.Add(new DayMeal() { Name = "Dinner", Order = 2 });
            DayMeals.Add(new DayMeal() { Name = "Snack", Order = 3 });
        }

        private void DayMeals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DaylyProteins += (e.NewItems[0] as DayMeal).Proteins;
            DaylyCarbs += (e.NewItems[0] as DayMeal).Carbs;
            DaylyFats += (e.NewItems[0] as DayMeal).Fats;
        }

        public User User { get; set; }  

        public ObservableCollection<DayMeal> DayMeals { get; set; }

        public double DaylyCalories { get; set; }
        public double DaylyCaloriesProgress { get; set; }


        private double daylyProteins;
        public double DaylyProteins
        {
            get { return daylyProteins; }
            set
            {
                daylyProteins = value;
                DaylyProteinProgress = daylyProteins / User.TargetProteins;
            }
        }
        public double DaylyProteinProgress { get; set; }


        private double daylyCarbs;
        public double DaylyCarbs
        {
            get { return daylyCarbs; }
            set
            {
                daylyCarbs = value;
                DaylyCarbsProgress = daylyCarbs / User.TargetCarbs; 
            }
        }
        public double DaylyCarbsProgress { get; set; }


        private double daylyFats;
        public double DaylyFats
        {
            get { return daylyFats; }
            set
            {
                daylyFats = value;
                DaylyFatsProgress = daylyFats / User.TargetFats;
            }
        }
        public double DaylyFatsProgress { get; set; }

    }
}