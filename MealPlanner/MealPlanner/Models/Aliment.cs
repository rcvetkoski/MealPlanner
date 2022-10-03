using MealPlanner.Helpers.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Models
{
    public class Aliment : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private double calories;
        public double Calories { get { return calories; } set { calories = value; OnPropertyChanged("Calories"); } }

        private double proteins;
        public double Proteins { get { return proteins; } set { proteins = value; OnPropertyChanged("Proteins"); } }

        private double carbs;
        public double Carbs { get { return carbs; } set { carbs = value; OnPropertyChanged("Carbs"); } }

        private double fats;
        public double Fats { get { return fats; } set { fats = value; OnPropertyChanged("Fats"); } }
        public double OriginalServingSize { get; set; }

        private double servingSize;
        public double ServingSize
        {
            get { return servingSize; }
            set
            {
                servingSize = value; OnPropertyChanged("ServingSize");
            }
        }
        public int DayMealAlimentID { get; set; } = 0;
        public AlimentTypeEnum AlimentType { get { return AlimentTypeEnum.Food; } }
        public AlimentUnitEnum Unit { get; set; }
    }
}
