using OfficeOpenXml;
using System.Data;

namespace template_web_api.Common.Utils;

public class ExcelUtils
{
    public static class ExcelReaderUtils
    {
        /// <summary>
        /// อ่านข้อมูล Excel เป็น DataTable
        /// </summary>
        /// <param name="stream">Stream ของไฟล์ Excel</param>
        /// <param name="worksheetName">ชื่อ worksheet (ถ้าไม่ระบุจะใช้ sheet แรก)</param>
        /// <param name="hasHeader">มี header row หรือไม่</param>
        /// <returns>DataTable ที่มีข้อมูล</returns>
        public static DataTable ReadExcelToDataTable(Stream stream, string worksheetName = null, bool hasHeader = true)
        {
            using var package = new ExcelPackage(stream);
            var worksheet = string.IsNullOrEmpty(worksheetName)
                ? package.Workbook.Worksheets[0]
                : package.Workbook.Worksheets[worksheetName];

            var dataTable = new DataTable();
            var startRow = hasHeader ? 2 : 1;
            var headerRow = hasHeader ? 1 : 0;

            // สร้าง columns
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                var columnName = hasHeader
                    ? worksheet.Cells[headerRow, col].Value?.ToString() ?? $"Column{col}"
                    : $"Column{col}";
                dataTable.Columns.Add(columnName);
            }

            // อ่านข้อมูล
            for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
            {
                var dataRow = dataTable.NewRow();
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    dataRow[col - 1] = worksheet.Cells[row, col].Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// อ่านข้อมูล Excel เป็น List of Objects
        /// </summary>
        /// <typeparam name="T">ประเภทของ Object</typeparam>
        /// <param name="stream">Stream ของไฟล์ Excel</param>
        /// <param name="worksheetName">ชื่อ worksheet</param>
        /// <returns>List of Objects</returns>
        public static List<T> ReadExcelToList<T>(Stream stream, string worksheetName = null) where T : new()
        {
            var dataTable = ReadExcelToDataTable(stream, worksheetName);
            var result = new List<T>();
            var properties = typeof(T).GetProperties();

            foreach (DataRow row in dataTable.Rows)
            {
                var item = new T();
                foreach (var prop in properties)
                {
                    if (dataTable.Columns.Contains(prop.Name))
                    {
                        var value = row[prop.Name];
                        if (value != DBNull.Value)
                        {
                            prop.SetValue(item, Convert.ChangeType(value, prop.PropertyType));
                        }
                    }
                }
                result.Add(item);
            }

            return result;
        }
    }

    /// <summary>
    /// ExcelWriterUtils - เขียนข้อมูลลงไฟล์ Excel
    /// </summary>
    public static class ExcelWriterUtils
    {
        /// <summary>
        /// เขียน DataTable ลงไฟล์ Excel
        /// </summary>
        /// <param name="dataTable">DataTable ที่ต้องการเขียน</param>
        /// <param name="worksheetName">ชื่อ worksheet</param>
        /// <returns>MemoryStream ของไฟล์ Excel</returns>
        public static MemoryStream WriteDataTableToExcel(DataTable dataTable, string worksheetName = "Sheet1")
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);

            // เขียน header
            for (int col = 0; col < dataTable.Columns.Count; col++)
            {
                worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
            }

            // เขียนข้อมูล
            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                }
            }

            // จัดรูปแบบ header
            using (var range = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// เขียน List of Objects ลงไฟล์ Excel
        /// </summary>
        /// <typeparam name="T">ประเภทของ Object</typeparam>
        /// <param name="data">List of Objects</param>
        /// <param name="worksheetName">ชื่อ worksheet</param>
        /// <returns>MemoryStream ของไฟล์ Excel</returns>
        public static MemoryStream WriteListToExcel<T>(List<T> data, string worksheetName = "Sheet1")
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);

            var properties = typeof(T).GetProperties();

            // เขียน header
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cells[1, col + 1].Value = properties[col].Name;
            }

            // เขียนข้อมูล
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(data[row]);
                }
            }

            // จัดรูปแบบ header
            using (var range = worksheet.Cells[1, 1, 1, properties.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
