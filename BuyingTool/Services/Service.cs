using System;
using System.Linq;
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

            //filterList = _dBContext.GetMainTable(GetCurrentUserId(), GetCurrentUserPass()).GetAwaiter().GetResult();
        }
        #endregion

        #region User Info Session
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

        #region Cache
        public Task<DataTable> GetFiltersWindow(string userid, string pass)
        {
            // Make the cache key unique to the specific user
            string cacheKey = $"FiltersWindow_{userid}";

            DataTable dtMainTable = HttpRuntime.Cache[cacheKey] as DataTable;

            if (dtMainTable == null)
            {
                dtMainTable = _dBContext.GetMainTable(userid, pass).GetAwaiter().GetResult();

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

        // Clear cache only for this specific user
        public void ClearFiltersWindowCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersWindow_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

        public Task<DataTable> GetFinalDataSetFilterCache()
        {
            var userId = GetCurrentUserId();

            string cacheKey = $"FinalDataSetFilter_{userId}";

            DataTable dataSet = HttpRuntime.Cache[cacheKey] as DataTable;

            if (dataSet == null)
            {
                dataSet = _dBContext.GetFinalDataSetFilter(userId).GetAwaiter().GetResult();

                HttpRuntime.Cache.Insert(
                    cacheKey,
                    dataSet,
                    null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(30)
                );
            }

            return Task.FromResult(dataSet);
        }

        public void ClearFinalDataSetFilterCache(string userId)
        {
            string cacheKey = $"FinalDataSetFilter_{userId}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

        // NEW Filter Pulling
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

        // Clear cache only for this specific user
        public void ClearGetFiltersAttributeCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersAttribute_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

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

        // Clear cache only for this specific user
        public void ClearGetFiltersOrganizationCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersOrganization_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

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

        // Clear cache only for this specific user
        public void ClearGetFiltersRequiredCache(string userid)
        {
            // Construct the exact same user-specific key
            string cacheKey = $"FiltersRequired_{userid}";

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

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

        // Clear cache only for this specific user
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

        #region DBContext In Use

        public bool CheckMainTableForLogin(string userid, string pass)
        {
            string response = _dBContext.VerifyAccount(userid, pass);

            if (response == "Access Confirmed")
            {
                GetFiltersWindow(userid, pass).GetAwaiter().GetResult();
                return true;
            }

            return false;
        }

        // Account Verification
        public bool AccountVerification(string idnumber, string pass)
        {
            var (response, dbResponse) = _dBContext.AccountVerification(idnumber, pass).GetAwaiter().GetResult();

            if(response == "")
            {
                GetFiltersAttributeCache(idnumber).GetAwaiter().GetResult();
                GetFiltersOrganizationCache(idnumber).GetAwaiter().GetResult();
                GetFiltersRequiredCache(idnumber).GetAwaiter().GetResult();
                GetFiltersStylesCache(idnumber).GetAwaiter().GetResult();

                return true;
            }

            return false;
        }

        // Plan Period
        public Task<DataTable> GetPeriodPlan()
        {
            var dataSet = _dBContext.GetPeriodPlan().GetAwaiter().GetResult();

            return Task.FromResult(dataSet);
        }

        // Calculation
        public Task<DataTable> GetCalculation(string brand, string division, string department, string category, string firstGRPeriodYear, string periodPlan, decimal wos, decimal plc, decimal factorRate)
        {
            var calTable = _dBContext.Calculate(brand, division, department, category, firstGRPeriodYear, periodPlan, GetCurrentUserId(), wos, plc, factorRate).GetAwaiter().GetResult();

            return Task.FromResult(calTable);
        }

        #region Table Population
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

        // Distribution Summary
        public Task<(DataTable[], string dbResponse)> GetDistributionSummary(string size, string storegrade)
        {
            var userId = GetCurrentUserId();

            var (dataSet, response) = _dBContext.GetDistributionSummryTable(userId, storegrade, size).GetAwaiter().GetResult();

            return Task.FromResult((dataSet, response));
        }

        // Per Site
        public Task<(List<string> tableHeader, List<string[]> tableRows, string dbResponse)> GetPerSiteTable(string storegrade, string size, string type)
        {
            var userId = GetCurrentUserId();

            var (dataSet, response) = _dBContext.GetPerSiteTable(userId, storegrade, size, type).GetAwaiter().GetResult();

            var (tHeader, tRows) = serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((tHeader, tRows, response));
        }

        // Export
        public Task<(List<string> tableHeader, List<string[]> tableRows, string dbResponse)> GetExportTable(string storegrade, string size, string type)
        {
            var userId = GetCurrentUserId();

            var (dataSet, response) = _dBContext.GetExportTable(userId, storegrade, size, type).GetAwaiter().GetResult();

            var (tHeader, tRows) = serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((tHeader, tRows, response));
        }

        // Size Curve
        public Task<(List<string> tableHeader, List<string[]> tableRows, string dbResponse)> GetSizeCurveTable(string brand, string division, string department, string category, string planperiodyear)
        {
            var (dataSet, dbResponse) = _dBContext.GetSizeCurveTable(GetCurrentUserId(), brand, division, department, category, planperiodyear).GetAwaiter().GetResult();

            var (theader, trows) =  serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((theader, trows, dbResponse));
        }
        
        // Monitoring for Exported to Alloc
        public Task<(List<string> tableHeader, List<string[]> tableRows, string dbResponse)> GetExortedToAllocToolHistory()
        {
            var trows = new List<string[]>();
            var thead = new List<string>();
            var dataSet = new DataTable();
            string dbResponse = string.Empty;

            var userId = GetCurrentUserId();

            (dataSet, dbResponse) = _dBContext.GetExortedToAllocToolHistory(userId).GetAwaiter().GetResult();

            (thead, trows) = serviceHelper.PopulateDatatableHelper(dataSet).GetAwaiter().GetResult();

            return Task.FromResult((thead, trows, dbResponse));
        }

        #endregion


        #region Export Service
        // To Alloc
        public Task<(string response, string dbResponse)> ExportToAlloc(List<string> theader, List<string[]> trows, string brand, string collection, int fiscalMonth, int fiscalYear, string matno, string color)
        {
            DataTable dataTable = serviceHelper.ConvertListToDataTable(theader, trows).GetAwaiter().GetResult();

            XDocument xmlDoc = serviceHelper.DataTabletoXML(dataTable).GetAwaiter().GetResult();

            var (response, dbResponse )= _dBContext.ExportToAlloc(xmlDoc, brand, collection, fiscalMonth, fiscalYear, matno, color, GetCurrentUserId()).GetAwaiter().GetResult();

            return Task.FromResult((response, dbResponse));
        }

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
        #endregion

        #endregion
    }
}