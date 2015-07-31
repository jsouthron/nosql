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

namespace NoSql
{
    public static class ExcelWriter
    {
        public static FileInfo ToExcelFile(this MongoWorksheetCollection collection)
        {
            string tempPath = Path.GetTempPath() + "Report-" + Guid.NewGuid().ToString() + ".xlsx";

            FileInfo newFile = new FileInfo(tempPath);
            ExcelPackage package = new ExcelPackage(newFile);
            
            foreach (var ws in collection.Worksheets)
            {
                AddWorksheet(ws.Data, ws.Name, ws.Headers, package);
            }

            package.Save();

            return newFile;
        }

        public static FileInfo ToExcelFile(this IEnumerable<BsonDocument> data, string worksheetName, IEnumerable<string> headings)
        {
            string tempPath = Path.GetTempPath() + "Report-" + Guid.NewGuid().ToString() + ".xlsx";
            FileInfo newFile = new FileInfo(tempPath);

            ExcelPackage package = new ExcelPackage(newFile);
            AddWorksheet(data, worksheetName, headings, package);
            package.Save();

            return newFile;
        }

        private static void AddWorksheet(IEnumerable<BsonDocument> data, string worksheetName, IEnumerable<string> headings, ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetName);
            int headingCount = 0;
            foreach (var item in headings)
                worksheet.Cells[1, ++headingCount].Value = item;

            int row = 1;

            foreach (var item in data)
            {
                ++row;
                for (int col = 1; col <= headingCount; col++)
                {
                    if (item.Names.Contains(headings.ElementAt(col - 1)))
                    {
                        ExcelRange cell = worksheet.Cells[row, col];
                        cell.Value = BsonTypeMapper.MapToDotNetValue(item[headings.ElementAt(col - 1)]);
                    }
                }
            }

            using (ExcelRange range = worksheet.Cells[1, 1, 1, headingCount])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range.Style.Font.Color.SetColor(Color.White);
            }

            worksheet.View.PageLayoutView = false;

            ExcelRange dataRange = worksheet.Cells[worksheet.Dimension.Address.ToString()];
            dataRange.AutoFitColumns();

            ExcelPivotTable pivotTable = worksheet.PivotTables.Add(worksheet.Cells["F3"], dataRange, "Pivotname");
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

            ExcelPivotTableField orgNameField = pivotTable.Fields["Name"];
            pivotTable.RowFields.Add(orgNameField);

            ExcelPivotTableField countField = pivotTable.Fields[3];
            pivotTable.DataFields.Add(countField);

            ExcelPivotTableField monthField = pivotTable.Fields["Month"];
            monthField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;
            pivotTable.ColumnFields.Add(monthField);
        }

        public static void OpenExcel(this FileInfo file, bool delete = true)
        {
            Process process = Process.Start(file.FullName);
            process.WaitForExit();
            if (delete) File.Delete(file.FullName);
        }
    }

    public class MongoWorksheet
    {
        public IEnumerable<BsonDocument> Data { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Headers { get; set; }
    }

    public class MongoWorksheetCollection 
    {
        public List<MongoWorksheet> Worksheets { get; set; }

        public MongoWorksheetCollection()
        {
            Worksheets = new List<MongoWorksheet>();
        }

        public MongoWorksheetCollection(params MongoWorksheet[] items)
        {
            Worksheets = new List<MongoWorksheet>();
            Worksheets.AddRange(items);
        }

        public MongoWorksheetCollection Add(MongoWorksheet item)
        {
            Worksheets.Add(item);
            return this;
        }

        public MongoWorksheetCollection Add(params MongoWorksheet[] items)
        {
            Worksheets.AddRange(items);
            return this;
        }
    }
}
