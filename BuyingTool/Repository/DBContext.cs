using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;

using BuyingTool.Data;
using BuyingTool.Shared;
using BuyingTool.Logs;


namespace BuyingTool.Repository
{
    public class DBContext : IDBContext
    {
        private readonly IDatabaseConnection _databaseConnection;

        public DBContext(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        #region ACCOUNT VERIFICATION

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

                        // Execute the command and capture the single string response
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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Login, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Login, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var AccountVerification = new Log.AccountVerification();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = AccountVerification.Page;
                BaseModel.Section = AccountVerification.Section;
                BaseModel.Activity = AccountVerification.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = AccountVerification.Function;
                BaseModel.StoreProcedure = AccountVerification.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Login: ",
                        ex);
                }
            }

            return Task.FromResult((response, logMessage));
        }

        #endregion

        #region FILTERS

        // ATTRIBUTE REGION FILTER
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
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Attribute, ex);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Attribute, ex);
            }

            return Task.FromResult(dataTable);
        }

        // ORGANIZATION REGION FILTER
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
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Organization, ex);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Organization, ex);
            }

            return Task.FromResult(dataTable);
        }

        // REQUIRED REGION FILTER
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
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Required, ex);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Required, ex);
            }

            return Task.FromResult(dataTable);
        }

        // STYLE REGION FILTER
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
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Style, ex);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Style, ex);
            }

            return Task.FromResult(dataTable);
        }

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
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.StoregradeAndSize, ex);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.StoregradeAndSize, ex);
            }

            return Task.FromResult(dataTable);
        }

        #endregion

        #region SORT REFERENCE
        
         public Task<DataTable> GetSortReference()
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
                        command.CommandText = "select * from [INTERIM_BUYING_TOOL].[dbo].[SortReference]";
                        command.CommandType = CommandType.Text;

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
        
        #endregion

        #region SET FILTERS WINDOW

        // PERIOD PLAN
        public Task<DataTable> GetPeriodPlan()
        {
            var dataTable = new DataTable();
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
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.PeriodPlan, ex);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.PeriodPlan, ex);
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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.OptionGallery, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.OptionGallery, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var SetFiltersWindowOptGal = new Log.SetFiltersWindowOptionGallery();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = userId,
                    Brand = brand,
                    Division = division,
                    Department = department,
                    Category = category,
                    SubCategory = subCategory,
                    ProductLines = productLine,
                    FirstGRPeriodYear = firstGRPeriodYear,
                    PlanPeriodYear = planPeriodYear,
                    Color = color,
                    Silhouette = silhouette,
                    Collection = collection,
                    StyleCollections = styleCollection,
                    FitCut = fitCut,
                    SleeveType = sleeveType,
                    PrintTexture = printTexture,
                    MaterialComposition = materialComposition,
                    MaterialConstruction = materialConstruction,
                    GenericMaterial = genericMaterial,
                    SizeGroup = sizeGroup,
                    StyleCode = styleCode,
                    TagFilter = tagFilter,
                    OrigSrp = origSrp,
                    SiteCode = siteCode,
                    SiteName = siteName,
                    StoreGrade = storeGrade,
                    Region = region,
                    Country = country,
                    Territory = territory,
                    Area = area,
                    Channel = channel,
                    OutletType = outletType
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = SetFiltersWindowOptGal.Page;
                BaseModel.Section = SetFiltersWindowOptGal.Section;
                BaseModel.Activity = SetFiltersWindowOptGal.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = SetFiltersWindowOptGal.Function;
                BaseModel.StoreProcedure = SetFiltersWindowOptGal.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = userId;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Filters Wndow Display or Option Gallery: ",
                        ex);
                }
            }

            return Task.FromResult(optionGalTable);
        }


        // CALCULATION
        public Task<DataTable> Calculate(string brand, string division, string department, string category, string firstGRPeriodYear, string periodPlan, string idnumber, decimal wos, decimal plc, decimal factorRate)
        {
            string logMessage = string.Empty;
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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Calculation, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Calculation, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var SetFiltersWindowCalcu = new Log.SetFiltersWindowCalculate();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber,
                    Brand = brand,
                    Division = division,
                    Department = department,
                    Category = category,
                    FirstGRPeriodYear = firstGRPeriodYear,
                    PlanPeriodYear = periodPlan,
                    Wos = wos,
                    Plc = plc,
                    FactorRate = factorRate
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = SetFiltersWindowCalcu.Page;
                BaseModel.Section = SetFiltersWindowCalcu.Section;
                BaseModel.Activity = SetFiltersWindowCalcu.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = SetFiltersWindowCalcu.Function;
                BaseModel.StoreProcedure = SetFiltersWindowCalcu.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Filters Window Calculate: ",
                        ex);
                }
            }

            return Task.FromResult(calculateResult);
        }
        
        #endregion

        #region DISTRIBUTION SUMMARY

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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.SummaryDistribution, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.SummaryDistribution, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var DistributationSummaryTable = new Log.DistributationSummaryTable();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber,
                    StoreGrade = storegrade,
                    Size = size
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = DistributationSummaryTable.Page;
                BaseModel.Section = DistributationSummaryTable.Section;
                BaseModel.Activity = DistributationSummaryTable.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = DistributationSummaryTable.Function;
                BaseModel.StoreProcedure = DistributationSummaryTable.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Summary Distribution: ",
                        ex);
                }
            }

            return Task.FromResult((distriSumTables.ToArray(), logMessage));
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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Override, ex);
                response = "SP Error";
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Override, ex);
                response = "System Error";
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var DistributationSummaryOverride = new Log.DistributationSummaryOverride();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber,
                    StoreGrade = storegrade,
                    Size = size,
                    Input = input
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = DistributationSummaryOverride.Page;
                BaseModel.Section = DistributationSummaryOverride.Section;
                BaseModel.Activity = DistributationSummaryOverride.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = DistributationSummaryOverride.Function;
                BaseModel.StoreProcedure = DistributationSummaryOverride.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Summary Distribution Override: ",
                        ex);
                }
            }

            return Task.FromResult((overrideTable, response, logMessage));
        }

        #endregion

        #region STORE BREAKDOWN OR PER SITE        

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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.StoreBreakdown, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.StoreBreakdown, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var StoreBreakdownTable = new Log.StoreBreakdownTable();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber,
                    StoreGrade = storegrade,
                    Size = size,
                    Type = type
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = StoreBreakdownTable.Page;
                BaseModel.Section = StoreBreakdownTable.Section;
                BaseModel.Activity = StoreBreakdownTable.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = StoreBreakdownTable.Function;
                BaseModel.StoreProcedure = StoreBreakdownTable.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Store Breakdown or Per Site: ",
                        ex);
                }
            }

            return Task.FromResult((perSiteTable, logMessage));
        }

        #endregion

        #region EXPORT

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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.Export, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.Export, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var ExportTable = new Log.ExportTable();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserId = idnumber,
                    StoreGrade = storegrade,
                    Size = size,
                    Type = type
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = ExportTable.Page;
                BaseModel.Section = ExportTable.Section;
                BaseModel.Activity = ExportTable.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = ExportTable.Function;
                BaseModel.StoreProcedure = ExportTable.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Export: ",
                        ex);
                }
            }

            return Task.FromResult((exportTable, logMessage));
        }

        #endregion

        #region SIZE CURVE

        // SIZE CURVE
        public Task<DataTable> GetSizeCurveTable(string idnumber, string brand, string division, string department, string category, string planperiodyear)
        {
            var optionGalTable = new DataTable();
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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.SizeCurve, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.SizeCurve, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var SizeCurve = new Log.SizeCurve();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber,
                    Brand = brand,
                    Division = division,
                    Department = department,
                    Category = category,
                    PeriodPlan = planperiodyear
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = SizeCurve.Page;
                BaseModel.Section = SizeCurve.Section;
                BaseModel.Activity = SizeCurve.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = SizeCurve.Function;
                BaseModel.StoreProcedure = SizeCurve.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Size Curve: ",
                        ex);
                }
            }

            return Task.FromResult(optionGalTable);
        }

        #endregion

        #region ALLOC
        
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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.ExportToAlloc, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.ExportToAlloc, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var ExportToAlloc = new Log.ExportToAlloc();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber,
                    XMLDoc = xmlDoc,
                    Brand = brand,
                    Collection = collection,
                    FiscalMonth = fiscalMonth,
                    FiscalYear = fiscalYear,
                    Matno = matno,
                    Color = color
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = ExportToAlloc.Page;
                BaseModel.Section = ExportToAlloc.Section;
                BaseModel.Activity = ExportToAlloc.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = ExportToAlloc.Function;
                BaseModel.StoreProcedure = ExportToAlloc.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Exorpt To Alloc: ",
                        ex);
                }
            }

            return Task.FromResult((response, logMessage));
        }

        // MONITORING OF EXPORTED TO ALLOC TOOL
        public Task<(DataTable, string dbResponse)> GetExortedToAllocToolHistory(string idnumber)
        {
            string logMessage = string.Empty;
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
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Db + ErrorMessage.ExportToAllocMonitoring, ex);
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
                LogHelper.LogError(ErrorMessage.Sys + ErrorMessage.ExportToAllocMonitoring, ex);
            }
            finally
            {
                var BaseModel = new Log.BaseLogModel();
                var ExportToAllocHistory = new Log.ExportToAllocHistory();

                string ActivityStatus = string.Empty;

                var param = new
                {
                    UserID = idnumber
                };

                string jsonResult = JsonConvert.SerializeObject(param);

                if (string.IsNullOrEmpty(logMessage))
                {
                    ActivityStatus = "Success";
                    BaseModel.SystemResponse = string.Empty;
                }
                else
                {
                    ActivityStatus = "Failed";
                    BaseModel.SystemResponse = logMessage;
                }

                BaseModel.Page = ExportToAllocHistory.Page;
                BaseModel.Section = ExportToAllocHistory.Section;
                BaseModel.Activity = ExportToAllocHistory.Button;
                BaseModel.Parameter = jsonResult;
                BaseModel.Function = ExportToAllocHistory.Function;
                BaseModel.StoreProcedure = ExportToAllocHistory.StoreProcedure;
                BaseModel.ActivityStatus = ActivityStatus;
                BaseModel.User = idnumber;

                try
                {
                    using (DbConnection logConnection = _databaseConnection.CreateConnection())
                    {
                        if (logConnection == null)
                        {
                            throw new InvalidOperationException(
                                "The database connection could not be created.");
                        }

                        if (logConnection.State != ConnectionState.Open)
                        {
                            logConnection.Open();
                        }

                        using (DbCommand logCommand = logConnection.CreateCommand())
                        {
                            logCommand.CommandText = StoredProcedures.GetUserLog;
                            logCommand.CommandType = CommandType.StoredProcedure;

                            DbProviderFactory logFactory;
                            string logError = _databaseConnection.TryGetDbProviderFactory(out logFactory);

                            if (!string.IsNullOrEmpty(logError))
                            {
                                throw new InvalidOperationException(logError);
                            }

                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Page", BaseModel.Page, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Section", BaseModel.Section, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Activity", BaseModel.Activity, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Parameter", BaseModel.Parameter, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Function", BaseModel.Function, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@StoreProcedure", BaseModel.StoreProcedure, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@ActivityStatus", BaseModel.ActivityStatus, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@SystemResponse", BaseModel.SystemResponse, DbType.String, logFactory));
                            logCommand.Parameters.Add(DatabaseParameterHelper.CreateParameter("@User", BaseModel.User, DbType.String, logFactory));

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(
                        "[BuyingTool Log Error] Failed to write audit/activity log to database in Exorpt To Alloc Monitoring: ",
                        ex);
                }
            }

            return Task.FromResult((historyTable, logMessage));
        }
        
        #endregion
    }
}
