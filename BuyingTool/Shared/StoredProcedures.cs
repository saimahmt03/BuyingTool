using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingTool.Shared
{
    public class StoredProcedures
    {
        // Verify user, User Id session
        public const string VerifyUserAccount = "[dbo].[usp_ValidateAccess]";

        // Calculation
        public const string GetCalculation = "[dbo].[usp_Get_Final_DataSet]";

        // Filter Windows
        public const string GetMainTable = "[dbo].[usp_GetUserDataSet_test]";

        // Perido Plan
        public const string GetPeriodPlan = "[dbo].[usp_GetPlanPeriods]";

        // Option Gallery, Distribution Summary, Per Site, Export
        public const string GetOptionGallery = "[dbo].[usp_Get_Option_Gallery]";
        public const string GetSummaryDistribution = "[dbo].[usp_GetStoreOrderBufferDashboard]";
        public const string GetStoreSizeBreakdown = "[dbo].[usp_GetStoreSizeBreakdown]";
        public const string GetExport = "[dbo].[StoreOrderBufferExport]";
        public const string GetSizeCure = "[dbo].[usp_Get_Size_Curve]";

        // Add to Alloc
        public const string AddToAlloc = "[AFR].[IA].[UPSERT_BuyAlloc]";

        // Monitoring of Exported to Alloc
        public const string GetExportedToAllocToolHistory = "[AFR].[IA].[usp_GeExportedToAllocHistoryByCreatedBy]";

        // Store Grade and Size Filters for Distribution Summary, Per Site, and Export
        public const string GetStoreGradeAndSizeFilter = "[dbo].[usp_GetStoreGradeAndSizeByUser]";

        // Override
        public const string GetOverride = "[dbo].[usp_OverrideDrops]";

        // New Filters Pulling
        public const string GetFiltersAttribute = "[dbo].[usp_GetFilters_Attributes]";
        public const string GetFiltersOrganization = "[dbo].[usp_GetFilters_Organization]";
        public const string GetFiltersRequired = "[dbo].[usp_GetFilters_Required]";
        public const string GetFiltersStyles = "[dbo].[usp_GetFilters_Styles]";

        // New Account Login
        public const string AccoutVerification = "[dbo].[usp_Login]";
    }
}