using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingTool.Logs
{
    public class ErrorMessage
    {
        public const string Db = "Database OR Stored Procedure Error in ";
        public const string Sys = "System Error in ";

        public const string Login = "Login. ";

        public const string Attribute = "Attribute Filter Region. ";
        public const string Organization = "Organization Filter Region. ";
        public const string Required = "Required Filter Region. ";
        public const string Style = "Style Filter Region. ";
        public const string StoregradeAndSize = "Final Dataset Store Grade and Size Filter. ";

        public const string PeriodPlan = "Period Plan Filter. ";
        public const string OptionGallery = "Option Gallery. ";
        public const string Calculation = "Calculate. ";

        public const string SummaryDistribution = "Summary Distribution. ";
        public const string Override = "Summary Distribution Override. ";

        public const string StoreBreakdown = "Store Breakdown or Per Site. ";

        public const string Export = "Export. ";

        public const string SizeCurve = "Size Curve. ";

        public const string ExportToAlloc = "Export to Alloc. ";
        public const string ExportToAllocMonitoring = "Export to Alloc Monitoring. ";
    }
}