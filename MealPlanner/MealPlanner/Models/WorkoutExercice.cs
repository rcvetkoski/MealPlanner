using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class WorkoutExercice
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciceId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan RestTimeBetweenSets { get; set; }
    }
}
