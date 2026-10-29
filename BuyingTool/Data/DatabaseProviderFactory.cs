using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Npgsql;
using MySql.Data.MySqlClient;

namespace BuyingTool.Data
{
    internal static class DatabaseProviderFactory
    {
        internal static DbProviderFactory GetDbProviderFactory(string providerName)
        {
            switch (providerName)
            {
                case "Microsoft.Data.SqlClient":
                    return SqlClientFactory.Instance;
                case "PostgreSQL":
                    return NpgsqlFactory.Instance;
                case "MySQL":
                    return MySqlClientFactory.Instance;
                default:
                    throw new ArgumentException("Unsupported database provider: " + providerName);
            }
        }
    }
}