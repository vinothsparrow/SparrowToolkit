using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class DateTimeXAxis : XAxis
    {
        public DateTimeXAxis() :
            base()
        {
            this.Type = XType.DateTime;
        }
    }
}
