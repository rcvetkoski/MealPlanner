using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public interface IDataBase
    {
        //User
        Task<User> GetUserAsync();
        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task<int> DropTableUser();


        // Meal
        Task<Meal> GetMealAsync(int id);
        Task<List<Meal>> GetAllMealsAsync();
        Task<int> AddMealAsync(Meal meal);
        Task<int> UpdateMealAsync(Meal meal);
        Task<int> DropTableMeal();


        // Food
        Task<Food> GetFoodAsync(int id);
        Task<List<Food>> GetAllFoodsAsync();
        Task<int> AddFoodAsync(Food food);
        Task<int> UpdateFoodAsync(Food food);
        Task<int> DropTableFood();


        // MealFood
        Task<MealFood> GetMealFoodAsync(int id);
        Task<List<MealFood>> GetMealFoodsAsync(int meal_id, int food_id);
        Task<List<MealFood>> GetAllMealFoodsAsync();
        Task<int> AddMealFoodAsync(MealFood mealFood);
        Task<int> UpdateMealFood(MealFood mealFood);
        Task<int> DeleteMealFoodAsync(MealFood mealFood);
        Task<int> DeleteAllMealFoodsAsync();
        Task<int> DropTableMealFood();

        //DayMeal
        Task<DayMeal> GetDayMealAsync(int id);
        Task<List<DayMeal>> GetAllDayMealsAsync();
        Task<int> AddDayMealAsync(DayMeal dayMeal);
        Task<int> UpdateDayMeal(DayMeal dayMeal);
        Task<int> DeleteDayMealAsync(DayMeal dayMeal);
        Task<int> DeleteAllDayMealsAsync();
        Task<int> DropTableDayMeal();
    }
}
