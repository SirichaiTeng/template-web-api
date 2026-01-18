using ClosedXML.Excel;

namespace OriginalExample.Utils;

public static class ExcelHelper
{
    public static byte[] ExportToExcel<T>(IEnumerable<T> data, string[] headerMap)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.AddWorksheet("DataExport");

        var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        // เพิ่ม Header
        for(int i = 0; i < headerMap.Length; i++)
        {
            ws.Cell(1, i+1).Value = headerMap[i];
            ws.Cell(1, i + 1).Style.Font.Bold = true;
        }

        // เพิ่มข้อมูล
        int row = 2;
        foreach(var item in data)
        {
            for(int col = 0; col < headerMap.Length; col++)
            {
                var prop = props.FirstOrDefault(x =>
                string.Equals(x.Name, headerMap[col], StringComparison.OrdinalIgnoreCase));
                if(prop == null)
                    continue;

                object value = prop.GetValue(item);

                ws.Cell(row, col + 1).SetValue(value?.ToString() ?? string.Empty); 
            }
            row++;
        }
        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
