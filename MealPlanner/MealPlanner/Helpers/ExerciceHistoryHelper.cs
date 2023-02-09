using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Helpers
{
    public class ExerciceHistoryHelper
    {
        public ExerciceHistoryHelper()
        {
            Sets = new List<Set>();
        }

        public DateTime Date { get; set; }
        public List<Set> Sets { get; set; }
    }
}
