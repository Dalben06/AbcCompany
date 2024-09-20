using MySql.Data.MySqlClient;
using System.Data;

namespace AbcCompany.Core.Domain.Data
{
    public class MySqlServerStrategy : IDbStrategy
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
