using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Models
{
    public class WorkoutWeek
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        [Ignore]
        public ObservableCollection<Workout> Workouts { get; set; } 

        public WorkoutWeek()
        {
            Workouts = new ObservableCollection<Workout>(); 
        }
    }
}
