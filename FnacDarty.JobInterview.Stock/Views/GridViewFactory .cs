using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Views
{
    public class GridViewFactory : IGridViewFactory
    {
        public IGridView CreateNew(IReadOnlyList<GridColumn> columns)
        {
            return new GridView(columns);
        }
    }
}
