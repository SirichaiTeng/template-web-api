using System.Text.Json;

namespace OriginalExample.Utils;

public static class FileUtil
{
    public static async Task<T?> ReadJsonFileAsync<T>(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            return await JsonSerializer.DeserializeAsync<T>(fs, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, IncludeFields = true});
        }
    }

    public static async Task<string> ReadTextFileAsync(string filePath)
    {
        using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new StreamReader(fs);
        return await reader.ReadToEndAsync();
    }
}
