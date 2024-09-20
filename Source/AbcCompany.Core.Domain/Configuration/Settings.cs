using AbcCompany.Core.Domain.Enums;

namespace AbcCompany.Core.Domain.Configuration
{
    public sealed class Settings
    {
        public DatabaseContext DatabaseContext { get; set; }
    }

    public sealed class DatabaseContext
    {
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
    }
}
