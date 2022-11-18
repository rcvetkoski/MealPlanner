using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Models
{
    public class JournalTemplate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        [Ignore]
        public ObservableCollection<DayOfWeekHelper> DaysOfWeek { get; set; }

        public JournalTemplate()
        {
            DaysOfWeek = new ObservableCollection<DayOfWeekHelper>();
            foreach (DayOfWeek dayOfWeek in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
                DaysOfWeek.Add(new DayOfWeekHelper() { DayOfWeek = dayOfWeek });
        }
    }

    public class DayOfWeekHelper
    {
        public DayOfWeekHelper()
        {
            Meals = new ObservableCollection<Meal>();
        }

        public DayOfWeek DayOfWeek { get; set; }

        [Ignore]
        public string Description
        {
            get
            {
                double proteins = 0;
                double carbs = 0;
                double fats = 0;
                calories = 0;

                foreach (Meal meal in Meals)
                {
                    proteins += meal.Proteins;
                    carbs += meal.Carbs;    
                    fats += meal.Fats;  
                    calories += meal.Calories;  
                }

                return $"P: {proteins}, C: {carbs}, F: {fats}";
            }
        }

        private double calories;

        [Ignore]
        public string CaloriesString
        {
            get
            {
                return $"{calories} Kcal";
            }
        }

        public ObservableCollection<Meal> Meals { get; set; }
    }
}
