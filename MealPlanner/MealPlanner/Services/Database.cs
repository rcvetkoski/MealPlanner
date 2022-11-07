using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealPlanner.Services
{
    public class Database : IDataBase
    {
        private SQLiteAsyncConnection dbConnection;
        public const string DatabaseFilename = "MealPlanner.db3";

        public Database()
        {
            if (dbConnection != null)
                return;

            // Establish connection
            dbConnection = new SQLiteAsyncConnection(DatabasePath);

            // Create tables in database, based on model clases if not created
            CreateTables();
        }

        private Task CreateTables()
        {
            dbConnection.CreateTableAsync<Log>();
            dbConnection.CreateTableAsync<LogMeal>();
            dbConnection.CreateTableAsync<User>();
            dbConnection.CreateTableAsync<Recipe>();
            dbConnection.CreateTableAsync<Food>();
            dbConnection.CreateTableAsync<TemplateMeal>();
            dbConnection.CreateTableAsync<Meal>();
            dbConnection.CreateTableAsync<MealAliment>();
            dbConnection.CreateTableAsync<RecipeFood>().Wait();
            return Task.CompletedTask;
        }

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        #region Log

        /// <summary>
        /// Returns a Log object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Log> GetLogAsync(int id)
        {
            return dbConnection.GetAsync<Log>(id);
        }

        /// <summary>
        /// Returns a list of Logs
        /// </summary>
        /// <returns></returns>
        public Task<List<Log>> GetAllLogsAsync()
        {
            return dbConnection.Table<Log>().ToListAsync();
        }

        /// <summary>
        /// Inserts new Log in database
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public Task<int> AddLogAsync(Log log)
        {
            return dbConnection.InsertAsync(log);
        }

        /// <summary>
        /// Updates a Log in database if it exists
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public Task<int> UpdateLogAsync(Log log)
        {
            if (GetLogAsync(log.Id) != null)
                return dbConnection.UpdateAsync(log);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableLog()
        {
            return dbConnection.DropTableAsync<Log>();
        }

        #endregion

        #region LogMeals

        /// <summary>
        /// Returns a LogMeal object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<LogMeal> GetLogMealAsync(int id)
        {
            return dbConnection.GetAsync<LogMeal>(id);
        }

        /// <summary>
        /// Returns a list of LogMeals
        /// </summary>
        /// <returns></returns>
        public Task<List<LogMeal>> GetAllLogMealsAsync()
        {
            return dbConnection.Table<LogMeal>().ToListAsync();
        }

        /// <summary>
        /// Inserts new LogMeal in database
        /// </summary>
        /// <param name="logMeal"></param>
        /// <returns></returns>
        public Task<int> AddLogMealAsync(LogMeal logMeal)
        {
            return dbConnection.InsertAsync(logMeal);
        }

        /// <summary>
        /// Updates a LogMeal in database if it exists
        /// </summary>
        /// <param name="logMeal"></param>
        /// <returns></returns>
        public Task<int> UpdateLogMealAsync(LogMeal logMeal)
        {
            if (GetLogMealAsync(logMeal.Id) != null)
                return dbConnection.UpdateAsync(logMeal);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableLogMeal()
        {
            return dbConnection.DropTableAsync<LogMeal>();
        }

        #endregion

        #region User

        /// <summary>
        /// Returns a User object
        /// </summary>
        /// <returns></returns>
        public Task<User> GetUserAsync()
        {
            return dbConnection.Table<User>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts new User in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> AddUserAsync(User user)
        {
            return dbConnection.InsertAsync(user);
        }

        /// <summary>
        /// Updates a User in database if it exists
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> UpdateUserAsync(User user)
        {
            user.SelectedTypeOfRegimeDB = user.SelectedTypeOfRegime.TypeOfRegime;
            user.SelectedPhysicalActivityLevelDB = user.SelectedPhysicalActivityLevel.PALItemType;
            user.SelectedObjectiflDB = user.SelectedObjectif.ObjectifType;

            if (GetFoodAsync(user.Id) != null)
                return dbConnection.UpdateAsync(user);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableUser()
        {
            return dbConnection.DropTableAsync<User>();
        }

        #endregion

        #region Recipe

        /// <summary>
        /// Returns a Recipe object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Recipe> GetRecipeAsync(int id)
        {
            return dbConnection.GetAsync<Recipe>(id);
        }

        /// <summary>
        /// Returns a list of Recipes
        /// </summary>
        /// <returns></returns>
        public Task<List<Recipe>> GetAllRecipesAsync()
        {
            return dbConnection.Table<Recipe>().ToListAsync();
        }

        /// <summary>
        /// Inserts new Recipe in database
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public Task<int> AddRecipeAsync(Recipe recipe)
        {
            return dbConnection.InsertAsync(recipe);
        }

        /// <summary>
        /// Updates a Recipe in database if it exists
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public Task<int> UpdateRecipeAsync(Recipe recipe)
        {
            if (GetRecipeAsync(recipe.Id) != null)
                return dbConnection.UpdateAsync(recipe);
            else
                return Task.FromResult(0);
        }

        // TODO delete part

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableRecipe()
        {
            return dbConnection.DropTableAsync<Recipe>();
        }

        #endregion

        #region Food

        /// <summary>
        /// Returns a Food object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Food> GetFoodAsync(int id)
        {
            return dbConnection.GetAsync<Food>(id);
        }

        /// <summary>
        /// Returns a list of Foods
        /// </summary>
        /// <returns></returns>
        public Task<List<Food>> GetAllFoodsAsync()
        {
            return dbConnection.Table<Food>().ToListAsync();
        }

        /// <summary>
        /// Inserts new Food in database
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        public Task<int> AddFoodAsync(Food food)
        {
            return dbConnection.InsertAsync(food);
        }

        /// <summary>
        /// Updates a Food in database if it exists
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        public Task<int> UpdateFoodAsync(Food food)
        {
            if (GetFoodAsync(food.Id) != null)
                return dbConnection.UpdateAsync(food);
            else
                return Task.FromResult(0);
        }

        // TODO delete part

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableFood()
        {
            return dbConnection.DropTableAsync<Food>();
        }

        #endregion

        #region RecipeFood

        /// <summary>
        /// Returns a RecipeFood from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<RecipeFood> GetRecipeFoodAsync(int id)
        {
            return dbConnection.GetAsync<RecipeFood>(id);
        }

        /// <summary>
        /// Returns a list of RecipeFoods based on recipe_id or food_id
        /// </summary>
        /// <param name="recipe_id"></param>
        /// <param name="food_id"></param>
        /// <returns></returns>
        public Task<List<RecipeFood>> GetRecipeFoodsAsync(int recipe_id = 0, int food_id = 0)
        {
            if (recipe_id != 0 && food_id != 0)
                return dbConnection.Table<RecipeFood>().Where(x => x.RecipeId == recipe_id && x.FoodId == food_id).ToListAsync();
            else if (recipe_id != 0)
                return dbConnection.Table<RecipeFood>().Where(x => x.RecipeId == recipe_id).ToListAsync();
            else if (food_id != 0)
                return dbConnection.Table<RecipeFood>().Where(x => x.FoodId == food_id).ToListAsync();
            else
                return Task.FromResult(new List<RecipeFood>());
        }

        /// <summary>
        /// Returns all RecipeFood from database
        /// </summary>
        /// <returns></returns>
        public Task<List<RecipeFood>> GetAllRecipeFoodsAsync()
        {
            return dbConnection.Table<RecipeFood>().ToListAsync();
        }

        /// <summary>
        /// Inserts a RecipeFood in database
        /// </summary>
        /// <param name="recipeFood"></param>
        /// <returns></returns>
        public Task<int> AddRecipeFoodAsync(RecipeFood recipeFood)
        {
            return dbConnection.InsertAsync(recipeFood);
        }

        /// <summary>
        /// Updates a RecipeFood from database
        /// </summary>
        /// <param name="recipeFood"></param>
        /// <returns></returns>
        public Task<int> UpdateRecipeFood(RecipeFood recipeFood)
        {
            if (GetRecipeFoodAsync(recipeFood.Id) != null)
                return dbConnection.UpdateAsync(recipeFood);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a RecipeFood from database
        /// </summary>
        /// <param name="recipeFood"></param>
        /// <returns></returns>
        public Task<int> DeleteRecipeFoodAsync(RecipeFood recipeFood)
        {
            return dbConnection.DeleteAsync(recipeFood);
        }

        /// <summary>
        /// Deletes all RecipeFood from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllRecipeFoodsAsync()
        {
            return dbConnection.DeleteAllAsync<RecipeFood>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableRecipeFood()
        {
            return dbConnection.DropTableAsync<RecipeFood>();
        }

        #endregion

        #region TemplateMeal

        /// <summary>
        /// Returns a TemplateMeal from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TemplateMeal> GetTemplateMealAsync(int id)
        {
            return dbConnection.GetAsync<TemplateMeal>(id);
        }

        /// <summary>
        /// Returns all TemplateMeal from database
        /// </summary>
        /// <returns></returns>
        public Task<List<TemplateMeal>> GetAllTemplateMealsAsync()
        {
            return dbConnection.Table<TemplateMeal>().ToListAsync();
        }

        /// <summary>
        /// Inserts a TemplateMeal in database
        /// </summary>
        /// <param name="templateMeal"></param>
        /// <returns></returns>
        public Task<int> AddTemplateMealAsync(TemplateMeal templateMeal)
        {
            return dbConnection.InsertAsync(templateMeal);
        }

        /// <summary>
        /// Updates a TemplateMeal from database
        /// </summary>
        /// <param name="templateMeal"></param>
        /// <returns></returns>
        public Task<int> UpdateTemplateMealAsync(TemplateMeal templateMeal)
        {
            if (GetTemplateMealAsync(templateMeal.Id) != null)
                return dbConnection.UpdateAsync(templateMeal);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a TemplateMeal from database
        /// </summary>
        /// <param name="templateMeal"></param>
        /// <returns></returns>
        public Task<int> DeleteTemplateMealAsync(TemplateMeal templateMeal)
        {
            return dbConnection.DeleteAsync(templateMeal);
        }

        #endregion

        #region Meal

        /// <summary>
        /// Returns a Meal from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Meal> GetMealAsync(int id)
        {
            return dbConnection.GetAsync<Meal>(id);
        }

        /// <summary>
        /// Returns all Meal from database
        /// </summary>
        /// <returns></returns>
        public Task<List<Meal>> GetAllMealsAsync()
        {
            return dbConnection.Table<Meal>().ToListAsync();
        }

        /// <summary>
        /// Inserts a Meal in database
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        public Task<int> AddMealAsync(Meal meal)
        {
            return dbConnection.InsertAsync(meal);
        }

        /// <summary>
        /// Updates a Meal from database
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        public Task<int> UpdateMealAsync(Meal meal)
        {
            if (GetRecipeFoodAsync(meal.Id) != null)
                return dbConnection.UpdateAsync(meal);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a Meal from database
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        public Task<int> DeleteMealAsync(Meal meal)
        {
            return dbConnection.DeleteAsync(meal);
        }

        /// <summary>
        /// Deletes all Meal from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllMealsAsync()
        {
            return dbConnection.DeleteAllAsync<Meal>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableMeal()
        {
            return dbConnection.DropTableAsync<Meal>();
        }

        #endregion

        #region MealAliment

        /// <summary>
        /// Returns a MealAliment from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MealAliment> GetMealAlimentAsync(int id)
        {
            return dbConnection.GetAsync<MealAliment>(id);
        }

        /// <summary>
        /// Returns all MealAliment from database
        /// </summary>
        /// <returns></returns>
        public Task<List<MealAliment>> GetAllMealAlimentsAsync()
        {
            return dbConnection.Table<MealAliment>().ToListAsync();
        }

        /// <summary>
        /// Returns a MealAliment based on meal_id , aliment_id and alimentType
        /// </summary>
        /// <param name="alimentType"></param>
        /// <param name="meal_id"></param>
        /// <param name="aliment_id"></param>
        /// <returns></returns>
        public Task<MealAliment> GetMealAlimentAsync(AlimentTypeEnum alimentType, int meal_id = 0, int aliment_id = 0)
        {
            if (meal_id != 0 && aliment_id != 0)
                return dbConnection.Table<MealAliment>().Where(x => x.MealId == meal_id && x.AlimentId == aliment_id && x.AlimentType == alimentType).FirstOrDefaultAsync();
            else
                return null;
        }

        /// <summary>
        /// Returns a list of MealAliment based on meal_id or aliment_id or alimentType
        /// </summary>
        /// <param name="alimentType"></param>
        /// <param name="meal_id"></param>
        /// <param name="aliment_id"></param>
        /// <returns></returns>
        public Task<List<MealAliment>> GetMealAlimentsAsync(AlimentTypeEnum alimentType, int meal_id = 0, int aliment_id = 0)
        {
            if (meal_id != 0 && aliment_id != 0)
                return dbConnection.Table<MealAliment>().Where(x => x.MealId == meal_id && x.AlimentId == aliment_id && x.AlimentType == alimentType).ToListAsync();
            else if (meal_id != 0)
                return dbConnection.Table<MealAliment>().Where(x => x.MealId == meal_id).ToListAsync();
            else if (aliment_id != 0)
                return dbConnection.Table<MealAliment>().Where(x => x.AlimentId == aliment_id && x.AlimentType == alimentType).ToListAsync();
            else
                return Task.FromResult(new List<MealAliment>());
        }

        /// <summary>
        /// Inserts a MealAliment in database
        /// </summary>
        /// <param name="mealAliment"></param>
        /// <returns></returns>
        public Task<int> AddMealAlimentAsync(MealAliment mealAliment)
        {
            return dbConnection.InsertAsync(mealAliment);
        }

        /// <summary>
        /// Updates a MealAliment from database
        /// </summary>
        /// <param name="mealAliment"></param>
        /// <returns></returns>
        public Task<int> UpdateMealAliment(MealAliment mealAliment)
        {
            if (GetRecipeFoodAsync(mealAliment.Id) != null)
                return dbConnection.UpdateAsync(mealAliment);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a MealAliment from database
        /// </summary>
        /// <param name="mealAliment"></param>
        /// <returns></returns>
        public Task<int> DeleteMealAlimentAsync(MealAliment mealAliment)
        {
            return dbConnection.DeleteAsync(mealAliment);
        }

        /// <summary>
        /// Deletes all MealAliment from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllMealAlimentsAsync()
        {
            return dbConnection.DeleteAllAsync<MealAliment>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableMealAliment()
        {
            return dbConnection.DropTableAsync<MealAliment>();
        }

        #endregion
    }
}
