using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FnacDarty.JobInterview.Stock.Views
{
    internal class GridDefinition : IGridDefinition
    {
        private readonly IList<GridColumn> _columns;

        public GridDefinition() 
        {
            _columns = new List<GridColumn>();
        }

        public IGridDefinition AddColumn(string columnName, Type typeName)
        {
            var column = new GridColumn(columnName, typeName);
            _columns.Add(column);
            column.Ordinal = _columns.Count - 1;
            return this;
        }

        public IGridDefinition AddColumns<T>()
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                AddColumn(property.Name, property.PropertyType);
            }
            return this;
        }

        public IGridDefinition AddEmptyColumn()
        {
            return AddColumn(string.Empty, typeof(string));
        }

        public IGridView CreateView()
        {
            return new GridView(_columns.ToArray());
        }
    }
}
