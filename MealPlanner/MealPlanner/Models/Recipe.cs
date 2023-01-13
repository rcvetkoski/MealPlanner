using MealPlanner.Helpers.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Models
{
    public class Recipe : Aliment
    {
        private ObservableCollection<Food> food;
        [Ignore]
        public ObservableCollection<Food> Foods
        { 
            get 
            { 
                return food;
            } 
            set 
            { 
                if(food != value)
                {
                    food = value;
                    OnPropertyChanged("Foods");
                }
            }
        }

        private ObservableCollection<RecipeInstruction> recipeInstructions;
        [Ignore]
        public ObservableCollection<RecipeInstruction> RecipeInstructions
        {
            get
            {
                return recipeInstructions;
            }
            set
            {
                if (recipeInstructions != value)
                {
                    recipeInstructions = value;
                    OnPropertyChanged("RecipeInstructions");
                }
            }
        }


        public Recipe()
        {
            Foods = new ObservableCollection<Food>();
            RecipeInstructions = new ObservableCollection<RecipeInstruction>();
        }

        private string description;
        public string Description 
        { 
            get
            {
                return description;
            } 
            set 
            { 
                if(description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }      

        public override AlimentTypeEnum AlimentType 
        { 
            get
            {
                return AlimentTypeEnum.Recipe; 
            }
        }
    }
}
