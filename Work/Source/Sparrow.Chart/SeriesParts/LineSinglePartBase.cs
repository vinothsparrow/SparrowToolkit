using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Sparrow.Chart
{
    public class LineSinglePartBase : SeriesPartBase
    {
        public LineSinglePartBase()
        {
        }

        private PointCollection linePoints;
        public PointCollection LinePoints 
        {
            get { return linePoints; }
            set { linePoints = value; }
        }

        
    }
}
