using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BuyingTool.Data;
using BuyingTool.Shared;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuyingTool.Repository
{
    public class DBContext : IDBContext
    {
        private readonly IDatabaseConnection _databaseConnection;

        public DBContext(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        #region VERIFY USER

        public string VerifyAccount(string userid, string pass)
        {
            var response = string.Empty;

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    // Explicitly check if the connection object was successfully created
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        // Updated to point to your validation stored procedure (or use your StoredProcedures constant)
                        command.CommandText = StoredProcedures.VerifyUserAccount;
                        command.CommandType = CommandType.StoredProcedure;

                        // Increase timeout from default 30 seconds to 60 seconds (or more if needed)
                        //command.CommandTimeout = 20;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@USERID", userid, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PASSWORD", pass, DbType.String, factory));

                        // 2. Execute the command and capture the single string response
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            response = result.ToString();
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in Login. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in Login. ", ex.Message);
                throw;
            }

            return response;
        }

        public Task<(string response, string dbResponse)> AccountVerification(string idnumber, string pass)
        {
            string logMessage = string.Empty;
            var response = string.Empty;

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    // Explicitly check if the connection object was successfully created
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        // Updated to point to your validation stored procedure (or use your StoredProcedures constant)
                        command.CommandText = StoredProcedures.AccoutVerification;
                        command.CommandType = CommandType.StoredProcedure;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@USERID", idnumber, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PASSWORD", pass, DbType.String, factory));

                        // 2. Execute the command and capture the single string response
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            response = result.ToString();
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                logMessage = string.Concat("Database error in Login. ", ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                logMessage = string.Concat("System error in Login. ", ex.Message);
                //throw;
            }

            return Task.FromResult((response, logMessage));
        }

        public Task<(string AccountStatus, string LogMessage)> Get(string userId, string pass)
        {
            string accountStatus = string.Empty;
            var perSiteTable = new DataTable();
            string logMessage = string.Empty;

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.AccoutVerification;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@USERID", userId ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PASSWORD", pass ?? string.Empty, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            // 1. Read the first result set (Account Status / Login Message)
                            if (reader.Read())
                            {
                                accountStatus = reader[0]?.ToString() ?? string.Empty;
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                logMessage = string.Concat("Database error in GetPerSiteTableAsync. ", ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                logMessage = string.Concat("System error in GetPerSiteTableAsync. ", ex.Message);
                //throw;
            }

            return Task.FromResult((accountStatus, logMessage));
        }

        #endregion

        // CALCULATION
        public Task<DataTable> Calculate(string brand, string division, string department, string category, string firstGRPeriodYear, string periodPlan, string idnumber, decimal wos, decimal plc, decimal factorRate)
        {
            var calculateResult = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {

                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetCalculation;
                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Brand", brand ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Division", division ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Department", department ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Category", category ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@FirstGRPeriodYear", firstGRPeriodYear ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PlanPeriodYear", periodPlan ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@WOS", wos, DbType.Decimal, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PLC", plc, DbType.Decimal, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@FactorRate", factorRate, DbType.Decimal, factory));

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }


                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            calculateResult.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in GetPerSiteTableAsync. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in GetPerSiteTableAsync. ", ex.Message);
                throw;
            }

            return Task.FromResult(calculateResult);
        }

        // FILTERS
        public Task<DataTable> GetMainTable(string userid, string pass)
        {
            var mainTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetMainTable;
                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandTimeout = 120;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", userid, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Password", pass, DbType.String, factory));

                        // True asynchronous reader execution
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            mainTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in GetMainTableAsync. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in GetMainTableAsync. ", ex.Message);
                throw;
            }

            return Task.FromResult(mainTable);
        }

        #region New Filters Pulling

        public Task<DataTable> GetFiltersAttribute(string idnumber)
        {
            var dataTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetFiltersAttribute;
                        command.CommandType = CommandType.StoredProcedure;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        // Add parameter using DatabaseParameterHelper
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@USERID", idnumber, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in retrieving Attribute Filters. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in retrieving Attribute Filters. ", ex.Message);
                throw;
            }

            return Task.FromResult(dataTable);
        }

        public Task<DataTable> GetFiltersOrganization(string idnumber)
        {
            var dataTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetFiltersOrganization;
                        command.CommandType = CommandType.StoredProcedure;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        // Add parameter using DatabaseParameterHelper
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@USERID", idnumber, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in retrieving Period Plan Table. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in retreiving Period Plan Table. ", ex.Message);
                throw;
            }

            return Task.FromResult(dataTable);
        }

        public Task<DataTable> GetFiltersRequired(string idnumber)
        {
            var dataTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetFiltersRequired;
                        command.CommandType = CommandType.StoredProcedure;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        // Add parameter using DatabaseParameterHelper
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@USERID", idnumber, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in retrieving Period Plan Table. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in retreiving Period Plan Table. ", ex.Message);
                throw;
            }

            return Task.FromResult(dataTable);
        }

        public Task<DataTable> GetFiltersStyles(string idnumber)
        {
            var dataTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetFiltersStyles;
                        command.CommandType = CommandType.StoredProcedure;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        // Add parameter using DatabaseParameterHelper
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@USERID", idnumber, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in retrieving Period Plan Table. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in retreiving Period Plan Table. ", ex.Message);
                throw;
            }

            return Task.FromResult(dataTable);
        }

        #endregion


        // DISTRIBUTION SUMMARY, PER SITE, EXPORT FILTER
        public Task<DataTable> GetFinalDataSetFilter(string idnumber)
        {
            var dataTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetStoreGradeAndSizeFilter;
                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }


                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in retrieving Store Grade and Size Filter.", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in retreiving Store Grade and Size Filter.", ex.Message);
                throw;
            }

            return Task.FromResult(dataTable);
        }

        // PERIOD PLAN (concat)
        public Task<DataTable> GetPeriodPlan()
        {
            var dataTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetPeriodPlan;
                        command.CommandType = CommandType.StoredProcedure;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in retrieving Period Plan Table. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in retreiving Period Plan Table. ", ex.Message);
                throw;
            }

            return Task.FromResult(dataTable);
        }

        // OPTION GELLARY
        public Task<DataTable> GetOptionGalTable(
            // Required Parameters
            string brand,
            string division,
            string department,
            string category,
            string firstGRPeriodYear,
            string planPeriodYear,

            // Attributes Parameters
            string color,
            string silhouette,
            string collection,
            string fitCut,
            string materialComposition,
            string materialConstruction,
            string printTexture,
            string productLine,
            string sleeveType,
            string styleCollection,
            string sizeGroup,
            string styleCode,
            string genericMaterial,
            string subCategory,
            string origSrp,

            // Site Parameters
            string siteCode,
            string region,
            string country,
            string channel,
            string territory,
            string area,
            string outletType,
            string siteName,
            string storeGrade,

            // New Parameters
            string tagFilter,
            string userId
            )
        {
            var optionGalTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetOptionGallery;
                        command.CommandType = CommandType.StoredProcedure;

                        // Increase timeout from default 30 seconds to 60 seconds (or more if needed)
                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Brand", brand ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Division", division ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Department", department ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Category", category ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@FirstGRPeriodYear", firstGRPeriodYear ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PlanPeriodYear", planPeriodYear ?? string.Empty, DbType.String, factory));

                        // Attributes Parameters
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Color", color ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Silhouette", silhouette ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Collection", collection ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@FitCut", fitCut ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@MaterialComposition", materialComposition ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@MaterialConstruction", materialConstruction ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PrintTexture", printTexture ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ProductLine", productLine ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SleeveType", sleeveType ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StyleCollection", styleCollection ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SizeGroup", sizeGroup ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StyleCode", styleCode ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@GenericMaterial", genericMaterial ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SubCategory", subCategory ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@OrigSRP", origSrp ?? string.Empty, DbType.String, factory));

                        // Site Parameters
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SiteCode", siteCode ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Region", region ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Country", country ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Channel", channel ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Territory", territory ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Area", area ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@OutletType", outletType ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SiteName", siteName ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreGrade", storeGrade ?? string.Empty, DbType.String, factory));

                        // New Parameters
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@TagFilter", tagFilter ?? string.Empty, DbType.AnsiString, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", userId ?? string.Empty, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            optionGalTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in retrieving Export Table. ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("System error in retreiving Export Table. ", ex.Message);
                throw;
            }

            return Task.FromResult(optionGalTable);
        }

        // DISTRIBUTION SUMMARY
        public Task<(DataTable[], string dbResponse)> GetDistributionSummryTable(string idnumber, string storegrade, string size)
        {
            var logMessage = string.Empty;
            var distriSumTables = new List<DataTable>();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetSummaryDistribution;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        // Explicitly align parameters to match the stored procedure contract
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Sizes", size ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreGrades", storegrade ?? string.Empty, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            while (!reader.IsClosed)
                            {
                                var table = new DataTable();
                                table.Load(reader); 
                                distriSumTables.Add(table);
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                logMessage = string.Concat("Database error in retrieving Export Table. ", ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                logMessage = string.Concat("System error in retrieving Export Table. ", ex.Message);
                throw;
            }

            return Task.FromResult((distriSumTables.ToArray(), logMessage));
        }

        // PER SITE
        public Task<(DataTable, string dbResponse)> GetPerSiteTable(string idnumber, string storegrade, string size, string type)
        {
            string logMessage = string.Empty;
            var perSiteTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetStoreSizeBreakdown;
                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Type", type ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Sizes", size ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreGrades", storegrade ?? string.Empty, DbType.String, factory));

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                       
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            perSiteTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                logMessage = string.Concat("Database error in GetPerSiteTableAsync. ", ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                logMessage = string.Concat("System error in GetPerSiteTableAsync. ", ex.Message);
                //throw;
            }

            return Task.FromResult((perSiteTable, logMessage));
        }

        // EXPORT
        public Task<(DataTable, string dbResponse)> GetExportTable(string idnumber, string storegrade, string size, string type)
        {
            string logMessage = string.Empty;
            var exportTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetExport;
                        command.CommandType = CommandType.StoredProcedure;

                        // Increase timeout from default 30 seconds to 60 seconds (or more if needed)
                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Type", type ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Sizes", size ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreGrades", storegrade ?? string.Empty, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            exportTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                logMessage = string.Concat("Database error in retrieving Export Table. ", ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                logMessage = string.Concat("System error in retreiving Export Table. ", ex.Message);
                //throw;
            }

            return Task.FromResult((exportTable, logMessage));
        }

        // SIZE CURVE
        public Task<(DataTable, string dbResponse)> GetSizeCurveTable(string idnumber, string brand, string division, string department, string category, string planperiodyear)
        {
            string logMessage = string.Empty;
            var optionGalTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetSizeCure;
                        command.CommandType = CommandType.StoredProcedure;

                        // Increase timeout from default 30 seconds to 60 seconds (or more if needed)
                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Brand", brand ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Division", division ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Department", department ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Category", category ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@PlanPeriodYear", planperiodyear ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            optionGalTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                 logMessage = string.Concat("Database error in retrieving Export Table. ", ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                 logMessage = string.Concat("System error in retreiving Export Table. ", ex.Message);
                //throw;
            }

            return Task.FromResult((optionGalTable,logMessage));
        }

        // EXPORT TO ALLOC
        public Task<(string response, string dbResponse)> ExportToAlloc(XDocument xmlDoc, string brand, string collection, int fiscalMonth, int fiscalYear, string matno, string color, string idnumber)
        {
            var response = "Success";
            var logMessage = string.Empty;

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    DbProviderFactory factory;
                    string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        throw new InvalidOperationException(errorMessage);
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.AddToAlloc;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 60;

                        // Add parameters matching the updated stored procedure signature
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Brand", brand ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Collection", collection ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@FiscalMonth", fiscalMonth, DbType.Int32, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@FiscalYear", fiscalYear, DbType.Int32, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@MatNo", matno ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Color", color ?? string.Empty, DbType.String, factory));

                        string xmlString = xmlDoc != null ? xmlDoc.ToString() : string.Empty;
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@FileXML", xmlString, DbType.Xml, factory));

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (DbException ex)
            {
                 logMessage = string.Concat("Database error in ExportToAlloc. ", ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                 logMessage = string.Concat("System error in ExportToAlloc. ", ex.Message);
                //throw;
            }

            return Task.FromResult((response, logMessage));
        }

        // MONITORING OF EXPORTED TO ALLOC TOOL
        public Task<(DataTable, string dbResponse)> GetExortedToAllocToolHistory(string idnumber)
        {
            string logMessage = "Success";
            var historyTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetExportedToAllocToolHistory;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        // Trim leading/trailing whitespace from idnumber to prevent mismatch issues
                        string sanitizedId = idnumber?.Trim() ?? string.Empty;

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserId", sanitizedId, DbType.String, factory));

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            historyTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                logMessage = string.Concat("Database error in Get Exported to Alloc History. ", ex.Message);
                throw; // Active so you immediately catch configuration or query failures
            }
            catch (Exception ex)
            {
                logMessage = string.Concat("System error in Get Exported to Alloc History. ", ex.Message);
                throw; // Active so you immediately catch system-level exceptions
            }

            return Task.FromResult((historyTable, logMessage));
        }

        // OVERRIDE
        public Task<(DataTable, string response, string dbResponse)> GetOverride(string idnumber, string size, string storegrade, Int16 input)
        {
            string logMessage = string.Empty;
            string response = "Success";
            var overrideTable = new DataTable();

            try
            {
                using (DbConnection connection = _databaseConnection.CreateConnection())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("The database connection could not be created because CreateConnection() returned null.");
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StoredProcedures.GetOverride;
                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandTimeout = 60;

                        DbProviderFactory factory;
                        string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@UserID", idnumber ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Sizes", size ?? string.Empty, DbType.String, factory));
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreGrades", storegrade ?? string.Empty, DbType.String, factory));

                        // Fixed: Changed parameter name from @Input to @OverrideValue to match the stored procedure definition
                        command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@OverrideValue", input, DbType.Int16, factory));

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            overrideTable.Load(reader);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                logMessage = string.Concat("Database error in Get Override. ", ex.Message);
                response = "SP Error";
                //throw;
            }
            catch (Exception ex)
            {
                logMessage = string.Concat("System error in Get Override. ", ex.Message);
                response = "System Error";
                //throw;
            }

            return Task.FromResult((overrideTable, response, logMessage));
        }
    }
}