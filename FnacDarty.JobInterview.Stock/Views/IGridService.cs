using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Views
{
    public interface IGridService
    {
        IGridView DisplayDataInGrid(object data, IReadOnlyList<GridColumn> columns);
    }
}
