using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.Views
{
    public sealed class GridView : IGridView
    {
        private readonly IReadOnlyList<GridColumn> _columns;
        private IEnumerable _dataSource;

        public GridView(IReadOnlyList<GridColumn> columns, object dataSource = null)
        {
            if(columns == null) throw new ArgumentNullException(nameof(columns));
            SetColumnsOrdinal(columns);
            _columns = columns;
            _dataSource = new ArrayList();
        }

        private void SetColumnsOrdinal(IReadOnlyList<GridColumn> columns)
        {
            int index = 0;
            foreach (var column in columns)
            {
                column.Ordinal = index++;
            }
        }

        public void Bind(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var dataSource = Normalize(source);

            _dataSource = dataSource;
        }

        public void Render(TextWriter writer)
        {
            int[] columnWidths = CalculateColumnWidths();

            // Render header
            RenderSeparator(writer, columnWidths);
            RenderRow(writer, _columns.Select(c => c.Name).ToArray(), columnWidths);
            RenderSeparator(writer, columnWidths);


            // Render rows
            foreach (var row in _dataSource)
            {            
                var values = _columns.Select(column => column.GetValueAsString(row.GetType(), row)).ToArray();
                RenderRow(writer, values, columnWidths);
            }

            RenderSeparator(writer, columnWidths);
        }

        private void RenderSeparator(TextWriter writer, int[] columnWidths)
        {
            writer.Write("+");
            foreach (var width in columnWidths)
            {
                writer.Write(new string('-', width + 2));
                writer.Write("+");
            }
            writer.WriteLine();
        }

        private void RenderRow(TextWriter writer, string[] values, int[] columnWidths)
        {
            writer.Write("|");
            for (int i = 0; i < values.Length; i++)
            {
                string paddedValue = values[i].PadRight(columnWidths[i]);
                writer.Write($" {paddedValue} |");
            }
            writer.WriteLine();
        }

        private int[] CalculateColumnWidths()
        {
            var maxColumnWidths = _columns.Select(column => column.Name.Length).ToArray();

            foreach (var row in _dataSource)
            {
                int columnIndex = 0;
                var sourceType = row.GetType();

                foreach (var column in _columns)
                {
                    string value = column.GetValueAsString(sourceType, row);
                    if (value != null && value.Length > maxColumnWidths[columnIndex])
                    {
                        maxColumnWidths[columnIndex] = value.Length;
                    }
                    columnIndex++;
                }
            }

            return maxColumnWidths;
        }

        private IEnumerable Normalize(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var elementType = source.GetElementType();
            if (elementType != null && SourceHasProperties(elementType))
            {
                return (IEnumerable)source;
            }

            return new ArrayList() { source };
        }

        private bool SourceHasProperties(Type sourceType) => _columns.All(column => column.IsProperty(sourceType));
    }
}
