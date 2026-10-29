using System.Data.Common;

namespace BuyingTool.Data
{
    public interface IDatabaseConnection
    {
        DbConnection CreateConnection();
        string TryGetDbProviderFactory(out DbProviderFactory factory);
    }
}