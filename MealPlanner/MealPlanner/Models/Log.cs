using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class Log
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public DateTime Date { get; set; }    
        public double UserWeight { get; set; }  
        public double UserBodyFat { get; set; }
        public int WorkoutId { get; set; }

        [Ignore]
        public List<Meal> Meals { get; set; }
        [Ignore]
        public Workout Workout { get; set; }
    }
}
