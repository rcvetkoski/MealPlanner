using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Models
{
    public class WorkoutProgram
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 

        public string Name { get; set; }

        [Ignore]
        public ObservableCollection<WorkoutRoutine> WorkoutRoutines { get; set; }

        public WorkoutProgram()
        {
            WorkoutRoutines = new ObservableCollection<WorkoutRoutine>();
        }
    }
}
