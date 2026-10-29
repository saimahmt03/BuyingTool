using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuyingTool.Services
{
    public class ServiceHelper
    {

        #region Populate DataTable Helper

        public Task<(List<string> tableHeader, List<string[]> tableRows)> PopulateDatatableHelper(DataTable dataTable)
        {
            if (dataTable == null)
            {
                return Task.FromResult((new List<string>(), new List<string[]>()));
            }

            // Extract table headers
            List<string> tableHeader = dataTable.Columns.Cast<DataColumn>()
                                                .Select(c => c.ColumnName)
                                                .ToList();

            // Extract table rows with null safety
            List<string[]> tableRows = dataTable.Rows.Cast<DataRow>()
                                               .Select(r => r.ItemArray.Select(v => v?.ToString() ?? string.Empty).ToArray())
                                               .ToList();

            return Task.FromResult((tableHeader, tableRows));
        }

        #endregion

        #region Table Column and Row Hidder Helper

        public Task<(List<string> tHeader, List<string[]> tRows)> TableColumnAndRowHidder(
            List<string> tHeader,
            List<string[]> tRows,
            string columnsToHide
         )
        {
            if (tHeader == null || !tHeader.Any() || tRows == null)
            {
                return Task.FromResult((new List<string>(), new List<string[]>()));
            }

            // 1. Rebuild a temporary DataTable from tHeader and tRows to ensure absolute sync
            var table = new DataTable();

            foreach (var header in tHeader)
            {
                string columnName = header ?? "Column";

                // Ensure unique column names in case duplicate headers exist
                string uniqueName = columnName;
                int counter = 1;
                while (table.Columns.Contains(uniqueName))
                {
                    uniqueName = $"{columnName}_{counter++}";
                }

                table.Columns.Add(uniqueName, typeof(string));
            }

            foreach (var rowArray in tRows)
            {
                var workRow = table.NewRow();
                for (int i = 0; i < Math.Min(rowArray.Length, table.Columns.Count); i++)
                {
                    workRow[i] = rowArray[i] ?? string.Empty;
                }
                table.Rows.Add(workRow);
            }

            // 2. Parse the comma-separated string into a case-insensitive HashSet for fast lookup
            var hideSet = new HashSet<string>(
                string.IsNullOrEmpty(columnsToHide)
                    ? Enumerable.Empty<string>()
                    : columnsToHide.Split(',').Select(c => c.Trim()),
                StringComparer.OrdinalIgnoreCase
            );

            // 3. Remove unwanted columns directly from the DataTable schema
            var columnsToRemove = table.Columns.Cast<DataColumn>()
                .Where(c => hideSet.Contains(c.ColumnName))
                .ToList();

            foreach (var col in columnsToRemove)
            {
                table.Columns.Remove(col);
            }

            // 4. Extract the synchronized headers and rows back from the filtered DataTable
            List<string> filteredHeader = table.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            List<string[]> filteredRows = table.Rows.Cast<DataRow>()
                .Select(r => r.ItemArray.Select(v => v?.ToString() ?? string.Empty).ToArray())
                .ToList();

            return Task.FromResult((filteredHeader, filteredRows));
        }

        #endregion

        #region Export to Excel Helper

        public Task ExportToExcel(List<string> tableHeader, List<string[]> tableRows, string customFileName, HttpResponse response = null)
        {
            response = response ?? HttpContext.Current.Response;

            // 1. Convert headers and rows into a DataTable
            DataTable dataTable = new DataTable("ExportedData");

            foreach (var header in tableHeader)
            {
                string columnName = header?.Trim() ?? "Column";
                string uniqueColumnName = columnName;
                int counter = 1;

                while (dataTable.Columns.Contains(uniqueColumnName))
                {
                    uniqueColumnName = $"{columnName}_{counter++}";
                }

                dataTable.Columns.Add(uniqueColumnName, typeof(string));
            }

            foreach (var rowValues in tableRows)
            {
                DataRow row = dataTable.NewRow();
                for (int i = 0; i < tableHeader.Count; i++)
                {
                    row[i] = i < rowValues.Length ? (rowValues[i] ?? string.Empty).Trim() : string.Empty;
                }
                dataTable.Rows.Add(row);
            }

            // 2. Generate Excel using ClosedXML in a MemoryStream
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(dataTable, "Report");

                // Style the header row
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");
                headerRow.Style.Font.FontColor = XLColor.White;

                // Auto-fit column widths
                worksheet.Columns().AdjustToContents();

                // 3. Clean up and format the user-provided filename safely
                string safeFileName = string.IsNullOrWhiteSpace(customFileName) ? "MyReport" : customFileName.Trim();
                if (!safeFileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    safeFileName += ".xlsx";
                }

                // 4. Stream the file to the browser with the custom filename
                response.Clear();
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.AddHeader("content-disposition", $"attachment;filename={safeFileName}");

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(response.OutputStream);
                }

                response.Flush();
                response.End();
            }

            return Task.CompletedTask;
        }

        #endregion


        public Task<(string matno, string color)> SliceFileName(string filename)
        {
            var words = filename.Split('_');

            string matno = words.ElementAtOrDefault(0) ?? string.Empty;
            string color = words.ElementAtOrDefault(1) ?? string.Empty;

            // Notice the double parentheses: Task.FromResult((tuple_values))
            return Task.FromResult((matno, color));
        }

        #region XML for Alloc
        public Task<DataTable> ConvertListToDataTable(List<string> tableHeaders, List<string[]> tableRows)
        {
            DataTable dataTable = new DataTable();

            // 1. Add columns from tableHeaders
            foreach (var header in tableHeaders)
            {
                dataTable.Columns.Add(header ?? "Column");
            }

            // 2. Add rows from tableRows (List<string[]>)
            foreach (var rowArray in tableRows)
            {
                DataRow dataRow = dataTable.NewRow();

                for (int i = 0; i < rowArray.Length && i < dataTable.Columns.Count; i++)
                {
                    dataRow[i] = (object)rowArray[i] ?? DBNull.Value;
                }

                dataTable.Rows.Add(dataRow);
            }

            return Task.FromResult(dataTable);
        }

        public Task<XDocument> DataTabletoXML(DataTable dataTable)
        {
            DataSet ds = new DataSet("Sheet");
            ds.Tables.Add(dataTable);

            XDocument xmlDoc = XDocument.Parse(ds.GetXml());

            return Task.FromResult(xmlDoc);
        }

        #endregion
    }
}