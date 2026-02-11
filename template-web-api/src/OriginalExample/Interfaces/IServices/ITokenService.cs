namespace OriginalExample.Interfaces.IServices;

public interface ITokenService
{
    Task<string> GetTokenAsync();
    Task<string> GetTokenAsync2();
    Task<bool> ValidateTokenAsync(string token);
}
