using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingTool.Repository.Mock
{
    public class MockObjects
    {

        #region DROPDOWNS FOR PARAMETERS
        // Required Filter
        public class Brand 
        {
            public int Brand_Id { get; set; }
            public string Brand_Name { get; set; }
        }

        public class Division
        {
            public int Division_Id { get; set; }
            public string Division_Name { get; set; }
        }

        public class Department
        {
            public int Department_id { get; set; }
            public string Department_Name { get; set; }
        }

        public class Category
        {
            public int Category_Id { get; set; }
            public string Category_Name { get; set; }
        }

        public class PlanPeriod
        {
            public int PlanPeriod_Id { get; set; }
            public string PlanPeriod_Name { get; set; }
        }

        public class Collection
        {
            public int Collection_Id { get; set; }
            public string Collection_Name { get; set; }
        }







        // Style
        public class Style
        {
            public int Style_Id { get; set; }
            public string Style_Code { get; set; }
            public string Style_Name { get; set; }
        }






        // Organization 
        public class Region
        {
            public int Region_Id { get; set; }
            public string Region_Name { get; set; }
        }

        public class Country
        {
            public int Country_Id { get; set; }
            public string Country_Name { get; set; }
        }

        public class Channel
        {
            public int Channel_Id { get; set; }
            public string Channel_Name { get; set; }
        }

        public class Territory
        {
            public int Territory_Id { get; set; }
            public string Territory_Name { get; set; }
        }

        public class Area
        {
            public int Area_Id { get; set; }
            public string Area_Name { get; set; }
        }

        public class OutletType
        {
            public int OutletType_Id { get; set; }
            public string OutletType_Name { get; set; }
        }

        public class Site
        {
            public int Site_Id { get; set; }
            public string Site_Code { get; set; }
            public string Site_Name { get; set; }
        }

        public class StoreGrade
        {
            public int StoreGrade_Id { get; set; }
            public string StoreGrade_Name { get; set; }
        }




        // User Defined or Input
        public class FactorRate
        {
            public decimal FactorRate_Value { get; set; }
        }

        public class PLC
        {
            public int PLC_Value { get; set; }
        }

        public class WeekOfStock
        {
            public int WeekOfStock_Value { get; set; }
        }

        public class MaxTag
        {
            public int MaxTag_Value { get; set; }
        }







        // Attribute
        public class Color
        {
            public int Color_Id { get; set; }
            public string Color_Name { get; set; }
        }

        public class Silhouette
        {
            public int Silhouette_Id { get; set; }
            public string Silhouette_Name { get; set; }
        }

        public class FitCut
        {
            public int FitCut_Id { get; set; }
            public string FitCut_Name { get; set; }
        }

        public class MaterialComposition
        {
            public int MaterialComposition_Id { get; set; }
            public string MaterialComposition_Name { get; set; }
        }

        public class MaterialConstruction
        {
            public int MaterialConstruction_Id { get; set; }
            public string MaterialConstruction_Name { get; set; }
        }

        public class PrintTexture
        {
            public int PrintTexture_Id { get; set; }
            public string PrintTexture_Name { get; set; }
        }

        public class ProductLine
        {
            public int ProductLine_Id { get; set; }
            public string ProductLine_Name { get; set; }
        }

        public class SleeveType
        {
            public int SleeveType_Id { get; set; }
            public string SleeveType_Name { get; set; }
        }

        public class StyleCollection
        {
            public int StyleCollection_Id { get; set; }
            public string StyleCollection_Name { get; set; }
        }

        public class SubCategory
        {
            public int SubCategory_Id { get; set; }
            public string SubCategory_Name { get; set; }
        }

        public class SizeGroup
        {
            public int SizeGroup_Id { get; set; }
            public string SizeGroup_Name { get; set; }
        }

        public class OrigSRP
        {
            public int OrigSRP_Id { get; set; }
            public string OrigSRP_Name { get; set; }
        }
        #endregion




        #region TABLES FOR DATASETS  

        // Step 1
        public class StyleRanking
        {
            public int Style_Id { get; set; }
            public string Style_Code { get; set; }
            public string Image_Url { get; set; }
            public string Color_Name { get; set; }
            public decimal OrigSRP { get; set; }
            public decimal PercentSold { get; set; }
            public int ReceiptsUnit { get; set; }
            public int SoldUnit { get; set; }
            public decimal NetSales { get; set; }
            public decimal SoldUnitPercent { get; set; }
            public decimal NetSalesPercent { get; set; }
            public decimal SOBPercentRank { get; set; }
            public decimal FinalSOB { get; set; }
            public string Tag { get; set; }
            public int Rank { get; set; }
        }


        // Step 3
        public class StoreSizeBreakdown
        {
            public string Site_Code { get; set; }
            public string Site_Name { get; set; }
            public string StoreGrade_Name { get; set; }
            public string Comp_Flag { get; set; }
            public int Order_Qty { get; set; }
            public int IA_Qty_Final { get; set; }
            public int Buffer { get; set; }
            public decimal AWS { get; set; }
            public decimal Lead_Time { get; set; }
            public decimal WOS { get; set; }

            public int OrderQty_6XL { get; set; }
            public int OrderQty_XXL { get; set; }
            public int OrderQty_XXS { get; set; }
            public int OrderQty_XS { get; set; }
            public int OrderQty_S { get; set; }
            public int OrderQty_M { get; set; }
            public int OrderQty_L { get; set; }
            public int OrderQty_XL { get; set; }

            public int IAQty_6XL { get; set; }
            public int IAQty_XXL { get; set; }
            public int IAQty_XXS { get; set; }
            public int IAQty_XS { get; set; }
            public int IAQty_S { get; set; }
            public int IAQty_M { get; set; }
            public int IAQty_L { get; set; }
            public int IAQty_XL { get; set; }
        }


        // Step 4
        public class IAExport
        {
            public string Site_Code { get; set; }
            public int AWS { get; set; }
            public int Size_Id { get; set; }
            public string Size_Name { get; set; }
            public int IA_Qty { get; set; }
        }

        #endregion
    }
}