using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using template_web_api_M.Application.DTOs;

namespace template_web_api_M.Application.Interfaces;
public interface IUserService
{
    Task<UserResponseDto> GetByIdAsync(int id);
    Task<UserResponseDto> GetByUsernameAsync(string username);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<IEnumerable<UserResponseDto>> GetActiveUsersAsync();
    Task<UserResponseDto> CreateAsync(CreateUserDto createUserDto);
    Task<UserResponseDto> UpdateAsync(int id, UpdateUserDto updateUserDto);
    Task<bool> DeleteAsync(int id);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    Task<bool> ActivateUserAsync(int id);
    Task<bool> DeactivateUserAsync(int id);
}
