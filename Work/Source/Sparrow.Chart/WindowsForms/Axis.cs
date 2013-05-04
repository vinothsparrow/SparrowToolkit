using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public abstract class Axis 
    {       

       
        internal RectF PositionedRect
        {
            get; 
            set;
        }

        internal double ActualInterval
        {
            get; 
            set;
        }

        internal double ActualMinValue
        {
            get;
            set;
        }

        internal double ActualMaxValue
        {
            get; 
            set;
        }

        internal double DesiredInterval
        {
            get;
            set;
        }

        internal double DesiredMaxValue
        {
            get;
            set;
        }

        internal double DesiredMinValue
        {
            get;
            set;
        }

        internal AxisPosition AxisPosition
        {
            get; 
            set;
        }

        public virtual float PointFromValue(double value)
        {
           return 0;
        }

        public bool CheckValue(double value)
        {
            if (value >= DesiredMinValue && value <= DesiredMaxValue)
                return true;
            else
                return false;
        }

        protected virtual double CalculateActualInterval(object interval)
        {
            return 0;
        }

        protected virtual double CalculateActualMinValue(object minValue)
        {
            return 0;
        }

        protected virtual double CalculateActualMaxValue(object maxValue)
        {
            return 0;
        }
    }
}
