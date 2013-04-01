using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class LinearYAxis : YAxis
    {
        public LinearYAxis() :
            base()
        {
            this.Type = YType.Double;
        }
    }
}
