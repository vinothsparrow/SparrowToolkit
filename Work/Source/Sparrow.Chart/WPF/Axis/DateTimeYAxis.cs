using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class DateTimeYAxis : YAxis
    {
        public DateTimeYAxis() :
            base()
        {
            this.Type = YType.DateTime;
        }
    }
}
