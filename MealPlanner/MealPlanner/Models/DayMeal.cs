using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Models
{
    public class DayMeal
    {
        public DayMeal()
        {
            Aliments = new ObservableCollection<IAliment>();
            Aliments.CollectionChanged += Aliments_CollectionChanged;
        }

        private void Aliments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Calories += (e.NewItems[0] as IAliment).Calories;
            Proteins += (e.NewItems[0] as IAliment).Proteins;
            Carbs += (e.NewItems[0] as IAliment).Carbs;
            Fats += (e.NewItems[0] as IAliment).Fats;
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }

        public ObservableCollection<IAliment> Aliments { get; set; }    

    }
}
