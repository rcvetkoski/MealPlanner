using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class DayMeal
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }

        public List<IAliment> Aliments { get; set; }    

    }
}
