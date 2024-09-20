using AbcCompany.Core.Domain.Configuration;
using AbcCompany.Core.Domain.Enums;
using System.Data;

namespace AbcCompany.Core.Domain.Data
{
    public class DbFactory
    {
        private readonly string _connectionString;
        private readonly DatabaseType _provider;
        public DbFactory(Settings OptionsSettings)
        {
            this._connectionString = OptionsSettings?.DatabaseContext?.ConnectionString ?? throw new Exception("Configure ConnectionString to Application");
            this._provider = OptionsSettings?.DatabaseContext.DatabaseType ?? DatabaseType.SqlServer;
        }

        public IDbConnection GetConnection()
        {
            switch (_provider)
            {
                case DatabaseType.MySql:
                    return new MySqlServerStrategy().GetConnection(this._connectionString);
                case DatabaseType.SqlServer:
                    return new SqlServerStrategy().GetConnection(this._connectionString);
                default:
                    return null;
            }

        }

    }
}
