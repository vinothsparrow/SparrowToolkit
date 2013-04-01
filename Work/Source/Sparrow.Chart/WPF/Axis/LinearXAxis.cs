using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class LinearXAxis : XAxis
    {
        public LinearXAxis() :     
            base()
        {
            this.Type = XType.Double; 
        }
    }
}
