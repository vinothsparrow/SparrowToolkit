# +**Create a basic bulletgraph using BulletGraph Control**+
# [#WPF](#WPF)
# [#Silverlight](#Silverlight)
# [#WindowsPhone](#WindowsPhone)
# [#WinRT](#WinRT)
{anchor:WPF}
## +**WPF**+
* Obtain the BulletGraph for WPF from nuget => [Sparrow.BulletGraph.Wpf](https://nuget.org/packages/Sparrow.BulletGraph.Wpf/)
* Import the following namespace in the XAML
{{
 xmlns:sparrow="http://sparrowtoolkit.codeplex.com/wpf"
 }} 
* Create the BulletGraph in XAML
{{

<sparrow:BulletGraph Minimum="0" Interval="500" MinorTickStep="3" PerformanceMeasure="1700" ComparativeMeasureSpacing="0.3"   ComparativeMeasure="2100"   Maximum="2500"  Height="45" VerticalAlignment="Center">
                        <sparrow:QualitativeRange Maximum="2000" Fill="#D8D8D8"/>
                        <sparrow:QualitativeRange Maximum="1400" Fill="#7F7F7F"/>
                        <sparrow:QualitativeRange Maximum="2500" Fill="#EBEBEB"/>
  </sparrow:BulletGraph> 
}}
![](Create a Basic BulletGraph_http://download-codeplex.sec.s-msft.com/Download?ProjectName=sparrowtoolkit&DownloadId=686173)

{anchor:Silverlight}
## +**Silverlight**+
* Obtain the BulletGraph for Silverlight from nuget => [Sparrow.BulletGraph.Silverlight](https://nuget.org/packages/Sparrow.BulletGraph.Silverlight/)
* Import the following namespace in the XAML
{{
 xmlns:sparrow="http://sparrowtoolkit.codeplex.com/silverlight"
 }} 
* Create the BulletGraph in XAML
{{

<sparrow:BulletGraph Minimum="0" Interval="500" MinorTickStep="3" PerformanceMeasure="1700" ComparativeMeasureSpacing="0.3"   ComparativeMeasure="2100"   Maximum="2500"  Height="45" VerticalAlignment="Center">
                        <sparrow:QualitativeRange Maximum="2000" Fill="#D8D8D8"/>
                        <sparrow:QualitativeRange Maximum="1400" Fill="#7F7F7F"/>
                        <sparrow:QualitativeRange Maximum="2500" Fill="#EBEBEB"/>
  </sparrow:BulletGraph> 
}}
![](Create a Basic BulletGraph_http://download-codeplex.sec.s-msft.com/Download?ProjectName=sparrowtoolkit&DownloadId=686174)
{anchor:WindowsPhone}
## +**Windows Phone 7.1 and 8**+
* Obtain the BulletGraph for Windows Phone from nuget => 
| WP7 | [Sparrow.BulletGraph.WP7](https://nuget.org/packages/Sparrow.BulletGraph.WP7/) |
| WP8 | [Sparrow.BulletGraph.WP8](https://nuget.org/packages/Sparrow.BulletGraph.WP8/) |
* Import the following namespace in the XAML
For WP7
{{
xmlns:sparrow="clr-namespace:Sparrow.BulletGraph;assembly=Sparrow.BulletGraph.WP7.45"
 }} 
For WP8
{{
xmlns:sparrow="clr-namespace:Sparrow.BulletGraph;assembly=Sparrow.BulletGraph.WP8.45" 
 }} 
* Create the BulletGraph in XAML
{{
<sparrow:BulletGraph Minimum="0" Interval="500" MinorTickStep="3" PerformanceMeasure="1700" ComparativeMeasureSpacing="0.3"   ComparativeMeasure="2100"   Maximum="2500"  Height="45" VerticalAlignment="Center">
                        <sparrow:QualitativeRange Maximum="2000" Fill="#D8D8D8"/>
                        <sparrow:QualitativeRange Maximum="1400" Fill="#7F7F7F"/>
                        <sparrow:QualitativeRange Maximum="2500" Fill="#EBEBEB"/>
  </sparrow:BulletGraph> 
}}
![](Create a Basic BulletGraph_http://download-codeplex.sec.s-msft.com/Download?ProjectName=sparrowtoolkit&DownloadId=686175)
{anchor:WinRT}
## +**WinRT**+
* Obtain the BulletGraph for WinRT from nuget => [Sparrow.BulletGraph.WinRT](https://nuget.org/packages/Sparrow.BulletGraph.WinRT)
* Import the following namespace in the XAML
{{
xmlns:sparrow="using:Sparrow.BulletGraph" 
 }} 
* Create the BulletGraph in XAML
{{

<sparrow:BulletGraph Minimum="0" Interval="500" MinorTickStep="3" PerformanceMeasure="1700" ComparativeMeasureSpacing="0.3"   ComparativeMeasure="2100"   Maximum="2500"  Height="45" VerticalAlignment="Center">
                        <sparrow:QualitativeRange Maximum="2000" Fill="#D8D8D8"/>
                        <sparrow:QualitativeRange Maximum="1400" Fill="#7F7F7F"/>
                        <sparrow:QualitativeRange Maximum="2500" Fill="#EBEBEB"/>
  </sparrow:BulletGraph> 
}}
![](Create a Basic BulletGraph_http://download-codeplex.sec.s-msft.com/Download?ProjectName=sparrowtoolkit&DownloadId=686172)