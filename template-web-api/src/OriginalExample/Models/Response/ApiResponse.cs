namespace OriginalExample.Models.Response;

public class ApiResponse<T>
{
    public string? Code { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string ResponseTime { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

    public ApiResponse() { }
    public ApiResponse(string code, string? message, T? data, bool success)
    {
        Code = code;
        Message = message;
        Data = data; 
        Success = success;
        ResponseTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
