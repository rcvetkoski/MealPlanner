using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Models
{
    public class Workout : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Ignore]
        public ObservableCollection<Exercice> Exercices { get; set; }


        public Workout()
        {
            Exercices = new ObservableCollection<Exercice>();
        }
    }
}
