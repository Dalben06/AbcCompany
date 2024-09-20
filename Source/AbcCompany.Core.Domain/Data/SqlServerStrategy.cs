using System.Data;
using System.Data.SqlClient;

namespace AbcCompany.Core.Domain.Data
{
    public class SqlServerStrategy : IDbStrategy
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
