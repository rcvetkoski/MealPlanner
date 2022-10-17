using MealPlanner.Helpers;
using MealPlanner.Services;
using MealPlanner.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner
{
    public partial class App : Application
    {
        public static IDataBase DataBaseRepo;
        public static ReferentialData RefData;
        public static IRestService RestService;
        public static IImageService ImageService;
        public static IStatusBarColor StatusBarColor;


        public App()
        {
            InitializeComponent();

            DependencyService.Register<Database>();
            DependencyService.Register<IRestService, OpenFoodFactsRestService>();
            DataBaseRepo = DependencyService.Get<IDataBase>(DependencyFetchTarget.GlobalInstance);
            RefData = new ReferentialData();
            HttpClientHelper.Initialisation();
            RestService = DependencyService.Get<IRestService>(DependencyFetchTarget.GlobalInstance);
            ImageService = DependencyService.Get<IImageService>();
            StatusBarColor = DependencyService.Get<IStatusBarColor>();


            MainPage = new AppShell();

            // Set theme
            SetTheme();

            // Respond to the theme change
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                if (a.RequestedTheme == OSAppTheme.Light)
                {
                    StatusBarColor.SetStatusBarColor(Color.White, true);
                }
                else if (a.RequestedTheme == OSAppTheme.Dark)
                {
                    StatusBarColor.SetStatusBarColor(Color.FromHex("#1C1C1E"), false);
                }
            };
        }

        private void SetTheme()
        {
            if(Current.RequestedTheme == OSAppTheme.Unspecified || Current.RequestedTheme == OSAppTheme.Light)
                StatusBarColor.SetStatusBarColor(Color.White, true);
            else
                StatusBarColor.SetStatusBarColor(Color.FromHex("#1C1C1E"), false);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
