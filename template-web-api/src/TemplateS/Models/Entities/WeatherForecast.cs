namespace template_web_api.Models.Entities;

public class WeatherForecast
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
    public string City { get; set; }
    public string? Description { get; set; }
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
