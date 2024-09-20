using System.Data;

namespace AbcCompany.Core.Domain.Data
{
    public interface IDbStrategy
    {
        IDbConnection GetConnection(string connectionString);
    }
}
