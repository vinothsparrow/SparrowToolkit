using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Threading;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
#endif

namespace Sparrow.Chart
{
    public class StockChartBase : SeriesBase
    {
        internal PointsCollection lowPoints;
        internal List<double> highValues;
        internal List<double> lowValues;
        public virtual void GeneratePointsFromSource()
        {
            xValues = this.GetReflectionValues(this.XPath, PointsSource, xValues, false);
            yValues = this.GetReflectionValues(this.LowPath, PointsSource, yValues, false);
            lowValues = yValues;
            lowPoints = new PointsCollection();
            if (xValues != null && xValues.Count > 0)
            {
                this.lowPoints = GetPointsFromValues(xValues, yValues);
            }
            xValues = this.GetReflectionValues(this.XPath, PointsSource, xValues, false);
            yValues = this.GetReflectionValues(this.HighPath, PointsSource, yValues, false);
            if (xValues != null && xValues.Count > 0)
            {
                this.Points = GetPointsFromValues(xValues, yValues);               
            }
            highValues = yValues;
        }
        override public void Refresh()
        {
            base.Refresh();
            if (IsRefresh)
            {
#if WPF
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(this.GenerateDatas));
#elif WINRT
                IAsyncAction action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, this.GenerateDatas);
#else
                Dispatcher.BeginInvoke(new Action(this.GenerateDatas));
#endif
               // isRefreshed = true;
            }
        }

        public IEnumerable PointsSource
        {
            get { return (IEnumerable)GetValue(PointsSourceProperty); }
            set { SetValue(PointsSourceProperty, value); }
        }

        public static readonly DependencyProperty PointsSourceProperty =
            DependencyProperty.Register("PointsSource", typeof(IEnumerable), typeof(StockChartBase), new PropertyMetadata(null, OnPointsSourceChanged));

        private static void OnPointsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            StockChartBase series = sender as StockChartBase;
            series.PointsSourceChanged(sender, args);

        }
        internal void PointsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue is INotifyCollectionChanged)
            {
                (args.OldValue as INotifyCollectionChanged).CollectionChanged -= PointsSource_CollectionChanged;
            }

            if (args.NewValue is INotifyCollectionChanged)
            {
                (args.NewValue as INotifyCollectionChanged).CollectionChanged += PointsSource_CollectionChanged;
            }
            GeneratePointsFromSource();
        }

        void PointsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                double xValue = GetReflectionValue(XPath, PointsSource, xValues.Count + 1);
                double yValue = GetReflectionValue(HighPath, PointsSource, yValues.Count + 1);
                this.xValues.Add(xValue);
                this.yValues.Add(yValue);
                this.Points.Add(new ChartPoint() { XValue = xValue, YValue = yValue });
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                IList oldItems = e.OldItems;
                double oldXValue = GetReflectionValueFromItem(XPath, oldItems[0]);
                int index = this.xValues.IndexOf(oldXValue);
                this.xValues.RemoveAt(index);
                this.yValues.RemoveAt(index);
                this.Points.RemoveAt(index);
            }

        }

        public string HighPath
        {
            get { return (string)GetValue(HighPathProperty); }
            set { SetValue(HighPathProperty, value); }
        }

        public static readonly DependencyProperty HighPathProperty =
            DependencyProperty.Register("HighPath", typeof(string), typeof(StockChartBase), new PropertyMetadata(null));

        public string LowPath
        {
            get { return (string)GetValue(LowPathProperty); }
            set { SetValue(LowPathProperty, value); }
        }

        public static readonly DependencyProperty LowPathProperty =
            DependencyProperty.Register("LowPath", typeof(string), typeof(StockChartBase), new PropertyMetadata(null));



        public Brush BearFill
        {
            get { return (Brush)GetValue(BearFillProperty); }
            set { SetValue(BearFillProperty, value); }
        }

        public static readonly DependencyProperty BearFillProperty =
            DependencyProperty.Register("BearFill", typeof(Brush), typeof(StockChartBase), new PropertyMetadata(new SolidColorBrush(Colors.Green)));



        public Brush BullFill
        {
            get { return (Brush)GetValue(BullFillProperty); }
            set { SetValue(BullFillProperty, value); }
        }

        public static readonly DependencyProperty BullFillProperty =
            DependencyProperty.Register("BullFill", typeof(Brush), typeof(StockChartBase), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        
        public PointCollection HighPoints
        {
            get { return (PointCollection)GetValue(HighPointsProperty); }
            set { SetValue(HighPointsProperty, value); }
        }

        public static readonly DependencyProperty HighPointsProperty =
            DependencyProperty.Register("HighPoints", typeof(PointCollection), typeof(StockChartBase), new PropertyMetadata(null));


        public PointCollection LowPoints
        {
            get { return (PointCollection)GetValue(LowPointsProperty); }
            set { SetValue(LowPointsProperty, value); }
        }

        public static readonly DependencyProperty LowPointsProperty =
            DependencyProperty.Register("LowPoints", typeof(PointCollection), typeof(StockChartBase), new PropertyMetadata(null));



    }
}
