using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public abstract class Series 
    {
        public Series()
        {            
            this.StrokeThickness = 1.0f;
        }
        public string XPath
        {
            get;
            set;
        }

        public Brush Stroke
        {
            get; 
            set;
        }

        public float StrokeThickness
        {
            get; 
            set;
        }

        internal Graphics RootGraphics
        {
            get;
            set;
        }

        public string Label
        {
            get; 
            set; 
        }

        public XAxis XAxis
        {
            get; 
            set;
        }

        public YAxis YAxis
        {
            get;
            set;
        }

        public PartCollection Parts
        {
            get; 
            set;
        }

        public IEnumerable PointsSource
        {
            get; 
            set;
        }

        public SparrowChart Chart
        {
            get; 
            set;
        }

        protected void DrawAllSeriesParts()
        {
            if (this.RootGraphics != null)
                foreach (var part in Parts)
                {
                    part.DrawSeriesPart();
                }
        }

        protected virtual void GeneratePoints()
        {
           
        }

        internal void Refresh()
        {
            this.GeneratePoints();
        }

        internal virtual double GetMinimumFromPoints(Axis axis)
        {
            return 0;
        }

        internal virtual double GetMaximumFromPoints(Axis axis)
        {
            return 0;
        }
       
    }
}
