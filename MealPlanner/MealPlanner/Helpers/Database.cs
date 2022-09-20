﻿using MealPlanner.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealPlanner.Helpers
{
    public  class Database
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
            dbConnection.CreateTableAsync<Meal>();
            dbConnection.CreateTableAsync<Food>();
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
        public Task<int> AddRoutineExerciseAsync(MealFood mealFood)
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
        public Task<int> DeleteAllMealFoodAsync()
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
    }
}
