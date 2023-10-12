
namespace FnacDarty.JobInterview.Stock.Entities
{
    internal static class HashCode
    {
        public static int Combine(params object[] entities)
        {
            int hash = 1;

            foreach (var entity in entities)
            {
                hash ^= entity != null ? entity.GetHashCode() : 0;
            }

            return hash;
        }
    }
}
