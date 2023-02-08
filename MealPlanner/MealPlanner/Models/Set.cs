using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class Set : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Order { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int WorkoutExerciceId { get; set; }
        public DateTime Date { get; set; }
    }
}
