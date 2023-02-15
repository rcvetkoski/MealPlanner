using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Models
{
    public class WorkoutRoutine
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [Ignore]
        public ObservableCollection<Exercice> Exercices { get; set; }

        public WorkoutRoutine()
        {
            Exercices = new ObservableCollection<Exercice>();
        }
    }
}
