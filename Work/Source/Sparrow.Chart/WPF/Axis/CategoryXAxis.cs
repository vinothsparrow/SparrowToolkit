using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class CategoryXAxis : XAxis
    {
        public CategoryXAxis()
            : base()
        {
            this.Type = XType.Category;
        }
    }
}
