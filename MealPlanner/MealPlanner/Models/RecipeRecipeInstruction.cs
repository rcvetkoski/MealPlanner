using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class RecipeRecipeInstruction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int RecipeInstructionId { get; set; }
    }
}
