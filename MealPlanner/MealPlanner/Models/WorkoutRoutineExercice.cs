using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class WorkoutRoutineExercice
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ExerciceId { get; set; }
        public int WorkoutRoutineId { get; set; }
    }
}
