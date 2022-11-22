using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Helpers
{
    public class CopiedDayHelper
    {
        public HomePageTypeEnum HomePageType { get; set; }
        public DateTime Date { get; set; }
        public List<Meal> Meals { get; set; }
        public int DayOfWeek { get; set; }
    }
}
