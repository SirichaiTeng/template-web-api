using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace template_web_api.Common.Utils;

public class CsvUtils
{
    // อ่าน CSV เป็น List<string[]> (ข้อมูลดิบ)
    public static List<string[]> ReadCsvAsList(string filePath)
    {
        var result = new List<string[]>();
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            while (csv.Read())
            {
                result.Add(csv.GetRecord<dynamic>().ToString().Split(','));
            }
        }
        return result;
    }

    // อ่าน CSV เป็น List<T> (Generic Type)
    public static List<T> ReadCsvAsGeneric<T>(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    // อ่าน CSV เป็น List<T> ด้วย Class Map
    public static List<T> ReadCsvWithMapping<T>(string filePath, ClassMap<T> classMap)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            csv.Context.RegisterClassMap(classMap);
            return csv.GetRecords<T>().ToList();
        }
    }

    // เขียน List<T> ลงไฟล์ CSV
    public static void WriteCsv<T>(string filePath, List<T> records)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }

    // อ่าน CSV เป็น List<Dictionary<string, string>> (คอลัมน์เป็น key-value)
    public static List<Dictionary<string, string>> ReadCsvAsDictionary(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            var headers = csv.HeaderRecord;
            var result = new List<Dictionary<string, string>>();
            while (csv.Read())
            {
                var row = new Dictionary<string, string>();
                for (int i = 0; i < headers.Length; i++)
                {
                    row[headers[i]] = csv.GetField(i);
                }
                result.Add(row);
            }
            return result;
        }
    }

    // อ่าน CSV แบบ Stream สำหรับไฟล์ขนาดใหญ่ (yield return)
    public static IEnumerable<T> ReadCsvAsStream<T>(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            var records = csv.GetRecords<T>();
            foreach (var record in records)
            {
                yield return record;
            }
        }
    }

    // กรองข้อมูล CSV ตามเงื่อนไข (ใช้ Func เดลิเกต)
    public static List<T> FilterCsv<T>(string filePath, Func<T, bool> predicate)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            return csv.GetRecords<T>().Where(predicate).ToList();
        }
    }

    // อ่านเฉพาะคอลัมน์ที่ระบุ
    public static List<string[]> ReadSpecificColumns(string filePath, params string[] columnNames)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            var result = new List<string[]>();
            var headerIndices = new List<int>();
            csv.Read();
            csv.ReadHeader();
            foreach (var column in columnNames)
            {
                if (csv.HeaderRecord.Contains(column))
                {
                    headerIndices.Add(Array.IndexOf(csv.HeaderRecord, column));
                }
            }
            while (csv.Read())
            {
                var row = headerIndices.Select(i => csv.GetField(i)).ToArray();
                result.Add(row);
            }
            return result;
        }
    }

    // อ่าน CSV แบบ Async สำหรับ Web API
    public static async Task<List<T>> ReadCsvAsGenericAsync<T>(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            var records = new List<T>();
            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }
            return records;
        }
    }

    // ตรวจสอบความถูกต้องของ CSV (เช่น ตรวจสอบ header)
    public static bool ValidateCsvHeaders(string filePath, params string[] expectedHeaders)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        }))
        {
            csv.Read();
            csv.ReadHeader();
            return expectedHeaders.All(h => csv.HeaderRecord.Contains(h));
        }
    }
 }
