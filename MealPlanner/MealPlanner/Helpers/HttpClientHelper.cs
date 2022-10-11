using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace MealPlanner.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient Client;
        public static void Initialisation()
        {
            Client = new HttpClient();
            Client.Timeout = TimeSpan.FromSeconds(15);
        }
    }
}
