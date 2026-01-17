namespace OriginalExample.Models.Entity;

public class UserInfo
{
    public string? UserName { get; set; }
    public string UserEmail { get; set; }= string.Empty;
    public string UserPhone { get; set; } = string.Empty;
    public string? UserPassword { get; set; }
    public string? PasswordHash { get; set; }
}
