﻿using MealPlanner.Helpers.Enums;
using MealPlanner.Helpers.Extensions;
using MealPlanner.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static MealPlanner.Models.TestModel;
using static MealPlanner.Models.User;
using static Xamarin.Essentials.Permissions;

namespace MealPlanner.Helpers
{
    public class ReferentialData : INotifyPropertyChanged
    {
        public User User { get; set; }
        public ObservableCollection<Meal> Meals { get; set; }
        public List<Meal> AllMeals { get; set; }
        public ObservableCollection<Recipe> Recipes { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public ObservableCollection<RecipeFood> RecipeFoods { get; set; }
        public ObservableCollection<Aliment> Aliments { get; set; }
        public ObservableCollection<MealAliment> MealAliments { get; set; }
        public ObservableCollection<JournalTemplate> JournalTemplates { get; set; }
        public ObservableCollection<JournalTemplateMeal> JournalTemplateMeals { get; set; }
        public ObservableCollection<Exercice> Exercices { get; set; }
        public ObservableCollection<WorkoutProgram> WorkoutPrograms { get; set; }
        public ObservableCollection<WorkoutWeek> WorkoutWeeks { get; set; }
        public ObservableCollection<WorkoutWeekProgram> WorkoutWeekPrograms { get; set; }


        private Workout currentWorkout;
        public Workout CurrentWorkout 
        {
            get
            {
                return currentWorkout;
            }
            set
            {
                if(currentWorkout != value)
                {
                    currentWorkout = value;
                    OnPropertyChanged(nameof(CurrentWorkout));
                }
            }
        }
        public ObservableCollection<Workout> Workouts { get; set; }
        public ObservableCollection<Set> Sets { get; set; }
        public ObservableCollection<MuscleGroup> MuscleGroups { get; set; }
        public ObservableCollection<WorkoutExercice> WorkoutExercices { get; set; }
        public ObservableCollection<WorkoutWeekRoutine> WorkoutWeekRoutines { get; set; }

        

        public List<Meal> DefaultMeals { get; set; }
        public List<TemplateMeal> TemplateMeals { get; set; }

        // User
        public List<TypeOfRegimeItem> TypesOfRegime { get; set; }
        public List<PALItem> PhysicalActivityLevels { get; set; }
        public List<ObjectifItem> Objectifs { get; set; }
        public List<string> BMRFormulas { get; set; }


        private DateTime currentDay;
        public DateTime CurrentDay 
        {
            get
            { 
                return currentDay; 
            }
            set
            {
                if(currentDay != value)
                {
                    currentDay = value;
                    OnPropertyChanged(nameof(CurrentDay));
                }
            }
        }

        private JournalTemplate currentJournalTemplate;
        public JournalTemplate CurrentJournalTemplate 
        {
            get
            {
                return currentJournalTemplate;
            }
            set
            {
                if(currentJournalTemplate != value)
                {
                    currentJournalTemplate = value;
                    OnPropertyChanged(nameof(CurrentJournalTemplate));
                }
            }
        }  

        public CopiedDayHelper CopiedDay { get; set; }
        public List<Aliment> CopiedAliments { get; set; }

        public List<Log> Logs { get; set; }
        public List<LogMeal> LogMeals { get; set; } 

        public ReferentialData()
        {
            //ResetDB();
            CurrentDay = DateTime.Now;
            CopiedAliments = new List<Aliment>();
            InitDefaultMeals();
            ResetDBCommand = new Command(ResetDB);
            InitDB();
        }

        public ICommand ResetDBCommand { get; set; }
        public void ResetDB()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(basePath, "MealPlanner.db3");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }


        private void InitDB()
        {
            //App.DataBaseRepo.DropTableExercice().Wait();
            //App.DataBaseRepo.DropTableMuscleGroup().Wait();
            //App.DataBaseRepo.DropTableSet().Wait()  ;
            //App.DataBaseRepo.DropTableWorkout().Wait();
            //App.DataBaseRepo.DropTableWorkoutExercice().Wait();

            // User
            InitUser();

            // Foods
            Foods = App.DataBaseRepo.GetAllFoodsAsync().Result.ToObservableCollection();

            // Recipes
            Recipes = App.DataBaseRepo.GetAllRecipesAsync().Result.ToObservableCollection();

            // Aliments
            Aliments = new ObservableCollection<Aliment>();

            foreach (Recipe recipe in Recipes)
                Aliments.Add(recipe);

            foreach (Food food in Foods)
                Aliments.Add(food);


            // Add foods to Recipe
            RecipeFoods = App.DataBaseRepo.GetAllRecipeFoodsAsync().Result.ToObservableCollection();
            foreach (RecipeFood recipeFood in RecipeFoods)
            {
                Recipe recipe = Aliments.FirstOrDefault(x => x.Id == recipeFood.RecipeId && x.AlimentType == Enums.AlimentTypeEnum.Recipe) as Recipe;
                Food existingFood = Aliments.FirstOrDefault(x => x.Id == recipeFood.FoodId && x.AlimentType == Enums.AlimentTypeEnum.Food) as Food;

                var ratio = recipeFood.ServingSize / existingFood.OriginalServingSize;
                Food food = CreateAndCopyAlimentProperties(existingFood, ratio) as Food;
                food.ServingSize = recipeFood.ServingSize;
                food.RecipeFoodId = recipeFood.Id;

                if (recipe != null)
                    recipe.Foods.Add(food);
            }

            // Add RecipeInstruction to recipe
            var recipeInstructions = App.DataBaseRepo.GetAllRecipeInstructionsAsync().Result;
            foreach (Recipe recipe in Recipes)
            {
                foreach(RecipeInstruction recipeInstruction in recipeInstructions)
                {
                    if (recipeInstruction.RecipeId == recipe.Id)
                    {
                        if (recipeInstruction != null)
                            recipe.RecipeInstructions.Add(recipeInstruction);
                    }
                }
            }

            // Meal Aliments
            MealAliments = App.DataBaseRepo.GetAllMealAlimentsAsync().Result.ToObservableCollection();

            // Meals
            Meals = new ObservableCollection<Meal>();
            AllMeals = App.DataBaseRepo.GetAllMealsAsync().Result;

            // Journal Templates
            JournalTemplates = App.DataBaseRepo.GetAllJournalTemplatesAsync().Result.ToObservableCollection();
            JournalTemplateMeals = App.DataBaseRepo.GetAllJournalTemplateMealsAsync().Result.ToObservableCollection();

            foreach (JournalTemplate journalTemplate in JournalTemplates)
            {
                foreach (JournalTemplateMeal journalTemplateMeal in JournalTemplateMeals.Where(x=> x.JournalTemplateId == journalTemplate.Id))
                {
                    var meal = AllMeals.FirstOrDefault(y => y.Id == journalTemplateMeal.MealId);

                    if (meal == null)
                        continue;

                    meal.Aliments.Clear();
                    PopulateMeal(meal);
                    journalTemplate.DaysOfWeek.FirstOrDefault(x=> x.DayOfWeek == journalTemplateMeal.DayOfWeek).Meals.Add(meal);    
                }
            }

            if (JournalTemplates.Any())
                CurrentJournalTemplate = JournalTemplates.FirstOrDefault(x => x.Id == User.CurrentJournalTemplateId);


            // Exercices
            Exercices = App.DataBaseRepo.GetAllExercicesAsync().Result.ToObservableCollection();

            // Workouts
            Workouts = App.DataBaseRepo.GetAllWorkoutsAsync().Result.ToObservableCollection();

            // WorkoutExercices
            WorkoutExercices = App.DataBaseRepo.GetAllWorkoutExercicesAsync().Result.ToObservableCollection();

            // Sets
            Sets = App.DataBaseRepo.GetAllSetsAsync().Result.ToObservableCollection();

            // MuscleGroup
            MuscleGroups = App.DataBaseRepo.GetAllMuscleGroupsAsync().Result.ToObservableCollection();
            // Create default muscle goups if none
            if(!MuscleGroups.Any())
            {
                foreach (MuscleGroupEnum muscleGroupEnum in (MuscleGroupEnum[])Enum.GetValues(typeof(MuscleGroupEnum)))
                {
                    MuscleGroup muscleGroup = new MuscleGroup() { Name = muscleGroupEnum.ToString() };
                    App.DataBaseRepo.AddMuscleGroupAsync(muscleGroup).Wait();
                    MuscleGroups.Add(muscleGroup);
                }
            }

            // Set Muscle group dans Exercice
            foreach(Exercice exercice in Exercices)
            {
                MuscleGroup muscleGroup = MuscleGroups.FirstOrDefault(x => x.Id == exercice.MuscleGroupId);
                exercice.MuscleGroup = muscleGroup;
            }

            // Logs
            Logs = App.DataBaseRepo.GetAllLogsAsync().Result;
            LogMeals = App.DataBaseRepo.GetAllLogMealsAsync().Result;

            foreach (Log log in Logs)
            {
                log.Meals = new List<Meal>();
                log.Workout = Workouts.FirstOrDefault(x => x.Id == log.WorkoutId);

                foreach (LogMeal logMeal in LogMeals.Where(x => x.LogId == log.Id))
                {
                    foreach (Meal meal in AllMeals)
                    {
                        //meal.Aliments.Clear();
                        //PopulateMeal(meal);
                        if (meal.Id == logMeal.MealId)
                            log.Meals.Add(meal);
                    }
                }
            }

            // TODO remove when clean install
            UpdateDb();

            // WorkoutPrograms
            WorkoutPrograms = App.DataBaseRepo.GetAllWorkoutProgramsAsync().Result.ToObservableCollection();

            // WorkoutWeeks
            WorkoutWeeks = App.DataBaseRepo.GetAllWorkoutWeeksAsync().Result.ToObservableCollection();

            // WorkoutWeekPrograms
            WorkoutWeekPrograms = App.DataBaseRepo.GetAllWorkoutWeekProgramsAsync().Result.ToObservableCollection();

            // WorkoutWeekRoutines
            WorkoutWeekRoutines = App.DataBaseRepo.GetAllWorkoutWeekRoutinesAsync().Result.ToObservableCollection();


            // Fill WorkoutWeeks
            foreach (WorkoutWeek workoutWeek in WorkoutWeeks)
            {
                foreach (WorkoutWeekRoutine workoutWeekRoutine in WorkoutWeekRoutines)
                {
                    if (workoutWeek.Id == workoutWeekRoutine.WorkoutWeekId)
                    {
                        Workout workout = Workouts.SingleOrDefault(x => x.Id == workoutWeekRoutine.WorkoutId);

                        if (workout == null)
                            continue;

                        // fill workout routine with exercices
                        PopulateWorkout(workout);

                        workoutWeek.Workouts.Add(workout);
                    }
                }
            }

            // Fill WorkoutPrograms
            foreach (WorkoutProgram workoutProgram in WorkoutPrograms)
            {
                foreach (WorkoutWeekProgram workoutWeekProgram in WorkoutWeekPrograms)
                {
                    if (workoutWeekProgram.WorkoutProgramId == workoutProgram.Id)
                    {
                        WorkoutWeek workoutWeek = WorkoutWeeks.SingleOrDefault(x => x.Id == workoutWeekProgram.WorkoutWeekId);

                        if (workoutWeek == null)
                            continue;

                        workoutProgram.WorkoutWeeks.Add(workoutWeek);
                    }
                }

                // Set SelectedWorkoutWeek
                workoutProgram.SelectedWorkoutWeek = workoutProgram.WorkoutWeeks.FirstOrDefault();
            }


            GetMealsAtDate(DateTime.Now);
            GetWorkoutAtDay(DateTime.Now);
        }

        /// <summary>
        /// Called when editing Journal template
        /// </summary>
        /// <param name="dayOfWeek"></param>
        public void CreateJournalTemplates(DayOfWeek dayOfWeek)
        {
            User.DailyProteins = 0;
            User.DailyCarbs = 0;
            User.DailyFats = 0;
            User.DailyCalories = 0;

            Meals.Clear();

            foreach (JournalTemplateMeal journalTemplateMeal in JournalTemplateMeals.Where(x=> x.JournalTemplateId == CurrentJournalTemplate.Id && x.DayOfWeek == dayOfWeek))
            {
                var journalMeal = AllMeals.FirstOrDefault(x => x.Id == journalTemplateMeal.MealId);

                if (journalMeal != null && journalMeal.Order <= TemplateMeals.Count)
                {
                    journalMeal.Aliments.Clear();
                    PopulateMeal(journalMeal);
                    Meals.Add(journalMeal);
                }
            }
        }

        public void GetMealsAtDate(DateTime date)
        {
            User.DailyProteins = 0;
            User.DailyCarbs = 0;
            User.DailyFats = 0;
            User.DailyCalories = 0;

            Meals.Clear();

            Log currentLog = Logs.FirstOrDefault(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day);
            var isCurrentOrFutureDay = date.Date >= DateTime.Now.Date;

            if (currentLog != null)
            {
                var todayLogMeals = LogMeals.Where(x => x.LogId == currentLog.Id);

                // Check if new meal templates added if yes add them
                // Only applies to days >= Today
                if(isCurrentOrFutureDay)
                {
                    if (todayLogMeals.Count() < TemplateMeals.Count)
                    {
                        for (int i = todayLogMeals.Count(); i < TemplateMeals.Count; i++)
                        {
                            Meal meal = new Meal() { Name = TemplateMeals[i].Name, Order = TemplateMeals[i].Order };
                            App.DataBaseRepo.AddMealAsync(meal).Wait();
                            AllMeals.Add(meal);
                            LogMeal logMeal = new LogMeal() { LogId = currentLog.Id, MealId = meal.Id };
                            App.DataBaseRepo.AddLogMealAsync(logMeal).Wait();
                            LogMeals.Add(logMeal);
                        }
                    }

                    // Refresh todayLogMeals
                    todayLogMeals = LogMeals.Where(x => x.LogId == currentLog.Id);

                    // Update log
                    App.DataBaseRepo.UpdateLogAsync(currentLog).Wait();
                }


                // Fill Meals
                foreach (LogMeal logMeal in todayLogMeals)
                {
                    var meal = AllMeals.FirstOrDefault(x => x.Id == logMeal.MealId);

                    if (meal != null)
                    {
                        // meal.Order <= TemplateMeals.Count in order to not include TemplateMeals that were disabled
                        // Allow only for pass days
                        if (isCurrentOrFutureDay && meal.Order > TemplateMeals.Count)
                            continue;

                        // Add aliments to Meal if any
                        PopulateMeal(meal);
                        Meals.Add(AllMeals.FirstOrDefault(x => x.Id == logMeal.MealId));
                    }
                }
            }
            else
            {
                // Add new log
                Log log = new Log()
                {
                    Date = date,
                    UserWeight = User.Weight,
                    UserBodyFat = User.BodyFat
                };
                log.Meals = new List<Meal>();

                // Add log to db
                App.DataBaseRepo.AddLogAsync(log).Wait();
                Logs.Add(log);



                if (User.AutoGenerateJournalEnabled && isCurrentOrFutureDay)
                     AutoGenerateMeals(date, log);
                else
                     GenerateDefaultMeals(date, log);
            }
        }

        private void InitDefaultMeals()
        {
            // Populate default meals
            DefaultMeals = new List<Meal>();
            // Breakfast
            var breakfast = new Meal() { Name = "Breakfast", Order = 1 };
            // Lunch
            var lunch = new Meal() { Name = "Lunch", Order = 2 };
            // Dinner
            var dinner = new Meal() { Name = "Dinner", Order = 3 };
            // Snacks
            var snack = new Meal() { Name = "Snack", Order = 4 };
            DefaultMeals.Add(breakfast);
            DefaultMeals.Add(lunch);
            DefaultMeals.Add(dinner);
            DefaultMeals.Add(snack);


            // get all TemplateMeals
            TemplateMeals = App.DataBaseRepo.GetAllTemplateMealsAsync().Result;

            if(!TemplateMeals.Any())
            {
                foreach(Meal meal in DefaultMeals)
                {
                    TemplateMeal templateMeal = new TemplateMeal() { Name = meal.Name, Order = meal.Order };
                    TemplateMeals.Add(templateMeal);
                    App.DataBaseRepo.AddTemplateMealAsync(templateMeal).Wait();
                }
            }
        }

        /// <summary>
        /// Generate meals using template
        /// </summary>
        /// <param name="date"></param>
        private async void AutoGenerateMeals(DateTime date, Log log)
        {
            // Get template for the given day
            DayOfWeekHelper dayOfWeekHelper = CurrentJournalTemplate.DaysOfWeek.FirstOrDefault(x => x.DayOfWeek == date.DayOfWeek);
            if (dayOfWeekHelper == null)
                return;

            // Copy all meals and aliments
            // TODO Check strategy for templated meals
            foreach (Meal copiedMeal in dayOfWeekHelper.Meals)
            {
                Meal meal = new Meal()
                {
                    Name = copiedMeal.Name,
                    Order = copiedMeal.Order
                };
                await App.DataBaseRepo.AddMealAsync(meal);
                AllMeals.Add(meal);
                PopulateMeal(meal, copiedMeal);
                Meals.Add(meal);

                LogMeal logMeal = new LogMeal()
                {
                    LogId = log.Id,
                    MealId = meal.Id
                };
                await App.DataBaseRepo.AddLogMealAsync(logMeal);
                LogMeals.Add(logMeal);
                log.Meals.Add(meal);
            }

            UpdateDailyValues();
        }

        private async void GenerateDefaultMeals(DateTime date, Log log)
        {
            // Add meals
            foreach (TemplateMeal templateMeal in TemplateMeals)
            {
                Meal meal = new Meal() { Name = templateMeal.Name, Order = templateMeal.Order };
                await App.DataBaseRepo.AddMealAsync(meal);
                Meals.Add(meal);
                AllMeals.Add(meal);
                log.Meals.Add(meal);
                var logMeal = new LogMeal() { LogId = log.Id, MealId = meal.Id };
                await App.DataBaseRepo.AddLogMealAsync(logMeal);
                LogMeals.Add(logMeal);
            }

            // Update log
            await App.DataBaseRepo.UpdateLogAsync(log);
        }

        public void PopulateMeal(Meal meal, Meal copyFromMeal = null, bool updateDailyValues = true)
        {
            meal.Aliments.Clear();

            int mealId = copyFromMeal != null ? copyFromMeal.Id : meal.Id;

            foreach (MealAliment mealAliment in MealAliments.Where(x => x.MealId == mealId).ToList())
            {
                Aliment existingAliment = Aliments.FirstOrDefault(x => x.Id == mealAliment.AlimentId && x.AlimentType == mealAliment.AlimentType);

                if (existingAliment != null)
                {
                    var ratio = mealAliment.ServingSize / existingAliment.OriginalServingSize;
                    Aliment aliment = CreateAndCopyAlimentProperties(existingAliment, ratio);
                    aliment.MealAlimentId = mealAliment.Id;
                    aliment.ServingSize = mealAliment.ServingSize;

                    meal?.Aliments.Add(aliment);

                    if(copyFromMeal != null)
                    {
                        MealAliment newMealAliment = new MealAliment() { AlimentId = aliment.Id, MealId = meal.Id, ServingSize = aliment.ServingSize };
                        App.DataBaseRepo.AddMealAlimentAsync(newMealAliment).Wait();
                        aliment.MealAlimentId = newMealAliment.Id;
                        MealAliments.Add(newMealAliment);
                    }

                    // Update daily values
                    if(updateDailyValues)
                    {
                        User.DailyProteins += aliment.Proteins;
                        User.DailyCarbs += aliment.Carbs;
                        User.DailyFats += aliment.Fats;
                        User.DailyCalories += aliment.Calories;
                    }
                }
            }

            // Update meal values
            UpdateMealValues(meal);
        }

        public void AddAliment(Aliment aliment, Meal meal)
        {
            // Add aliment to meal
            meal.Aliments.Add(aliment);

            // Update meal values
            UpdateMealValues(meal);

            // Update daily values
            UpdateDailyValues();

            CreateMealAliment(aliment, meal);
        }

        public void CreateMealAliment(Aliment aliment, Meal meal)
        {
            MealAliment mealAliment = new MealAliment();
            mealAliment.MealId = meal.Id;
            mealAliment.AlimentId = aliment.Id;
            mealAliment.ServingSize = aliment.ServingSize;
            mealAliment.AlimentType = aliment.AlimentType;

            // Save to db
            App.DataBaseRepo.AddMealAlimentAsync(mealAliment).Wait();

            // Asign MealAlimentId to aliment and add it to MealAliments
            aliment.MealAlimentId = mealAliment.Id;
            MealAliments.Add(mealAliment);
        }

        public Aliment CreateAndCopyAlimentProperties(Aliment existingAliment, double ratio = 1)
        {
            Aliment aliment;

            if (existingAliment.AlimentType == Enums.AlimentTypeEnum.Recipe)
            {
                aliment = new Recipe();
                (aliment as Recipe).Description = (existingAliment as Recipe).Description;
                foreach(Food food in (existingAliment as Recipe).Foods)
                    (aliment as Recipe).Foods.Add(food);

                foreach(RecipeInstruction recipeInstruction in (existingAliment as Recipe).RecipeInstructions)
                    (aliment as Recipe).RecipeInstructions.Add(recipeInstruction);
            }
            else
            {
                aliment = new Food();
            }


            // Fill properties
            aliment.Id = existingAliment.Id;
            aliment.Name = existingAliment.Name;
            aliment.ImageSourcePath = existingAliment.ImageSourcePath;
            aliment.ImageBlob = existingAliment.ImageBlob;
            aliment.Unit = existingAliment.Unit;

            aliment.Calories = existingAliment.Calories * ratio;
            aliment.Proteins = existingAliment.Proteins * ratio;
            aliment.Carbs = existingAliment.Carbs * ratio;
            aliment.Fibers = existingAliment.Fibers * ratio;
            aliment.Sugars = existingAliment.Sugars * ratio;
            aliment.Fats = existingAliment.Fats * ratio;
            aliment.SaturatedFat = existingAliment.SaturatedFat * ratio;
            aliment.Salt = existingAliment.Salt * ratio;
            aliment.Sodium = existingAliment.Sodium * ratio;

            aliment.ServingQuantity = existingAliment.ServingQuantity;
            aliment.ServingQuantityUnit = existingAliment.ServingQuantityUnit;

            aliment.CaloriesServing = existingAliment.CaloriesServing;
            aliment.ProteinsServing = existingAliment.ProteinsServing;
            aliment.CarbsServing = existingAliment.CarbsServing;
            aliment.FibersServing = existingAliment.FibersServing;
            aliment.SugarsServing = existingAliment.SugarsServing;
            aliment.FatsServing = existingAliment.FatsServing;
            aliment.SaturatedFatServing = existingAliment.SaturatedFatServing;
            aliment.SaltServing = existingAliment.SaltServing;
            aliment.SodiumServing = existingAliment.SodiumServing;

            aliment.ServingSize = existingAliment.ServingSize;
            aliment.OriginalServingSize = existingAliment.OriginalServingSize;


            return aliment;
        }

        public void UpdateDailyValues()
        {
            double proteins = 0;
            double carbs = 0;
            double fats = 0;
            double calories = 0;

            foreach (Meal meal in Meals)
            {
                proteins += meal.Proteins;
                carbs += meal.Carbs;
                fats += meal.Fats;
                calories += meal.Calories;
            }

            User.DailyProteins = proteins;
            User.DailyCarbs = carbs;
            User.DailyFats = fats;
            User.DailyCalories = calories;

            User.NotifyProgressBars();
        }

        public void UpdateMealValues(Meal meal, double ratio = 1)
        {
            double proteins = 0;
            double carbs = 0;
            double fats = 0;
            double calories = 0;

            foreach (Aliment aliment in meal.Aliments)
            {
                proteins += aliment.Proteins;
                carbs += aliment.Carbs;
                fats += aliment.Fats;
                calories += aliment.Calories;
            }

            meal.Proteins = proteins * ratio;
            meal.Carbs = carbs * ratio;
            meal.Fats = fats * ratio;
            meal.Calories = calories * ratio;
        }

        public void UpdateRecipeValues(Recipe recipe, double ratio = 1)
        {
            double proteins = 0;
            double carbs = 0;
            double fibers = 0;
            double sugars = 0;
            double fats = 0;
            double saturatedFat = 0;
            double sodium = 0;
            double salt = 0;
            double calories = 0;

            foreach (Food food in recipe.Foods)
            {
                proteins += food.Proteins;
                carbs += food.Carbs;
                fibers += food.Fibers;
                sugars += food.Sugars;
                fats += food.Fats;
                saturatedFat += food.SaturatedFat;
                sodium += food.Sodium;
                salt += food.Salt;
                calories += food.Calories;
            }

            recipe.Proteins = proteins * ratio;
            recipe.Carbs = carbs * ratio;
            recipe.Fibers = fibers * ratio;
            recipe.Sugars = sugars * ratio;
            recipe.Fats = fats * ratio;
            recipe.SaturatedFat = saturatedFat * ratio;
            recipe.Sodium = sodium * ratio;
            recipe.Salt = salt * ratio;
            recipe.Calories = calories * ratio;
        }

        private void InitUser()
        {
            // Bmr formulas
            BMRFormulas = new List<string>();
            BMRFormulas.Add("Mifflin - St Jeor");
            BMRFormulas.Add("Harris-Benedict");
            BMRFormulas.Add("Revised Harris-Benedict");
            BMRFormulas.Add("Katch-McArdle");
            BMRFormulas.Add("Schofield");

            // Type of regimes
            TypesOfRegime = new List<TypeOfRegimeItem>();
            TypesOfRegime.Add(new TypeOfRegimeItem()
            {
                TypeOfRegime = TypesOfRegimeEnum.Standard,
                Name = "Standard",
                //Description = "Carbs 50%, Proteins 20%, Fats 30%",
                CarbsPercentage = 0.5,
                ProteinPercentage = 0.2,
                FatsPercentage = 0.3
            });
            TypesOfRegime.Add(new TypeOfRegimeItem()
            {
                TypeOfRegime = TypesOfRegimeEnum.Balanced,
                Name = "Balanced",
                //Description = "Carbs 50%, Proteins 25%, Fats 25%",
                CarbsPercentage = 0.5,
                ProteinPercentage = 0.25,
                FatsPercentage = 0.35
            });
            TypesOfRegime.Add(new TypeOfRegimeItem()
            {
                TypeOfRegime = TypesOfRegimeEnum.LowInFats,
                Name = "Low in fats",
                //Description = "Carbs 60%, Proteins 25%, Fats 15%",
                CarbsPercentage = 0.6,
                ProteinPercentage = 0.25,
                FatsPercentage = 0.15
            });
            TypesOfRegime.Add(new TypeOfRegimeItem()
            {
                TypeOfRegime = TypesOfRegimeEnum.RichInProteins,
                Name = "Rich in proteins",
                //Description = "Carbs 25%, Proteins 40%, Fats 35%",
                CarbsPercentage = 0.25,
                ProteinPercentage = 0.4,
                FatsPercentage = 0.35
            });
            TypesOfRegime.Add(new TypeOfRegimeItem()
            {
                TypeOfRegime = TypesOfRegimeEnum.Keto,
                Name = "Keto",
                //Description = "Carbs 5%, Proteins 30%, Fats 65%",
                CarbsPercentage = 0.05,
                ProteinPercentage = 0.3,
                FatsPercentage = 0.65
            });

            TypeOfRegimeItem customTypeOfRegime = App.DataBaseRepo.GetTypeOfRegimeItemAsync().Result;
            if (customTypeOfRegime == null)
            {
                customTypeOfRegime = new TypeOfRegimeItem()
                {
                    TypeOfRegime = TypesOfRegimeEnum.Custom,
                    Name = "Custom",
                    //Description = "Carbs 35%, Proteins 35%, Fats 30%",
                    CarbsPercentage = 0.35,
                    ProteinPercentage = 0.35,
                    FatsPercentage = 0.30
                };

                App.DataBaseRepo.AddTypeOfRegimeItemAsync(customTypeOfRegime).Wait();
            }
            TypesOfRegime.Add(customTypeOfRegime);


            // PAL
            PhysicalActivityLevels = new List<PALItem>();
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "little / no exercise",
                Description = "sedentary lifestyle",
                PALItemType = PALItemTypeEnum.Little_none_exercise,
                Ratio = 1.2
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "light exercise",
                Description = "1 - 2 times / week",
                PALItemType = PALItemTypeEnum.Light_exercise,
                Ratio = 1.375
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "moderate exercise",
                Description = "3 - 4 times / week",
                PALItemType = PALItemTypeEnum.Moderate_exercise,
                Ratio = 1.55
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "hard exercise",
                Description = "4 - 5 times / week",
                PALItemType = PALItemTypeEnum.Hard_exercise,
                Ratio = 1.725
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "physical job or hard exercise",
                Description = "6 - 7 times / week",
                PALItemType = PALItemTypeEnum.PhysicalJob_hard_exercise,
                Ratio = 1.9
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "professional athlete",
                Description = "Extreme training",
                PALItemType = PALItemTypeEnum.Professional_athelete,
                Ratio = 2.4
            });

            // Objectifs
            Objectifs = new List<ObjectifItem>();
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Lose Weight 20%",
                ObjectifType = ObjectifTypeEnum.Lose_Weight_20,
                Description = "Decrease caloric intake by 20%",
                Ratio = 0.8
            }); ;
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Lose Weight slowly 10%",
                ObjectifType = ObjectifTypeEnum.Lose_Weight_slowly_10,
                Description = "Decrease caloric intake by 10%",
                Ratio = 0.9
            });
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Maintain Weight",
                ObjectifType = ObjectifTypeEnum.Maintain_Weight,
                Description = "Keep the recommended caloric intake",
                Ratio = 1
            });
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Gain Weight slowly 10%",
                ObjectifType = ObjectifTypeEnum.Gain_Weight_slowly_10,
                Description = "Increase caloric intake by 10%",
                Ratio = 1.1
            });
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Gain Weight 20%",
                ObjectifType = ObjectifTypeEnum.Gain_Weight_20,
                Description = "Increase caloric intake by 20%",
                Ratio = 1.2
            });


            User = App.DataBaseRepo.GetUserAsync().Result;

            if (User == null)
            {
                User = new User();
                //User = new User() {Name = "Rade", Age = 32, Height = 180, Weight = 69, BodyFat = 14.5, Gender = Enums.GenderEnum.Male, TDEE = 2986, TargetProteins = 300, TargetCarbs = 323, TargetFats = 89 };
            }
            else
            {
                User.SelectedTypeOfRegime = TypesOfRegime.FirstOrDefault(x => x.TypeOfRegime == User.SelectedTypeOfRegimeDB);
                User.SelectedPhysicalActivityLevel = PhysicalActivityLevels.FirstOrDefault(x => x.PALItemType == User.SelectedPhysicalActivityLevelDB);
                User.SelectedObjectif = Objectifs.FirstOrDefault(x => x.ObjectifType == User.SelectedObjectiflDB);
                User.SelectedObjectif.IsSelected = true;
                User.SelectedPhysicalActivityLevel.IsSelected = true;
                User.SelectedTypeOfRegime.IsSelected = true;    
            }
        }

        /// <summary>
        /// Returns Log for the given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Log GetLog(DateTime date)
        {
            if (!Logs.Any())
                return null;

            return Logs.FirstOrDefault(x => x.Date.Date == date.Date);
        }

        public bool IsAlimentEqual(Aliment aliment, Aliment existingAliment)
        {
            if (aliment.Calories == existingAliment.Calories &&
               aliment.Proteins == existingAliment.Proteins &&
               aliment.Carbs == existingAliment.Carbs &&
               aliment.Fats == existingAliment.Fats)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Get and set the current workout for the given day
        /// </summary>
        /// <param name="date"></param>
        public async void GetWorkoutAtDay(DateTime date)
        {
            Log currentLog = Logs.FirstOrDefault(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day);

            if (currentLog != null)
            {
                if(currentLog.WorkoutId == 0)
                {
                    // Add workout
                    Workout workout = new Workout() { Date = currentLog.Date };
                    await App.DataBaseRepo.AddWorkoutAsync(workout);
                    Workouts.Add(workout);
                    currentLog.WorkoutId = workout.Id;
                    currentLog.Workout = workout;

                    // Update log
                    await App.DataBaseRepo.UpdateLogAsync(currentLog);
                }

                CurrentWorkout = Workouts.FirstOrDefault(x => x.Id == currentLog.WorkoutId);

                // Populate workout
                PopulateWorkout(CurrentWorkout);
            }
            else
            {
                CurrentWorkout.Exercices.Clear();

                // Add log
                Log log = new Log() { Date = date, UserWeight = User.Weight, UserBodyFat = User.BodyFat };
                log.Meals = new List<Meal>();

                // Add log to db
                await App.DataBaseRepo.AddLogAsync(log);
                Logs.Add(log);

                // Add workout
                Workout workout = new Workout() { Date = log.Date };
                await App.DataBaseRepo.AddWorkoutAsync(workout);
                Workouts.Add(workout);
                log.WorkoutId = workout.Id;
                log.Workout = workout;

                // Update log
                await App.DataBaseRepo.UpdateLogAsync(log);
                CurrentWorkout = Workouts.FirstOrDefault(x => x.Id == log.WorkoutId);
            }
        }

        public void PopulateWorkout(Workout workout)
        {
            workout.Exercices.Clear();

            foreach (WorkoutExercice workoutExercice in WorkoutExercices.Where(x=> x.WorkoutId == workout.Id))
            {
                Exercice existingExercice = Exercices.FirstOrDefault(x=> x.Id == workoutExercice.ExerciceId);

                if (existingExercice == null)
                    continue;

                // Copy exercice
                Exercice exercice = CreateAndCopyExerciceProperties(existingExercice);
                exercice.WorkoutExerciceId = workoutExercice.Id;
                exercice.RestTimeBetweenSets = workoutExercice.RestTimeBetweenSets;
                exercice.Sets.Clear();

                // Add sets to exercice
                foreach (var set in Sets)
                {
                    if (set.WorkoutExerciceId == workoutExercice.Id)
                        exercice.Sets.Add(set);
                }

                workout.Exercices.Add(exercice);
            }

            workout.SetAndNotifyMainProperties();
        }

        /// <summary>
        /// Creates a copy from existing exercice
        /// </summary>
        /// <param name="existingExercice"></param>
        /// <returns></returns>
        public Exercice CreateAndCopyExerciceProperties(Exercice existingExercice)
        {
            Exercice exercice = new Exercice()
            {
                Id = existingExercice.Id,
                Name = existingExercice.Name,
                Description = existingExercice.Description,
                MuscleGroup = existingExercice.MuscleGroup,
                MuscleGroupId = existingExercice.MuscleGroupId,
                ImageSourcePath = existingExercice.ImageSourcePath,
                ImageBlob = existingExercice.ImageBlob,
                RestTimeBetweenSets= existingExercice.RestTimeBetweenSets
            };

            foreach (Set set in existingExercice.Sets)
                exercice.Sets.Add(set);

            return exercice;
        }

        /// <summary>
        /// Returns WorkoutExercice where the exercice was last performed
        /// </summary>
        /// <param name="exercice"></param>
        /// <returns></returns>
        private WorkoutExercice GetLastWorkoutExercice(Exercice exercice)
        {
            foreach (Log log in Logs.Where(x => x.Date.Date != DateTime.Now.Date).OrderByDescending(x => x.Date))
            {
                foreach (WorkoutExercice workoutExercice in WorkoutExercices.Where(x => x.WorkoutId == log.WorkoutId && x.ExerciceId == exercice.Id))
                    if (workoutExercice != null)
                        return workoutExercice;
            }

            return null;
        }

        /// <summary>
        /// Returns a Enumerable<set> for the last time performance of an exercice
        /// </summary>
        /// <param name="workoutExercice"></param>
        /// <returns></returns>
        public ExerciceHistoryHelper GetExerciceHistoryLastTime(Exercice exercice)
        {
            var workoutExercice = GetLastWorkoutExercice(exercice);
            if (workoutExercice != null)
            {
                ExerciceHistoryHelper exerciceHistoryHelper = new ExerciceHistoryHelper()
                {
                    Date = workoutExercice.Date,
                    Sets = Sets.Where(x => x.WorkoutExerciceId == workoutExercice.Id).ToList()
                };

                return exerciceHistoryHelper;
            }

            return null;
        }

        /// <summary>
        /// Returns all WorkoutExercices for the given exercice and timeframe
        /// </summary>
        /// <param name="days"></param>
        /// <param name="exercice"></param>
        /// <returns></returns>
        private List<WorkoutExercice> GetWorkoutExercicesOverPeriod(int days, Exercice exercice)
        {
            List<WorkoutExercice> workoutExerciceList = new List<WorkoutExercice>();

            foreach (Log log in Logs.Where(x => x.Date.Date >= DateTime.Now.Date.AddDays(-days) && x.Date.Date < DateTime.Now.Date).OrderByDescending(x => x.Date))
            {
                foreach(WorkoutExercice workoutExercice in WorkoutExercices.Where(x => x.WorkoutId == log.WorkoutId && x.ExerciceId == exercice.Id))
                    workoutExerciceList.Add(workoutExercice);
            }

            return workoutExerciceList;
        }

        /// <summary>
        /// Return a list of ExerciceHistoryHelper which reprsent the exercice history over given period
        /// </summary>
        /// <param name="days"></param>
        /// <param name="exercice"></param>
        /// <returns></returns>
        public List<ExerciceHistoryHelper> GetExerciceHistory(int days, Exercice exercice)
        {
            List<ExerciceHistoryHelper> history = new List<ExerciceHistoryHelper>();   

            var workoutExercices = GetWorkoutExercicesOverPeriod(days, exercice);
            if (workoutExercices != null)
            {
                foreach (WorkoutExercice workoutExercice in workoutExercices)
                {
                    ExerciceHistoryHelper exerciceHistoryHelper = new ExerciceHistoryHelper()
                    {
                        Date = workoutExercice.Date,
                        Sets = Sets.Where(x => x.WorkoutExerciceId == workoutExercice.Id).ToList()
                    };

                    history.Add(exerciceHistoryHelper);
                }
                return history;
            }
            else
                return null;
        }

        private void UpdateDb()
        {
            foreach(Log log in Logs)
            {
                if (log.WorkoutId == 0)
                    continue;

                var workout = Workouts.Single(x => x.Id == log.WorkoutId);
                workout.Date = log.Date;

                foreach(WorkoutExercice workoutExercice in WorkoutExercices.Where(x=> x.WorkoutId == workout.Id))
                {
                    workoutExercice.Date = workout.Date;
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
