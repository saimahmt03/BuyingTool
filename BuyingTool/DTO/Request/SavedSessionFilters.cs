using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingTool.DTO.Request
{
    public class SavedSessionFilters
    {
        public int Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public string  UserID { get; set; }
        public string Brand { get; set; } = "";
        public string Division { get; set; } = "";
        public string Category { get; set; } = "";
        public string Department { get; set; } = "";
        public string PeriodPlan { get; set; } = "";
        public string Collection { get; set; } = "";
        public string Style { get; set; } = "";
        public string StyleCode { get; set; } = "";
        public string Region { get; set; } = "";
        public string Country { get; set; } = "";
        public string Channel { get; set; } = "";
        public string Territory { get; set; } = "";
        public string Area { get; set; } = "";
        public string OutletType { get; set; } = "";
        public string SiteCode { get; set; } = "";
        public string SiteName { get; set; } = "";
        public string StoreGrade { get; set; } = "";
        public string Color { get; set; } = "";
        public string Silhouette { get; set; } = "";
        public string CollectionAttri { get; set; } = "";
        public string FitCut { get; set; } = "";
        public string MaterialComposition { get; set; } = "";
        public string MaterialConstruction { get; set; } = "";
        public string PrintTexture { get; set; } = "";
        public string ProductLine { get; set; } = "";
        public string SleeveType { get; set; } = "";
        public string StyleCollection { get; set; } = "";
        public string SubCategory { get; set; } = "";
        public string SizeGroup { get; set; } = "";
        public string OrigSrp { get; set; } = "";
    }
}