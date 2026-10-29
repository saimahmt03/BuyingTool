using System;
using System.Data;
using System.Collections.Generic;
using BuyingTool.Models.MainModel;

namespace BuyingTool.Repository.Mock
{
    public class MockDB
    {


        #region DROPDOWNS FOR PARAMETERS
        // Required Filters
        public readonly List<MockObjects.Brand> BrandList = new List<MockObjects.Brand>
        {
            new MockObjects.Brand { Brand_Id = 1, Brand_Name = "PENSHOPPE" },
            new MockObjects.Brand { Brand_Id = 2, Brand_Name = "MEMO" },
            new MockObjects.Brand { Brand_Id = 3, Brand_Name = "OXYGN" },
        };
        #endregion


        #region TABLES FOR DATASETS

        // Step 3
        public readonly List<ReviewSiteSummary> StoreSizeBreakdownList = new List<ReviewSiteSummary>
        {
            new ReviewSiteSummary { Site_Code = "P001", Site_Name = "P-BTQ SM SOUTHMALL", Store_Grade = "A", Comp_Flag = "COMP", Order_QTY = 44, IA_QTY_Final = 15, Buffer = 29, AWS = 3.1, Lead_Time = 1.4, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 5, Order_S = 7, Order_M = 12, Order_L = 12, Order_XL = 8, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 2, IA_M = 4, IA_L = 4, IA_XL = 3, type = "MAX" },
                new ReviewSiteSummary { Site_Code = "P002", Site_Name = "P-BTQ SM STA. MESA", Store_Grade = "B", Comp_Flag = "COMP", Order_QTY = 27, IA_QTY_Final = 9, Buffer = 18, AWS = 1.9, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 3, Order_S = 5, Order_M = 7, Order_L = 7, Order_XL = 5, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 1, IA_S = 2, IA_M = 2, IA_L = 2, IA_XL = 2, type = "MAX" },
                new ReviewSiteSummary { Site_Code = "P003", Site_Name = "P-BTQ SM CITY MANILA", Store_Grade = "B", Comp_Flag = "COMP", Order_QTY = 54, IA_QTY_Final = 16, Buffer = 38, AWS = 3.7, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 7, Order_S = 11, Order_M = 13, Order_L = 14, Order_XL = 9, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 3, IA_M = 4, IA_L = 4, IA_XL = 3, type = "MIN" },
                new ReviewSiteSummary { Site_Code = "P004", Site_Name = "P-BTQ SM CITY NORTH EDSA", Store_Grade = "B", Comp_Flag = "COMP", Order_QTY = 120, IA_QTY_Final = 36, Buffer = 84, AWS = 8.3, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 15, Order_S = 23, Order_M = 35, Order_L = 29, Order_XL = 18, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 4, IA_S = 7, IA_M = 11, IA_L = 9, IA_XL = 5, type = "AVE" },
                new ReviewSiteSummary { Site_Code = "P005", Site_Name = "P-BTQ SM CITY FAIRVIEW", Store_Grade = "A", Comp_Flag = "COMP", Order_QTY = 80, IA_QTY_Final = 24, Buffer = 56, AWS = 5.5, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 7, Order_S = 15, Order_M = 24, Order_L = 21, Order_XL = 13, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 5, IA_M = 7, IA_L = 6, IA_XL = 4, type = "MIN" },
                new ReviewSiteSummary { Site_Code = "P006", Site_Name = "P-BTQ SM MEGAMALL", Store_Grade = "B", Comp_Flag = "COMP", Order_QTY = 70, IA_QTY_Final = 20, Buffer = 50, AWS = 4.9, Lead_Time = 1.1, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 7, Order_S = 13, Order_M = 22, Order_L = 18, Order_XL = 10, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 4, IA_M = 6, IA_L = 5, IA_XL = 3, type = "AVE" },
                new ReviewSiteSummary { Site_Code = "P007", Site_Name = "P-BTQ EVER COMMONWEALTH", Store_Grade = "B", Comp_Flag = "COMP", Order_QTY = 83, IA_QTY_Final = 26, Buffer = 57, AWS = 5.8, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 8, Order_S = 17, Order_M = 23, Order_L = 19, Order_XL = 16, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 3, IA_S = 5, IA_M = 7, IA_L = 6, IA_XL = 5, type = "MAX" },
                new ReviewSiteSummary { Site_Code = "P008", Site_Name = "P-BTQ TANGUB MISAMIS OCCIDENTAL", Store_Grade = "C", Comp_Flag = "COMP", Order_QTY = 5, IA_QTY_Final = 5, Buffer = 0, AWS = 0.3, Lead_Time = 2.4, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 0, Order_S = 1, Order_M = 2, Order_L = 2, Order_XL = 0, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 0, IA_S = 1, IA_M = 2, IA_L = 2, IA_XL = 0, type = "MIN" },
                new ReviewSiteSummary { Site_Code = "P009", Site_Name = "P-BTQ ROBINSONS ILIGAN", Store_Grade = "C", Comp_Flag = "COMP", Order_QTY = 42, IA_QTY_Final = 16, Buffer = 26, AWS = 2.9, Lead_Time = 2.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 5, Order_S = 10, Order_M = 13, Order_L = 9, Order_XL = 5, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 4, IA_M = 5, IA_L = 3, IA_XL = 2, type = "MIN" },
                new ReviewSiteSummary { Site_Code = "P010", Site_Name = "P-BTQ SM CITY J MALL", Store_Grade = "B", Comp_Flag = "COMP", Order_QTY = 32, IA_QTY_Final = 10, Buffer = 22, AWS = 2.1, Lead_Time = 2.4, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 4, Order_S = 7, Order_M = 10, Order_L = 6, Order_XL = 5, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 1, IA_S = 2, IA_M = 3, IA_L = 2, IA_XL = 2, type = "AVE" },
                new ReviewSiteSummary { Site_Code = "P011", Site_Name = "P-BTQ ROBINSONS GALLERIA", Store_Grade = "C", Comp_Flag = "COMP", Order_QTY = 0, IA_QTY_Final = 0, Buffer = 0, AWS = 0.0, Lead_Time = 1.2, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 0, Order_S = 0, Order_M = 0, Order_L = 0, Order_XL = 0, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 0, IA_S = 0, IA_M = 0, IA_L = 0, IA_XL = 0, type = "MAX" },
                new ReviewSiteSummary { Site_Code = "P012", Site_Name = "P-BTQ ROBINSONS PLACE ERMITA", Store_Grade = "B", Comp_Flag = "COMP", Order_QTY = 50, IA_QTY_Final = 15, Buffer = 35, AWS = 3.5, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 6, Order_S = 10, Order_M = 14, Order_L = 12, Order_XL = 8, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 3, IA_M = 4, IA_L = 4, IA_XL = 2, type = "AVE" },
                new ReviewSiteSummary { Site_Code = "P013", Site_Name = "P-BTQ STA LUCIA EAST", Store_Grade = "C", Comp_Flag = "COMP", Order_QTY = 32, IA_QTY_Final = 9, Buffer = 23, AWS = 2.3, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 4, Order_S = 7, Order_M = 9, Order_L = 8, Order_XL = 4, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 1, IA_S = 2, IA_M = 3, IA_L = 2, IA_XL = 1, type = "MAX" },
                new ReviewSiteSummary { Site_Code = "P014", Site_Name = "P-BTQ ALIMALL", Store_Grade = "A", Comp_Flag = "COMP", Order_QTY = 48, IA_QTY_Final = 15, Buffer = 33, AWS = 3.3, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 6, Order_S = 9, Order_M = 15, Order_L = 12, Order_XL = 6, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 3, IA_M = 4, IA_L = 4, IA_XL = 2, type = "AVE" },
                new ReviewSiteSummary { Site_Code = "P015", Site_Name = "P-BTQ FARMERS CUBAO", Store_Grade = "A", Comp_Flag = "COMP", Order_QTY = 49, IA_QTY_Final = 15, Buffer = 34, AWS = 3.4, Lead_Time = 1.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 3, Order_S = 9, Order_M = 15, Order_L = 14, Order_XL = 8, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 1, IA_S = 3, IA_M = 5, IA_L = 4, IA_XL = 2, type = "MIN" },
                new ReviewSiteSummary { Site_Code = "P016", Site_Name = "P-BTQ FILINVEST", Store_Grade = "C", Comp_Flag = "COMP", Order_QTY = 63, IA_QTY_Final = 19, Buffer = 44, AWS = 4.4, Lead_Time = 1.4, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 8, Order_S = 13, Order_M = 17, Order_L = 16, Order_XL = 9, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 2, IA_S = 5, IA_M = 5, IA_L = 4, IA_XL = 3, type = "MAX" },
                new ReviewSiteSummary { Site_Code = "P017", Site_Name = "P-BTQ GEEGE MALL", Store_Grade = "C", Comp_Flag = "COMP", Order_QTY = 9, IA_QTY_Final = 6, Buffer = 3, AWS = 0.7, Lead_Time = 2.3, WOS = 3.0, Order_6XL = 0, Order_XXL = 0, Order_XXS = 0, Order_XS = 1, Order_S = 2, Order_M = 3, Order_L = 2, Order_XL = 1, IA_6XL = 0, IA_XXL = 0, IA_XXS = 0, IA_XS = 1, IA_S = 2, IA_M = 1, IA_L = 1, IA_XL = 1, type = "MIN" }
        };


        // Step 4
        public readonly List<MockObjects.IAExport> IAExportList = new List<MockObjects.IAExport>
        {
            new MockObjects.IAExport { Site_Code = "P001", AWS = 0, Size_Id = 7, Size_Name = "6XL", IA_Qty = 0 },
            new MockObjects.IAExport { Site_Code = "P001", AWS = 0, Size_Id = 1, Size_Name = "XS", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P001", AWS = 0, Size_Id = 2, Size_Name = "S", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P001", AWS = 0, Size_Id = 3, Size_Name = "M", IA_Qty = 2 },
            new MockObjects.IAExport { Site_Code = "P001", AWS = 0, Size_Id = 4, Size_Name = "L", IA_Qty = 2 },
            new MockObjects.IAExport { Site_Code = "P001", AWS = 0, Size_Id = 5, Size_Name = "XL", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P001", AWS = 0, Size_Id = 6, Size_Name = "XXL", IA_Qty = 0 },
            new MockObjects.IAExport { Site_Code = "P002", AWS = 0, Size_Id = 7, Size_Name = "6XL", IA_Qty = 0 },
            new MockObjects.IAExport { Site_Code = "P002", AWS = 0, Size_Id = 1, Size_Name = "XS", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P002", AWS = 0, Size_Id = 2, Size_Name = "S", IA_Qty = 2 },
            new MockObjects.IAExport { Site_Code = "P002", AWS = 0, Size_Id = 3, Size_Name = "M", IA_Qty = 2 },
            new MockObjects.IAExport { Site_Code = "P002", AWS = 0, Size_Id = 4, Size_Name = "L", IA_Qty = 2 },
            new MockObjects.IAExport { Site_Code = "P002", AWS = 0, Size_Id = 5, Size_Name = "XL", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P002", AWS = 0, Size_Id = 6, Size_Name = "XXL", IA_Qty = 0 },
            new MockObjects.IAExport { Site_Code = "P003", AWS = 0, Size_Id = 7, Size_Name = "6XL", IA_Qty = 0 },
            new MockObjects.IAExport { Site_Code = "P003", AWS = 0, Size_Id = 1, Size_Name = "XS", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P003", AWS = 0, Size_Id = 2, Size_Name = "S", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P003", AWS = 0, Size_Id = 3, Size_Name = "M", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P003", AWS = 0, Size_Id = 4, Size_Name = "L", IA_Qty = 2 },
            new MockObjects.IAExport { Site_Code = "P003", AWS = 0, Size_Id = 5, Size_Name = "XL", IA_Qty = 1 },
            new MockObjects.IAExport { Site_Code = "P003", AWS = 0, Size_Id = 6, Size_Name = "XXL", IA_Qty = 0 },
            new MockObjects.IAExport { Site_Code = "P004", AWS = 0, Size_Id = 7, Size_Name = "6XL", IA_Qty = 0 },
            new MockObjects.IAExport { Site_Code = "P004", AWS = 0, Size_Id = 1, Size_Name = "XS", IA_Qty = 3 }
        };


        #endregion




        // USER TEST DATA
        public readonly List<User> UserList = new List<User>
        {
            new User { Id_Number = "015726", Password = "user" },
        };


        // STEP 1: OPTIONS GALLERY TABLE
        public readonly List<OptionsGallery> OptionsGalleryList = new List<OptionsGallery>
        {
            new OptionsGallery
            {
                Image = "",
                Style = "994547",
                Style_Code = "PMT0426GTS_012",
                Color = "Black",
                Orig_SRP = 799.00m,
                Percent_Sold = 0.8595m,
                Receipts_Unit = 6620,
                Sold_Unit = 5690,
                Net_Sales = 4498270.27m,
                Sold_Unit_Percent = 0.0224m,
                Net_Sales_Percent = 0.0340m,
                SOB_Percent_Rank = 2, // Note: represented as int per your model
                Final_SOB = 0.0282m,
                Tag = "MAX",
                Rank = 1
            },

            new OptionsGallery
            {
                Image = "",
                Style = "993665",
                Style_Code = "PMT0426GTS_010",
                Color = "Ivory",
                Orig_SRP = 749.00m,
                Percent_Sold = 0.8400m,
                Receipts_Unit = 6093,
                Sold_Unit = 5118,
                Net_Sales = 3773373.39m,
                Sold_Unit_Percent = 0.0202m,
                Net_Sales_Percent = 0.0285m,
                SOB_Percent_Rank = 2,
                Final_SOB = 0.0526m,
                Tag = "MAX",
                Rank = 2
            },

            new OptionsGallery
            {
                Image = "",
                Style = "992878",
                Style_Code = "PMT0326GTS_008",
                Color = "Black",
                Orig_SRP = 749.00m,
                Percent_Sold = 0.8135m,
                Receipts_Unit = 6150,
                Sold_Unit = 5003,
                Net_Sales = 3704573.05m,
                Sold_Unit_Percent = 0.0197m,
                Net_Sales_Percent = 0.0280m,
                SOB_Percent_Rank = 2,
                Final_SOB = 0.0765m,
                Tag = "MAX",
                Rank = 3
            },

            new OptionsGallery
            {
                Image = "",
                Style = "994494",
                Style_Code = "PMT0626GTS_007",
                Color = "Black",
                Orig_SRP = 749.00m,
                Percent_Sold = 0.7738m,
                Receipts_Unit = 6065,
                Sold_Unit = 4693,
                Net_Sales = 3467062.61m,
                Sold_Unit_Percent = 0.0185m,
                Net_Sales_Percent = 0.0262m,
                SOB_Percent_Rank = 2,
                Final_SOB = 0.0988m,
                Tag = "MAX",
                Rank = 4
            },

            new OptionsGallery
            {
                Image = "",
                Style = "992870",
                Style_Code = "PMT0326GTS_009",
                Color = "White",
                Orig_SRP = 379.00m,
                Percent_Sold = 0.8583m,
                Receipts_Unit = 7806,
                Sold_Unit = 6700,
                Net_Sales = 2305033.11m,
                Sold_Unit_Percent = 0.0264m,
                Net_Sales_Percent = 0.0174m,
                SOB_Percent_Rank = 2,
                Final_SOB = 0.1207m,
                Tag = "MAX",
                Rank = 5
            },

            new OptionsGallery
            {
                Image = "",
                Style = "993558",
                Style_Code = "PMT0226GTS_006",
                Color = "Ivory",
                Orig_SRP = 799.00m,
                Percent_Sold = 0.7708m,
                Receipts_Unit = 5612,
                Sold_Unit = 4326,
                Net_Sales = 3408227.79m,
                Sold_Unit_Percent = 0.0171m,
                Net_Sales_Percent = 0.0258m,
                SOB_Percent_Rank = 2,
                Final_SOB = 0.1422m,
                Tag = "MAX",
                Rank = 6
            },

            new OptionsGallery
            {
                Image = "",
                Style = "993645",
                Style_Code = "PMT0426GTS_004",
                Color = "Black",
                Orig_SRP = 379.00m,
                Percent_Sold = 0.7837m,
                Receipts_Unit = 8100,
                Sold_Unit = 6348,
                Net_Sales = 2133385.35m,
                Sold_Unit_Percent = 0.0250m,
                Net_Sales_Percent = 0.0161m,
                SOB_Percent_Rank = 2,
                Final_SOB = 0.1627m,
                Tag = "MAX",
                Rank = 7
            }
        };


        // STEP 4: EXPORT TO EXCEL TABLE
        public readonly List<Export> ExportList = new List<Export>
        {
            new Export { Site_Code = "P001", AWS = "0", Size = "6XL", IA_QTY = "0" },
            new Export { Site_Code = "P001", AWS = "0", Size = "XS", IA_QTY = "1" },
            new Export { Site_Code = "P001", AWS = "0", Size = "S", IA_QTY = "1" },
            new Export { Site_Code = "P001", AWS = "0", Size = "M", IA_QTY = "2" },
            new Export { Site_Code = "P001", AWS = "0", Size = "L", IA_QTY = "2" },
            new Export { Site_Code = "P001", AWS = "0", Size = "XL", IA_QTY = "1" },
            new Export { Site_Code = "P001", AWS = "0", Size = "XXL", IA_QTY = "0" },
            new Export { Site_Code = "P002", AWS = "0", Size = "6XL", IA_QTY = "0" },
            new Export { Site_Code = "P002", AWS = "0", Size = "XS", IA_QTY = "1" },
            new Export { Site_Code = "P002", AWS = "0", Size = "S", IA_QTY = "2" },
            new Export { Site_Code = "P002", AWS = "0", Size = "M", IA_QTY = "2" },
            new Export { Site_Code = "P002", AWS = "0", Size = "L", IA_QTY = "2" },
            new Export { Site_Code = "P002", AWS = "0", Size = "XL", IA_QTY = "1" },
            new Export { Site_Code = "P002", AWS = "0", Size = "XXL", IA_QTY = "0" },
            new Export { Site_Code = "P003", AWS = "0", Size = "6XL", IA_QTY = "0" },
            new Export { Site_Code = "P003", AWS = "0", Size = "XS", IA_QTY = "1" },
            new Export { Site_Code = "P003", AWS = "0", Size = "S", IA_QTY = "1" },
            new Export { Site_Code = "P003", AWS = "0", Size = "M", IA_QTY = "1" },
            new Export { Site_Code = "P003", AWS = "0", Size = "L", IA_QTY = "2" },
            new Export { Site_Code = "P003", AWS = "0", Size = "XL", IA_QTY = "1" },
            new Export { Site_Code = "P003", AWS = "0", Size = "XXL", IA_QTY = "0" }
        };



        public readonly List<MainDataset> MainDatasetList = new List<MainDataset>
        {
            new MainDataset { Type = "MIN", Store_Grade = "A", Territory_Code = "R3", Drop = "Drop 2", Lead_Time = 1.4, WOS = 3, Site_Code = "F004", Comp_Flag = "", Site_Name = "", AWS = 1.1, Size = "XXL", Order_QTY = 2, IA_QTY = 1, Buffer = 1 },
            new MainDataset { Type = "MIN", Store_Grade = "B", Territory_Code = "R2", Drop = "Drop 2", Lead_Time = 0.0, WOS = 3, Site_Code = "F096", Comp_Flag = "", Site_Name = "", AWS = 0.0, Size = "XXL", Order_QTY = 0, IA_QTY = 0, Buffer = 0 },
            new MainDataset { Type = "MIN", Store_Grade = "D", Territory_Code = "R5", Drop = "Drop 1", Lead_Time = 2.3, WOS = 3, Site_Code = "F061", Comp_Flag = "", Site_Name = "", AWS = 0.3, Size = "M", Order_QTY = 2, IA_QTY = 2, Buffer = 0 },
            new MainDataset { Type = "MIN", Store_Grade = "C", Territory_Code = "R3", Drop = "Drop 2", Lead_Time = 1.4, WOS = 3, Site_Code = "F052", Comp_Flag = "", Site_Name = "", AWS = 0.8, Size = "M", Order_QTY = 3, IA_QTY = 1, Buffer = 2 },
            new MainDataset { Type = "MIN", Store_Grade = "C", Territory_Code = "R5", Drop = "Drop 1", Lead_Time = 2.4, WOS = 3, Site_Code = "F173", Comp_Flag = "", Site_Name = "", AWS = 0.3, Size = "XXS", Order_QTY = 0, IA_QTY = 0, Buffer = 0 },
            new MainDataset { Type = "MAX", Store_Grade = "C", Territory_Code = "R1", Drop = "Drop 2", Lead_Time = 1.6, WOS = 3, Site_Code = "F103", Comp_Flag = "", Site_Name = "", AWS = 1.8, Size = "XXL", Order_QTY = 2, IA_QTY = 1, Buffer = 1 },
            new MainDataset { Type = "AVE", Store_Grade = "C", Territory_Code = "R4", Drop = "Drop 1", Lead_Time = 2.3, WOS = 3, Site_Code = "F100", Comp_Flag = "", Site_Name = "", AWS = 0.5, Size = "L", Order_QTY = 2, IA_QTY = 2, Buffer = 0 },
            new MainDataset { Type = "MIN", Store_Grade = "A", Territory_Code = "R4", Drop = "Drop 1", Lead_Time = 2.3, WOS = 3, Site_Code = "F086", Comp_Flag = "", Site_Name = "", AWS = 1.1, Size = "XXL", Order_QTY = 2, IA_QTY = 1, Buffer = 1 },
            new MainDataset { Type = "AVE", Store_Grade = "B", Territory_Code = "R5", Drop = "Drop 1", Lead_Time = 2.3, WOS = 3, Site_Code = "F071", Comp_Flag = "", Site_Name = "", AWS = 0.9, Size = "XS", Order_QTY = 1, IA_QTY = 1, Buffer = 0 },
            new MainDataset { Type = "MIN", Store_Grade = "B", Territory_Code = "R2", Drop = "Drop 2", Lead_Time = 1.4, WOS = 3, Site_Code = "F147", Comp_Flag = "", Site_Name = "", AWS = 0.7, Size = "XL", Order_QTY = 3, IA_QTY = 2, Buffer = 1 },
            new MainDataset { Type = "MAX", Store_Grade = "B", Territory_Code = "R2", Drop = "Drop 2", Lead_Time = 0.0, WOS = 3, Site_Code = "F096", Comp_Flag = "", Site_Name = "", AWS = 0.0, Size = "XXS", Order_QTY = 0, IA_QTY = 0, Buffer = 0 },
            new MainDataset { Type = "AVE", Store_Grade = "C", Territory_Code = "R4", Drop = "Drop 1", Lead_Time = 2.3, WOS = 3, Site_Code = "F026", Comp_Flag = "", Site_Name = "", AWS = 1.1, Size = "L", Order_QTY = 5, IA_QTY = 2, Buffer = 3 },
            new MainDataset { Type = "MIN", Store_Grade = "Online", Territory_Code = "Online", Drop = "Drop 2", Lead_Time = 1.1, WOS = 3, Site_Code = "FX03", Comp_Flag = "", Site_Name = "", AWS = 8.1, Size = "XL", Order_QTY = 19, IA_QTY = 5, Buffer = 14 },
            new MainDataset { Type = "AVE", Store_Grade = "B", Territory_Code = "R1", Drop = "Drop 2", Lead_Time = 1.4, WOS = 3, Site_Code = "F169", Comp_Flag = "", Site_Name = "", AWS = 1.2, Size = "XXL", Order_QTY = 2, IA_QTY = 1, Buffer = 1 },
            new MainDataset { Type = "MAX", Store_Grade = "B", Territory_Code = "R5", Drop = "Drop 1", Lead_Time = 2.1, WOS = 3, Site_Code = "F159", Comp_Flag = "", Site_Name = "", AWS = 0.9, Size = "L", Order_QTY = 4, IA_QTY = 2, Buffer = 2 },
            new MainDataset { Type = "MIN", Store_Grade = "B", Territory_Code = "R4", Drop = "Drop 1", Lead_Time = 2.1, WOS = 3, Site_Code = "F153", Comp_Flag = "", Site_Name = "", AWS = 0.6, Size = "M", Order_QTY = 2, IA_QTY = 2, Buffer = 0 },
            new MainDataset { Type = "MAX", Store_Grade = "C", Territory_Code = "R4", Drop = "Drop 1", Lead_Time = 1.9, WOS = 3, Site_Code = "F165", Comp_Flag = "", Site_Name = "", AWS = 0.7, Size = "L", Order_QTY = 3, IA_QTY = 2, Buffer = 1 },
            new MainDataset { Type = "MIN", Store_Grade = "B", Territory_Code = "R4", Drop = "Drop 1", Lead_Time = 2.3, WOS = 3, Site_Code = "F078", Comp_Flag = "", Site_Name = "", AWS = 1.0, Size = "L", Order_QTY = 3, IA_QTY = 1, Buffer = 2 },
            new MainDataset { Type = "MIN", Store_Grade = "C", Territory_Code = "R3", Drop = "Drop 2", Lead_Time = 1.3, WOS = 3, Site_Code = "F023", Comp_Flag = "", Site_Name = "", AWS = 1.1, Size = "M", Order_QTY = 4, IA_QTY = 2, Buffer = 2 },
            new MainDataset { Type = "MAX", Store_Grade = "B", Territory_Code = "R2", Drop = "Drop 2", Lead_Time = 1.4, WOS = 3, Site_Code = "F147", Comp_Flag = "", Site_Name = "", AWS = 0.7, Size = "XXS", Order_QTY = 0, IA_QTY = 0, Buffer = 0 },
            new MainDataset { Type = "MIN", Store_Grade = "C", Territory_Code = "R1", Drop = "Drop 2", Lead_Time = 1.4, WOS = 3, Site_Code = "F072", Comp_Flag = "", Site_Name = "", AWS = 0.7, Size = "XS", Order_QTY = 1, IA_QTY = 0, Buffer = 1 },
            new MainDataset { Type = "AVE", Store_Grade = "B", Territory_Code = "R5", Drop = "Drop 1", Lead_Time = 2.4, WOS = 3, Site_Code = "F099", Comp_Flag = "", Site_Name = "", AWS = 1.2, Size = "L", Order_QTY = 5, IA_QTY = 2, Buffer = 3 },
            new MainDataset { Type = "AVE", Store_Grade = "B", Territory_Code = "R1", Drop = "Drop 2", Lead_Time = 1.1, WOS = 3, Site_Code = "F006", Comp_Flag = "", Site_Name = "", AWS = 3.6, Size = "XXS", Order_QTY = 0, IA_QTY = 0, Buffer = 0 },
            new MainDataset { Type = "MAX", Store_Grade = "B", Territory_Code = "R1", Drop = "Drop 2", Lead_Time = 1.4, WOS = 3, Site_Code = "F138", Comp_Flag = "", Site_Name = "", AWS = 2.6, Size = "L", Order_QTY = 10, IA_QTY = 3, Buffer = 7 },
            new MainDataset { Type = "AVE", Store_Grade = "C", Territory_Code = "R3", Drop = "Drop 2", Lead_Time = 1.6, WOS = 3, Site_Code = "F166", Comp_Flag = "", Site_Name = "", AWS = 0.4, Size = "XL", Order_QTY = 1, IA_QTY = 1, Buffer = 0 },
            new MainDataset { Type = "AVE", Store_Grade = "C", Territory_Code = "R1", Drop = "Drop 2", Lead_Time = 1.6, WOS = 3, Site_Code = "F103", Comp_Flag = "", Site_Name = "", AWS = 0.9, Size = "XL", Order_QTY = 3, IA_QTY = 1, Buffer = 2 },
            new MainDataset { Type = "MIN", Store_Grade = "B", Territory_Code = "R2", Drop = "Drop 2", Lead_Time = 1.3, WOS = 3, Site_Code = "F046", Comp_Flag = "", Site_Name = "", AWS = 1.2, Size = "L", Order_QTY = 4, IA_QTY = 1, Buffer = 3 },
            new MainDataset { Type = "MAX", Store_Grade = "B", Territory_Code = "R3", Drop = "Drop 2", Lead_Time = 1.3, WOS = 3, Site_Code = "F076", Comp_Flag = "", Site_Name = "", AWS = 1.6, Size = "M", Order_QTY = 6, IA_QTY = 2, Buffer = 4 },
            new MainDataset { Type = "MIN", Store_Grade = "C", Territory_Code = "R4", Drop = "Drop 1", Lead_Time = 2.3, WOS = 3, Site_Code = "F026", Comp_Flag = "", Site_Name = "", AWS = 0.6, Size = "S", Order_QTY = 1, IA_QTY = 1, Buffer = 0 },
        };



        public readonly List<FiltersDropdowns> FiltersDropdownsList = new List<FiltersDropdowns>
        {
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Visayas",
                Area = "ASM 18",
                OutletType = "Boutique",
                SiteCode = "F008",
                StoreName = "F-BTQ AYALA CENTER CEBU",
                StoreGrade = "A",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Visayas",
                Area = "ASM 20",
                OutletType = "Boutique",
                SiteCode = "F112",
                StoreName = "F-BTQ GAISANO CALBAYOG",
                StoreGrade = "B",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Online",
                Territory = "Online",
                Area = "Not Applicable",
                OutletType = "Online Store",
                SiteCode = "FZ91",
                StoreName = "FORME MARKETING SEEDING",
                StoreGrade = "Online",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Visayas",
                Area = "ASM 18",
                OutletType = "Boutique",
                SiteCode = "F100",
                StoreName = "F-BTQ CENTRAL BLOC",
                StoreGrade = "C",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Online",
                Territory = "Online",
                Area = "Not Applicable",
                OutletType = "Online Store",
                SiteCode = "FX01",
                StoreName = "F-ECO LAZADA PH",
                StoreGrade = "Online",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "North NCR/Luzon",
                Area = "ASM 1",
                OutletType = "Boutique",
                SiteCode = "F035",
                StoreName = "F-BTQ CSI CITY MALL LA UNION",
                StoreGrade = "C",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Online",
                Territory = "Online",
                Area = "Not Applicable",
                OutletType = "Online Store",
                SiteCode = "FZ93",
                StoreName = "FORME MARKETING PULL OUT",
                StoreGrade = "Online",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "North NCR/Luzon",
                Area = "ASM 4",
                OutletType = "Boutique",
                SiteCode = "F033",
                StoreName = "F-BTQ NE PACIFIC MALL",
                StoreGrade = "D",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "South NCR/Luzon",
                Area = "ASM 15",
                OutletType = "Boutique",
                SiteCode = "F020",
                StoreName = "F-BTQ AYALA MALL LEGASPI",
                StoreGrade = "C",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Visayas",
                Area = "ASM 18",
                OutletType = "Boutique",
                SiteCode = "F030",
                StoreName = "F-BTQ SM CITY CEBU",
                StoreGrade = "A",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Central NCR",
                Area = "GMA",
                OutletType = "Boutique",
                SiteCode = "FZ90",
                StoreName = "FORME BORROWED ITEMS",
                StoreGrade = " ",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Online",
                Territory = "Online",
                Area = "Not Applicable",
                OutletType = "Online Store",
                SiteCode = "FX02",
                StoreName = "F-ECO ZALORA PH",
                StoreGrade = "Online",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Visayas",
                Area = "ASM 18",
                OutletType = "Boutique",
                SiteCode = "F064",
                StoreName = "F-BTQ SM CONSOLACION",
                StoreGrade = "B",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "South NCR/Luzon",
                Area = "ASM 15",
                OutletType = "Boutique",
                SiteCode = "F163",
                StoreName = "F-BTQ SM CITY DAET",
                StoreGrade = "C",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "North NCR/Luzon",
                Area = "ASM 4",
                OutletType = "Boutique",
                SiteCode = "F139",
                StoreName = "F-BTQ SM TELABASTAGAN",
                StoreGrade = "C",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "North NCR/Luzon",
                Area = "ASM 1",
                OutletType = "Boutique",
                SiteCode = "F175",
                StoreName = "F-BTQ SM CITY LA UNION",
                StoreGrade = "B",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Central NCR",
                Area = "ASM 7",
                OutletType = "Boutique",
                SiteCode = "F147",
                StoreName = "F-BTQ VENICE GRAND CANAL",
                StoreGrade = "B",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "North NCR/Luzon",
                Area = "ASM 1",
                OutletType = "Boutique",
                SiteCode = "F103",
                StoreName = "F-BTQ VIGAN PLAZA MAESTRO",
                StoreGrade = "C",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            },
            new FiltersDropdowns
            {
                Brand = "FORME",
                Division = "LADIES",
                Departmet = "TOPS",
                Category = "GRAPHIC TEE",
                PlanPeriod = "P11 2025",
                Collection = "Commercial",
                StyleCode = "FLT1125GTS_002",
                Style = "992077",
                Region = "Local",
                Country = "Philippines",
                Channel = "Retail",
                Territory = "Mindanao",
                Area = "ASM 25",
                OutletType = "Department Store",
                SiteCode = "F414",
                StoreName = "SOUTHSEAS - F",
                StoreGrade = "Dept. Store",
                Color = "Tan",
                Silhouette = "T-Shirt",
                CollectionAttribute = "",
                FitCut = "Semi Cropped",
                MaterialComposition = "NULL",
                MaterialConstruction = "NULL",
                PrintTexture = "NULL",
                ProductLine = "Commercial",
                SleeveType = "Short Sleeves",
                StyleCollecion = "Commercial",
                SubCategory = "NULL",
                SizeGroup = "Alpha",
                OrigSRP = "598"
            }
        };
   

        public DataTable GetSiteSizeDataTable1()
        {
            // 1. Initialize the DataTable with columns matching the grid layout
            DataTable dt = new DataTable("SiteSizeData");
    
            dt.Columns.Add("SiteCode", typeof(string));
            dt.Columns.Add("SiteName", typeof(string));
            dt.Columns.Add("Size", typeof(string));
            dt.Columns.Add("Receipts", typeof(int));
            dt.Columns.Add("Sales", typeof(int));
            dt.Columns.Add("SellThruPct", typeof(decimal));
            dt.Columns.Add("EstSalesQTY", typeof(int));
            dt.Columns.Add("NewQTY_V1", typeof(int));
            dt.Columns.Add("NewCurve_V1", typeof(decimal));
            dt.Columns.Add("NewCurve_V1_Dup", typeof(decimal));
            dt.Columns.Add("BegSizeCurve", typeof(decimal));
            dt.Columns.Add("VarNewCurve", typeof(decimal));

            // Helper method to add rows cleanly
            void AddRow(string code, string name, string size, int receipts, int sales, decimal sellThru, int estSales, int newQty, decimal newCurve, decimal begCurve, decimal varCurve)
            {
                dt.Rows.Add(code, name, size, receipts, sales, sellThru, estSales, newQty, newCurve, newCurve, begCurve, varCurve);
            }

            // --- SITE P006: P-BTQ SM MEGAMALL ---
            AddRow("P006", "P-BTQ SM MEGAMALL", "6XL", 0, 0, 0.00m, 0, 0, 0.00m, 0.00m, -0.13m);
            AddRow("P006", "P-BTQ SM MEGAMALL", "XXL", 6, 0, 0.00m, 0, 0, 0.00m, 0.13m, -0.13m);
            AddRow("P006", "P-BTQ SM MEGAMALL", "XXS", 0, 0, 0.00m, 0, 0, 0.00m, 0.00m, 0.00m);
            AddRow("P006", "P-BTQ SM MEGAMALL", "XS", 483, 221, 45.76m, 819, 221, 10.10m, 10.41m, -0.31m);
            AddRow("P006", "P-BTQ SM MEGAMALL", "S", 846, 394, 46.57m, 1459, 394, 18.01m, 18.24m, -0.23m);
            AddRow("P006", "P-BTQ SM MEGAMALL", "M", 1485, 689, 46.40m, 2552, 689, 31.49m, 32.02m, -0.53m);
            AddRow("P006", "P-BTQ SM MEGAMALL", "L", 1166, 564, 48.37m, 2089, 564, 25.78m, 25.14m, 0.64m);
            AddRow("P006", "P-BTQ SM MEGAMALL", "XL", 652, 320, 49.08m, 1185, 320, 14.63m, 14.06m, 0.57m);
            // Total Row for P006
            AddRow("P006", "P-BTQ SM MEGAMALL", "Total", 4638, 2188, 47.18m, 8104, 2188, 100.00m, 100.00m, 0.00m);


            // --- SITE P007: P-BTQ EVER COMMONWEALTH ---
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "6XL", 0, 0, 0.00m, 0, 0, 0.00m, 0.00m, 0.00m);
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "XXL", 9, 1, 11.11m, 4, 1, 0.15m, 0.28m, -0.14m);
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "XXS", 0, 0, 0.00m, 0, 0, 0.00m, 0.00m, 0.00m);
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "XS", 333, 69, 20.72m, 256, 69, 10.03m, 10.41m, -0.38m);
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "S", 591, 137, 23.18m, 507, 137, 19.91m, 18.47m, 1.44m);
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "M", 1005, 187, 18.61m, 693, 187, 27.18m, 31.42m, -4.24m);
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "L", 803, 161, 20.05m, 596, 161, 23.40m, 25.10m, -1.70m);
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "XL", 458, 133, 29.04m, 493, 133, 19.33m, 14.32m, 5.01m);
            // Total Row for P007
            AddRow("P007", "P-BTQ EVER COMMONWEALTH", "Total", 3199, 688, 21.51m, 2548, 688, 100.00m, 100.00m, 0.00m);


            // --- SITE P008: P-BTQ TANGUB MISAMIS OCCIDENTAL ---
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "6XL", 0, 0, 0.00m, 0, 0, 0.00m, 0.00m, 0.00m);
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "XXL", 0, 0, 0.00m, 0, 0, 0.00m, 0.00m, 0.00m);
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "XXS", 0, 0, 0.00m, 0, 0, 0.00m, 0.00m, 0.00m);
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "XS", 92, 7, 7.61m, 26, 7, 7.07m, 12.78m, -5.71m);
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "S", 131, 21, 16.03m, 78, 21, 21.21m, 18.19m, 3.02m);
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "M", 199, 28, 14.07m, 104, 28, 28.28m, 27.64m, 0.64m);
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "L", 205, 35, 17.07m, 130, 35, 35.35m, 28.47m, 6.88m);
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "XL", 93, 8, 8.60m, 30, 8, 8.08m, 12.92m, -4.84m);
            // Total Row for P008
            AddRow("P008", "P-BTQ TANGUB MISAMIS OCCIDENTAL", "Total", 720, 99, 13.75m, 268, 99, 100.00m, 100.00m, 0.00m);

            return dt;
        }
    
        public DataTable GetSiteSizeDataTable2()
        {
            // 1. Initialize the DataTable (mimicking a dynamic Stored Procedure result)
            DataTable dt = new DataTable("SizeMetricMatrix");

            // 2. Define the columns dynamically based on the image layout
            // The first column acts as the Row Identifier/Metric Name (matching the "Size" header column)
            dt.Columns.Add("Size", typeof(string));
            dt.Columns.Add("XXL", typeof(string));
            dt.Columns.Add("XS", typeof(string));
            dt.Columns.Add("S", typeof(string));
            dt.Columns.Add("M", typeof(string));
            dt.Columns.Add("L", typeof(string));
            dt.Columns.Add("XL", typeof(string));

            // Helper method to populate rows matching the image values
            void AddRow(string metricName, string xxl, string xs, string s, string m, string l, string xl)
            {
                dt.Rows.Add(metricName, xxl, xs, s, m, l, xl);
            }

            // 3. Add rows matching the image data exactly[cite: 3]
            AddRow("New QTY", "237", "17,152", "29,230", "43,193", "34,700", "20,428");
            AddRow("SOB %", "0.16%", "11.83%", "20.18%", "29.80%", "23.94%", "14.09%");

            return dt;
        }
    }
}

