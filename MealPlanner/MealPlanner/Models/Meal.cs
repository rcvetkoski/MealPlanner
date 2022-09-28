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
        public string Name { get; set; }
        public string Description { get; set; }

        private double calories;
        public double Calories { get { return calories; } set { calories = value; OnPropertyChanged("Calories"); } }

        private double proteins;
        public double Proteins { get { return proteins; } set { proteins = value; OnPropertyChanged("Proteins"); } }

        private double carbs;
        public double Carbs { get { return carbs; } set { carbs = value; OnPropertyChanged("Carbs"); } }

        private double fats;
        public double Fats { get { return fats; } set { fats = value; OnPropertyChanged("Fats"); } }
        public double Portion { get; set; }
        public int NumberOfPortions { get; set; }

        public AlimentUnitEnum Unit { get; set; }

        [Ignore]
        public ObservableCollection<Food> Foods { get; set; }

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
