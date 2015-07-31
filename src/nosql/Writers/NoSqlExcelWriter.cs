namespace nosql.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using MongoDB.Bson;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using OfficeOpenXml.Table.PivotTable;

    public static class ExcelWriter
    {
        public static FileInfo ToExcelFile(this MongoWorksheetCollection collection)
        {
            var tempPath = Path.GetTempPath() + "Report-" + Guid.NewGuid() + ".xlsx";

            var newFile = new FileInfo(tempPath);
            var package = new ExcelPackage(newFile);
            
            foreach (var ws in collection.Worksheets)
            {
                AddWorksheet(ws.Data, ws.Name, ws.Headers, package);
            }

            package.Save();

            return newFile;
        }

        public static FileInfo ToExcelFile(this IEnumerable<BsonDocument> data, string worksheetName, IEnumerable<string> headings)
        {
            var tempPath = Path.GetTempPath() + "Report-" + Guid.NewGuid() + ".xlsx";
            var newFile = new FileInfo(tempPath);

            var package = new ExcelPackage(newFile);
            AddWorksheet(data, worksheetName, headings, package);
            package.Save();

            return newFile;
        }

        private static void AddWorksheet(IEnumerable<BsonDocument> data, string worksheetName, IEnumerable<string> headings, ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);
            var headingCount = 0;
            foreach (var item in headings)
                worksheet.Cells[1, ++headingCount].Value = item;

            int row = 1;

            foreach (var item in data)
            {
                ++row;
                for (var col = 1; col <= headingCount; col++)
                {
                    if (item.Names.Contains(headings.ElementAt(col - 1)))
                    {
                        ExcelRange cell = worksheet.Cells[row, col];
                        cell.Value = BsonTypeMapper.MapToDotNetValue(item[headings.ElementAt(col - 1)]);
                    }
                }
            }

            using (var range = worksheet.Cells[1, 1, 1, headingCount])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range.Style.Font.Color.SetColor(Color.White);
            }

            worksheet.View.PageLayoutView = false;

            var dataRange = worksheet.Cells[worksheet.Dimension.Address.ToString()];
            dataRange.AutoFitColumns();

            var pivotTable = worksheet.PivotTables.Add(worksheet.Cells["F3"], dataRange, "Pivotname");
            pivotTable.MultipleFieldFilters = true;
            pivotTable.RowGrandTotals = true;
            pivotTable.ColumGrandTotals = true;
            pivotTable.Compact = true;
            pivotTable.CompactData = true;
            pivotTable.GridDropZones = false;
            pivotTable.Outline = false;
            pivotTable.OutlineData = false;
            pivotTable.ShowError = true;
            pivotTable.ErrorCaption = "[error]";
            pivotTable.ShowHeaders = true;
            pivotTable.UseAutoFormatting = true;
            pivotTable.ApplyWidthHeightFormats = true;
            pivotTable.ShowDrill = true;
            pivotTable.FirstDataCol = 2;
            pivotTable.RowHeaderCaption = "Counts";

            var orgNameField = pivotTable.Fields["Name"];
            pivotTable.RowFields.Add(orgNameField);

            var countField = pivotTable.Fields[3];
            pivotTable.DataFields.Add(countField);

            var monthField = pivotTable.Fields["Month"];
            monthField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;
            pivotTable.ColumnFields.Add(monthField);
        }

        public static void OpenExcel(this FileInfo file, bool delete = true)
        {
            var process = Process.Start(file.FullName);
            if (process != null) process.WaitForExit();
            if (delete) File.Delete(file.FullName);
        }
    }
}
