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

        public App()
        {
            InitializeComponent();

            DependencyService.Register<Database>();
            DependencyService.Register<IRestService, OpenFoodFactsRestService>();
            DataBaseRepo = DependencyService.Get<IDataBase>(DependencyFetchTarget.GlobalInstance);
            RefData = new ReferentialData();
            HttpClientHelper.Initialisation();
            RestService = DependencyService.Get<IRestService>(DependencyFetchTarget.GlobalInstance);
            MainPage = new AppShell();
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
