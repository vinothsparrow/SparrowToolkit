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
    /// Chart Legend
    /// </summary>
    public class Legend : ItemsControl
    {        
        ResourceDictionary styles;
        public Legend()
        {
            this.DefaultStyleKey = typeof(Legend);
            styles = new ResourceDictionary()
            {
#if X86
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x86;component/Themes/Styles.xaml", UriKind.Relative)
#elif X64
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x64;component/Themes/Styles.xaml", UriKind.Relative)
#elif WPF
                Source = new Uri(@"/Sparrow.Chart.Wpf;component/Themes/Styles.xaml", UriKind.Relative)
#elif SILVERLIGHT
                Source = new Uri(@"/Sparrow.Chart.Silverlight;component/Themes/Styles.xaml", UriKind.Relative)
#elif WINRT
                Source = new Uri(@"ms-appx:///Sparrow.Chart.WinRT/Themes/Styles.xaml")
#elif WP7
                Source = new Uri(@"/Sparrow.Chart.WP7;component/Themes/Styles.xaml", UriKind.Relative)
#elif WP8
                Source = new Uri(@"/Sparrow.Chart.WP8;component/Themes/Styles.xaml", UriKind.Relative)
#endif
            };
            this.HeaderTemplate = (DataTemplate)styles["legendTitleTemplate"];
            this.Loaded += Legend_Loaded;
        }

        void Legend_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeLegendOrientation();
        }       

        public SparrowChart Chart
        {
            get { return (SparrowChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(Legend), new PropertyMetadata(null));



        internal Orientation LegendOrientaion
        {
            get { return (Orientation)GetValue(LegendOrientaionProperty); }
            set { SetValue(LegendOrientaionProperty, value); }
        }

        internal static readonly DependencyProperty LegendOrientaionProperty =
            DependencyProperty.Register("LegendOrientaion", typeof(Orientation), typeof(Legend), new PropertyMetadata(Orientation.Horizontal,OnLegendOrientationChanged));

        private static void OnLegendOrientationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as Legend).ChangeLegendOrientation();
        }

        internal void ChangeLegendOrientation()
        {
            ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(this);
            if (itemsPresenter != null)
            {
                if (VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
                {
#if WINRT
                    StackPanel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 1) as StackPanel;
#else
                    StackPanel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as StackPanel;
#endif
                    if (itemsPanel != null)
                    {
                        itemsPanel.Orientation = this.LegendOrientaion;                       
                    }
                }
            }
        }

        /// <summary>
        /// http://svgvijay.blogspot.in/2013/01/how-to-get-datagrid-cell-in-wpf.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
       
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

            switch (this.Dock)
            {
                case Dock.Bottom:
                case Dock.Top:
                    this.LegendOrientaion = Orientation.Horizontal;
                    break;
                case Dock.Left:
                case Dock.Right:
                    this.LegendOrientaion = Orientation.Vertical;
                    break;
                default:
                    break;
            }
            ChangeLegendOrientation();
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(Legend), new PropertyMetadata(null));



        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Legend), new PropertyMetadata(null));



        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Legend), new PropertyMetadata(new CornerRadius(0)));


        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(Legend), new PropertyMetadata(true));        

    }
}
