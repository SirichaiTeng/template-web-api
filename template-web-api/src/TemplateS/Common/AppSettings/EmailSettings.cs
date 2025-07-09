namespace template_web_api.Common.AppSettings;

public class EmailSettings
{
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? FromEmail { get; set; }
    public string? FromName { get; set; }
}
