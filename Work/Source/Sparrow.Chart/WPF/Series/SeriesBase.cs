using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows;

#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
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
    /// Base for All Series 
    /// </summary>
    public abstract class SeriesBase : Control
    {
        protected double xMin { get; set; }
        protected double xMax { get; set; }
        protected double yMin { get; set; }
        protected double yMax { get; set; }
        double xDifference;
        double yDifference;
        double xAbs;
        double yAbs;
        internal List<double> xValues;
        internal List<double> yValues;
        internal SeriesContainer seriesContainer;
        internal bool isFill;
        protected bool isRefreshed;
        internal bool isPointsGenerated;

        public virtual void GenerateDatas()
        {
        }
        

        public SeriesBase()
        {
            this.DefaultStyleKey = typeof(SeriesBase);
            this.Parts = new PartsCollection();
            this.Points = new PointsCollection();
        }

        internal void CheckMinMaxandInterval(Double value, AxisBase axis)
        {
            axis.m_MinValue = Math.Min(axis.m_MinValue, value);
            axis.m_MaxValue = Math.Max(axis.m_MaxValue, value);
        }
        
        public virtual void CalculateMinAndMax()
        {
            if (this.XAxis != null)
            {
                xMin = this.XAxis.m_MinValue;
                xMax = this.XAxis.m_MaxValue;
            }
            if (this.YAxis != null)
            {
                yMin = this.YAxis.m_MinValue;
                yMax = this.YAxis.m_MaxValue;
            }
        }

        public Point NormalizePoint(Point pt)
        {
            Point result = new Point();
            //if (this.XAxis != null)
            //    result.X = this.XAxis.DataToPoint(pt.X);
            //if (this.YAxis != null)
            //    result.Y = this.YAxis.DataToPoint(pt.Y);
            result.X = ((pt.X - xMin) * (seriesContainer.collection.ActualWidth / (xMax - xMin)));
            result.Y = (seriesContainer.collection.ActualHeight - ((pt.Y - yMin) * seriesContainer.collection.ActualHeight) / (yMax - yMin));
            return result;
        }       

        public virtual void Refresh()
        {
            if (this.XAxis != null)
            {
                this.XAxis.m_MinValue = 0;
                this.XAxis.m_MaxValue = 1;
                this.XAxis.CalculateIntervalFromSeriesPoints();
                this.XAxis.Refresh();
            }
            if (this.YAxis != null)
            {
                this.YAxis.m_MinValue = 0;
                this.YAxis.m_MaxValue = 1;
                this.YAxis.CalculateIntervalFromSeriesPoints();
               this.YAxis.Refresh();
            }
        }

        internal virtual SeriesContainer CreateContainer()
        {
            return null;
        }

        public void RefreshWithoutAxis(AxisBase axis)
        {           
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

        protected PointsCollection GetPointsFromValues(List<double> xValues,List<double> yValues)
        {
            PointsCollection tempPoints = new PointsCollection();
            for (int i = 0; (i < xValues.Count && i < yValues.Count); i++)
            {
                ChartPoint point = new ChartPoint() { XValue = xValues[i], YValue = yValues[i] };
                tempPoints.Add(point);
            }
            return tempPoints;
        }

        protected void IntializePoints()
        {
            xDifference = xMax - xMin;
            yDifference = yMax - yMin;
            xAbs = Math.Abs(xDifference / Width);
            yAbs = Math.Abs(yDifference / Height);
        }

        protected bool CheckValuePoint(ChartPoint oldPoint,ChartPoint point)
        {
            //point.YValue <= yMax && point.YValue >= yMin && point.XValue <= xMax && point.XValue >= xMin && 
            if ((Math.Abs(oldPoint.XValue - point.XValue) >= xAbs || Math.Abs(oldPoint.YValue - point.YValue) >= yAbs))
                return true;
            else
                return false;
        }

        public double GetReflectionValueFromItem(string path, Object item)
        {
#if !WINRT
            PropertyInfo propertInfo = item.GetType().GetProperty(path);
#else
            PropertyInfo propertInfo = item.GetType().GetRuntimeProperty(path);
#endif
            FastProperty fastPropertInfo = new FastProperty(propertInfo);

            if (propertInfo != null)
            {
                object value = fastPropertInfo.Get(item);
                if (value is Double || value is int)
                {
                    return Double.Parse(value.ToString());
                }
                else if (value is DateTime)
                {
                    return ((DateTime)value).ToOADate();
                }
                else if (value is String)
                {
                    if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
                        SparrowChart.ActualCategoryValues.Add(value.ToString());
                    return SparrowChart.ActualCategoryValues.IndexOf(value.ToString());
                }
                else
                    throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
            }
            return 0d;
        }

        public double GetReflectionValue(string path, IEnumerable source, int position)
        {
            if (!String.IsNullOrEmpty(path))
            {
                IEnumerator enumerator = source.GetEnumerator();
                double index = 0;
                for (int i = 0; i < position - 1; i++)
                {
                    enumerator.MoveNext();
                }

                if (enumerator.MoveNext())
                {
#if !WINRT
                    PropertyInfo propertInfo = enumerator.Current.GetType().GetProperty(path);
#else
                    PropertyInfo propertInfo = enumerator.Current.GetType().GetRuntimeProperty(path);
#endif
                    FastProperty fastPropertInfo = new FastProperty(propertInfo);
                    if (propertInfo != null)
                    {
                        object value = fastPropertInfo.Get(enumerator.Current);
                        if (value is Double || value is int)
                        {
                            return Double.Parse(value.ToString());
                        }
                        else if (value is DateTime)
                        {
                            return ((DateTime)value).ToOADate();
                        }
                        else if (value is String)
                        {
                            if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
                                SparrowChart.ActualCategoryValues.Add(value.ToString());
                            return SparrowChart.ActualCategoryValues.IndexOf(value.ToString());
                        }
                        else
                            throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
                        
                    }
                }
            }
            return 0d;
        }

        public List<double> GetReflectionValues(string path,IEnumerable source, List<double> oldValues,bool isUpdate)
        {
            List<double> values;
            if(isUpdate)
                values=oldValues;
            else
                values = new List<double>();
            bool notifyCollectionChanged=false;
            if (!string.IsNullOrEmpty(path))
            {
                IEnumerator enumerator = source.GetEnumerator();
                double index = 0d;
                
                if (enumerator.MoveNext())
                {                    
                    if (enumerator.Current is INotifyPropertyChanged)
                        notifyCollectionChanged = true;
#if !WINRT
                    PropertyInfo xPropertInfo = enumerator.Current.GetType().GetProperty(path);
#else
                    PropertyInfo xPropertInfo = enumerator.Current.GetType().GetRuntimeProperty(path);
#endif
                    FastProperty fastPropertInfo = new FastProperty(xPropertInfo);
                    do
                    {                        
                        if (xPropertInfo != null)
                        {
                            object value = fastPropertInfo.Get(enumerator.Current);
                            if (value is Double || value is int)
                            {
                                values.Add(Double.Parse(value.ToString()));
                            }
                            else if (value is DateTime)
                            {
                                values.Add(((DateTime)value).ToOADate());
                            }
                            else if (value is String)
                            {
                                if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
                                    SparrowChart.ActualCategoryValues.Add(value.ToString());
                                values.Add(SparrowChart.ActualCategoryValues.IndexOf(value.ToString()));
                            }
                            else
                                throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
                            index++;
                        }
                    } while (enumerator.MoveNext());
                    if (notifyCollectionChanged)
                    {
                        enumerator.Reset();
                        while (enumerator.MoveNext())
                        {
                            (enumerator.Current as INotifyPropertyChanged).PropertyChanged -= Collection_PropertyChanged;
                            (enumerator.Current as INotifyPropertyChanged).PropertyChanged += Collection_PropertyChanged;
                        }

                    }
                }

            }
            return values;
        }

        virtual protected void Collection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }

        public XAxis XAxis
        {
            get { return (XAxis)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(SeriesBase), new PropertyMetadata(null));        


        public bool IsRefresh
        {
            get { return (bool)GetValue(IsRefreshProperty); }
            set { SetValue(IsRefreshProperty, value); }
        }

        public static readonly DependencyProperty IsRefreshProperty =
            DependencyProperty.Register("IsRefresh", typeof(bool), typeof(SeriesBase), new PropertyMetadata(true));

        public YAxis YAxis
        {
            get { return (YAxis)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(SeriesBase), new PropertyMetadata(null));


        public PartsCollection Parts
        {
            get { return (PartsCollection)GetValue(PartsProperty); }
            set { SetValue(PartsProperty, value); }
        }

        public static readonly DependencyProperty PartsProperty =
            DependencyProperty.Register("Parts", typeof(PartsCollection), typeof(SeriesBase), new PropertyMetadata(null));



        public bool UseSinglePart
        {
            get { return (bool)GetValue(UseSinglePartProperty); }
            set { SetValue(UseSinglePartProperty, value); }
        }

        public static readonly DependencyProperty UseSinglePartProperty =
            DependencyProperty.Register("UseSinglePart", typeof(bool), typeof(SeriesBase), new PropertyMetadata(false));



        public SparrowChart Chart
        {
            get { return (SparrowChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(SeriesBase), new PropertyMetadata(null));



        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(SeriesBase), new PropertyMetadata(null));

#if !WINRT && !WP7 && !WP8
        [TypeConverter(typeof(StringToChartPointConverter))]
#endif
        public PointsCollection Points
        {
            get { return (PointsCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointsCollection), typeof(SeriesBase), new PropertyMetadata(null, OnPointsChanged));

        private static void OnPointsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SeriesBase series = sender as SeriesBase;
            series.PointsChanged(args);           
        }
        internal void PointsChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue is INotifyCollectionChanged)
            {
                (args.OldValue as INotifyCollectionChanged).CollectionChanged -= Points_CollectionChanged;
            }

            if (args.NewValue is INotifyCollectionChanged)
            {
                (args.NewValue as INotifyCollectionChanged).CollectionChanged += Points_CollectionChanged;
            }
            isPointsGenerated = false;
            
            if (this.IsRefresh)
                Refresh();            
        }

        void Points_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            isPointsGenerated = false;
            if (this.IsRefresh)
                Refresh();
        }

        public string XPath
        {
            get { return (string)GetValue(XPathProperty); }
            set { SetValue(XPathProperty, value); }
        }



        public object Label
        {
            get { return (object)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(object), typeof(SeriesBase), new PropertyMetadata(null));



        public static readonly DependencyProperty XPathProperty =
            DependencyProperty.Register("XPath", typeof(string), typeof(SeriesBase), new PropertyMetadata(null));

        internal RenderingMode RenderingMode
        {
            get { return (RenderingMode)GetValue(RenderingModeProperty); }
            set { SetValue(RenderingModeProperty, value); }
        }

        internal static readonly DependencyProperty RenderingModeProperty =
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(SeriesBase), new PropertyMetadata(RenderingMode.Default));

      
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
          DependencyProperty.Register("StrokeThickness", typeof(double), typeof(SeriesBase), new PropertyMetadata(1d));

        protected virtual void SetBindingForStrokeandStrokeThickness(SeriesPartBase part)
        {
            Binding strokeBinding = new Binding();
            strokeBinding.Path = new PropertyPath("Stroke");
            strokeBinding.Source = this;
            Binding strokeThicknessBinding = new Binding();
            strokeThicknessBinding.Path=new PropertyPath("StrokeThickness");
            strokeThicknessBinding.Source = this;
            part.SetBinding(SeriesPartBase.StrokeProperty, strokeBinding);
            part.SetBinding(SeriesPartBase.StrokeThicknessProperty, strokeThicknessBinding);
        }

        private int index;
        internal int Index
        {
            get { return index; }
            set { index = value; }
        }
    }   
}
