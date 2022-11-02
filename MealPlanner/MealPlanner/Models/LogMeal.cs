using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class LogMeal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public int LogId { get; set; }
        public int MealId { get; set; } 
    }
}
