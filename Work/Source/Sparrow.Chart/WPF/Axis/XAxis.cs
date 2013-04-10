using System;
using System.Collections.Generic;
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
    /// XAxis for Sparrow Chart
    /// </summary>
    public abstract class XAxis : AxisBase
    {
        public XAxis()
            : base()
        {           
        }

        protected override bool CheckType()
        {
            if (this.Type == XType.Category)
                return false;
            return true;
        }

        internal void Update()
        {
            double desiredHeight = 0;
            double labelSize = 0;
            CalculateAutoInterval();
            GenerateLabels();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
                double xAxisWidthStep = this.ActualWidth / ((m_IntervalCount > 0) ? m_IntervalCount : 1);
                double xAxisWidthPosition = 0;
                double minorstep = 0;
                axisLine.X2 = this.ActualWidth;                
                Rect oldRect = new Rect(0, 0, 0, 0);
               
                if (this.m_Labels.Count == labels.Count)
                {
                    int k = 0;
                    int minorCount = 0;
                    for (int i = 0; i < this.m_Labels.Count; i++)
                    {
                        xAxisWidthPosition = this.DataToPoint(m_startValue + (m_Interval * k));
                        ContentControl label = labels[k];
                        label.Content = m_Labels[k];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        Line tickLine = majorTickLines[k];
                        double labelPadding = 0;
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        tickLine.X1 = xAxisWidthPosition ;
                        tickLine.X2 = xAxisWidthPosition ;
                        if (!(i == this.m_Labels.Count - 1))
                        {
                            double minorWidth = xAxisWidthStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = minorTickLines[minorCount];
                                minorLine.X1 = (xAxisWidthPosition + minorstep * (j + 1));
                                minorLine.X2 = (xAxisWidthPosition + minorstep * (j + 1));
                                minorLine.Y1 = 0;         
                                minorCount++;
                            }
                        }
                        if (this.ShowMajorTicks)
                        {
                            labelPadding = tickLine.Y2;
                            desiredHeight = tickLine.Y2;
                        }
                        Canvas.SetLeft(label, xAxisWidthPosition - (label.DesiredSize.Width / 2));
                        Canvas.SetTop(label, desiredHeight);                        
                        k++;
                        labelSize = Math.Max(labelSize, label.DesiredSize.Height);  
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
                            tickLine.X1 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);
                            tickLine.X2 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);
                            tickLine.Y1 = 0;
                            Binding tickSizeBinding = new Binding();
                            tickSizeBinding.Path = new PropertyPath("MajorLineSize");
                            tickSizeBinding.Source = this;
                            tickLine.SetBinding(Line.Y2Property, tickSizeBinding);
                            Binding ticklineVisibilityBinding = new Binding();
                            ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                            ticklineVisibilityBinding.Source = this;
                            ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                            tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                            double minorWidth = xAxisWidthStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int k = 0; k < this.MinorTicksCount; k++)
                            {
                                Line minorLine = new Line();
                                Binding minorstyleBinding = new Binding();
                                minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                                minorstyleBinding.Source = this;
                                minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                                minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                                minorLine.X1 = (xAxisWidthPosition + minorstep);
                                minorLine.X2 = (xAxisWidthPosition + minorstep);
                                minorLine.Y1 = 0;
                                Binding minortickSizeBinding = new Binding();
                                minortickSizeBinding.Path = new PropertyPath("MinorLineSize");
                                minortickSizeBinding.Source = this;
                                minorLine.SetBinding(Line.Y2Property, minortickSizeBinding);

                                minorTickLines.Add(minorLine);
                                this.Children.Add(minorLine);
                                minorstep += minorstep;
                            }
                            majorTickLines.Add(tickLine);
                            this.Children.Add(label);
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
                    for (int i = 0; i < this.m_Labels.Count; i++)
                    {
                        ContentControl label = labels[i];
                        label.Content = m_Labels[i];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        Line tickLine = majorTickLines[i];
                        double labelPadding = 0;
                        int minorCount = 0;
                        tickLine.X1 = xAxisWidthPosition;
                        tickLine.X2 = xAxisWidthPosition;
                        tickLine.Y1 = 0;
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        if (!(i == this.m_Labels.Count - 1))
                        {
                            double minorWidth = xAxisWidthStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = minorTickLines[minorCount];
                                minorLine.X1 = (xAxisWidthPosition + minorstep * (j + 1));
                                minorLine.X2 = (xAxisWidthPosition + minorstep * (j + 1));
                                minorLine.Y1 = 0;                                
                                minorCount++;
                            }
                        }
                        if (this.ShowMajorTicks)
                        {
                            labelPadding = tickLine.Y2;
                            desiredHeight = tickLine.Y2;
                        }
                        Canvas.SetLeft(label, xAxisWidthPosition - (label.DesiredSize.Width / 2));
                        Canvas.SetTop(label, desiredHeight);
                        xAxisWidthPosition += xAxisWidthStep;
                        labelSize = Math.Max(labelSize, label.DesiredSize.Height);  
                    }  
                }
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                Canvas.SetLeft(header, (this.ActualWidth / 2) - (header.DesiredSize.Width / 2));
                Canvas.SetTop(header, (this.ActualHeight / 2) - (header.DesiredSize.Height / 2) + desiredHeight);
                desiredHeight += header.DesiredSize.Height + labelSize;              
            }
            if (this.Chart.AxisHeight < desiredHeight)
                this.Chart.AxisHeight = desiredHeight + 1;
        }

        internal void Initialize()
        {
            double desiredHeight = 0;
            double labelSize = 0;
            //if (m_MinValue == m_startValue + m_Interval)
            CalculateAutoInterval();
            GenerateLabels();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
               
                this.Children.Clear();
                double xAxisWidthStep = this.ActualWidth / ((m_IntervalCount > 0) ? m_IntervalCount : 1);
                double xAxisWidthPosition = this.DataToPoint(m_startValue);
                double minorstep = 0;
                //m_offset = this.DataToPoint(m_MinValue + m_Interval);
                Rect oldRect = new Rect(0, 0, 0, 0);
                axisLine = new Line();
                axisLine.X1 = 0;
                axisLine.X2 = this.ActualWidth;
                axisLine.Y1 = 0;
                axisLine.Y2 = 0;
                Binding binding = new Binding();
                binding.Path = new PropertyPath("AxisLineStyle");
                binding.Source = this;
                axisLine.SetBinding(Line.StyleProperty, binding);
                labels = new List<ContentControl>();
                majorTickLines = new List<Line>();
                minorTickLines = new List<Line>();
                for (int i = 0; i < this.m_Labels.Count; i++)
                {
                    ContentControl label = new ContentControl();
                    label.Content = m_Labels[i];
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
                    tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                    tickLine.X1 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);
                    tickLine.X2 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);
                    tickLine.Y1 = 0;
                    Binding tickSizeBinding = new Binding();
                    tickSizeBinding.Path=new PropertyPath("MajorLineSize");
                    tickSizeBinding.Source = this;
                    tickLine.SetBinding(Line.Y2Property, tickSizeBinding);
                    Binding ticklineVisibilityBinding = new Binding();
                    ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                    ticklineVisibilityBinding.Source = this;
                    ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                    tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                    majorTickLines.Add(tickLine);
                    this.Children.Add(tickLine);
                    if (!(i == this.m_Labels.Count - 1))
                    {
                        double minorWidth = xAxisWidthStep;
                        minorstep = minorWidth / (MinorTicksCount + 1);
                        for (int j = 0; j < this.MinorTicksCount; j++)
                        {
                            Line minorLine = new Line();
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                            minorstyleBinding.Source = this;
                            minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                            minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            minorLine.X1 = (xAxisWidthPosition + minorstep * (j + 1));
                            minorLine.X2 = (xAxisWidthPosition + minorstep * (j + 1));
                            minorLine.Y1 = 0;
                            Binding minortickSizeBinding = new Binding();
                            minortickSizeBinding.Path = new PropertyPath("MinorLineSize");
                            minortickSizeBinding.Source = this;
                            minorLine.SetBinding(Line.Y2Property, minortickSizeBinding);
                            
                            minorTickLines.Add(minorLine);
                            this.Children.Add(minorLine);                           
                        }
                    }
                    if (this.ShowMajorTicks)
                    {
                        labelPadding = tickLine.Y2;
                        desiredHeight = MajorLineSize;
                    }
                    Canvas.SetLeft(label, xAxisWidthPosition - (label.DesiredSize.Width / 2));
                    Canvas.SetTop(label, desiredHeight);
                    labelSize = Math.Max(labelSize, label.DesiredSize.Height);  
                    this.Children.Add(label);                   
                    xAxisWidthPosition += xAxisWidthStep;
                }
                header = new ContentControl();
                header.DataContext = null;
                Binding contentBinding = new Binding();
                contentBinding.Path = new PropertyPath("Header");
                contentBinding.Source = this;
                header.SetBinding(ContentControl.ContentProperty, contentBinding);
                Binding headerTemplateBinding = new Binding();
                headerTemplateBinding.Path = new PropertyPath("HeaderTemplate");
                headerTemplateBinding.Source = this;
                header.SetBinding(ContentControl.ContentTemplateProperty, headerTemplateBinding);
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                Canvas.SetLeft(header, (this.ActualWidth / 2) - (header.DesiredSize.Width / 2));
                Canvas.SetTop(header, (this.ActualHeight / 2) - (header.DesiredSize.Height / 2) + desiredHeight);
                desiredHeight += header.DesiredSize.Height + labelSize;
                this.Children.Add(header);
                this.Children.Add(axisLine);
                isInitialized = true;
            }
            if (this.Chart.AxisHeight < desiredHeight)
                this.Chart.AxisHeight = desiredHeight + 1;
        }

        protected override void GetStyles()
        {
            base.GetStyles();
            this.HeaderTemplate = (DataTemplate)styles["xAxisHeaderTemplate"];
        }

#if WPf
        public static Rect BoundsRelativeTo(FrameworkElement element, Visual relativeTo)
        {
            return element.TransformToVisual(relativeTo).TransformBounds(LayoutInformation.GetLayoutSlot(element));
        }
#endif
       
        internal override void InvalidateVisuals()
        {
            if (!isInitialized)
                Initialize();
            else
                Update();
        }

        public override double DataToPoint(double value)
        {
            if (!(m_MinValue == m_MaxValue))
                return ((value - m_MinValue) * (this.ActualWidth / (m_MaxValue - m_MinValue)));
            else
                return 0;
        }

        public override void CalculateIntervalFromSeriesPoints()
        {
            List<double> xValues = new List<double>();
            if (this.Series != null)
                foreach (SeriesBase series in Series)
                {
                    if (series.Points != null)
                        foreach (var point in series.Points)
                        {
                            xValues.Add(point.XValue);
                        }
                }
            if (xValues.Count > 0)
            {
                this.AddMinMax(xValues.ToArray().Min(), xValues.ToArray().Max());                
            }
        }

        override public string GetOriginalLabel(double value)
        {
            switch (Type)
            {
                case XType.Double:
                    return value.ToString(this.StringFormat);                    
                case XType.DateTime:
#if !WINRT
                    return DateTime.FromOADate(value).ToString(this.StringFormat);
#else
                    return value.FromOADate().ToString(this.StringFormat);
#endif
                case XType.Category:
                    if (SparrowChart.ActualCategoryValues.Count > (int)value)
                        return SparrowChart.ActualCategoryValues[(int)value];
                    else
                        return "";
                default:
                    return value.ToString(this.StringFormat);                    
            }
        }

        internal XType Type
        {
            get { return (XType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        internal static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(XType), typeof(XAxis), new PropertyMetadata(XType.Double,OnTypeChanged));
        private static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as XAxis).TypeChanged(args);
        }
        internal void TypeChanged(DependencyPropertyChangedEventArgs args)
        {           
            switch (Type)
            {
                case XType.Double:
#if WPF
                    this.ActualType =(ActualType)Enum.Parse(typeof(ActualType),XType.Double.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.Double.ToString(),false);
#endif
                    break;
                case XType.DateTime:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.DateTime.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.DateTime.ToString(),false);
#endif
                    break;
                case XType.Category:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.Category.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.Category.ToString(),false);
#endif
                    break;
                default:
                    break;
            }
        }

    }
}
