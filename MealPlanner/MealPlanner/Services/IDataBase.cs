using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public interface IDataBase
    {
        // Log
        Task<Log> GetLogAsync(int id);
        Task<List<Log>> GetAllLogsAsync();
        Task<int> AddLogAsync(Log log);
        Task<int> UpdateLogAsync(Log log);
        Task<int> DropTableLog();

        // LogMeals
        Task<LogMeal> GetLogMealAsync(int id);
        Task<List<LogMeal>> GetAllLogMealsAsync();
        Task<int> AddLogMealAsync(LogMeal logMeal);
        Task<int> UpdateLogMealAsync(LogMeal logMeal);
        Task<int> DropTableLogMeal();


        // User
        Task<User> GetUserAsync();
        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task<int> DropTableUser();


        // Recipe
        Task<Recipe> GetRecipeAsync(int id);
        Task<List<Recipe>> GetAllRecipesAsync();
        Task<int> AddRecipeAsync(Recipe recipe);
        Task<int> UpdateRecipeAsync(Recipe recipe);
        Task<int> DropTableRecipe();


        // Food
        Task<Food> GetFoodAsync(int id);
        Task<List<Food>> GetAllFoodsAsync();
        Task<int> AddFoodAsync(Food food);
        Task<int> UpdateFoodAsync(Food food);
        Task<int> DropTableFood();


        // RecipeFood
        Task<RecipeFood> GetRecipeFoodAsync(int id);
        Task<List<RecipeFood>> GetRecipeFoodsAsync(int recipe_id, int food_id);
        Task<List<RecipeFood>> GetAllRecipeFoodsAsync();
        Task<int> AddRecipeFoodAsync(RecipeFood recipeFood);
        Task<int> UpdateRecipeFood(RecipeFood recipeFood);
        Task<int> DeleteRecipeFoodAsync(RecipeFood recipeFood);
        Task<int> DeleteAllRecipeFoodsAsync();
        Task<int> DropTableRecipeFood();

        //Meal
        Task<Meal> GetMealAsync(int id);
        Task<List<Meal>> GetAllMealsAsync();
        Task<int> AddMealAsync(Meal meal);
        Task<int> UpdateMeal(Meal meal);
        Task<int> DeleteMealAsync(Meal meal);
        Task<int> DeleteAllMealsAsync();
        Task<int> DropTableMeal();


        //MealAliment
        Task<MealAliment> GetMealAlimentAsync(int id);
        Task<List<MealAliment>> GetAllMealAlimentsAsync();
        Task<MealAliment> GetMealAlimentAsync(AlimentTypeEnum alimentType, int meal_id, int aliment_id);
        Task<List<MealAliment>> GetMealAlimentsAsync(AlimentTypeEnum alimentType,int meal_id, int aliment_id);
        Task<int> UpdateMealAliment(MealAliment mealAliment);
        Task<int> AddMealAlimentAsync(MealAliment mealAliment);
        Task<int> DeleteMealAlimentAsync(MealAliment mealAliment);
        Task<int> DeleteAllMealAlimentsAsync();
        Task<int> DropTableMealAliment();
    }
}
