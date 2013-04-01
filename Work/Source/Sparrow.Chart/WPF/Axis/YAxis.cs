using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Shapes;
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
    /// YAxis for Sparrow Chart
    /// </summary>
    public abstract class YAxis : AxisBase
    {

        public YAxis()
            : base()
        {

        }

        protected override void GetStyles()
        {
            base.GetStyles();
            this.HeaderTemplate = (DataTemplate)styles["yAxisHeaderTemplate"];
        }

        protected override bool CheckType()
        {
            return true; 
        }

        internal override void InvalidateVisuals()
        {
            if (!isInitialized)
                Initialize();
            else
                Update();
        }
        internal void Update()
        {
            double desiredWidth = 0;
            CalculateAutoInterval();
            GenerateLabels();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
                double yAxisHeightStep = this.ActualHeight / ((m_IntervalCount > 0) ? m_IntervalCount : 1);
                double yAxisHeightPosition = 0;
                Rect oldRect = new Rect(0, 0, 0, 0);
                axisLine.X1 = this.ActualWidth;
                axisLine.X2 = this.ActualWidth;
                axisLine.Y1 = 0;
                axisLine.Y2 = this.ActualHeight;
                Binding binding = new Binding();
                binding.Path = new PropertyPath("AxisLineStyle");
                binding.Source = this;
                axisLine.SetBinding(Line.StyleProperty, binding);
                double labelSize = 0;
                int minorCount = 0;
                if (this.m_Labels.Count == labels.Count)
                {
                    for (int i = this.m_Labels.Count - 1; i >=0; i--)
                    {
                        yAxisHeightPosition = this.DataToPoint(m_startValue + (m_Interval * i));
                        ContentControl label = labels[i];
                        label.Content = m_Labels[i];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        Line tickLine = majorTickLines[i];
                        double labelPadding = 0;
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        tickLine.X1 = this.ActualWidth;
                        tickLine.Y1 = yAxisHeightPosition;
                        tickLine.Y2 = yAxisHeightPosition;
                        tickLine.X2 = tickLine.X1 - MajorLineSize;
                        
                        desiredWidth = 0;
                        double minorstep = 0;                       
                        if (!(i == 0))
                        {
                            double minorWidth = yAxisHeightStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = minorTickLines[minorCount];
                                minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.X1 = this.ActualWidth - (minorLine.StrokeThickness);
                                minorLine.X2 = minorLine.X1 - MinorLineSize;
                                minorCount++;
                            }
                        }
                        if (this.ShowMajorTicks)
                        {
                            labelPadding = tickLine.X2;
                            desiredWidth = this.MajorLineSize;
                        }
                        Canvas.SetLeft(label, this.ActualWidth - (label.DesiredSize.Width) - desiredWidth - 1);
                        Canvas.SetTop(label, yAxisHeightPosition - (label.DesiredSize.Height / 2));
                        labelSize = Math.Max(labelSize, label.DesiredSize.Width);                        
                    }
                }
                else
                {
                    if (this.m_Labels.Count > labels.Count)
                    {
                        int offset = this.m_Labels.Count - labels.Count;
                        for (int j = 0; j < offset; j++)
                        {
                            ContentControl label = new ContentControl();
                            label.Content = m_Labels[this.m_Labels.Count - offset - 1];
                            //label.ContentTemplate = this.LabelTemplate;
                            Binding labelTemplateBinding = new Binding();
                            labelTemplateBinding.Path = new PropertyPath("LabelTemplate");
                            labelTemplateBinding.Source = this;
                            label.SetBinding(ContentControl.ContentTemplateProperty, labelTemplateBinding);
                            label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            labels.Add(label);
                            Line tickLine = new Line();
                            Binding styleBinding = new Binding();
                            styleBinding.Path = new PropertyPath("MajorLineStyle");
                            styleBinding.Source = this;
                            tickLine.SetBinding(Line.StyleProperty, styleBinding);
                            tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            tickLine.X1 = this.ActualWidth - (axisLine.StrokeThickness);
                            tickLine.Y1 = yAxisHeightPosition;
                            tickLine.Y2 = yAxisHeightPosition;
                            //tickLine.X2 = tickLine.X1 - MajorLineSize; 
                            Binding tickSizeBinding = new Binding();
                            tickSizeBinding.Path = new PropertyPath("MajorLineSize");
                            tickSizeBinding.Source = this;
                            tickSizeBinding.Converter = new MajorSizeThicknessConverter();
                            tickSizeBinding.ConverterParameter = tickLine.X1;
                            tickLine.SetBinding(Line.X2Property, tickSizeBinding);
                            Binding ticklineVisibilityBinding = new Binding();
                            ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                            ticklineVisibilityBinding.Source = this;
                            ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                            tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                            double minorWidth = yAxisHeightStep;
                            double minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int k = 0; k < this.MinorTicksCount; j++)
                            {
                                Line minorLine = new Line();
                                Binding minorstyleBinding = new Binding();
                                minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                                minorstyleBinding.Source = this;
                                minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                                minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                                minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.X1 = this.ActualWidth - (minorLine.StrokeThickness);
                                minorLine.X2 = minorLine.X1 - MinorLineSize;

                                minorTickLines.Add(minorLine);
                                this.Children.Add(minorLine);
                            }
                            this.Children.Add(label);
                            majorTickLines.Add(tickLine);
                            this.Children.Add(tickLine);
                        }
                    }
                    else
                    {
                        int offset = labels.Count - this.m_Labels.Count;
                        for (int j = 0; j < offset; j++)
                        {
                            this.Children.Remove(labels[labels.Count - 1]);
                            labels.RemoveAt(labels.Count - 1);
                            for (int k = 0; k < this.MinorTicksCount; k++)
                            {
                                this.Children.Remove(minorTickLines[minorTickLines.Count - 1]);
                                minorTickLines.RemoveAt(minorTickLines.Count - 1);
                            }
                            this.Children.Remove(majorTickLines[majorTickLines.Count - 1]);
                            majorTickLines.RemoveAt(majorTickLines.Count - 1);
                        }
                    }
                    for (int i = this.m_Labels.Count - 1; i >= 0; i--)
                    {
                        ContentControl label = labels[i];
                        label.Content = m_Labels[i];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        Line tickLine = majorTickLines[i];
                        double labelPadding = 0;
                        tickLine.X1 = this.ActualWidth - (axisLine.StrokeThickness);
                        tickLine.Y1 = yAxisHeightPosition;
                        tickLine.Y2 = yAxisHeightPosition;
                        tickLine.X2 = tickLine.X1 - MajorLineSize;
                        
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        desiredWidth = 0;
                        double minorstep = 0;
                        if (!(i == 0))
                        {
                            double minorWidth = yAxisHeightStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = minorTickLines[minorCount];
                                minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.X1 = this.ActualWidth - (minorLine.StrokeThickness);
                                minorLine.X2 = minorLine.X1 - MinorLineSize;
                                minorCount++;
                            }
                        }
                        if (this.ShowMajorTicks)
                        {
                            labelPadding = tickLine.X2;
                            desiredWidth = this.MajorLineSize;
                        }
                        Canvas.SetLeft(label, this.ActualWidth - (label.DesiredSize.Width) - desiredWidth - 1);
                        Canvas.SetTop(label, yAxisHeightPosition - (label.DesiredSize.Height / 2));
                        labelSize = Math.Max(labelSize, label.DesiredSize.Width);
                        yAxisHeightPosition += yAxisHeightStep;
                        //desiredWidth = desiredWidth + labelSize;
                    }
                }
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                Canvas.SetLeft(header, this.ActualWidth - labelSize - header.DesiredSize.Height - this.MajorLineSize - 1);
                Canvas.SetTop(header, this.ActualHeight / 2);
                desiredWidth = desiredWidth+ header.DesiredSize.Height + labelSize;
            }
            if (this.Chart.AxisWidth < desiredWidth)
                this.Chart.AxisWidth = desiredWidth + 1;
        }
        private void Initialize()
        {
            double desiredWidth = 0;
            CalculateAutoInterval();
            GenerateLabels();            
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
               
                this.Children.Clear();
                double yAxisHeightStep = this.ActualHeight / ((m_IntervalCount > 0) ? m_IntervalCount : 1);
                double yAxisHeightPosition = 0;
                Rect oldRect = new Rect(0, 0, 0, 0);
                axisLine = new Line();
                Binding binding = new Binding();
                binding.Path = new PropertyPath("AxisLineStyle");
                binding.Source = this;
                axisLine.SetBinding(Line.StyleProperty, binding);
                axisLine.X1 = this.ActualWidth;
                axisLine.X2 = this.ActualWidth;
                axisLine.Y1 = 0;
                axisLine.Y2 = this.ActualHeight;
                axisLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                labels = new List<ContentControl>();
                majorTickLines = new List<Line>();
                minorTickLines = new List<Line>();
                double labelSize = 0;
                for (int i = this.m_Labels.Count - 1; i >= 0; i--)
                {
                    ContentControl label = new ContentControl();
                    label.Content = m_Labels[i];
                    //label.ContentTemplate = this.LabelTemplate;
                    Binding labelTemplateBinding = new Binding();
                    labelTemplateBinding.Path = new PropertyPath("LabelTemplate");
                    labelTemplateBinding.Source = this;
                    label.SetBinding(ContentControl.ContentTemplateProperty, labelTemplateBinding);
                    label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                    labels.Add(label);
                    Line tickLine = new Line();
                    double labelPadding = 0;
                    Binding styleBinding = new Binding();
                    styleBinding.Path = new PropertyPath("MajorLineStyle");
                    styleBinding.Source = this;
                    tickLine.SetBinding(Line.StyleProperty, styleBinding);
                    //tickLine.Style = MajorLineStyle;
                    tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                    tickLine.X1 = this.ActualWidth - (axisLine.StrokeThickness);
                    tickLine.Y1 = yAxisHeightPosition;
                    tickLine.Y2 = yAxisHeightPosition;
                    tickLine.X2 = tickLine.X1 - MajorLineSize;
                   
                    Binding ticklineVisibilityBinding = new Binding();
                    ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                    ticklineVisibilityBinding.Source = this;
                    ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                    tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                    majorTickLines.Add(tickLine);
                    this.Children.Add(tickLine);
                    desiredWidth = 0;
                    double minorstep = 0;
                    if (!(i == 0))
                    {
                        double minorWidth = yAxisHeightStep;
                        minorstep = minorWidth / (MinorTicksCount + 1);
                        for (int j = 0; j < this.MinorTicksCount; j++)
                        {
                            Line minorLine = new Line();
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                            minorstyleBinding.Source = this;
                            minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                            minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                            minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));
                            minorLine.X1 = this.ActualWidth - (minorLine.StrokeThickness);
                            minorLine.X2 = minorLine.X1 - MinorLineSize;

                            minorTickLines.Add(minorLine);
                            this.Children.Add(minorLine);
                        }
                    }
                    if (this.ShowMajorTicks)
                    {
                        labelPadding = tickLine.X2;
                        desiredWidth = this.MajorLineSize;
                    }

                    Canvas.SetLeft(label, this.ActualWidth - (label.DesiredSize.Width) - desiredWidth - 1);
                    Canvas.SetTop(label, yAxisHeightPosition - (label.DesiredSize.Height / 2));
                    labelSize = Math.Max(labelSize, label.DesiredSize.Width);
                    this.Children.Add(label);
                    yAxisHeightPosition += yAxisHeightStep;
                }
                header = new ContentControl();
                header.DataContext = null;
                header.Content = this.Header;
                //header.ContentTemplate = this.HeaderTemplate;
                Binding contentBinding = new Binding();
                contentBinding.Path = new PropertyPath("Header");
                contentBinding.Source = this;
                header.SetBinding(ContentControl.ContentProperty, contentBinding);
                Binding headerTemplateBinding = new Binding();
                headerTemplateBinding.Path = new PropertyPath("HeaderTemplate");
                headerTemplateBinding.Source = this;
                header.SetBinding(ContentControl.ContentTemplateProperty, headerTemplateBinding);
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                Canvas.SetLeft(header, this.ActualWidth - labelSize - header.DesiredSize.Height - this.MajorLineSize - 1);
                Canvas.SetTop(header, this.ActualHeight / 2);
                desiredWidth = desiredWidth + header.DesiredSize.Height + labelSize;
                this.Children.Add(header);
                this.Children.Add(axisLine);
                isInitialized = true;
            }
            if (this.Chart.AxisWidth < desiredWidth)
                this.Chart.AxisWidth = desiredWidth + 1;
        }

        public override double DataToPoint(double value)
        {
            if (!(m_MinValue == m_MaxValue))
                return (this.ActualHeight - ((value - m_MinValue) * this.ActualHeight) / (m_MaxValue - m_MinValue));
            else
                return 0;
        }

        public override void CalculateIntervalFromSeriesPoints()
        {
            List<double> yValues = new List<double>();
            if (this.Series != null)
                foreach (SeriesBase series in Series)
                {
                    if(series.Points != null)
                    foreach (var point in series.Points)
                    {
                        yValues.Add(point.YValue);
                    }
                }
            if (yValues.Count > 1)
            {
                this.AddMinMax(yValues.ToArray().Min(), yValues.ToArray().Max());
            }
        }

        override public string GetOriginalLabel(double value)
        {
            switch (Type)
            {
                case YType.Double:
                    return value.ToString(this.StringFormat);
                case YType.DateTime:
#if !WINRT
                    return DateTime.FromOADate(value).ToString(this.StringFormat);
#else
                    return value.FromOADate().ToString(this.StringFormat);
#endif
                default:
                    return value.ToString(this.StringFormat);
            }
        }

        internal YType Type
        {
            get { return (YType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        internal static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(YType), typeof(YAxis), new PropertyMetadata(YType.Double,OnTypeChanged));

        private static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as YAxis).TypeChanged(args);
        }
        internal void TypeChanged(DependencyPropertyChangedEventArgs args)
        {
            switch (Type)
            {
                case YType.Double:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.Double.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.Double.ToString(),false);
#endif
                    break;
                case YType.DateTime:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.DateTime.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.DateTime.ToString(),false);
#endif
                    break;               
                default:
                    break;
            }
        }

    }
}
