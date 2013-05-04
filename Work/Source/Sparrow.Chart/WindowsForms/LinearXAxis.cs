using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class LinearXAxis : XAxis
    {
        private double? interval;
        public double? Interval
        {
            get { return interval; }
            set { interval = value; ActualInterval = CalculateActualInterval(interval); DesiredInterval = ActualInterval; }
        }

        private double? maxValue;
        public double? MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; ActualMaxValue = CalculateActualMaxValue(maxValue); DesiredMaxValue = ActualMaxValue; }
        }

        private double? minValue;
        public double? MinValue
        {
            get { return minValue; }
            set { minValue = value; ActualMinValue = CalculateActualMinValue(minValue); DesiredMinValue = ActualMinValue; }
        }

        protected override double CalculateActualInterval(object interval)
        {
            return Convert.ToDouble(interval.ToString());
        }

        protected override double CalculateActualMaxValue(object maxValue)
        {
            return Convert.ToDouble(maxValue.ToString());
        }

        protected override double CalculateActualMinValue(object minValue)
        {
            return Convert.ToDouble(minValue.ToString());
        }
    }
}
