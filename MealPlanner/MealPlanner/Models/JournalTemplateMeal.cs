using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class JournalTemplateMeal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int JournalTemplateId { get; set; }
        public int MealId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
