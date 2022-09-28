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
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
        }

        public ICommand DeletteAlimentCommand { get; set; } 
        private void DeletteAliment(object[] objects)
        {
            if (objects == null)
                return;

            if (objects.Count() < 2)
                return;

            if (!(objects[0] is DayMeal) || !(objects[1] is IAliment))
                return;


            DayMeal dayMeal = objects[0] as DayMeal;
            IAliment aliment = objects[1] as IAliment;

            DayMealAliment dayMealAliment = App.DataBaseRepo.GetDayMealAlimentsAsync(aliment.AlimentType, dayMeal.Id, aliment.Id).Result.FirstOrDefault();

            if(dayMealAliment != null)
                App.DataBaseRepo.DeleteDayMealAlimentAsync(dayMealAliment);

            dayMeal.Aliments.Remove(aliment);

            RefData.DaylyProteins -= aliment.Proteins;
            RefData.DaylyCarbs -= aliment.Carbs;
            RefData.DaylyFats -= aliment.Fats;
            RefData.DaylyCalories -= aliment.Calories;
        }
    }
}