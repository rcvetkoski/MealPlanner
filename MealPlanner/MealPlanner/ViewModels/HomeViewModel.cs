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
            RefData.DayMeals.CollectionChanged += DayMeals_CollectionChanged;
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