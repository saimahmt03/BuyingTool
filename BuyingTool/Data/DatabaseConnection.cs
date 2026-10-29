using System;
using System.Configuration;
using System.Data.Common;

namespace BuyingTool.Data
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _conString;
        private readonly string _providerName;

        public DatabaseConnection()
        {
            // Read connection string from <connectionStrings> in Web.config
            var connSetting = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (connSetting == null || string.IsNullOrEmpty(connSetting.ConnectionString))
            {
                throw new ArgumentNullException("DefaultConnection", "Database connection string is missing in Web.config.");
            }
            _conString = connSetting.ConnectionString;

            // Read provider name from <appSettings> in Web.config
            var provider = ConfigurationManager.AppSettings["DatabaseProvider"];
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException("DatabaseProvider", "Database provider name is missing in Web.config.");
            }
            _providerName = provider;
        }

        public DbConnection CreateConnection()
        {
            DbProviderFactory factory = DatabaseProviderFactory.GetDbProviderFactory(_providerName);
            if (factory == null)
            {
                throw new InvalidOperationException("Unsupported database provider: " + _providerName);
            }

            DbConnection connection = factory.CreateConnection();
            if (connection == null)
            {
                throw new InvalidOperationException("Failed to create a database connection.");
            }

            connection.ConnectionString = _conString;
            connection.Open(); // Synchronous open
            return connection;
        }

        public string TryGetDbProviderFactory(out DbProviderFactory factory)
        {
            try
            {
                factory = DatabaseProviderFactory.GetDbProviderFactory(_providerName);
            }
            catch (Exception)
            {
                factory = null;
            }

            if (factory == null)
            {
                return "Database provider factory is missing.";
            }

            return string.Empty;
        }
    }
}