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
    /// Chart Legend
    /// </summary>
    public class Legend : HeaderedItemsControl
    {

        public Legend()
        {
            this.DefaultStyleKey = typeof(Legend);
        }

        public SparrowChart Chart
        {
            get { return (SparrowChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(Legend), new PropertyMetadata(null));

       
        public Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockProperty, value); }
        }

        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register("Dock", typeof(Dock), typeof(Legend), new PropertyMetadata(Dock.Top,OnDockChanged));

        private static void OnDockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as Legend).DockChanged(args);
        }
        internal void DockChanged(DependencyPropertyChangedEventArgs args)
        {
            DockPanel.SetDock(this, this.Dock);
            if (this.Chart != null)
            {
                if (this.Chart.rootDockPanel != null)
                    this.Chart.rootDockPanel.InvalidateMeasure();
            }
        }        
        
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is LegendItem);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new LegendItem();
        }
    }
}
