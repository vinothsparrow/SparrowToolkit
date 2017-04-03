# +**Create a basic chart using SparrowChart Control**+
# [#WPF](#WPF)
# [#Silverlight](#Silverlight)
# [#WindowsPhone](#WindowsPhone)
# [#WinRT](#WinRT)
{anchor:WPF}
## +**WPF**+
* Obtain the SparrowChart for WPF from nuget => [Sparrow.Chart.Wpf](https://nuget.org/packages/Sparrow.Chart.Wpf/) 
* Import the following namespace in the XAML
{{
 xmlns:sparrow="http://sparrowtoolkit.codeplex.com/wpf"
 }} 
* Create the sparrow chart in XAML
* **Method 1**
{{
<sparrow:SparrowChart>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries>             
                <sparrow:LineSeries.Points>
                        <sparrow:DoublePoint Data="0" Value="1"/>
                        <sparrow:DoublePoint Data="1" Value="2"/>
                        <sparrow:DoublePoint Data="2" Value="3"/>
                        <sparrow:DoublePoint Data="3" Value="4"/> 
                 </sparrow:LineSeries.Points>
            </sparrow:LineSeries>
</sparrow:SparrowChart>
}}
* **Method 2**
**Points="x,y,x,y......etc"**
{{
<sparrow:SparrowChart>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries Points="0,1,1,2,2,3,3,4"/>
</sparrow:SparrowChart>
}}
* **Method 3**
**MVVM Based**
{{
//Create a model
public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }
       
        public Model(double x,double y)
        {
            X = x;
            Y = y;           
        }      
    }

// Create a ViewModel
public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
            this.Collection.Add(new Model(0, 1));
            this.Collection.Add(new Model(1, 2));
            this.Collection.Add(new Model(2, 3));
            this.Collection.Add(new Model(3, 4));
        }
    }

//Use the viewmodel in the Sparrow Chart
<sparrow:SparrowChart>
       <sparrow:SparrowChart.DataContext> 
              <local:ViewModel/>
      </sparrow:SparrowChart.DataContext>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries PointsSource="{Binding Collection}" XPath="X" YPath="Y"/>
</sparrow:SparrowChart>
}}
{anchor:Silverlight}
## +**Silverlight**+
* Obtain the SparrowChart for Silverlight from nuget => [Sparrow.Chart.Silverlight](https://nuget.org/packages/Sparrow.Chart.Silverlight/)
* Import the following namespace in the XAML
{{
 xmlns:sparrow="http://sparrowtoolkit.codeplex.com/silverlight"
 }} 
* Create the sparrow chart in XAML
* **Method 1**
{{
<sparrow:SparrowChart>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries>             
                <sparrow:LineSeries.Points>
                        <sparrow:DoublePoint Data="0" Value="1"/>
                        <sparrow:DoublePoint Data="1" Value="2"/>
                        <sparrow:DoublePoint Data="2" Value="3"/>
                        <sparrow:DoublePoint Data="3" Value="4"/> 
                 </sparrow:LineSeries.Points>
            </sparrow:LineSeries>
</sparrow:SparrowChart>
}}
* **Method 2**
**MVVM Based**
{{
//Create a model
public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }
       
        public Model(double x,double y)
        {
            X = x;
            Y = y;           
        }      
    }

// Create a ViewModel
public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
            this.Collection.Add(new Model(0, 1));
            this.Collection.Add(new Model(1, 2));
            this.Collection.Add(new Model(2, 3));
            this.Collection.Add(new Model(3, 4));
        }
    }

//Use the viewmodel in the Sparrow Chart
<sparrow:SparrowChart>
       <sparrow:SparrowChart.DataContext> 
              <local:ViewModel/>
      </sparrow:SparrowChart.DataContext>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries PointsSource="{Binding Collection}" XPath="X" YPath="Y"/>
</sparrow:SparrowChart>
}}
{anchor:WindowsPhone}
## +**Windows Phone 7.1 and 8**+
* Obtain the SparrowChart for Windows Phone from nuget => 
| WP7 | [Sparrow.Chart.WP7](https://nuget.org/packages/Sparrow.Chart.WP7/) |
| WP8 | [Sparrow.Chart.WP8](https://nuget.org/packages/Sparrow.Chart.WP8/) |
* Import the following namespace in the XAML
For WP7
{{
xmlns:sparrow="clr-namespace:Sparrow.Chart;assembly=Sparrow.Chart.WP7.45"
 }} 
For WP8
{{
xmlns:sparrow="clr-namespace:Sparrow.Chart;assembly=Sparrow.Chart.WP8.45"
 }} 
* Create the sparrow chart in XAML
* **Method 1**
{{
<sparrow:SparrowChart>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries>             
                <sparrow:LineSeries.Points>
                        <sparrow:DoublePoint Data="0" Value="1"/>
                        <sparrow:DoublePoint Data="1" Value="2"/>
                        <sparrow:DoublePoint Data="2" Value="3"/>
                        <sparrow:DoublePoint Data="3" Value="4"/> 
                 </sparrow:LineSeries.Points>
            </sparrow:LineSeries>
</sparrow:SparrowChart>
}}
* **Method 2**
**MVVM Based**
{{
//Create a model
public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }
       
        public Model(double x,double y)
        {
            X = x;
            Y = y;           
        }      
    }

// Create a ViewModel
public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
            this.Collection.Add(new Model(0, 1));
            this.Collection.Add(new Model(1, 2));
            this.Collection.Add(new Model(2, 3));
            this.Collection.Add(new Model(3, 4));
        }
    }

//Use the viewmodel in the Sparrow Chart
<sparrow:SparrowChart>
       <sparrow:SparrowChart.DataContext> 
              <local:ViewModel/>
      </sparrow:SparrowChart.DataContext>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries PointsSource="{Binding Collection}" XPath="X" YPath="Y"/>
</sparrow:SparrowChart>
}}
{anchor:WinRT}
## +**WinRT**+
* Obtain the SparrowChart for WinRT from nuget => [Sparrow.Chart.WinRT](https://nuget.org/packages/Sparrow.Chart.WinRT/) 
* Import the following namespace in the XAML
{{
 xmlns:sparrow="using:Sparrow.Chart"
 }} 
* Create the sparrow chart in XAML
* **Method 1**
{{
<sparrow:SparrowChart>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries>             
                <sparrow:LineSeries.Points>
                        <sparrow:DoublePoint Data="0" Value="1"/>
                        <sparrow:DoublePoint Data="1" Value="2"/>
                        <sparrow:DoublePoint Data="2" Value="3"/>
                        <sparrow:DoublePoint Data="3" Value="4"/> 
                 </sparrow:LineSeries.Points>
            </sparrow:LineSeries>
</sparrow:SparrowChart>
}}
* **Method 2**
**MVVM Based**
{{
//Create a model
public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }
       
        public Model(double x,double y)
        {
            X = x;
            Y = y;           
        }      
    }

// Create a ViewModel
public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
            this.Collection.Add(new Model(0, 1));
            this.Collection.Add(new Model(1, 2));
            this.Collection.Add(new Model(2, 3));
            this.Collection.Add(new Model(3, 4));
        }
    }

//Use the viewmodel in the Sparrow Chart
<sparrow:SparrowChart>
       <sparrow:SparrowChart.DataContext> 
              <local:ViewModel/>
      </sparrow:SparrowChart.DataContext>
            <sparrow:SparrowChart.XAxis>
                    <sparrow:LinearXAxis/>
            </sparrow:SparrowChart.XAxis>
            <sparrow:SparrowChart.YAxis>
                    <sparrow:LinearYAxis/>
            </sparrow:SparrowChart.YAxis>
           <sparrow:LineSeries PointsSource="{Binding Collection}" XPath="X" YPath="Y"/>
</sparrow:SparrowChart>
}}
