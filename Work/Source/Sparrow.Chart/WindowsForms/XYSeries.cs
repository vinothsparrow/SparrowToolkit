using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public abstract class XYSeries : Series
    {

        public string YPath
        {
            get;
            set;
        }

        public PointCollection Points
        {
            get;
            set;
        }


        internal override double GetMaximumFromPoints(Axis axis)
        {
            if (Points.Count > 0)
            {
                if (axis is XAxis)
                {
                    double maximum = this.Points[0].XValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Max(maximum, point.XValue);
                    }
                    return maximum;
                }
                else
                {
                    double maximum = this.Points[0].YValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Max(maximum, point.YValue);
                    }
                    return maximum;
                }
            }
            else
                return 0;
        }

        internal override double GetMinimumFromPoints(Axis axis)
        {
            if (Points.Count > 0)
            {
                if (axis is XAxis)
                {
                    double maximum = this.Points[0].XValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Max(maximum, point.XValue);
                    }
                    return maximum;
                }
                else
                {
                    double maximum = this.Points[0].YValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Max(maximum, point.YValue);
                    }
                    return maximum;
                }
            }
            else
                return 0;
        }
    }
}
