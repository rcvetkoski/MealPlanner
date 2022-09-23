using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class DayMealAliment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public int DayMealId { get; set; }
        public int AlimentId { get; set; }
    }
}
