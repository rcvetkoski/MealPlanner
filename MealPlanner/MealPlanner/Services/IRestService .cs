using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public interface IRestService
    {
        /// <summary>
        /// Returns an aliment based on barcode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Aliment> ScanBarCodeAsync(string code);

        /// <summary>
        /// Returns aliments based on search text parameter
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        Task<List<Aliment>> SearchAlimentAsync(string searchText);
    }
}
