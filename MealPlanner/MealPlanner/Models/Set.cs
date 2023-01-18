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
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int ExerciceId { get; set; }
    }
}
