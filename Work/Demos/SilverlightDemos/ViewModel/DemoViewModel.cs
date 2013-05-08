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
        
        public ObservableCollection<SampleModel> Samples 
        {
            get;
            set; 
        }               

        public DemoViewModel()
        {
           Samples=new ObservableCollection<SampleModel>();
            AddSamples();
            
        }

        private void AddSamples()
        {
            //CPUPerformance.CPUView cpuView = new CPUPerformance.CPUView();
            
            //Sparrow.Chart.Demos.Demos.PerformanceDemo.PerformanceDemo performanceView = new Demos.PerformanceDemo.PerformanceDemo();
            SampleModel performanceDemo = new SampleModel("Performance Demo", "", "Sparrow.Chart.Demos.Demos.PerformanceDemo.PerformanceDemo");
            performanceDemo.IsHeader = false;

            SampleModel liveDataDemo = new SampleModel("Live Datas Demo", "", "Sparrow.Chart.Demos.Demos.LiveDatasDemo.LiveDatasDemo");
            liveDataDemo.IsHeader = false;
            
            Samples.Add(liveDataDemo);
            Samples.Add(performanceDemo);
         
        }

       
    }
}
