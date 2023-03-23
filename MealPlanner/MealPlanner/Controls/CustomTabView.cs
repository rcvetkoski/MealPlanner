using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MealPlanner.Controls
{
    public class CustomTabView : Grid
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(CustomTabView), null, propertyChanged: OnPropertyChanged);
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomTabView).content.ItemsSource = (IEnumerable)newValue;
            BindableLayout.SetItemsSource((bindable as CustomTabView).tabsContent, (IEnumerable)newValue);
        }


        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CustomTabView), null, BindingMode.TwoWay);
        public object SelectedItem
        {
            get { return (IEnumerable)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        public static readonly BindableProperty ContentItemTemplateProperty = BindableProperty.Create(nameof(ContentItemTemplate), typeof(DataTemplate), typeof(CustomTabView), propertyChanged: OnContentItemTemplatePropertyChanged);
        public DataTemplate ContentItemTemplate
        {
            get { return (DataTemplate)GetValue(ContentItemTemplateProperty); }
            set { SetValue(ContentItemTemplateProperty, value); }
        }
        private static void OnContentItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomTabView).content.ItemTemplate = (DataTemplate)newValue;
        }


        public static readonly BindableProperty TabsItemTemplateProperty = BindableProperty.Create(nameof(TabsItemTemplate), typeof(DataTemplate), typeof(CustomTabView), null, propertyChanged: OnTabsItemTemplatePropertyChanged);
        public DataTemplate TabsItemTemplate
        {
            get { return (DataTemplate)GetValue(TabsItemTemplateProperty); }
            set { SetValue(TabsItemTemplateProperty, value); }
        }
        private static void OnTabsItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BindableLayout.SetItemTemplate((bindable as CustomTabView).tabsContent, (DataTemplate)newValue);
        }


        public static readonly BindableProperty SliderColorProperty = BindableProperty.Create(nameof(SliderColor), typeof(Color), typeof(CustomTabView), Color.White, propertyChanged: OnSliderColorPropertyChanged);
        public Color SliderColor
        {
            get { return (Color)GetValue(SliderColorProperty); }
            set { SetValue(SliderColorProperty, value); }
        }
        private static void OnSliderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomTabView).slider.BackgroundColor = (Color)newValue;   
        }


        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(CustomTabView), Color.White, propertyChanged: OnSeparatorColorPropertyChanged);
        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }
        private static void OnSeparatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomTabView).separator.BackgroundColor = (Color)newValue;
        }


        private object tempVisualElement;
        private CustomScrollView tabs;
        private BoxView slider;
        private StackLayout tabsContent;
        private CarouselView content;
        private BoxView separator;
        private int currentPosition = 0;
        private double scrollRatio = 0;
        private bool IsAutoScroll = false;
        private int FirstIndex = -1;
        private int sign = 0;
        double translateX = 0;
        private double currentScrollX = 0;



        public CustomTabView()
        {
            currentPosition = 0;
            TapCommand = new Command<View>(Tap);
            DefaultTabsItemTemplate = new DataTemplate(() =>
            {
                var label = new Label()
                {
                    Padding = new Thickness(10,15,10,15),
                    FontAttributes = FontAttributes.Bold,   
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                label.SetBinding(Label.TextProperty, ".");

                return label;
            });


            // Main Grid
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(15)});
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            SetContent();
            SetTabs();
            SetSlider();
            SetSeparator();


            // Add to main grid
            Children.Add(tabs, 0, 0);
            Children.Add(separator, 0, 1);
            Children.Add(slider, 0, 1);
            Children.Add(content, 0, 2);

            RowSpacing = 0;
        }

        private void SetSeparator()
        {
            separator = new BoxView()
            {
                HeightRequest = 1,
                Margin = new Thickness(0, 0, 0, 14),
                BackgroundColor = SeparatorColor,
                VerticalOptions = LayoutOptions.Start
            };
        }

        private void SetContent()
        {
            content = new CarouselView()
            {
                Loop = false,
                ItemTemplate = ContentItemTemplate != null ? ContentItemTemplate : null
            };

            content.Scrolled += Content_Scrolled;
            content.CurrentItemChanged += Content_CurrentItemChanged;
            content.VisibleViews.CollectionChanged += VisibleViews_CollectionChanged;
        }

        private void Content_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            SelectedItem = e.CurrentItem;
        }

        private void VisibleViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var first = content.VisibleViews.Count > 0 ? content.VisibleViews[0] : null;
            if (tempVisualElement != first)
            {
                if (tempVisualElement == null)
                    FirstIndex = content.Position;
                else if (sign > 0)
                    FirstIndex++;
                else if (sign < 0)
                    FirstIndex--;

                var currentIndex = FirstIndex > 0 ? FirstIndex : 0;
                if (currentPosition != currentIndex)
                {
                    currentPosition = currentIndex;
                    currentScrollX = tabs.ScrollX;
                    //tabs.SCROLLX = tabs.ScrollX;

                    //Console.WriteLine($"currentScrollX {currentScrollX}");
                }

                tempVisualElement = first;
            }
        }

        private void SetSlider()
        {
            // Slider
            slider = new BoxView()
            {
                HeightRequest = 2,
                HorizontalOptions = LayoutOptions.Start,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Start
            };
        }

        private void SetTabs()
        {
            // Tabs scrollView
            tabs = new CustomScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never
            };

            // tabs content not recycled
            tabsContent = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };
            TabsItemTemplate = DefaultTabsItemTemplate;


            // Add content and slider to grid
            tabs.Content = tabsContent;

            tabs.Scrolled += Tabs_Scrolled;
            tabsContent.ChildAdded += Tabs_ChildAdded;
        }

        private void Tabs_ChildAdded(object sender, ElementEventArgs e)
        {
            var item = (sender as StackLayout).Children.Last();
            item.GestureRecognizers.Add(new TapGestureRecognizer() { Command = TapCommand, CommandParameter = item });

            if ((sender as StackLayout).Children.IndexOf(item) == currentPosition)
                item.SizeChanged += Item_MeasureInvalidated;
        }

        private void Item_MeasureInvalidated(object sender, EventArgs e)
        {
            slider.WidthRequest = (sender as View).Width;
            (sender as View).SizeChanged -= Item_MeasureInvalidated;
        }

        private void Tabs_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (!IsAutoScroll)
            {
                tabs.SCROLLX = e.ScrollX;
                slider.TranslationX = translateX - tabs.ScrollX;
            }

            IsAutoScroll = false;
        }

        private void Content_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            IsAutoScroll = true;
            //var first = content.VisibleViews.Count > 0 ? content.VisibleViews[0] as Label : null;
            //var FirstIndex = first != null ? contentItems.IndexOf(first) : 0;
            //var FirstIndex2 = first != null ? tabsContent.Children.IndexOf(tabsContent.Children.FirstOrDefault(x => (x as Label).Text == first.Text)) : 0;

            var currentIndex = FirstIndex > 0 ? FirstIndex : 0;
            //Console.WriteLine($" currentIndex {currentIndex}        FirstIndex2 {FirstIndex2}");

            sign = Math.Sign(e.HorizontalOffset - content.Width * currentIndex);
            var currentItem = tabsContent.Children.ElementAt(currentIndex);
            var nextItem = tabsContent.Children.Count > (currentIndex + sign) && (currentIndex + sign) >= 0 ? tabsContent.Children.ElementAt(currentIndex + sign) : null;

            // scrollRatio
            scrollRatio = (e.HorizontalOffset - content.Width * currentIndex) / content.Width;

            double currentItemWidth = currentItem.Width;
            double nextItemWidth = nextItem != null ? nextItem.Width : currentItem.Width;

            //Set slider X position
            //double translateX;

            if (sign > 0)
                translateX = currentItem.Bounds.X + scrollRatio * currentItemWidth;
            else
                translateX = currentItem.Bounds.X + scrollRatio * nextItemWidth;

            //slider.TranslationX = translateX;

            //Set slider width
            slider.WidthRequest = currentItemWidth - (currentItemWidth - nextItemWidth) * Math.Abs(scrollRatio);
            //slider.Layout(new Rectangle(slider.Bounds.Location, new Size(currentItemWidth - (currentItemWidth - nextItemWidth) * Math.Abs(scrollRatio), slider.Height)));

            double maxScrollX = tabs.ContentSize.Width - tabs.Width;
            var tx = sign > 0 ? scrollRatio * currentItemWidth : scrollRatio * nextItemWidth;
            double toScrollX = 0;
            
            if(FirstIndex == 0)
                toScrollX = currentItem.Bounds.X + tx / 2;
            else if(FirstIndex > 0)
                toScrollX = currentItem.Bounds.X + tx - tabsContent.Children.ElementAt(0).Bounds.Width / 2;
            else
                toScrollX = currentItem.Bounds.X + tx;


            if (toScrollX < 0)
            {
                tabs.GetMeheInjection().DoScroll(0, 0);
            }
            else if (toScrollX > maxScrollX)
            {
                tabs.GetMeheInjection().DoScroll(maxScrollX, 0);
            }
            else if (toScrollX <= maxScrollX)
            {
                tabs.GetMeheInjection().DoScroll(toScrollX, 0);
            }

            slider.TranslationX = translateX - tabs.ScrollX;
        }

        private ICommand TapCommand;
        private async void Tap(View item) 
        {
            var position = tabsContent.Children.IndexOf(item);
            content.Position = position;


            //item.Opacity = 0;   
            //item.BackgroundColor = Color.LightGray;
            //await item.FadeTo(1);
            //await item.FadeTo(0);
            //item.BackgroundColor = Color.Transparent;
            //item.Opacity = 1;
            //await item.ScaleTo(1.1, 120);
            //await item.ScaleTo(1, 120);
        }

        private DataTemplate DefaultTabsItemTemplate;
    }
}
