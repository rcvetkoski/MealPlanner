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
            dbConnection.CreateTableAsync<User>();
            dbConnection.CreateTableAsync<Meal>();
            dbConnection.CreateTableAsync<Food>();
            dbConnection.CreateTableAsync<DayMeal>();
            dbConnection.CreateTableAsync<DayMealAliment>();
            dbConnection.CreateTableAsync<MealFood>().Wait();
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

        #region User

        /// <summary>
        /// Returns a User object default id is 1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<User> GetUserAsync()
        {
            return dbConnection.GetAsync<User>(1);
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

        #region Meal

        /// <summary>
        /// Returns a Meal object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Meal> GetMealAsync(int id)
        {
            return dbConnection.GetAsync<Meal>(id);
        }

        /// <summary>
        /// Returns a list of Meals
        /// </summary>
        /// <returns></returns>
        public Task<List<Meal>> GetAllMealsAsync()
        {
            return dbConnection.Table<Meal>().ToListAsync();
        }

        /// <summary>
        /// Inserts new Meal in database
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        public Task<int> AddMealAsync(Meal meal)
        {
            return dbConnection.InsertAsync(meal);
        }

        /// <summary>
        /// Updates a Meal in database if it exists
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        public Task<int> UpdateMealAsync(Meal meal)
        {
            if (GetMealAsync(meal.Id) != null)
                return dbConnection.UpdateAsync(meal);
            else
                return Task.FromResult(0);
        }

        // TODO delete part

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableMeal()
        {
            return dbConnection.DropTableAsync<Meal>();
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

        #region MealFood

        /// <summary>
        /// Returns a MealFood from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MealFood> GetMealFoodAsync(int id)
        {
            return dbConnection.GetAsync<MealFood>(id);
        }

        /// <summary>
        /// Returns a list of MealFoods based on meal_id or food_id
        /// </summary>
        /// <param name="meal_id"></param>
        /// <param name="food_id"></param>
        /// <returns></returns>
        public Task<List<MealFood>> GetMealFoodsAsync(int meal_id = 0, int food_id = 0)
        {
            if (meal_id != 0 && food_id != 0)
                return dbConnection.Table<MealFood>().Where(x => x.MealId == meal_id && x.FoodId == food_id).ToListAsync();
            else if (meal_id != 0)
                return dbConnection.Table<MealFood>().Where(x => x.MealId == meal_id).ToListAsync();
            else if (food_id != 0)
                return dbConnection.Table<MealFood>().Where(x => x.FoodId == food_id).ToListAsync();
            else
                return Task.FromResult(new List<MealFood>());
        }

        /// <summary>
        /// Returns all MealFood from database
        /// </summary>
        /// <returns></returns>
        public Task<List<MealFood>> GetAllMealFoodsAsync()
        {
            return dbConnection.Table<MealFood>().ToListAsync();
        }

        /// <summary>
        /// Inserts a MealFood in database
        /// </summary>
        /// <param name="mealFood"></param>
        /// <returns></returns>
        public Task<int> AddMealFoodAsync(MealFood mealFood)
        {
            return dbConnection.InsertAsync(mealFood);
        }

        /// <summary>
        /// Updates a MealFood from database
        /// </summary>
        /// <param name="mealFood"></param>
        /// <returns></returns>
        public Task<int> UpdateMealFood(MealFood mealFood)
        {
            if (GetMealFoodAsync(mealFood.Id) != null)
                return dbConnection.UpdateAsync(mealFood);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a MealFood from database
        /// </summary>
        /// <param name="mealFood"></param>
        /// <returns></returns>
        public Task<int> DeleteMealFoodAsync(MealFood mealFood)
        {
            return dbConnection.DeleteAsync(mealFood);
        }

        /// <summary>
        /// Deletes all MealFood from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllMealFoodsAsync()
        {
            return dbConnection.DeleteAllAsync<MealFood>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableMealFood()
        {
            return dbConnection.DropTableAsync<MealFood>();
        }

        #endregion

        #region DayMeal

        /// <summary>
        /// Returns a MeDayMealalFood from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DayMeal> GetDayMealAsync(int id)
        {
            return dbConnection.GetAsync<DayMeal>(id);
        }

        /// <summary>
        /// Returns all DayMeal from database
        /// </summary>
        /// <returns></returns>
        public Task<List<DayMeal>> GetAllDayMealsAsync()
        {
            return dbConnection.Table<DayMeal>().ToListAsync();
        }

        /// <summary>
        /// Inserts a DayMeal in database
        /// </summary>
        /// <param name="dayMeal"></param>
        /// <returns></returns>
        public Task<int> AddDayMealAsync(DayMeal dayMeal)
        {
            return dbConnection.InsertAsync(dayMeal);
        }

        /// <summary>
        /// Updates a DayMeal from database
        /// </summary>
        /// <param name="dayMeal"></param>
        /// <returns></returns>
        public Task<int> UpdateDayMeal(DayMeal dayMeal)
        {
            if (GetMealFoodAsync(dayMeal.Id) != null)
                return dbConnection.UpdateAsync(dayMeal);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a DayMeal from database
        /// </summary>
        /// <param name="dayMeal"></param>
        /// <returns></returns>
        public Task<int> DeleteDayMealAsync(DayMeal dayMeal)
        {
            return dbConnection.DeleteAsync(dayMeal);
        }

        /// <summary>
        /// Deletes all DayMeal from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllDayMealsAsync()
        {
            return dbConnection.DeleteAllAsync<DayMeal>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableDayMeal()
        {
            return dbConnection.DropTableAsync<DayMeal>();
        }

        #endregion

        #region DayMealAliment

        /// <summary>
        /// Returns a DayMealAliment from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DayMealAliment> GetDayMealAlimentAsync(int id)
        {
            return dbConnection.GetAsync<DayMealAliment>(id);
        }

        /// <summary>
        /// Returns all DayMealAliment from database
        /// </summary>
        /// <returns></returns>
        public Task<List<DayMealAliment>> GetAllDayMealAlimentsAsync()
        {
            return dbConnection.Table<DayMealAliment>().ToListAsync();
        }

        /// <summary>
        /// Returns a DayMealAliment based on dayMeal_id , aliment_id and alimentType
        /// </summary>
        /// <param name="alimentType"></param>
        /// <param name="dayMeal_id"></param>
        /// <param name="aliment_id"></param>
        /// <returns></returns>
        public Task<DayMealAliment> GetDayMealAlimentAsync(AlimentTypeEnum alimentType, int dayMeal_id = 0, int aliment_id = 0)
        {
            if (dayMeal_id != 0 && aliment_id != 0)
                return dbConnection.Table<DayMealAliment>().Where(x => x.DayMealId == dayMeal_id && x.AlimentId == aliment_id && x.AlimentType == alimentType).FirstOrDefaultAsync();
            else
                return null;
        }

        /// <summary>
        /// Returns a list of DayMealAliment based on dayMeal_id or aliment_id or alimentType
        /// </summary>
        /// <param name="alimentType"></param>
        /// <param name="dayMeal_id"></param>
        /// <param name="aliment_id"></param>
        /// <returns></returns>
        public Task<List<DayMealAliment>> GetDayMealAlimentsAsync(AlimentTypeEnum alimentType, int dayMeal_id = 0, int aliment_id = 0)
        {
            if (dayMeal_id != 0 && aliment_id != 0)
                return dbConnection.Table<DayMealAliment>().Where(x => x.DayMealId == dayMeal_id && x.AlimentId == aliment_id && x.AlimentType == alimentType).ToListAsync();
            else if (dayMeal_id != 0)
                return dbConnection.Table<DayMealAliment>().Where(x => x.DayMealId == dayMeal_id).ToListAsync();
            else if (aliment_id != 0)
                return dbConnection.Table<DayMealAliment>().Where(x => x.AlimentId == aliment_id && x.AlimentType == alimentType).ToListAsync();
            else
                return Task.FromResult(new List<DayMealAliment>());
        }

        /// <summary>
        /// Inserts a DayMealAliment in database
        /// </summary>
        /// <param name="dayMealAliment"></param>
        /// <returns></returns>
        public Task<int> AddDayMealAlimentAsync(DayMealAliment dayMealAliment)
        {
            return dbConnection.InsertAsync(dayMealAliment);
        }

        /// <summary>
        /// Updates a DayMealAliment from database
        /// </summary>
        /// <param name="dayMealAliment"></param>
        /// <returns></returns>
        public Task<int> UpdateDayMealAliment(DayMealAliment dayMealAliment)
        {
            if (GetMealFoodAsync(dayMealAliment.Id) != null)
                return dbConnection.UpdateAsync(dayMealAliment);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a DayMealAliment from database
        /// </summary>
        /// <param name="dayMealAliment"></param>
        /// <returns></returns>
        public Task<int> DeleteDayMealAlimentAsync(DayMealAliment dayMealAliment)
        {
            return dbConnection.DeleteAsync(dayMealAliment);
        }

        /// <summary>
        /// Deletes all DayMealAliment from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllDayMealAlimentsAsync()
        {
            return dbConnection.DeleteAllAsync<DayMealAliment>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableDayMealAliment()
        {
            return dbConnection.DropTableAsync<DayMealAliment>();
        }

        #endregion
    }
}
