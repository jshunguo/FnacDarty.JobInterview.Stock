using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections;

namespace FnacDarty.JobInterview.Stock.Views
{
    public class GridColumn : IEquatable<GridColumn>
    {
        public string Name { get; }
        public int Ordinal { get; internal set; }

        public GridColumn(string name)
        {
            Name = name;
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

        bool IEquatable<GridColumn>.Equals(GridColumn other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as GridColumn;
            if(other == null) return false;
            return Name == other.Name && Ordinal == other.Ordinal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(string.IsNullOrEmpty(Name)? 0 : Name.GetHashCode(), Ordinal);
        }
    }
}
