using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static MealPlanner.Models.User;

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
            // Log
            dbConnection.CreateTableAsync<Log>();

            // Workout
            dbConnection.CreateTableAsync<Exercice>();
            dbConnection.CreateTableAsync<Workout>();
            dbConnection.CreateTableAsync<Set>();
            dbConnection.CreateTableAsync<MuscleGroup>();
            dbConnection.CreateTableAsync<WorkoutExercice>();
            dbConnection.CreateTableAsync<WorkoutProgram>();
            dbConnection.CreateTableAsync<WorkoutRoutine>();
            dbConnection.CreateTableAsync<WorkoutProgramRoutine>();
            dbConnection.CreateTableAsync<WorkoutRoutineExercice>();


            // Meal
            dbConnection.CreateTableAsync<TypeOfRegimeItem>();
            dbConnection.CreateTableAsync<JournalTemplate>();
            dbConnection.CreateTableAsync<JournalTemplateMeal>();
            dbConnection.CreateTableAsync<LogMeal>();
            dbConnection.CreateTableAsync<User>();
            dbConnection.CreateTableAsync<Recipe>();
            dbConnection.CreateTableAsync<RecipeInstruction>();
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

        #region Exercice

        /// <summary>
        /// Returns a Exercice from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Exercice> GetExerciceAsync(int id)
        {
            return dbConnection.GetAsync<Exercice>(id);
        }

        /// <summary>
        /// Returns all Exercice from database
        /// </summary>
        /// <returns></returns>
        public Task<List<Exercice>> GetAllExercicesAsync()
        {
            return dbConnection.Table<Exercice>().ToListAsync();
        }

        /// <summary>
        /// Inserts a Exercice in database
        /// </summary>
        /// <param name="exercice"></param>
        /// <returns></returns>
        public Task<int> AddExerciceAsync(Exercice exercice)
        {
            return dbConnection.InsertAsync(exercice);
        }

        /// <summary>
        /// Updates a Exercice from database
        /// </summary>
        /// <param name="exercice"></param>
        /// <returns></returns>
        public Task<int> UpdateExerciceAsync(Exercice exercice)
        {
            if (GetExerciceAsync(exercice.Id) != null)
                return dbConnection.UpdateAsync(exercice);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a Exercice from database
        /// </summary>
        /// <param name="exercice"></param>
        /// <returns></returns>
        public Task<int> DeleteExerciceAsync(Exercice exercice)
        {
            return dbConnection.DeleteAsync(exercice);
        }

        /// <summary>
        /// Deletes all Exercice from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllExercicesAsync()
        {
            return dbConnection.DeleteAllAsync<Exercice>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableExercice()
        {
            return dbConnection.DropTableAsync<Exercice>();
        }

        #endregion

        #region Workout

        /// <summary>
        /// Returns a Workout from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Workout> GetWorkoutAsync(int id)
        {
            return dbConnection.GetAsync<Workout>(id);
        }

        /// <summary>
        /// Returns all Workouts from database
        /// </summary>
        /// <returns></returns>
        public Task<List<Workout>> GetAllWorkoutsAsync()
        {
            return dbConnection.Table<Workout>().ToListAsync();
        }

        /// <summary>
        /// Inserts a Workout in database
        /// </summary>
        /// <param name="workout"></param>
        /// <returns></returns>
        public Task<int> AddWorkoutAsync(Workout workout)
        {
            return dbConnection.InsertAsync(workout);
        }

        /// <summary>
        /// Updates a Workout from database
        /// </summary>
        /// <param name="workout"></param>
        /// <returns></returns>
        public Task<int> UpdateWorkoutAsync(Workout workout)
        {
            if (GetWorkoutAsync(workout.Id) != null)
                return dbConnection.UpdateAsync(workout);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a Workout from database
        /// </summary>
        /// <param name="workout"></param>
        /// <returns></returns>
        public Task<int> DeleteWorkoutAsync(Workout workout)
        {
            return dbConnection.DeleteAsync(workout);
        }

        /// <summary>
        /// Deletes all Workout from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllWorkoutsAsync()
        {
            return dbConnection.DeleteAllAsync<Workout>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableWorkout()
        {
            return dbConnection.DropTableAsync<Workout>();
        }

        #endregion

        #region Set

        /// <summary>
        /// Returns a Set from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Set> GetSetAsync(int id)
        {
            return dbConnection.GetAsync<Set>(id);
        }

        /// <summary>
        /// Returns all Sets from database
        /// </summary>
        /// <returns></returns>
        public Task<List<Set>> GetAllSetsAsync()
        {
            return dbConnection.Table<Set>().ToListAsync();
        }

        /// <summary>
        /// Inserts a Set in database
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Task<int> AddSetAsync(Set set)
        {
            return dbConnection.InsertAsync(set);
        }

        /// <summary>
        /// Updates a Set from database
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Task<int> UpdateSetAsync(Set set)
        {
            if (GetSetAsync(set.Id) != null)
                return dbConnection.UpdateAsync(set);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a Set from database
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Task<int> DeleteSetAsync(Set set)
        {
            return dbConnection.DeleteAsync(set);
        }

        /// <summary>
        /// Deletes all Set from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllSetsAsync()
        {
            return dbConnection.DeleteAllAsync<Set>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableSet()
        {
            return dbConnection.DropTableAsync<Set>();
        }

        #endregion

        #region MuscleGroup

        /// <summary>
        /// Returns a MuscleGroup from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MuscleGroup> GetMuscleGroupAsync(int id)
        {
            return dbConnection.GetAsync<MuscleGroup>(id);
        }

        /// <summary>
        /// Returns all MuscleGroups from database
        /// </summary>
        /// <returns></returns>
        public Task<List<MuscleGroup>> GetAllMuscleGroupsAsync()
        {
            return dbConnection.Table<MuscleGroup>().ToListAsync();
        }

        /// <summary>
        /// Inserts a MuscleGroup in database
        /// </summary>
        /// <param name="muscleGroup"></param>
        /// <returns></returns>
        public Task<int> AddMuscleGroupAsync(MuscleGroup muscleGroup)
        {
            return dbConnection.InsertAsync(muscleGroup);
        }

        /// <summary>
        /// Updates a MuscleGroup from database
        /// </summary>
        /// <param name="muscleGroup"></param>
        /// <returns></returns>
        public Task<int> UpdateMuscleGroupAsync(MuscleGroup muscleGroup)
        {
            if (GetMuscleGroupAsync(muscleGroup.Id) != null)
                return dbConnection.UpdateAsync(muscleGroup);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a MuscleGroup from database
        /// </summary>
        /// <param name="muscleGroup"></param>
        /// <returns></returns>
        public Task<int> DeleteMuscleGroupAsync(MuscleGroup muscleGroup)
        {
            return dbConnection.DeleteAsync(muscleGroup);
        }

        /// <summary>
        /// Deletes all MuscleGroup from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllMuscleGroupsAsync()
        {
            return dbConnection.DeleteAllAsync<MuscleGroup>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableMuscleGroup()
        {
            return dbConnection.DropTableAsync<MuscleGroup>();
        }

        #endregion

        #region WorkoutExercice

        /// <summary>
        /// Returns a WorkoutExercice from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkoutExercice> GetWorkoutExerciceAsync(int id)
        {
            return dbConnection.GetAsync<WorkoutExercice>(id);
        }

        /// <summary>
        /// Returns all WorkoutExercices from database
        /// </summary>
        /// <returns></returns>
        public Task<List<WorkoutExercice>> GetAllWorkoutExercicesAsync()
        {
            return dbConnection.Table<WorkoutExercice>().ToListAsync();
        }

        /// <summary>
        /// Inserts a WorkoutExercice in database
        /// </summary>
        /// <param name="workoutExercice"></param>
        /// <returns></returns>
        public Task<int> AddWorkoutExerciceAsync(WorkoutExercice workoutExercice)
        {
            return dbConnection.InsertAsync(workoutExercice);
        }

        /// <summary>
        /// Updates a WorkoutExercice from database
        /// </summary>
        /// <param name="workoutExercice"></param>
        /// <returns></returns>
        public Task<int> UpdateWorkoutExerciceAsync(WorkoutExercice workoutExercice)
        {
            if (GetWorkoutExerciceAsync(workoutExercice.Id) != null)
                return dbConnection.UpdateAsync(workoutExercice);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a WorkoutExercice from database
        /// </summary>
        /// <param name="workoutExercice"></param>
        /// <returns></returns>
        public Task<int> DeleteWorkoutExerciceAsync(WorkoutExercice workoutExercice)
        {
            return dbConnection.DeleteAsync(workoutExercice);
        }

        /// <summary>
        /// Deletes all WorkoutExercice from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllWorkoutExercicesAsync()
        {
            return dbConnection.DeleteAllAsync<WorkoutExercice>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableWorkoutExercice()
        {
            return dbConnection.DropTableAsync<WorkoutExercice>();
        }

        #endregion

        #region WorkoutProgram

        /// <summary>
        /// Returns a WorkoutProgram from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkoutProgram> GetWorkoutProgramAsync(int id)
        {
            return dbConnection.GetAsync<WorkoutProgram>(id);
        }

        /// <summary>
        /// Returns all WorkoutProgram from database
        /// </summary>
        /// <returns></returns>
        public Task<List<WorkoutProgram>> GetAllWorkoutProgramsAsync()
        {
            return dbConnection.Table<WorkoutProgram>().ToListAsync();
        }

        /// <summary>
        /// Inserts a WorkoutProgram in database
        /// </summary>
        /// <param name="workoutProgram"></param>
        /// <returns></returns>
        public Task<int> AddWorkoutProgramAsync(WorkoutProgram workoutProgram)
        {
            return dbConnection.InsertAsync(workoutProgram);
        }

        /// <summary>
        /// Updates a WorkoutProgram from database
        /// </summary>
        /// <param name="workoutProgram"></param>
        /// <returns></returns>
        public Task<int> UpdateWorkoutProgramAsync(WorkoutProgram workoutProgram)
        {
            if (GetWorkoutProgramAsync(workoutProgram.Id) != null)
                return dbConnection.UpdateAsync(workoutProgram);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a WorkoutProgram from database
        /// </summary>
        /// <param name="workoutProgram"></param>
        /// <returns></returns>
        public Task<int> DeleteWorkoutProgramAsync(WorkoutProgram workoutProgram)
        {
            return dbConnection.DeleteAsync(workoutProgram);
        }

        /// <summary>
        /// Deletes all WorkoutProgram from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllWorkoutProgramsAsync()
        {
            return dbConnection.DeleteAllAsync<WorkoutProgram>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableWorkoutProgram()
        {
            return dbConnection.DropTableAsync<WorkoutProgram>();
        }

        #endregion

        #region WorkoutRoutine

        /// <summary>
        /// Returns a WorkoutRoutine from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkoutRoutine> GetWorkoutRoutineAsync(int id)
        {
            return dbConnection.GetAsync<WorkoutRoutine>(id);
        }

        /// <summary>
        /// Returns all WorkoutRoutine from database
        /// </summary>
        /// <returns></returns>
        public Task<List<WorkoutRoutine>> GetAllWorkoutRoutinesAsync()
        {
            return dbConnection.Table<WorkoutRoutine>().ToListAsync();
        }

        /// <summary>
        /// Inserts a WorkoutRoutine in database
        /// </summary>
        /// <param name="workoutRoutine"></param>
        /// <returns></returns>
        public Task<int> AddWorkoutRoutineAsync(WorkoutRoutine workoutRoutine)
        {
            return dbConnection.InsertAsync(workoutRoutine);
        }

        /// <summary>
        /// Updates a WorkoutRoutine from database
        /// </summary>
        /// <param name="workoutRoutine"></param>
        /// <returns></returns>
        public Task<int> UpdateWorkoutRoutineAsync(WorkoutRoutine workoutRoutine)
        {
            if (GetWorkoutRoutineAsync(workoutRoutine.Id) != null)
                return dbConnection.UpdateAsync(workoutRoutine);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a WorkoutRoutine from database
        /// </summary>
        /// <param name="workoutRoutine"></param>
        /// <returns></returns>
        public Task<int> DeleteWorkoutRoutineAsync(WorkoutRoutine workoutRoutine)
        {
            return dbConnection.DeleteAsync(workoutRoutine);
        }

        /// <summary>
        /// Deletes all WorkoutRoutine from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllWorkoutRoutinesAsync()
        {
            return dbConnection.DeleteAllAsync<WorkoutRoutine>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableWorkoutRoutine()
        {
            return dbConnection.DropTableAsync<WorkoutRoutine>();
        }

        #endregion

        #region WorkoutProgramRoutine

        /// <summary>
        /// Returns a WorkoutProgramRoutine from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkoutProgramRoutine> GetWorkoutProgramRoutineAsync(int id)
        {
            return dbConnection.GetAsync<WorkoutProgramRoutine>(id);
        }

        /// <summary>
        /// Returns all WorkoutProgramRoutine from database
        /// </summary>
        /// <returns></returns>
        public Task<List<WorkoutProgramRoutine>> GetAllWorkoutProgramRoutinesAsync()
        {
            return dbConnection.Table<WorkoutProgramRoutine>().ToListAsync();
        }

        /// <summary>
        /// Inserts a WorkoutProgramRoutine in database
        /// </summary>
        /// <param name="workoutProgramRoutine"></param>
        /// <returns></returns>
        public Task<int> AddWorkoutProgramRoutineAsync(WorkoutProgramRoutine workoutProgramRoutine)
        {
            return dbConnection.InsertAsync(workoutProgramRoutine);
        }

        /// <summary>
        /// Updates a WorkoutProgramRoutine from database
        /// </summary>
        /// <param name="workoutProgramRoutine"></param>
        /// <returns></returns>
        public Task<int> UpdateWorkoutProgramRoutineAsync(WorkoutProgramRoutine workoutProgramRoutine)
        {
            if (GetWorkoutProgramRoutineAsync(workoutProgramRoutine.Id) != null)
                return dbConnection.UpdateAsync(workoutProgramRoutine);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a WorkoutProgramRoutine from database
        /// </summary>
        /// <param name="workoutProgramRoutine"></param>
        /// <returns></returns>
        public Task<int> DeleteWorkoutProgramRoutineAsync(WorkoutProgramRoutine workoutProgramRoutine)
        {
            return dbConnection.DeleteAsync(workoutProgramRoutine);
        }

        /// <summary>
        /// Deletes all WorkoutProgramRoutine from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllWorkoutProgramRoutinesAsync()
        {
            return dbConnection.DeleteAllAsync<WorkoutProgramRoutine>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableWorkoutProgramRoutine()
        {
            return dbConnection.DropTableAsync<WorkoutProgramRoutine>();
        }

        #endregion

        #region WorkoutRoutineExercice

        /// <summary>
        /// Returns a WorkoutRoutineExercice from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkoutRoutineExercice> GetWorkoutRoutineExerciceAsync(int id)
        {
            return dbConnection.GetAsync<WorkoutRoutineExercice>(id);
        }

        /// <summary>
        /// Returns all WorkoutRoutineExercice from database
        /// </summary>
        /// <returns></returns>
        public Task<List<WorkoutRoutineExercice>> GetAllWorkoutRoutineExercicesAsync()
        {
            return dbConnection.Table<WorkoutRoutineExercice>().ToListAsync();
        }

        /// <summary>
        /// Inserts a WorkoutRoutineExercice in database
        /// </summary>
        /// <param name="workoutRoutineExercice"></param>
        /// <returns></returns>
        public Task<int> AddWorkoutRoutineExerciceAsync(WorkoutRoutineExercice workoutRoutineExercice)
        {
            return dbConnection.InsertAsync(workoutRoutineExercice);
        }

        /// <summary>
        /// Updates a WorkoutRoutineExercice from database
        /// </summary>
        /// <param name="workoutRoutineExercice"></param>
        /// <returns></returns>
        public Task<int> UpdateWorkoutRoutineExerciceAsync(WorkoutRoutineExercice workoutRoutineExercice)
        {
            if (GetWorkoutRoutineExerciceAsync(workoutRoutineExercice.Id) != null)
                return dbConnection.UpdateAsync(workoutRoutineExercice);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a WorkoutRoutineExercice from database
        /// </summary>
        /// <param name="workoutRoutineExercice"></param>
        /// <returns></returns>
        public Task<int> DeleteWorkoutRoutineExerciceAsync(WorkoutRoutineExercice workoutRoutineExercice)
        {
            return dbConnection.DeleteAsync(workoutRoutineExercice);
        }

        /// <summary>
        /// Deletes all WorkoutRoutineExercice from database
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllWorkoutRoutineExercicesAsync()
        {
            return dbConnection.DeleteAllAsync<WorkoutRoutineExercice>();
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableWorkoutRoutineExercice()
        {
            return dbConnection.DropTableAsync<WorkoutRoutineExercice>();
        }

        #endregion

        #region TypeOfRegimeItem

        /// <summary>
        /// Returns a TypeOfRegimeItem object
        /// </summary>
        /// <returns></returns>
        public Task<TypeOfRegimeItem> GetTypeOfRegimeItemAsync()
        {
            return dbConnection.Table<TypeOfRegimeItem>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts new TypeOfRegimeItem in database
        /// </summary>
        /// <param name="typeOfRegimeItem"></param>
        /// <returns></returns>
        public Task<int> AddTypeOfRegimeItemAsync(TypeOfRegimeItem typeOfRegimeItem)
        {
            return dbConnection.InsertAsync(typeOfRegimeItem);
        }

        /// <summary>
        /// Updates a TypeOfRegimeItem in database if it exists
        /// </summary>
        /// <param name="typeOfRegimeItem"></param>
        /// <returns></returns>
        public Task<int> UpdateTypeOfRegimeItemAsync(TypeOfRegimeItem typeOfRegimeItem)
        {
            if (GetTypeOfRegimeItemAsync() != null)
                return dbConnection.UpdateAsync(typeOfRegimeItem);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableTypeOfRegimeItem()
        {
            return dbConnection.DropTableAsync<TypeOfRegimeItem>();
        }

        #endregion

        #region JournalTemplate

        /// <summary>
        /// Returns a JournalTemplate object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<JournalTemplate> GetJournalTemplateAsync(int id)
        {
            return dbConnection.GetAsync<JournalTemplate>(id);
        }

        /// <summary>
        /// Returns a list of JournalTemplates
        /// </summary>
        /// <returns></returns>
        public Task<List<JournalTemplate>> GetAllJournalTemplatesAsync()
        {
            return dbConnection.Table<JournalTemplate>().ToListAsync();
        }

        /// <summary>
        /// Inserts new JournalTemplate in database
        /// </summary>
        /// <param name="journalTemplate"></param>
        /// <returns></returns>
        public Task<int> AddJournalTemplateAsync(JournalTemplate journalTemplate)
        {
            return dbConnection.InsertAsync(journalTemplate);
        }

        /// <summary>
        /// Updates a JournalTemplate in database if it exists
        /// </summary>
        /// <param name="journalTemplate"></param>
        /// <returns></returns>
        public Task<int> UpdateJournalTemplateAsync(JournalTemplate journalTemplate)
        {
            if (GetJournalTemplateAsync(journalTemplate.Id) != null)
                return dbConnection.UpdateAsync(journalTemplate);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableJournalTemplate()
        {
            return dbConnection.DropTableAsync<JournalTemplate>();
        }

        #endregion

        #region JournalTemplateMeal

        /// <summary>
        /// Returns a JournalTemplateMeal object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<JournalTemplateMeal> GetJournalTemplateMealAsync(int id)
        {
            return dbConnection.GetAsync<JournalTemplateMeal>(id);
        }

        /// <summary>
        /// Returns a list of JournalTemplateMeals
        /// </summary>
        /// <returns></returns>
        public Task<List<JournalTemplateMeal>> GetAllJournalTemplateMealsAsync()
        {
            return dbConnection.Table<JournalTemplateMeal>().ToListAsync();
        }

        /// <summary>
        /// Inserts new JournalTemplateMeal in database
        /// </summary>
        /// <param name="journalTemplateMeal"></param>
        /// <returns></returns>
        public Task<int> AddJournalTemplateMealAsync(JournalTemplateMeal journalTemplateMeal)
        {
            return dbConnection.InsertAsync(journalTemplateMeal);
        }

        /// <summary>
        /// Updates a JournalTemplateMeal in database if it exists
        /// </summary>
        /// <param name="journalTemplateMeal"></param>
        /// <returns></returns>
        public Task<int> UpdateJournalTemplateMealAsync(JournalTemplateMeal journalTemplateMeal)
        {
            if (GetJournalTemplateMealAsync(journalTemplateMeal.Id) != null)
                return dbConnection.UpdateAsync(journalTemplateMeal);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a JournalTemplateMeal from database
        /// </summary>
        /// <param name="journalTemplateMeal"></param>
        /// <returns></returns>
        public Task<int> DeleteJournalTemplateMealAsync(JournalTemplateMeal journalTemplateMeal)
        {
            return dbConnection.DeleteAsync(journalTemplateMeal);
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableJournalTemplateMeal()
        {
            return dbConnection.DropTableAsync<JournalTemplateMeal>();
        }

        #endregion

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
        /// Returns a Log object
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<Log> GetLogAsync(DateTime date)
        {
            var list = await dbConnection.Table<Log>().ToListAsync();
            return list.FirstOrDefault(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day);
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
        /// Deletes a Log from database
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public Task<int> DeleteLogAsync(Log log)
        {
            return dbConnection.DeleteAsync(log);
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
        /// Deletes a LogMeal from database
        /// </summary>
        /// <param name="logMeal"></param>
        /// <returns></returns>
        public Task<int> DeleteLogMealAsync(LogMeal logMeal)
        {
            return dbConnection.DeleteAsync(logMeal);
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

            if (GetUserAsync() != null)
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

        #region RecipeInstruction

        /// <summary>
        /// Returns a RecipeInstruction object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<RecipeInstruction> GetRecipeInstructionAsync(int id)
        {
            return dbConnection.GetAsync<RecipeInstruction>(id);
        }

        /// <summary>
        /// Returns a list of RecipeInstructions
        /// </summary>
        /// <returns></returns>
        public Task<List<RecipeInstruction>> GetAllRecipeInstructionsAsync()
        {
            return dbConnection.Table<RecipeInstruction>().ToListAsync();
        }

        /// <summary>
        /// Inserts new RecipeInstruction in database
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public Task<int> AddRecipeInstructionAsync(RecipeInstruction recipeInstruction)
        {
            return dbConnection.InsertAsync(recipeInstruction);
        }

        /// <summary>
        /// Updates a RecipeInstruction in database if it exists
        /// </summary>
        /// <param name="recipeInstruction"></param>
        /// <returns></returns>
        public Task<int> UpdateRecipeInstructionAsync(RecipeInstruction recipeInstruction)
        {
            if (GetRecipeInstructionAsync(recipeInstruction.Id) != null)
                return dbConnection.UpdateAsync(recipeInstruction);
            else
                return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes a RecipeInstruction from database
        /// </summary>
        /// <param name="recipeInstruction"></param>
        /// <returns></returns>
        public Task<int> DeleteRecipeInstructionAsync(RecipeInstruction recipeInstruction)
        {
            return dbConnection.DeleteAsync(recipeInstruction);
        }

        /// <summary>
        /// Drops the table
        /// </summary>
        /// <returns></returns>
        public Task<int> DropTableRecipeInstruction()
        {
            return dbConnection.DropTableAsync<RecipeInstruction>();
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
            if (GetMealAsync(meal.Id) != null)
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
            if (GetMealAlimentAsync(mealAliment.Id) != null)
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
