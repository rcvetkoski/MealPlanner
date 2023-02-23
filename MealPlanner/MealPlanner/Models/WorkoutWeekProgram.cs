using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class WorkoutWeekProgram
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int WorkoutProgramId { get; set; }
        public int WorkoutWeekId { get; set; }
    }
}
