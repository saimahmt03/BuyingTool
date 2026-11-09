using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingTool.Shared
{
    public class StoredProcedures
    {
        #region ACCOUNT VERIFICATION
        
        // Account Virefication
        public const string AccoutVerification = "[dbo].[usp_Login]";

        #endregion

        #region FILTERS

        // Attribute Region Filter
        public const string GetFiltersAttribute = "[dbo].[usp_GetFilters_Attributes]";

        // Organization Region Filter
        public const string GetFiltersOrganization = "[dbo].[usp_GetFilters_Organization]";

        // Required Region Filter
        public const string GetFiltersRequired = "[dbo].[usp_GetFilters_Required]";

        // Style Region Filter
        public const string GetFiltersStyles = "[dbo].[usp_GetFilters_Styles]";

        // Distribution Summary, Site Breakdow or Per Site, and Export Store Grade and Size Filter
        public const string GetStoreGradeAndSizeFilter = "[dbo].[usp_GetStoreGradeAndSizeByUser]";

        #endregion

        #region SET FILTERS WINDOW

        // Perido Plan
        public const string GetPeriodPlan = "[dbo].[usp_GetPlanPeriods]";

        // Option Gallery, Distribution Summary, Per Site, Export
        public const string GetOptionGallery = "[dbo].[usp_Get_Option_Gallery]";

        // Calculation
        public const string GetCalculation = "[dbo].[usp_Get_Final_DataSet]";

        #endregion

        #region DISTRIBUTION SUMMARY

        // Distribution Summary
        public const string GetSummaryDistribution = "[dbo].[usp_GetStoreOrderBufferDashboard]";

        // Override
        public const string GetOverride = "[dbo].[usp_OverrideDrops]";

        #endregion

        #region STORE BREAKDOWN OR PER SITE       

        // Store Breakedown or Per Site
        public const string GetStoreSizeBreakdown = "[dbo].[usp_GetStoreSizeBreakdown]";

        #endregion

        #region EXPORT

        // Export
        public const string GetExport = "[dbo].[StoreOrderBufferExport]";

        #endregion

        #region SIZE CURVE

        // Size Curve
        public const string GetSizeCure = "[dbo].[usp_Get_Size_Curve]";

        #endregion

        #region ALLOC

        // Add to Alloc
        public const string AddToAlloc = "[AFR].[IA].[UPSERT_BuyAlloc]";

        // Monitoring of Exported to Alloc
        public const string GetExportedToAllocToolHistory = "[AFR].[IA].[usp_GeExportedToAllocHistoryByCreatedBy]";

        #endregion

        #region LOG

        public const string GetUserLog = "[dbo].[sp_InsertBuyingToolLog]";

        #endregion
    }
}