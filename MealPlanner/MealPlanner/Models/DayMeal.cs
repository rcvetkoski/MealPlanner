using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Models
{
    public class DayMeal : INotifyPropertyChanged
    {
        public DayMeal()
        {
            Aliments = new ObservableCollection<IAliment>();
            Aliments.CollectionChanged += Aliments_CollectionChanged;
        }

        private void Aliments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                Calories += (e.NewItems[0] as IAliment).Calories;
                Proteins += (e.NewItems[0] as IAliment).Proteins;
                Carbs += (e.NewItems[0] as IAliment).Carbs;
                Fats += (e.NewItems[0] as IAliment).Fats;
            }
            else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                Calories -= (e.OldItems[0] as IAliment).Calories;
                Proteins -= (e.OldItems[0] as IAliment).Proteins;
                Carbs -= (e.OldItems[0] as IAliment).Carbs;
                Fats -= (e.OldItems[0] as IAliment).Fats;
            }
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
        public ObservableCollection<IAliment> Aliments { get; set; }



        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
