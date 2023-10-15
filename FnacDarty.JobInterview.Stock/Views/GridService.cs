using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Views
{
    public class GridService : IGridService
    {
        private readonly IGridViewFactory _gridViewFactory;

        public GridService(IGridViewFactory gridViewFactory)
        {
            _gridViewFactory = gridViewFactory;
        }

        public IGridView DisplayDataInGrid(object data, IReadOnlyList<GridColumn> columns)
        {
            var gridView = _gridViewFactory.CreateNew(columns);
            gridView.Bind(data);
            return gridView;
        }
    }
}
