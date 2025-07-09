namespace template_web_api.Common.Constans;

public class Constants
{
    public static class ApiRoutes
    {
        public const string Weather = "api/weather";
        public const string Products = "api/products";
    }

    public static class CacheKeys
    {
        public const string WeatherData = "weather_data_";
        public const string Products = "products_list";
    }

    public static class EmailTemplates
    {
        public const string Welcome = "Welcome to Weather App!";
        public const string WeatherAlert = "Weather Alert!";
    }
}
