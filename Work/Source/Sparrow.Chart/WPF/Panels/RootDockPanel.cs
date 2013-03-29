using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Data;
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
    /// Chart Root DockPanel
    /// </summary>
    public class RootPanel : DockPanel
    {

        public Dock LegendDock
        {
            get { return (Dock)GetValue(LegendDockProperty); }
            set { SetValue(LegendDockProperty, value); }
        }

        public static readonly DependencyProperty LegendDockProperty =
            DependencyProperty.Register("LegendDock", typeof(Dock), typeof(RootPanel), new PropertyMetadata(Dock.Top));

        public Size DesiredSize { get; set; }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            ContainerCollection container = (ContainerCollection)this.FindName("PART_containers");
            base.ArrangeOverride(arrangeSize);
            //if (container != null)
            //    container.Refresh();
            return arrangeSize;
        }
        protected override Size MeasureOverride(Size constraint)
        {
            DockPanel.SetDock(this.Children[0], LegendDock);
            Size desiredSize = new Size(0, 0);
            int count = 0;
            
            foreach (UIElement child in Children)
            {                
                Canvas.SetZIndex(child, count);
                count++;
                child.Measure(constraint);
                desiredSize.Width += child.DesiredSize.Width;
                desiredSize.Height += child.DesiredSize.Height;
            }
            if (Double.IsInfinity(constraint.Height))
                constraint.Height = desiredSize.Height;
            if (Double.IsInfinity(constraint.Width))
                constraint.Width = desiredSize.Width;
            
            DesiredSize = constraint;
            return base.MeasureOverride(constraint);
        }
    }
}
