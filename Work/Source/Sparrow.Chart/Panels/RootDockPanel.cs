using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize)
        {
            ContainerCollection container = (ContainerCollection)this.FindName("PART_containers");
            base.ArrangeOverride(arrangeSize);
            //if (container != null)
            //    container.Refresh();
            return arrangeSize;
        }
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            DockPanel.SetDock(this.Children[0], LegendDock);
            System.Windows.Size desiredSize = new System.Windows.Size(0, 0);
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
