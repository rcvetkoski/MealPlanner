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
    public class CustomizeMealsViewModel : BaseViewModel
    {
        public ObservableCollection<TemplateMeal> TempTemplateMeals { get; set; }
        private List<TemplateMeal> NewTemplateMeals;
        private List<TemplateMeal> RemovedTemplateMeals;
        public int TempTemplateCount
        {
            get
            {
                return TempTemplateMeals.Count;
            }
            set
            {
                if (value < TempTemplateMeals.Count)
                {
                    RemovedTemplateMeals.Add(TempTemplateMeals.ElementAt(value));
                    TempTemplateMeals.RemoveAt(value);
                }
                else if (value > TempTemplateMeals.Count)
                {
                    if (TempTemplateMeals.Count < RefData.DefaultMeals.Count)
                        TempTemplateMeals.Add(new TemplateMeal() { Name = RefData.DefaultMeals.ElementAt(value - 1).Name, Order = value });
                    else
                        TempTemplateMeals.Add(new TemplateMeal() { Name = $"Meal {value}", Order = value });

                    NewTemplateMeals.Add(TempTemplateMeals.LastOrDefault());
                }
            }
        }

        public CustomizeMealsViewModel()
        {
            Title = "Settings";
            TempTemplateMeals = new ObservableCollection<TemplateMeal>();
            NewTemplateMeals = new List<TemplateMeal>();
            RemovedTemplateMeals = new List<TemplateMeal>();
            SaveCommand = new Command(Save);    

            foreach (TemplateMeal templateMeal in RefData.TemplateMeals)
                TempTemplateMeals.Add(new TemplateMeal() { Name = templateMeal.Name, Order = templateMeal.Order });
        }

        public ICommand SaveCommand { get; set; }
        private void Save()
        {
            foreach (TemplateMeal removed in RemovedTemplateMeals)
            {
                var itemToRemove = RefData.TemplateMeals.Where(x => x.Order == removed.Order).FirstOrDefault();

                if (itemToRemove != null)
                {
                    RefData.TemplateMeals.Remove(itemToRemove);
                    App.DataBaseRepo.DeleteTemplateMealAsync(itemToRemove).Wait();
                }
            }

            foreach (TemplateMeal added in NewTemplateMeals)
            {
                TemplateMeal templateMeal = new TemplateMeal() { Name = added.Name, Order = added.Order };

                App.DataBaseRepo.AddTemplateMealAsync(templateMeal).Wait();
                RefData.TemplateMeals.Add(templateMeal);
            }


            // Update name change if any
            foreach (var item in TempTemplateMeals)
            {
                foreach (Meal meal in RefData.AllMeals.Where(x => x.Order == item.Order))
                {
                    if (item.Name == meal.Name)
                        continue;

                    var originalTemplateMeal = RefData.TemplateMeals.Where(x => x.Order == item.Order).FirstOrDefault();
                    if (originalTemplateMeal == null)
                    {
                        originalTemplateMeal.Name = item.Name;
                        App.DataBaseRepo.UpdateTemplateMealAsync(originalTemplateMeal).Wait();
                    }

                    meal.Name = item.Name;
                    App.DataBaseRepo.UpdateMealAsync(meal).Wait();
                }
            }


            RemovedTemplateMeals.Clear();
            NewTemplateMeals.Clear();
            RefData.GetMealsAtDate(DateTime.Now, DateTime.Now.DayOfWeek);
            RefData.UpdateDailyValues();
            Shell.Current.Navigation.PopAsync();
            //Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}
