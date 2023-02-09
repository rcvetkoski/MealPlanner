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

        private int order;
        public int Order 
        {
            get
            {
                return order;
            }
            set
            {
                if(order != value)
                {
                    order = value;  
                    OnPropertyChanged(nameof(Order));
                }
            }
        }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int WorkoutExerciceId { get; set; }
    }
}
