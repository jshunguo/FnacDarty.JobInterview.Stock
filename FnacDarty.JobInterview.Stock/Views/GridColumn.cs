using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace FnacDarty.JobInterview.Stock.Views
{
    public class GridColumn
    {
        public string Name { get; }
        public Type DataType { get; }
        public int Ordinal { get; internal set; }

        public GridColumn(string name, Type dataType)
        {
            Name = name;
            DataType = dataType;
        }

        internal string GetValueAsString(Type sourceType, object row)
        {
            var property = sourceType.GetProperty(Name);
            if (property != null)
            {
                 return property.GetValue(row)?.ToString();
            }
            else if(row.IsEnumerable())
            {
                var enumerable = (IEnumerable)row;
                return enumerable.ElementAt(Ordinal)?.ToString();
            }
            return row?.ToString();
        }

        internal bool IsProperty(Type sourceType)
        {
            return sourceType.GetProperty(Name) != null;
        }
    }
}
