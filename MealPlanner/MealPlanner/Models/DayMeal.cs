using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Models
{
    public class DayMeal : BaseModel
    {
        public DayMeal()
        {
            Aliments = new ObservableCollection<Aliment>();
            Aliments.CollectionChanged += Aliments_CollectionChanged;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        public int Order { get; set; }

        private double calories;
        [Ignore]
        public double Calories { get { return calories; } set { calories = value; OnPropertyChanged("Calories"); } }

        private double proteins;
        [Ignore]
        public double Proteins { get { return proteins; } set { proteins = value; OnPropertyChanged("Proteins"); } }

        private double carbs;
        [Ignore]
        public double Carbs { get { return carbs; } set { carbs = value; OnPropertyChanged("Carbs"); } }

        private double fats;
        [Ignore]
        public double Fats { get { return fats; } set { fats = value; OnPropertyChanged("Fats"); } }

        [Ignore]
        public ObservableCollection<Aliment> Aliments { get; set; }

        private void Aliments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var newItem = e.NewItems[0] as Aliment;

                Calories += newItem.Calories;
                Proteins += newItem.Proteins;
                Carbs += newItem.Carbs;
                Fats += newItem.Fats;
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var oldItem = e.OldItems[0] as Aliment;

                Calories -= oldItem.Calories;
                Proteins -= oldItem.Proteins;
                Carbs -= oldItem.Carbs;
                Fats -= oldItem.Fats;
            }
        }
    }
}
