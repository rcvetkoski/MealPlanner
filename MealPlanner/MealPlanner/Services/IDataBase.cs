using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MealPlanner.Models.User;

namespace MealPlanner.Services
{
    public interface IDataBase
    {
        // Exercice
        Task<Exercice> GetExerciceAsync(int id);
        Task<List<Exercice>> GetAllExercicesAsync();
        Task<int> AddExerciceAsync(Exercice exercice);
        Task<int> UpdateExerciceAsync(Exercice exercice);
        Task<int> DeleteExerciceAsync(Exercice exercice);
        Task<int> DeleteAllExercicesAsync();
        Task<int> DropTableExercice();

        // Workout
        Task<Workout> GetWorkoutAsync(int id);
        Task<List<Workout>> GetAllWorkoutsAsync();
        Task<int> AddWorkoutAsync(Workout workout);
        Task<int> UpdateWorkoutAsync(Workout workout);
        Task<int> DeleteWorkoutAsync(Workout workout);
        Task<int> DeleteAllWorkoutsAsync();
        Task<int> DropTableWorkout();

        // MuscleGroup
        Task<MuscleGroup> GetMuscleGroupAsync(int id);
        Task<List<MuscleGroup>> GetAllMuscleGroupsAsync();
        Task<int> AddMuscleGroupAsync(MuscleGroup muscleGroup);
        Task<int> UpdateMuscleGroupAsync(MuscleGroup muscleGroup);
        Task<int> DeleteMuscleGroupAsync(MuscleGroup muscleGroup);
        Task<int> DeleteAllMuscleGroupsAsync();
        Task<int> DropTableMuscleGroup();


        // Set
        Task<Set> GetSetAsync(int id);
        Task<List<Set>> GetAllSetsAsync();
        Task<int> AddSetAsync(Set set);
        Task<int> UpdateSetAsync(Set set);
        Task<int> DeleteSetAsync(Set set);
        Task<int> DeleteAllSetsAsync();
        Task<int> DropTableSet();


        // WorkoutExercice
        Task<WorkoutExercice> GetWorkoutExerciceAsync(int id);
        Task<List<WorkoutExercice>> GetAllWorkoutExercicesAsync();
        Task<int> AddWorkoutExerciceAsync(WorkoutExercice workoutExercice);
        Task<int> UpdateWorkoutExerciceAsync(WorkoutExercice workoutExercice);
        Task<int> DeleteWorkoutExerciceAsync(WorkoutExercice workoutExercice);
        Task<int> DeleteAllWorkoutExercicesAsync();
        Task<int> DropTableWorkoutExercice();


        // WorkoutProgram
        Task<WorkoutProgram> GetWorkoutProgramAsync(int id);
        Task<List<WorkoutProgram>> GetAllWorkoutProgramsAsync();
        Task<int> AddWorkoutProgramAsync(WorkoutProgram workoutProgram);
        Task<int> UpdateWorkoutProgramAsync(WorkoutProgram workoutProgram);
        Task<int> DeleteWorkoutProgramAsync(WorkoutProgram workoutProgram);
        Task<int> DeleteAllWorkoutProgramsAsync();
        Task<int> DropTableWorkoutProgram();



        // WorkoutProgramRoutine
        Task<WorkoutProgramRoutine> GetWorkoutProgramRoutineAsync(int id);
        Task<List<WorkoutProgramRoutine>> GetAllWorkoutProgramRoutinesAsync();
        Task<int> AddWorkoutProgramRoutineAsync(WorkoutProgramRoutine workoutProgramRoutine);
        Task<int> UpdateWorkoutProgramRoutineAsync(WorkoutProgramRoutine workoutProgramRoutine);
        Task<int> DeleteWorkoutProgramRoutineAsync(WorkoutProgramRoutine workoutProgramRoutine);
        Task<int> DeleteAllWorkoutProgramRoutinesAsync();
        Task<int> DropTableWorkoutProgramRoutine();



        // TypeOfRegimeItem
        Task<TypeOfRegimeItem> GetTypeOfRegimeItemAsync();
        Task<int> AddTypeOfRegimeItemAsync(TypeOfRegimeItem typeOfRegimeItem);
        Task<int> UpdateTypeOfRegimeItemAsync(TypeOfRegimeItem typeOfRegimeItem);
        Task<int> DropTableTypeOfRegimeItem();


        // JournalTemplate
        Task<JournalTemplate> GetJournalTemplateAsync(int id);
        Task<List<JournalTemplate>> GetAllJournalTemplatesAsync();
        Task<int> AddJournalTemplateAsync(JournalTemplate journalTemplate);
        Task<int> UpdateJournalTemplateAsync(JournalTemplate journalTemplate);
        Task<int> DropTableJournalTemplate();

        // JournalTemplateMeal
        Task<JournalTemplateMeal> GetJournalTemplateMealAsync(int id);
        Task<List<JournalTemplateMeal>> GetAllJournalTemplateMealsAsync();
        Task<int> AddJournalTemplateMealAsync(JournalTemplateMeal journalTemplateMeal);
        Task<int> UpdateJournalTemplateMealAsync(JournalTemplateMeal journalTemplateMeal);
        Task<int> DeleteJournalTemplateMealAsync(JournalTemplateMeal journalTemplateMeal);
        Task<int> DropTableJournalTemplateMeal();

        // Log
        Task<Log> GetLogAsync(int id);
        Task<Log> GetLogAsync(DateTime date);
        Task<List<Log>> GetAllLogsAsync();
        Task<int> AddLogAsync(Log log);
        Task<int> UpdateLogAsync(Log log);
        Task<int> DeleteLogAsync(Log log);
        Task<int> DropTableLog();

        // LogMeals
        Task<LogMeal> GetLogMealAsync(int id);
        Task<List<LogMeal>> GetAllLogMealsAsync();
        Task<int> AddLogMealAsync(LogMeal logMeal);
        Task<int> UpdateLogMealAsync(LogMeal logMeal);
        Task<int> DeleteLogMealAsync(LogMeal logMeal);
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


        // RecipeInstruction
        Task<RecipeInstruction> GetRecipeInstructionAsync(int id);
        Task<List<RecipeInstruction>> GetAllRecipeInstructionsAsync();
        Task<int> AddRecipeInstructionAsync(RecipeInstruction recipeInstruction);
        Task<int> UpdateRecipeInstructionAsync(RecipeInstruction recipeInstruction);
        Task<int> DeleteRecipeInstructionAsync(RecipeInstruction recipeInstruction);

        Task<int> DropTableRecipeInstruction();


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


        //TemplateMeal
        Task<TemplateMeal> GetTemplateMealAsync(int id);
        Task<List<TemplateMeal>> GetAllTemplateMealsAsync();
        Task<int> AddTemplateMealAsync(TemplateMeal templateMeal);
        Task<int> UpdateTemplateMealAsync(TemplateMeal templateMeal);
        Task<int> DeleteTemplateMealAsync(TemplateMeal templateMeal);


        //Meal
        Task<Meal> GetMealAsync(int id);
        Task<List<Meal>> GetAllMealsAsync();
        Task<int> AddMealAsync(Meal meal);
        Task<int> UpdateMealAsync(Meal meal);
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
