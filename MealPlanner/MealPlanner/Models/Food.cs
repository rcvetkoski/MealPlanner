using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class Food
    {
        public string Name { get; set; }    
        public double Weight { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
    }
}
