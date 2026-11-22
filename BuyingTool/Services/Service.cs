using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Data;
using BuyingTool.Repository;
using BuyingTool.Data;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuyingTool.Services
{
    public class Service
    {
        private readonly IDBContext _dBContext;
        private readonly ServiceHelper serviceHelper = new ServiceHelper();


        #region DBContext Setup
        public Service()
            : this(new DBContext(new DatabaseConnection()))
        {
        }

        public Service(IDBContext dBContext)
        {
            _dBContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
        }
        #endregion

        
        #region User Session
        private string GetCurrentUserId()
         {
            if (System.Web.HttpContext.Current != null &&
                System.Web.HttpContext.Current.Session["idnumber"] != null)
            {
                return System.Web.HttpContext.Current.Session["idnumber"].ToString();
            }
            return null;
        }

        private string GetCurrentUserPass()
        {
            if (System.Web.HttpContext.Current != null &&
                System.Web.HttpContext.Current.Session["idnumber"] != null)
            {
                return System.Web.HttpContext.Current.Session["password"].ToString();
            }
            return null;
        }
        #endregion


        #region Cache Filters

        // Distribution Summary, Per Site, Export Filter Region
        public Task<DataTable> GetFinalDataSetFilter()
        {
            string userid = GetCurrentUserId();

            var dataSet = _dBContext.GetFinalDataSetFilter(userid).GetAwaiter().GetResult();

            return Task.FromResult(dataSet);
        }


        // Attribute Filter Region
        public Task<DataTable> GetFiltersAttributeCache(string userid)
        {
            // Make the cache key unique to the specific user
            string cacheKey = $"FiltersAttribute_{userid}";

            DataTable dtMainTable = HttpRuntime.Cache[cacheKey] as DataTable;

            if (dtMainTable == null)
            {
                dtMainTable = _dBContext.GetFiltersAttribute(userid).GetAwaiter().GetResult();

                HttpRuntime.Cache.Insert(
                    cacheKey,
                    dtMainTable,
                    null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(30)
                );
            }

            return Task.FromResult(dtMainTable);
        }

        // Clear cache Attribute Filter Region
        public void ClearGetFiltersAttributeCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersAttribute_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }


        // Organization Filter Region
        public Task<DataTable> GetFiltersOrganizationCache(string userid)
        {
            // Make the cache key unique to the specific user
            string cacheKey = $"FiltersOrganization_{userid}";

            DataTable dtMainTable = HttpRuntime.Cache[cacheKey] as DataTable;

            if (dtMainTable == null)
            {
                dtMainTable = _dBContext.GetFiltersOrganization(userid).GetAwaiter().GetResult();

                HttpRuntime.Cache.Insert(
                    cacheKey,
                    dtMainTable,
                    null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(30)
                );
            }

            return Task.FromResult(dtMainTable);
        }

        // Clear cache Organization Filter Region
        public void ClearGetFiltersOrganizationCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersOrganization_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }


        // Required Filter Region
        public Task<DataTable> GetFiltersRequiredCache(string userid)
        {
            // Make the cache key unique to the specific user
            string cacheKey = $"FiltersRequired_{userid}";

            DataTable dtMainTable = HttpRuntime.Cache[cacheKey] as DataTable;

            if (dtMainTable == null)
            {
                dtMainTable = _dBContext.GetFiltersRequired(userid).GetAwaiter().GetResult();

                HttpRuntime.Cache.Insert(
                    cacheKey,
                    dtMainTable,
                    null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(30)
                );
            }

            return Task.FromResult(dtMainTable);
        }

        // Clear cache Required Filter Region
        public void ClearGetFiltersRequiredCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersRequired_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }


        // Style Filter Region
        public Task<DataTable> GetFiltersStylesCache(string userid)
        {
            // Make the cache key unique to the specific user
            string cacheKey = $"FiltersStyles_{userid}";

            DataTable dtMainTable = HttpRuntime.Cache[cacheKey] as DataTable;

            if (dtMainTable == null)
            {
                dtMainTable = _dBContext.GetFiltersStyles(userid).GetAwaiter().GetResult();

                HttpRuntime.Cache.Insert(
                    cacheKey,
                    dtMainTable,
                    null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(30)
                );
            }

            return Task.FromResult(dtMainTable);
        }

        // Clear cache Style Filter Region
        public void ClearGetFiltersStylesCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersStyles_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

        #endregion
        

        #region Sort Reference
        
        public Task<DataTable> GetSortReference()
        {
            var dtMainTable = _dBContext.GetSortReference().GetAwaiter().GetResult();
            return Task.FromResult(dtMainTable);
        }

        #endregion


        #region Account Verification

        // Account Verification
        public Task<(bool response, string useraccount)> AccountVerification(string idnumber, string pass)
        {
            var dataTable = new DataTable();

            dataTable = _dBContext.AccountVerification(idnumber, pass).GetAwaiter().GetResult();

            var account = dataTable.AsEnumerable()
                    .Select(row => new
                    {
                        Result = dataTable.Columns.Contains("Result")
                            ? row["Result"]?.ToString()
                            : null,
                        UserProfile = dataTable.Columns.Contains("UserProfile")
                            ? row["UserProfile"]?.ToString()
                            : null
                    })
                    .FirstOrDefault();

            if (account.Result == "200")
            {
                GetFiltersAttributeCache(idnumber).GetAwaiter().GetResult();
                GetFiltersOrganizationCache(idnumber).GetAwaiter().GetResult();
                GetFiltersRequiredCache(idnumber).GetAwaiter().GetResult();
                GetFiltersStylesCache(idnumber).GetAwaiter().GetResult();

                return Task.FromResult((true, account.UserProfile));
            }

            return Task.FromResult((false, string.Empty)); ;
        }

        #endregion


        #region Set Filters Window

        // Plan Period
        public Task<DataTable> GetPeriodPlan()
        {
            var dataSet = _dBContext.GetPeriodPlan().GetAwaiter().GetResult();

            return Task.FromResult(dataSet);
        }

        // Option Gallery 
        public Task<(List<string> tableHeader, List<string[]> tableRows)> GetOptionGalTable(
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
            string tagFilter
        )
        {
            var dataSet = _dBContext.GetOptionGalTable(
                // Required Parameters
                brand,
                division,
                department,
                category,
                firstGRPeriodYear,
                planPeriodYear,

                // Attributes Parameters
                color,
                silhouette,
                collection,
                fitCut,
                materialComposition,
                materialConstruction,
                printTexture,
                productLine,
                sleeveType,
                styleCollection,
                sizeGroup,
                styleCode,
                genericMaterial,
                subCategory,
                origSrp,

                // Site Parameters
                siteCode,
                region,
                country,
                channel,
                territory,
                area,
                outletType,
                siteName,
                storeGrade,

                // New Parameters
                tagFilter,
                GetCurrentUserId()
            ).GetAwaiter().GetResult();

            return serviceHelper.PopulateDatatableHelper(dataSet);
        }

        // Calculation
        public Task<DataTable> GetCalculation(string brand, string division, string department, string category, string firstGRPeriodYear, string periodPlan, decimal wos, decimal plc, decimal factorRate)
        {
            var calTable = _dBContext.Calculate(brand, division, department, category, firstGRPeriodYear, periodPlan, GetCurrentUserId(), wos, plc, factorRate).GetAwaiter().GetResult();

            return Task.FromResult(calTable);
        }

        #endregion


        #region Summary Distribution

        // Summary Distribution
        public Task<(DataTable[], string dbResponse)> GetDistributionSummary(string size, string storegrade)
        {
            var userId = GetCurrentUserId();

            var (dataSet, response) = _dBContext.GetDistributionSummryTable(userId, storegrade, size).GetAwaiter().GetResult();

            return Task.FromResult((dataSet, response));
        }

        // Override
        public Task<(List<string> tableHeader, List<string[]> tableRows, string response, string dbResponse)> GetOverrideTable(string size, string storegrade, Int16 input)
        {
            var userId = GetCurrentUserId();

            var (dataSet, response, dbResponse) = _dBContext.GetOverride(userId, size, storegrade, input).GetAwaiter().GetResult();

            var (thead, trows) = serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((thead, trows, response, dbResponse));
        }

        #endregion


        #region Store Breakdown or Per Site

        // Store Breakdown or Per Site
        public Task<(List<string> tableHeader, List<string[]> tableRows, string dbResponse)> GetPerSiteTable(string storegrade, string size, string type)
        {
            var userId = GetCurrentUserId();

            var (dataSet, response) = _dBContext.GetPerSiteTable(userId, storegrade, size, type).GetAwaiter().GetResult();

            var (tHeader, tRows) = serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((tHeader, tRows, response));
        }

        #endregion


        #region Export

        // Export
        public Task<(List<string> tableHeader, List<string[]> tableRows, string dbResponse)> GetExportTable(string storegrade, string size, string type)
        {
            var userId = GetCurrentUserId();

            var (dataSet, response) = _dBContext.GetExportTable(userId, storegrade, size, type).GetAwaiter().GetResult();

            var (tHeader, tRows) = serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((tHeader, tRows, response));
        }

        #endregion


        #region Size Curve

        // Size Curve
        public Task<(List<string> tableHeader, List<string[]> tableRows)> GetSizeCurveTable(string brand, string division, string department, string category, string planperiodyear)
        {
            var dataSet = _dBContext.GetSizeCurveTable(GetCurrentUserId(), brand, division, department, category, planperiodyear).GetAwaiter().GetResult();

            var (theader, trows) =  serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((theader, trows));
        }

        #endregion


        #region Alloc Sevice

        // To Alloc
        public Task<(string response, string dbResponse)> ExportToAlloc(List<string> theader, List<string[]> trows, string brand, string collection, int fiscalMonth, int fiscalYear, string matno, string color)
        {
            var response = "Success";
            var dbResponse = string.Empty;

            string baseBatchRandomId = new Random().Next(10000000, 100000000).ToString();
            var batchNo = string.Empty;

            StringBuilder errorMessages = new StringBuilder();

            const int batchSize = 1000;

            string userId = GetCurrentUserId();

            int totalRows = 0;
            int totalBatches = 0;
            int successfulBatches = 0;
            int totalDataProcessed = 0;

            DataTable dataTable = serviceHelper.ConvertListToDataTable(theader, trows).GetAwaiter().GetResult();

            if (dataTable == null)
            {
                response = "Failed";
                return Task.FromResult((response, dbResponse));
            }

            totalRows = dataTable.Rows.Count;

            List<DataTable> batches = serviceHelper.SplitDataTable(dataTable, batchSize).GetAwaiter().GetResult();

            totalBatches = batches.Count;

            for (int i = 0; i < batches.Count; i++)
            {
                DataTable batch = batches[i];

                batchNo = baseBatchRandomId + "_" + (i + 1);

                XDocument xmlDoc = serviceHelper.DataTabletoXML(batch).GetAwaiter().GetResult();

                (response, dbResponse) = _dBContext.ExportToAlloc(xmlDoc, brand, collection, fiscalMonth, fiscalYear, matno, color, userId).GetAwaiter().GetResult();

                if (response != "Success")
                {
                    errorMessages.AppendLine("Batch " + (i + 1) + " of " + totalBatches + " failed: " + dbResponse);

                    _dBContext.WriteExportTollocLog(xmlDoc, brand, collection, fiscalMonth, fiscalYear, matno, color, userId, batchNo, totalRows, totalBatches, totalDataProcessed, batchSize, response, dbResponse, errorMessages.ToString().Trim());

                    break;
                }

                successfulBatches++;
                totalDataProcessed += batch.Rows.Count;

                _dBContext.WriteExportTollocLog(xmlDoc, brand, collection, fiscalMonth, fiscalYear, matno, color, userId, batchNo, totalRows, totalBatches, totalDataProcessed, batchSize, response, dbResponse, errorMessages.ToString().Trim());
            }

            if (successfulBatches != totalBatches)
            {
                response = "Failed";
            }

            if (response == "Failed")
            {
                dbResponse = errorMessages.ToString().Trim();
            }
            else
            {
                dbResponse = "Successfully processed " +
                    totalRows + " records in " +
                    totalBatches + " batches.";
            }

            return Task.FromResult((response, dbResponse));
        }

        // History Monitoring for Exported Data to Alloc
        public Task<(List<string> tableHeader, List<string[]> tableRows, string dbResponse)> GetExortedToAllocToolHistory()
        {
            var userId = GetCurrentUserId();

            var (dataSet, dbResponse) = _dBContext.GetExortedToAllocToolHistory(userId).GetAwaiter().GetResult();

            var (thead, trows) = serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((thead, trows, dbResponse));
        }

        #endregion


        #region Download or Export as Excel Service

        // Export to Excel
        public Task ExportToExcel(List<string> tHeader, List<string[]> tRows, string customFileName, HttpResponse response)
        {
            return serviceHelper.ExportToExcel(tHeader, tRows, customFileName, response);
        }

        #endregion


        #region Other Service
        
        // Convert DataTable to Table Header and Table Rows
        public Task<(List<string> tableHeader, List<string[]> tableRows)> DatatableConvertToHT(DataTable dataTable)
        {
            return serviceHelper.PopulateDatatableHelper(dataTable);
        }

        // Hide Table Column and Row
        public Task<(List<string> tableHeader, List<string[]> tableRows)> HideTableColumnAndRow(List<string> theader, List<string[]> trows, string hiddenColumns)
        {
            var result = serviceHelper.TableColumnAndRowHidder(theader, trows, hiddenColumns).GetAwaiter().GetResult();

            return Task.FromResult(result);
        }

        // Split Filename
        public Task<(string matno, string color)> SplitFilename(string filename)
        {
            return Task.FromResult(serviceHelper.SliceFileName(filename).GetAwaiter().GetResult());
        }

        // Transpose Table from header to row and row to header
        public Task<(List<string> tableHeader, List<string[]> tableRows)> TransposeTable(List<string> oldHeaders, List<string[]> oldRows)
        {
            return serviceHelper.TransposeTable(oldHeaders, oldRows);
        }

        #endregion

    }
}
