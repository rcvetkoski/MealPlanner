using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MealPlanner.Helpers.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Converts list<T> to ObservableCollection<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();
            foreach (T item in list)
                observableCollection.Add(item);

            return observableCollection;
        }
    }
}
