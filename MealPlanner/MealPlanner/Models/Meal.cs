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
    public class Meal : Aliment
    {
        private ObservableCollection<Food> food;
        [Ignore]
        public ObservableCollection<Food> Foods { get { return food; } set { food = value; OnPropertyChanged("Foods"); } }


        public Meal()
        {
            Foods = new ObservableCollection<Food>();
        }
    }
}
