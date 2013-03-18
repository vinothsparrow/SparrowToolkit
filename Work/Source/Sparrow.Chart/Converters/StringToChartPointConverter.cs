using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    /// <summary>
    /// String To ChartPoint Converter
    /// </summary>
    public class StringToChartPointConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,System.Globalization.CultureInfo culture, object value)
        {
            List<string> result = ((string)value).Split(',').ToList();
            for (int j=0;j<result.Count;j++)
            {
                var point = result[j];
                if (string.IsNullOrEmpty(point))
                    result[j] = "0";
            }
            if (result.Count % 2 != 0)
                result.Add("0");
            PointsCollection collection = new PointsCollection();
            for (int i = 0; i < result.Count; i += 2)
            {
                collection.Add(new ChartPoint() { XValue = double.Parse(result[i].ToString()), YValue = double.Parse(result[i + 1].ToString()) });
            }
            return collection;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context,System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return null;
        }
    }
}
