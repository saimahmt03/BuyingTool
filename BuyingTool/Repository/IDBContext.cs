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
    public interface IDBContext
    {
        string VerifyAccount(string userid, string pass);

        Task<DataTable> GetMainTable(string userid, string pass);

        Task<DataTable> Calculate(string brand, string division, string department, string category, string firstGRPeriodYear, string periodPlan, string idnumber, decimal wos, decimal plc, decimal factorRate);

        Task<DataTable> GetPeriodPlan();

        Task<DataTable> GetOptionGalTable(
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
            );

        Task<(DataTable[], string dbResponse)> GetDistributionSummryTable(string idnumber, string size, string storegrade);

        Task<(DataTable, string dbResponse)> GetPerSiteTable(string idnumber, string storegrade, string size, string type);

        Task<(DataTable, string dbResponse)> GetExportTable(string idnumber, string storegrade, string size, string type);

        Task<(DataTable, string dbResponse)> GetSizeCurveTable(string idnumber, string brand, string division, string department, string category, string planperiodyear);

        Task<(string response, string dbResponse)> ExportToAlloc(XDocument xmlDoc, string brand, string collection, int fiscalMonth, int fiscalYear, string matno, string color, string idnumber);

        Task<DataTable> GetFinalDataSetFilter(string idnumber);

        Task<(DataTable, string dbResponse)> GetExortedToAllocToolHistory(string idnumber);

        Task<(DataTable, string response, string dbResponse)> GetOverride(string idnumber, string size, string storegrade, Int16 input);


        #region New Filter Pulling - Normalize Table
        Task<DataTable> GetFiltersAttribute(string idnumber);
        Task<DataTable> GetFiltersOrganization(string idnumber);
        Task<DataTable> GetFiltersRequired(string idnumber);
        Task<DataTable> GetFiltersStyles(string idnumber);

        // NEW Login
        Task<(string response, string dbResponse)> AccountVerification(string idnumber, string pass);
        #endregion


    }
}