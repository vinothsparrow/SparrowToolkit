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
using Line = System.Windows.Shapes.Line;
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
    /// Axis Cross Line Container
    /// </summary>
    public class AxisCrossLinesContainer : Panel
    {
        public AxisCrossLinesContainer()
        {
            this.SizeChanged += AxisCrossLinesContainer_SizeChanged;
        }

        void AxisCrossLinesContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           Refresh();
        }
               

        List<Line> xLines;
        List<Line> yLines;
        List<Line> xMinorLines;
        List<Line> yMinorLines;
        bool isInitialized;

        public void Refresh()
        {
            if (!isInitialized)
                Initialize();
            else
                Update();

        }
        private void Update()
        {
            double xAxisWidthStep = (int)this.ActualWidth / this.XAxis.m_IntervalCount;
            double xAxisWidthPosition = xAxisWidthStep;
            int xminorCount = 0;
            int yminorCount = 0;
            if ((this.XAxis.m_Labels.Count) == xLines.Count)
            {
                for (int i = 0; i < this.XAxis.m_Labels.Count; i++)
                {
                    Line line = xLines[i];
                    line.X1 = xAxisWidthPosition;
                    line.X2 = xAxisWidthPosition;                   
                    line.Y2 = (int)this.ActualHeight;
                    if (i != (this.XAxis.m_Labels.Count - 1))
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = xMinorLines[xminorCount];                          
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * i)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * i)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            xminorCount++;
                        }
                    }
                    xAxisWidthPosition += xAxisWidthStep;
                }
            }
            else
            {
                if ((this.XAxis.m_Labels.Count) > xLines.Count)
                {
                    int offset = (this.XAxis.m_Labels.Count) - xLines.Count;
                    for (int j = 0; j < offset; j++)
                    {
                        Line line = new Line();
                        line.X1 = 0;
                        line.X2 = 0;
                        line.Y2 = this.ActualHeight;
                        Binding styleBinding = new Binding();
                        styleBinding.Path = new PropertyPath("CrossLineStyle");
                        styleBinding.Source = this.XAxis;
                        Binding showCrossLines = new Binding();
                        showCrossLines.Path = new PropertyPath("ShowCrossLines");
                        showCrossLines.Source = this.XAxis;
                        showCrossLines.Converter = new BooleanToVisibilityConverter();
                        line.SetBinding(Line.VisibilityProperty, showCrossLines);
                        line.SetBinding(Line.StyleProperty, styleBinding);
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorCrossLineStyle");
                            minorstyleBinding.Source = this.XAxis;
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding();
                            minorshowCrossLines.Path = new PropertyPath("ShowMinorCrossLines");
                            minorshowCrossLines.Source = this.XAxis;
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * j)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * j)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            this.Children.Add(minorline);
                            xMinorLines.Add(minorline);
                        }
                        xLines.Add(line);
                        this.Children.Add(line);
                    }
                    
                }
                else
                {
                    int offset = xLines.Count - (this.XAxis.m_Labels.Count);
                    for (int j = 0; j < offset; j++)
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            this.Children.Remove(xMinorLines[xMinorLines.Count - 1]);
                            xMinorLines.RemoveAt(xMinorLines.Count - 1);
                        }
                        this.Children.Remove(xLines[xLines.Count - 1]);
                        xLines.RemoveAt(xLines.Count - 1);
                    }
                }
                for (int i = 0; i < this.XAxis.m_Labels.Count; i++)
                {
                    Line line = xLines[i];
                    line.X1 = xAxisWidthPosition;
                    line.X2 = xAxisWidthPosition;
                    line.Y2 = this.ActualHeight;
                    if (i != (this.XAxis.m_Labels.Count - 1))
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = xMinorLines[xminorCount];
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * i)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * i)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            xminorCount++;
                        }
                    }
                    xAxisWidthPosition += xAxisWidthStep;
                }  
            }
            double yAxisHeightStep = (int)this.ActualHeight / this.YAxis.m_IntervalCount;
            double yAxisHeightPosition = yAxisHeightStep;
            if (YAxis.m_Labels.Count == yLines.Count)
            {
                for (int i = 0; i < YAxis.m_Labels.Count ; i++)
                {
                    Line line = yLines[i];
                    line.X2 = this.ActualWidth;
                    line.Y1 = yAxisHeightPosition;
                    line.Y2 = yAxisHeightPosition;
                    if (i != (this.YAxis.m_Labels.Count - 1))
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = yMinorLines[yminorCount];
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * i)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * i)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            yminorCount++;
                        }
                    }
                    yAxisHeightPosition += yAxisHeightStep;
                }
            }
            else
            {
                if ((this.YAxis.m_Labels.Count) > yLines.Count)
                {
                    int offset = (this.YAxis.m_Labels.Count - 2) - yLines.Count;
                    for (int j = 0; j < offset; j++)
                    {
                        Line line = new Line();
                        line.X1 = 0;
                        line.X2 = this.ActualWidth;
                        line.Y1 = yAxisHeightPosition;
                        line.Y2 = yAxisHeightPosition;
                        Binding showCrossLines = new Binding();
                        showCrossLines.Path=new PropertyPath("ShowCrossLines");
                        showCrossLines.Source = this.YAxis;
                        showCrossLines.Converter = new BooleanToVisibilityConverter();
                        line.SetBinding(Line.VisibilityProperty, showCrossLines);
                        Binding styleBinding = new Binding();
                        styleBinding.Path = new PropertyPath("CrossLineStyle");
                        styleBinding.Source = this.YAxis;
                        line.SetBinding(Line.StyleProperty, styleBinding);
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorCrossLineStyle");
                            minorstyleBinding.Source = this.YAxis;
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding();
                            minorshowCrossLines.Path = new PropertyPath("ShowMinorCrossLines");
                            minorshowCrossLines.Source = this.YAxis;
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * j)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * j)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            this.Children.Add(minorline);
                            yMinorLines.Add(minorline);
                        }
                        this.Children.Add(line);
                        this.yLines.Add(line);
                    }
                }
                else
                {
                    int offset = yLines.Count - (this.YAxis.m_Labels.Count);
                    for (int j = 0; j < offset; j++)
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            this.Children.Remove(yMinorLines[yMinorLines.Count - 1]);
                            yMinorLines.RemoveAt(yMinorLines.Count - 1);
                        }
                        this.Children.Remove(yLines[yLines.Count - 1]);
                        yLines.RemoveAt(yLines.Count - 1);
                    }
                }
                for (int i = 0; i < YAxis.m_Labels.Count; i++)
                {
                    Line line = yLines[i];
                    line.X2 = this.ActualWidth;
                    line.Y1 = yAxisHeightPosition;
                    line.Y2 = yAxisHeightPosition;
                    if (i != (this.YAxis.m_Labels.Count - 1))
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = yMinorLines[yminorCount];
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * i)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * i)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            yminorCount++;
                        }
                    }
                    yAxisHeightPosition += yAxisHeightStep;
                }
            }
        }
        private void Initialize()
        {
            this.Children.Clear();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
                double xAxisWidthStep = this.ActualWidth / this.XAxis.m_IntervalCount;
                double xAxisWidthPosition = 0;
                xLines = new List<Line>();
                yLines = new List<Line>();
                xMinorLines = new List<Line>();
                yMinorLines = new List<Line>();
                int k = 0;
                for (int i = 0; i < this.XAxis.m_Labels.Count; i++)
                {
                    Line line = new Line();
                    line.X1 = this.XAxis.DataToPoint(this.XAxis.m_startValue + (this.XAxis.m_Interval * k));
                    line.X2 = this.XAxis.DataToPoint(this.XAxis.m_startValue + (this.XAxis.m_Interval * k));
                    line.Y1 = 0;
                    line.Y2 = this.ActualHeight;
                    Binding styleBinding = new Binding();
                    styleBinding.Path = new PropertyPath("CrossLineStyle");
                    styleBinding.Source = this.XAxis;
                    line.SetBinding(Line.StyleProperty, styleBinding);
                    Binding showCrossLines = new Binding();
                    showCrossLines.Path = new PropertyPath("ShowCrossLines");
                    showCrossLines.Source = this.XAxis;
                    showCrossLines.Converter = new BooleanToVisibilityConverter();
                    line.SetBinding(Line.VisibilityProperty, showCrossLines);
                    if (i != (this.XAxis.m_Labels.Count - 1))
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();                            
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorCrossLineStyle");
                            minorstyleBinding.Source = this.XAxis;
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding();
                            minorshowCrossLines.Path = new PropertyPath("ShowMinorCrossLines");
                            minorshowCrossLines.Source = this.XAxis;
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * k)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.m_startValue + (this.XAxis.m_Interval * k)) + ((this.XAxis.m_Interval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            this.Children.Add(minorline);
                            xMinorLines.Add(minorline);
                        }
                    }
                    xLines.Add(line);
                    this.Children.Add(line);
                    xAxisWidthPosition += xAxisWidthStep;
                    k++;
                }
                double yAxisHeightStep = this.ActualHeight / this.YAxis.m_IntervalCount;
                double yAxisHeightPosition = 0;
                int j = 0;
                for (int i = 0; i < YAxis.m_Labels.Count; i++)
                {
                    Line line = new Line();
                    line.X1 = 0;
                    line.X2 = this.ActualWidth;
                    line.Y1 = this.YAxis.DataToPoint(this.YAxis.m_startValue + (this.YAxis.m_Interval * j));
                    line.Y2 = this.YAxis.DataToPoint(this.YAxis.m_startValue + (this.YAxis.m_Interval * j));
                    Binding showCrossLines = new Binding();
                    showCrossLines.Path = new PropertyPath("ShowCrossLines");
                    showCrossLines.Source = this.YAxis;
                    showCrossLines.Converter = new BooleanToVisibilityConverter();
                    line.SetBinding(Line.VisibilityProperty, showCrossLines);
                    Binding styleBinding = new Binding();
                    styleBinding.Path = new PropertyPath("CrossLineStyle");
                    styleBinding.Source = this.YAxis;
                    line.SetBinding(Line.StyleProperty, styleBinding);
                    if (i != (this.YAxis.m_Labels.Count - 1))
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorCrossLineStyle");
                            minorstyleBinding.Source = this.YAxis;
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding();
                            minorshowCrossLines.Path = new PropertyPath("ShowMinorCrossLines");
                            minorshowCrossLines.Source = this.YAxis;
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * j)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.m_startValue + (this.YAxis.m_Interval * j)) + ((this.YAxis.m_Interval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            this.Children.Add(minorline);
                            yMinorLines.Add(minorline);
                        }
                    }
                    this.Children.Add(line);
                    this.yLines.Add(line);
                    yAxisHeightPosition += yAxisHeightStep;
                    j++;
                }
                isInitialized = true;
            }
        }

        public XAxis XAxis
        {
            get { return (XAxis)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(AxisCrossLinesContainer), new PropertyMetadata(null));

        public YAxis YAxis
        {
            get { return (YAxis)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(AxisCrossLinesContainer), new PropertyMetadata(null));

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(new Point(0, 0), finalSize));
            }
           
            return finalSize;
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size(0, 0);
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
                desiredSize.Width += child.DesiredSize.Width;
                desiredSize.Height += child.DesiredSize.Height;
            }
            if (Double.IsInfinity(availableSize.Height))
                availableSize.Height = desiredSize.Height;
            if (Double.IsInfinity(availableSize.Width))
                availableSize.Width = desiredSize.Width;
            return availableSize;
        }
    }
}
