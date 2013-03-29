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
            if ((this.XAxis.m_Labels.Count - 2) == xLines.Count)
            {
                for (int i = 0; i < this.XAxis.m_Labels.Count - 2; i++)
                {
                    Line line = xLines[i];
                    line.X1 = xAxisWidthPosition;
                    line.X2 = xAxisWidthPosition;                   
                    line.Y2 = (int)this.ActualHeight;
                   
                    xAxisWidthPosition += xAxisWidthStep;
                }
            }
            else
            {
                if ((this.XAxis.m_Labels.Count - 2) > xLines.Count)
                {
                    int offset = (this.XAxis.m_Labels.Count - 2) - xLines.Count;
                    for (int j = 0; j < offset; j++)
                    {
                        Line line = xLines[this.XAxis.m_Labels.Count - offset - 1];
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
                        xLines.Add(line);
                        this.Children.Add(line);
                    }
                    
                }
                else
                {
                    int offset = xLines.Count - (this.XAxis.m_Labels.Count - 2);
                    for (int j = 0; j < offset; j++)
                    {
                        this.Children.Remove(xLines[xLines.Count - 1]);
                        xLines.RemoveAt(xLines.Count - 1);
                    }
                }
                for (int i = 0; i < this.XAxis.m_Labels.Count - 2; i++)
                {
                    Line line = xLines[i];
                    line.X1 = xAxisWidthPosition;
                    line.X2 = xAxisWidthPosition;
                    line.Y2 = this.ActualHeight;
                    xAxisWidthPosition += xAxisWidthStep;
                }  
            }
            double yAxisHeightStep = (int)this.ActualHeight / this.YAxis.m_IntervalCount;
            double yAxisHeightPosition = yAxisHeightStep;
            if (YAxis.m_Labels.Count - 2 == yLines.Count)
            {
                for (int i = 0; i < YAxis.m_Labels.Count - 2; i++)
                {
                    Line line = yLines[i];
                    line.X2 = this.ActualWidth;
                    line.Y1 = yAxisHeightPosition;
                    line.Y2 = yAxisHeightPosition;                    
                    yAxisHeightPosition += yAxisHeightStep;
                }
            }
            else
            {
                if ((this.YAxis.m_Labels.Count - 2) > yLines.Count)
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
                        this.Children.Add(line);
                        this.yLines.Add(line);
                    }
                }
                else
                {
                    int offset = yLines.Count - (this.YAxis.m_Labels.Count - 2);
                    for (int j = 0; j < offset; j++)
                    {
                        this.Children.Remove(yLines[yLines.Count - 1]);
                        yLines.RemoveAt(yLines.Count - 1);
                    }
                }
                for (int i = 0; i < YAxis.m_Labels.Count - 2; i++)
                {
                    Line line = yLines[i];
                    line.X2 = this.ActualWidth;
                    line.Y1 = yAxisHeightPosition;
                    line.Y2 = yAxisHeightPosition;
                    yAxisHeightPosition += yAxisHeightStep;
                }
            }
        }
        private void Initialize()
        {
            this.Children.Clear();
            double xAxisWidthStep = this.ActualWidth / this.XAxis.m_IntervalCount;
            double xAxisWidthPosition = xAxisWidthStep;
            xLines = new List<Line>();
            yLines = new List<Line>();
            for (int i = 0; i < this.XAxis.m_Labels.Count - 2; i++)
            {
                Line line = new Line();
                line.X1 = xAxisWidthPosition;
                line.X2 = xAxisWidthPosition;
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
                xLines.Add(line);
                this.Children.Add(line);
                xAxisWidthPosition += xAxisWidthStep;
            }
            double yAxisHeightStep = this.ActualHeight / this.YAxis.m_IntervalCount;
            double yAxisHeightPosition = yAxisHeightStep;
            for (int i = 0; i < YAxis.m_Labels.Count - 2; i++)
            {
                Line line = new Line();
                line.X1 = 0;
                line.X2 = this.ActualWidth;
                line.Y1 = yAxisHeightPosition;
                line.Y2 = yAxisHeightPosition;
                Binding showCrossLines = new Binding();
                showCrossLines.Path = new PropertyPath("ShowCrossLines");
                showCrossLines.Source = this.YAxis;
                showCrossLines.Converter = new BooleanToVisibilityConverter();
                line.SetBinding(Line.VisibilityProperty, showCrossLines);
                Binding styleBinding = new Binding();
                styleBinding.Path = new PropertyPath("CrossLineStyle");
                styleBinding.Source = this.YAxis;
                line.SetBinding(Line.StyleProperty, styleBinding);
                this.Children.Add(line);
                this.yLines.Add(line);
                yAxisHeightPosition += yAxisHeightStep;
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
