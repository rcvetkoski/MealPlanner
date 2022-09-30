using MealPlanner.Helpers.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Models
{
    public class Meal : IAliment, INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private string description;
        public string Description { get { return description; } set { description = value; OnPropertyChanged("Description"); } }

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
        public double ServingSize { get { return servingSize; } set { servingSize = value; OnPropertyChanged("ServingSize"); } }
        public int DayMealAlimentID { get; set; } = 0;

        private AlimentUnitEnum unit;
        public AlimentUnitEnum Unit { get { return unit; } set { unit = value; OnPropertyChanged("AlimentUnitEnum"); } }
        public AlimentTypeEnum AlimentType { get { return AlimentTypeEnum.Meal; } }


        private ObservableCollection<Food> food;
        [Ignore]
        public ObservableCollection<Food> Foods { get { return food; } set { food = value; OnPropertyChanged("Foods"); } }

        public Meal()
        {
            Foods = new ObservableCollection<Food>();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
