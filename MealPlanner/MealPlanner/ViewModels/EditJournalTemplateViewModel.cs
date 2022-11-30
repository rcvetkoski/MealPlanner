using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class EditJournalTemplateViewModel : BaseViewModel
    {
        public ObservableCollection<Meal> Meals { get; set; }

        public EditJournalTemplateViewModel()
        {
            Title = "Monday";
            Meals = new ObservableCollection<Meal>();
            AddAlimentCommand = new Command<Meal>(AddAliment);
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
        }

        private AddAlimentPage addAlimentPage;
        public ICommand AddAlimentCommand { get; set; }
        private async void AddAliment(Meal meal)
        {
            if (addAlimentPage == null)
                addAlimentPage = new AddAlimentPage();

            (addAlimentPage.BindingContext as AddAlimentViewModel).SelectedMeal = meal;
            await App.Current.MainPage.Navigation.PushAsync(addAlimentPage);
        }

        public ICommand DeletteAlimentCommand { get; set; }
        private async void DeletteAliment(object[] objects)
        {
            if (objects == null)
                return;

            if (objects.Count() < 2)
                return;

            if (!(objects[0] is Meal) || !(objects[1] is Aliment))
                return;


            Meal meal = objects[0] as Meal;
            Aliment aliment = objects[1] as Aliment;

            MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(aliment.MealAlimentId);

            if (mealAliment != null)
            {
                await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                var realMealAliment = RefData.MealAliments.FirstOrDefault(x => x.Id == mealAliment.Id);
                if (realMealAliment != null)
                    RefData.MealAliments.Remove(realMealAliment);
            }

            meal.Aliments.Remove(aliment);

            // Update meal values
            RefData.UpdateMealValues(meal);

            // Update daily values
            RefData.UpdateDailyValues();
        }
    }
}
