using System;
using BuyingTool.Shared;

namespace BuyingTool.Shared
{
    public class Log
    {

        public class BaseLogModel
        {
            public string Page { get; set; } = string.Empty;
            public string Section { get; set; } = string.Empty;
            public string Activity { get; set; } = string.Empty;
            public string Parameter { get; set; } = string.Empty;
            public string Function { get; set; } = string.Empty;
            public string StoreProcedure { get; set; } = string.Empty;
            public string ActivityStatus { get; set; } = string.Empty;
            public string SystemResponse { get; set; } = string.Empty;
            public string User { get; set; } = string.Empty;
            public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        }


        public class AccountVerification
        {
            public string UserID { get; set; } = string.Empty;


            public string Page { get; set; } = "Index.cshtml";
            public string Section { get; set; } = "Login";
            public string Button { get; set; } = "Login Button";
            public string Function { get; set; } = "AccountVerification() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.AccoutVerification;
        }


        public class SetFiltersWindowOptionGallery
        {
            public string UserID { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public string Division { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string SubCategory { get; set; } = string.Empty;
            public string ProductLines { get; set; } = string.Empty;
            public string FirstGRPeriodYear { get; set; } = string.Empty;
            public string PlanPeriodYear { get; set; } = string.Empty;
            public string Color { get; set; } = string.Empty;
            public string Silhouette { get; set; } = string.Empty;
            public string Collection { get; set; } = string.Empty;
            public string StyleCollections { get; set; } = string.Empty;
            public string FitCut { get; set; } = string.Empty;
            public string SleeveType { get; set; } = string.Empty;
            public string PrintTexture { get; set; } = string.Empty;
            public string MaterialComposition { get; set; } = string.Empty;
            public string MaterialConstruction { get; set; } = string.Empty;
            public string GenericMaterial { get; set; } = string.Empty;
            public string SizeGroup { get; set; } = string.Empty;
            public string StyleCode { get; set; } = string.Empty;
            public string TagFilter { get; set; } = string.Empty;
            public string OrigSrp { get; set; } = string.Empty;
            public string SiteCode { get; set; } = string.Empty;
            public string SiteName { get; set; } = string.Empty;
            public string StoreGrade { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public string Territory { get; set; } = string.Empty;
            public string Area { get; set; } = string.Empty;
            public string Channel { get; set; } = string.Empty;
            public string OutletType { get; set; } = string.Empty;


            public string Page { get; set; } = "SetFiltersWindow.cshtml";
            public string Section { get; set; } = "Filters Window Option Gallery";
            public string Button { get; set; } = "Display Button";
            public string Function { get; set; } = "GetOptionGalTable() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetOptionGallery;
        }

        public class SetFiltersWindowCalculate
        {
            public string UserID { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public string Division { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string FirstGRPeriodYear { get; set; } = string.Empty;
            public string PlanPeriodYear { get; set; } = string.Empty;
            public decimal Wos { get; set; } = 0;
            public decimal Plc { get; set; } = 0;
            public decimal FactorRate { get; set; } = 0;


            public string Page { get; set; } = "SetFiltersWindow.cshtml";
            public string Section { get; set; } = "Filters Window CalculatE";
            public string Button { get; set; } = "Calculate Button";
            public string Function { get; set; } = "Calculate() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetCalculation;
        }

        
        public class DistributationSummaryTable
        {
            public string UserID { get; set; } = string.Empty;
            public string StoreGrade { get; set; } = string.Empty;
            public string Size { get; set; } = string.Empty;


            public string Page { get; set; } = "ReviewSummary.cshtml";
            public string Section { get; set; } = "Summary Distributation Tables";
            public string Button { get; set; } = string.Empty;
            public string Function { get; set; } = "GetDistributionSummryTable() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetSummaryDistribution;
        }

        public class DistributationSummaryOverride
        {
            public string UserID { get; set; } = string.Empty;
            public string StoreGrade { get; set; } = string.Empty;
            public string Size { get; set; } = string.Empty;
            public decimal Input { get; set; } = 0;


            public string Page { get; set; } = "ReviewSummary.cshtml";
            public string Section { get; set; } = "Summary Distributation - Override";
            public string Button { get; set; } = "Go Button";
            public string Function { get; set; } = "GetOverride() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetOverride;
        }


        public class StoreBreakdownTable
        {
            public string UserID { get; set; } = string.Empty;
            public string StoreGrade { get; set; } = string.Empty;
            public string Size { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;


            public string Page { get; set; } = "ReviewSiteDetails.cshtml";
            public string Section { get; set; } = "Store Breakdown or Per Site Table";
            public string Button { get; set; } = string.Empty;
            public string Function { get; set; } = "GetPerSiteTable() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetStoreSizeBreakdown;
        }


        public class ExportTable
        {
            public string UserID { get; set; } = string.Empty;
            public string StoreGrade { get; set; } = string.Empty;
            public string Size { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;


            public string Page { get; set; } = "Export.cshtml";
            public string Section { get; set; } = "Export Table";
            public string Button { get; set; } = string.Empty;
            public string Function { get; set; } = "GetExportTable() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetExport;
        }
    

        public class SizeCurve
        {
            public string UserID { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public string Division { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string PlanPeriod { get; set; } = string.Empty;


            public string Page { get; set; } = "SizeCurve.cshtml";
            public string Section { get; set; } = "Size Curve Table";
            public string Button { get; set; } = string.Empty;
            public string Function { get; set; } = "GetSizeCurveTable() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetSizeCure;
        }

    
        public class ExportToAlloc
        {
            public string UserID { get; set; } = string.Empty;
            public string XMLDoc { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public string Collection { get; set; } = string.Empty;
            public int FiscalMonth { get; set; } = 0;
            public int FiscalYear { get; set; } = 0;
            public string Matno { get; set; } = string.Empty;
            public string Color { get; set; } = string.Empty;


            public string Page { get; set; } = "Export.cshtml";
            public string Section { get; set; } = "Export To Alloc Tool";
            public string Button { get; set; } = "Export To Alloc Tool";
            public string Function { get; set; } = "ExportToAlloc() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.AddToAlloc;
        }

        public class ExportToAllocHistory
        {
            public string UserID { get; set; } = string.Empty;


            public string Page { get; set; } = "Monitoring.cshtml";
            public string Section { get; set; } = "Export To Alloc History Monitoring Table";
            public string Button { get; set; } = string.Empty;
            public string Function { get; set; } = "GetExortedToAllocToolHistory() in DBCotext";
            public string StoreProcedure { get; set; } = StoredProcedures.GetExportedToAllocToolHistory;
        }
    
    }
}