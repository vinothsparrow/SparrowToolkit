using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Sparrow.Chart.Demos
{
    public class DemoViewModel
    {
        public ICommand AboutCommand 
        {
            get; 
            set; 
        }
        
        public ObservableCollection<CategoryModel> Categories 
        {
            get;
            set; 
        }               

        public DemoViewModel()
        {
            Categories = new ObservableCollection<CategoryModel>();
            AddCategories();
            AboutCommand = new AboutCommand();
        }

        private void AddCategories()
        {
            CPUPerformance.CPUView cpuView = new CPUPerformance.CPUView();
            SampleModel cpuDemo = new SampleModel("Task Manager Demo", "", "CPUPerformance.CPUView");
            cpuDemo.IsHeader = false;
            List<SampleModel> showCaseSamples = new List<SampleModel>();
            showCaseSamples.Add(cpuDemo);

            CategoryModel showCase = new CategoryModel("Showcase", showCaseSamples);
            showCase.IsHeader = true;
            Categories.Add(showCase);
        }

       
    }
}
