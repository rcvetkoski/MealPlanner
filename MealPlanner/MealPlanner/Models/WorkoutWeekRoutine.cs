using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class WorkoutWeekRoutine
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int WorkoutWeekId { get; set; }
        public int WorkoutId { get; set; }
    }
}
