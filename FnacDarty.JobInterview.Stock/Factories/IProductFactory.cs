using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public interface IProductFactory
    {
        Product Get(string productId);
        Product Get(string productId, long quantity);
    }
}
