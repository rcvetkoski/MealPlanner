using MealPlanner.Models;
using MealPlanner.Views;
using MealPlanner.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;

namespace MealPlanner.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            SetTitle();
            MaximumDate = RefData.CurrentDay.AddDays(7);
            MealOptionsCommand = new Command<Meal>(MealOptions);
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            UpdateAlimentCommand = new Command<object[]>(UpdateAliment);
            AddAlimentCommand = new Command<Meal>(AddAliment);
            OpenUserPageCommand = new Command(OpenUserPage);
            OpenCalendarCommand = new Command<DatePicker>(OpenCalendar);
            PreviousDayCommand = new Command(PreviousDay);
            NextDayCommand = new Command(NextDay);
            ResetCurrentDayCommand = new Command(ResetCurrentDay);
        }

        public DateTime MaximumDate { get; set; }
        public bool NextDayCommandVisible
        {
            get
            {
                return MaximumDate > RefData.CurrentDay.AddDays(1);
            }
        }

        public void SetTitle()
        {
            Title = RefData.CurrentDay.Day == DateTime.Now.Day ? "Today" : RefData.CurrentDay.ToString(("d MMM"));
        }

        public ICommand MealOptionsCommand { get; set; }
        private void MealOptions(Meal meal)
        {
            RSPopup rSPopup = new RSPopup("", "", Xamarin.RSControls.Enums.RSPopupPositionEnum.Bottom);
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            rSPopup.SetPopupSize(Xamarin.RSControls.Enums.RSPopupSizeEnum.MatchParent, Xamarin.RSControls.Enums.RSPopupSizeEnum.WrapContent);
            rSPopup.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.BottomToTop);

            StackLayout stackLayout = new StackLayout() { Margin = 20, Spacing = 20};
            Label label = new Label() { Text = meal.Name };
            Label label1 = new Label() { Text = "Copy alimnets" };
            label1.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => 
                {
                    RefData.CopiedAliments.Clear();
                    foreach(Aliment aliment in meal.Aliments)
                        RefData.CopiedAliments.Add(aliment);

                    rSPopup.Close();
                })
            });

            Label label2 = new Label() { Text = "Paste alimnets", IsVisible = RefData.CopiedAliments.Any() };
            label2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    foreach (Aliment aliment in RefData.CopiedAliments)
                        RefData.AddAliment(aliment, meal);

                    rSPopup.Close();
                })
            });

            Label label3 = new Label() { Text = "Set Hour" };
            Label label4 = new Label() { Text = "Cancel", TextColor = Color.Red };
            stackLayout.Children.Add(label);
            stackLayout.Children.Add(label1);
            stackLayout.Children.Add(label2);
            stackLayout.Children.Add(label3);
            stackLayout.Children.Add(label4);
            rSPopup.SetCustomView(stackLayout);

            rSPopup.Show(); 
        }

        public ICommand DeletteAlimentCommand { get; set; } 
        private async void DeletteAliment(object[] objects)
        {
            if (objects == null)
                return;

            if (objects.Count() < 2)
                return;

            if (!(objects[0] is Meal) || !(objects[1] is Aliment))
                return;


            Meal meal = objects[0] as Meal;
            Aliment aliment = objects[1] as Aliment;

            MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(aliment.MealAlimentId);

            if(mealAliment != null)
            {
                await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                var realMealAliment = RefData.MealAliments.Where(x => x.Id == mealAliment.Id).FirstOrDefault();
                if(realMealAliment != null)
                    RefData.MealAliments.Remove(realMealAliment);
            }

            meal.Aliments.Remove(aliment);

            // Update meal values
            RefData.UpdateMealValues(meal);

            // Update daily values
            RefData.UpdateDailyValues();
        }


        public ICommand UpdateAlimentCommand { get; set; }
        private void UpdateAliment(object[] objects)
        {
            var meal = objects[0] as Meal;
            var aliment = objects[1] as Aliment;

            if (aliment is Aliment)
            {
                RSPopup rSPopup = new RSPopup();
                rSPopup.Title = aliment.Name;
                rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;

                RSPopupAlimentDetailPage rSPopupAlimentDetailPage = new RSPopupAlimentDetailPage();
                rSPopupAlimentDetailPage.BindingContext = new AlimentPopUpViewModel(aliment);
                var rSPopupAlimentDetailPageBindingContext = rSPopupAlimentDetailPage.BindingContext as AlimentPopUpViewModel;
                rSPopup.SetCustomView(rSPopupAlimentDetailPage);


                // Update
                rSPopup.AddAction("Update", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral, new Command(async () =>
                {
                    aliment.Proteins = rSPopupAlimentDetailPageBindingContext.AlimentProteins;
                    aliment.Carbs = rSPopupAlimentDetailPageBindingContext.AlimentCarbs;
                    aliment.Fats = rSPopupAlimentDetailPageBindingContext.AlimentFats;
                    aliment.Calories = rSPopupAlimentDetailPageBindingContext.AlimentCalories;
                    aliment.Unit = rSPopupAlimentDetailPageBindingContext.AlimentUnit;
                    aliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;

                    // Update meal values
                    RefData.UpdateMealValues(meal);

                    // Update daily values
                    RefData.UpdateDailyValues();

                    var al = aliment;
                    MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(aliment.MealAlimentId);
                    mealAliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;
                    await App.DataBaseRepo.UpdateMealAliment(mealAliment);

                    rSPopup.Close();
                }));


                // Delette
                rSPopup.AddAction("Delette", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Destructive, new Command(() => 
                {
                    if(aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                    {
                        DeletteAliment(objects);
                        rSPopup.Close();
                    }
                    else
                        App.Current.MainPage.Navigation.PushAsync(new RecipePage());
                }));


                // Close
                rSPopup.Show();
            }
            else if (aliment is Food)
                App.Current.MainPage.Navigation.PushAsync(new FoodPage());
        }


        public ICommand AddAlimentCommand { get; set; }
        private async void AddAliment(Meal meal)
        {
            AddAlimentPage addAlimentPage = new AddAlimentPage();
            (addAlimentPage.BindingContext as AddAlimentViewModel).SelectedMeal = meal;
            await App.Current.MainPage.Navigation.PushAsync(addAlimentPage);        
            //await Shell.Current.GoToAsync($"AddAlimentPage");
        }


        public ICommand OpenUserPageCommand { get; set; }
        private async void OpenUserPage()
        {
            //await Shell.Current.GoToAsync($"{nameof(UserPage)}");
            await App.Current.MainPage.Navigation.PushAsync(new UserPage());        
        }

        public ICommand OpenCalendarCommand { get; set; }
        private void OpenCalendar(DatePicker datePicker)
        {
            datePicker.Focus();
        }

        public ICommand PreviousDayCommand { get; set; }
        private void PreviousDay()
        {
            RefData.CurrentDay =  RefData.CurrentDay.Subtract(TimeSpan.FromDays(1));
            SetTitle();
            RefData.GetMealsAtDate(RefData.CurrentDay);
            RefData.UpdateDailyValues();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand NextDayCommand { get; set; }
        private void NextDay()
        {
            RefData.CurrentDay = RefData.CurrentDay.AddDays(1);
            SetTitle();
            RefData.GetMealsAtDate(RefData.CurrentDay);
            RefData.UpdateDailyValues();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand ResetCurrentDayCommand { get; set; }
        private void ResetCurrentDay()
        {
            RefData.CurrentDay = DateTime.Now;
            SetTitle();
            RefData.GetMealsAtDate(RefData.CurrentDay);
            RefData.UpdateDailyValues();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }
    }
}