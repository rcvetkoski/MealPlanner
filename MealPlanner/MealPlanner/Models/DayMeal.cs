using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Models
{
    public class DayMeal : BaseModel
    {
        public DayMeal()
        {
            Aliments = new ObservableCollection<Aliment>();
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        public int Order { get; set; }

        private double calories;
        [Ignore]
        public double Calories { get { return calories; } set { calories = value; OnPropertyChanged("Calories"); OnPropertyChanged("CaloriesString"); } }

        private double proteins;
        [Ignore]
        public double Proteins { get { return proteins; } set { proteins = value; OnPropertyChanged("Proteins"); OnPropertyChanged("NutritionValuesString"); } }

        private double carbs;
        [Ignore]
        public double Carbs { get { return carbs; } set { carbs = value; OnPropertyChanged("Carbs"); OnPropertyChanged("NutritionValuesString"); } }

        private double fats;
        [Ignore]
        public double Fats { get { return fats; } set { fats = value; OnPropertyChanged("Fats"); OnPropertyChanged("NutritionValuesString"); } }

        [Ignore]
        public ObservableCollection<Aliment> Aliments { get; set; }

        public string NutritionValuesString { get { return $"P: {Math.Round(Proteins, 2)},  C: {Math.Round(Carbs, 2)},  F: {Math.Round(Fats, 2)}"; } }
        [Ignore]
        public string CaloriesString { get { return $"{Math.Round(Calories, 2)} Kcal"; } }
    }
}
