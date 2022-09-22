using MealPlanner.Helpers.Extensions;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Helpers
{
    public class ReferentialData
    {
        public User User { get; set; }
        public ObservableCollection<DayMeal> DayMeals { get; set; }
        public ObservableCollection<Food> Foods { get; set; }


        public ReferentialData()
        {
            InitDB();
        }

        private void InitDB()
        {
            try
            {
                User =  App.DataBaseRepo.GetUserAsync().Result;
            }
            catch(Exception ex)
            {
                User = new User() { Age = 32, Height = 180, Weight = 69, TargetCalories = 2986, TargetProteins = 300, TargetCarbs = 323, TargetFats = 89 };
            }


            Foods = App.DataBaseRepo.GetAllFoodsAsync().Result.ToObservableCollection();
        }
    }
}
