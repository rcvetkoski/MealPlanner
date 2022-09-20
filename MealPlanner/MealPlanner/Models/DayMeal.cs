using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Models
{
    public class DayMeal
    {
        public DayMeal()
        {
            Aliments = new ObservableCollection<IAliment>();
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }

        public ObservableCollection<IAliment> Aliments { get; set; }    

    }
}
