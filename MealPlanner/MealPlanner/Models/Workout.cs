using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MealPlanner.Models
{
    public class Workout : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Ignore]
        public ObservableCollection<Exercice> Exercices { get; set; }

        [Ignore]
        public TimeSpan TotalTime { get; set; }
        [Ignore]
        public double TotalVolume { get; set; }
        [Ignore]
        public List<MuscleGroup> MuscleGroups { get; set; }

        public void SetAndNotifyMainProperties()
        {
            MuscleGroups = new List<MuscleGroup>();
            TotalTime = new TimeSpan();
            TotalVolume = 0;

            foreach (Exercice exercice in Exercices)
            {
                TotalTime = TotalTime.Add(exercice.RestTimeBetweenSets);
                TotalVolume += exercice.TotalWeight;

                if (!MuscleGroups.Contains(exercice.MuscleGroup))
                    MuscleGroups.Add(exercice.MuscleGroup);
            }

            OnPropertyChanged(nameof(TotalTime));
            OnPropertyChanged(nameof(TotalVolume));
            OnPropertyChanged(nameof(MuscleGroups));
        }

        public Workout()
        {
            Exercices = new ObservableCollection<Exercice>();
        }
    }
}
