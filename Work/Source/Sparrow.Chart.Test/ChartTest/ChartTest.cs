using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Threading;

namespace Sparrow.Chart.Test
{
    [TestClass]
    public class ChartTest
    {
        [TestMethod]
        public void TestDefaultRenderingMode()
        {
            Window window = new Window();

            Sparrow.Chart.SparrowChart Chart = new Sparrow.Chart.SparrowChart();
            Chart.XAxis = new Sparrow.Chart.XAxis();
            Chart.YAxis = new Sparrow.Chart.YAxis();
            Chart.Series.Add(new Sparrow.Chart.LineSeries());
            window.Content = Chart;
            window.Show();
            Thread.Sleep(1000);
            Assert.AreEqual(Sparrow.Chart.RenderingMode.DefaultWPFRendering, Chart.RenderingMode);
        }
        [TestMethod]
        public void TestDefaultSmoothingMode()
        {
            Sparrow.Chart.SparrowChart Chart = new Sparrow.Chart.SparrowChart();
            Chart.XAxis = new Sparrow.Chart.XAxis();
            Chart.YAxis = new Sparrow.Chart.YAxis();
            Chart.Series.Add(new Sparrow.Chart.LineSeries());
            Assert.AreEqual(Sparrow.Chart.SmoothingMode.HighQuality, Chart.SmoothingMode);
        }
    }
}
