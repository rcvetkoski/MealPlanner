using MealPlanner.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static MealPlanner.Views.WorkoutProgramPage;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutProgramPage : ContentPage
    {
        public WorkoutProgramPage()
        {
            InitializeComponent();
            currentPosition = carouselView.Position;
        }

        int currentPosition = 0;
        double scrollRatio = 0;
        bool IsAutoScroll = false;

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            //var item = weeks.Children.ElementAt((sender as CarouselView).Position);
            //var size = item.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins); 
            //slider.WidthRequest = size.Request.Width;
            //slider.TranslateTo(item.X - (size.Request.Width - item.Width) / 2, 0, 100);
            //scrollView.ScrollToAsync(item.X, scrollView.ScrollY, true);
        }

        private void CarouselView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            var sign = Math.Sign(e.HorizontalOffset - carouselView.Width * currentPosition);  
            var currentItem = weeks.Children.ElementAt(currentPosition);
            var nextItem = weeks.Children.Count > (currentPosition + sign) && (currentPosition + sign) >= 0 ? weeks.Children.ElementAt(currentPosition + sign) : null;

            // scrollRatio
            scrollRatio = (e.HorizontalOffset - carouselView.Width * currentPosition) / carouselView.Width;

            double currentItemWidth = currentItem.Width;
            double nextItemWidth = nextItem != null ? nextItem.Width : currentItem.Width;

            //Set slider X position
            double translateX;

            if(sign > 0)
                translateX = currentItem.Bounds.X + scrollRatio * currentItemWidth;
            else
                translateX = currentItem.Bounds.X + scrollRatio * nextItemWidth;

            slider.TranslationX = translateX;

            //Set slider width
            slider.WidthRequest = currentItemWidth - (currentItemWidth - nextItemWidth) * Math.Abs(scrollRatio);

            // Force scrollview to update position if needed
            //Console.WriteLine($"{currentPosition}  ratio {scrollRatio}  offset {e.HorizontalOffset}");
            //Console.WriteLine($"f {e.FirstVisibleItemIndex}  c {e.CenterItemIndex}  l {e.LastVisibleItemIndex}");

            double maxScrollX = scrollView.ContentSize.Width - scrollView.Width;

            //Console.WriteLine($"translateX {translateX}  scrollX {scrollView.ScrollX}");


            var scx = currentItem.Bounds.X + currentItemWidth;
            if (scx > maxScrollX)
                scx = maxScrollX;

            //Console.WriteLine($"scrollX {(scrollView as CustomScrollView).SCROLLX + (scx * scrollRatio)}");
            IsAutoScroll = true;
            double ppp = 0;
            if ((scx * scrollRatio + (scrollView as CustomScrollView).SCROLLX) <= maxScrollX)
            {
                ppp = (scx * scrollRatio + (scrollView as CustomScrollView).SCROLLX);

                if(ppp < 0)
                    (scrollView as CustomScrollView).GetMeheInjection().DoScroll(0, 0);
                else
                    (scrollView as CustomScrollView).GetMeheInjection().DoScroll((scrollView as CustomScrollView).SCROLLX + (scx * scrollRatio), 0);
            }
            else
            {
                (scrollView as CustomScrollView).GetMeheInjection().DoScroll(maxScrollX, 0);
            }

            Console.WriteLine($"ppp {ppp}");


            //// Change currentPositon
            //if (Math.Abs(scrollRatio) >= 1)
            //{
            //    if (scrollRatio > 0)
            //        currentPosition++;
            //    else
            //        currentPosition--;
            //}

            if (scrollRatio > 0)
            {
                if (currentPosition != e.FirstVisibleItemIndex)
                {
                    Console.WriteLine($"Position {e.FirstVisibleItemIndex}");
                    (scrollView as CustomScrollView).SCROLLX = scrollView.ScrollX;

                }

                currentPosition = e.FirstVisibleItemIndex;
            }
            else
            {
                if (currentPosition != e.LastVisibleItemIndex)
                {
                    Console.WriteLine($"Position {e.LastVisibleItemIndex}");
                    (scrollView as CustomScrollView).SCROLLX = scrollView.ScrollX;
                }

                currentPosition = e.LastVisibleItemIndex;
            }



            //if (e.FirstVisibleItemIndex == e.CenterItemIndex && e.CenterItemIndex == e.LastVisibleItemIndex && currentPosition != e.LastVisibleItemIndex)
            //{
            //    if (sign > 0)
            //        currentPosition++;
            //    else
            //        currentPosition--;
            //}
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var position = weeks.Children.IndexOf(sender as View);
            this.carouselView.Position = position;
        }

        private void scrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if(!IsAutoScroll)
                (sender as CustomScrollView).SCROLLX = e.ScrollX;

            IsAutoScroll = false;
        }
    }
}