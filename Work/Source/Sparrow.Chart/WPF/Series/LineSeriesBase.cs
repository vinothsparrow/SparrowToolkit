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
#endif


namespace Sparrow.Chart
{
    /// <summary>
    /// LineSeries Based Series
    /// </summary>
    public class LineSeriesBase : SeriesBase
    {
        
        public void GeneratePointsFromSource()
        {
            xValues = this.GetReflectionValues(this.XPath, PointsSource, xValues, false);
            yValues = this.GetReflectionValues(this.YPath, PointsSource, yValues, false);

            if (xValues != null && xValues.Count > 0)
            {
                this.Points = GetPointsFromValues(xValues, yValues);
            }
        }
        override public void Refresh()
        {
            base.Refresh();
            if (!isRefreshed && IsRefresh)
            {
#if WPF
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(this.GenerateDatas));
#elif WINRT
                IAsyncAction action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, this.GenerateDatas);
#else
                Dispatcher.BeginInvoke(new Action(this.GenerateDatas));
#endif
                isRefreshed = true;
            }
        }
       
        public IEnumerable PointsSource
        {
            get { return (IEnumerable)GetValue(PointsSourceProperty); }
            set { SetValue(PointsSourceProperty, value); }
        }

        public static readonly DependencyProperty PointsSourceProperty =
            DependencyProperty.Register("PointsSource", typeof(IEnumerable), typeof(LineSeriesBase), new PropertyMetadata(null, OnPointsSourceChanged));

        private static void OnPointsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            LineSeriesBase series = sender as LineSeriesBase;
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
                double yValue = GetReflectionValue(YPath, PointsSource, yValues.Count + 1);
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
             

        public string YPath
        {
            get { return (string)GetValue(YPathProperty); }
            set { SetValue(YPathProperty, value); }
        }

        public static readonly DependencyProperty YPathProperty =
            DependencyProperty.Register("YPath", typeof(string), typeof(LineSeriesBase), new PropertyMetadata(null));
    }
}
