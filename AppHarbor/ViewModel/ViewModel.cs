using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AppHarbor
{
    public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            Collection.Add(new Model(1, 8, 50));
            Collection.Add(new Model(2, 7, 40));
            Collection.Add(new Model(3, 4, 30));
            Collection.Add(new Model(4, 2, 25));
            Collection.Add(new Model(5, 5, 35));
        }
    }
}
