namespace template_web_api.Models.Response;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public int? StatusCode { get; set; }

    public static ApiResponse<T> Ok(T data) => new ApiResponse<T> { Success = true, Data = data, StatusCode = 200 };
    public static ApiResponse<T> Error(string message, int statusCode = 400) => new ApiResponse<T> { Success = false, Message = message, StatusCode = statusCode };
}
