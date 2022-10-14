using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MealPlanner.Helpers
{
    public interface IStatusBarColor
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
