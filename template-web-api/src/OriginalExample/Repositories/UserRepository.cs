using OriginalExample.Common.Constants;
using OriginalExample.Interfaces.IData;
using OriginalExample.Interfaces.IRepositories;
using OriginalExample.Models.Entity;

namespace OriginalExample.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IDapperWarpper _dapperWarpper;
    public UserRepository(IDapperWarpper dapperWarpper, IDbConnectionFactory dbConnectionFactory)
    {
        _connectionFactory = dbConnectionFactory;
        _dapperWarpper = dapperWarpper;
    }
    // GetAll
    public async Task<IEnumerable<UserInfo>> GetUsersAsync()
    {
        var connection = _connectionFactory.GetDatabaseInstance(DatabaseProvider.MicrosoftSql);
        var sql =  "";
        return await _dapperWarpper.QueryAsync<UserInfo>(connection,sql, null); 
    }

    // GetById
    public async Task<UserInfo?> GetUserByIdAsync(int id)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.MicrosoftSql);

        var sql = @"SELECT Id, Username, Email, CreatedAt, IsActive
                FROM Users
                WHERE Id = @Id";

        return await _dapperWarpper
            .QueryFirstOrDefaultAsync<UserInfo>(connection, sql, new { Id = id });
    }

    // Insert แล้ว Return Id
    public async Task<int> CreateUserAsync(string username, string email)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.MicrosoftSql);

        var sql = @"
        INSERT INTO Users (Username, Email, CreatedAt, IsActive)
        VALUES (@Username, @Email, GETDATE(), 1);

        SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = await _dapperWarpper
            .QueryFirstOrDefaultAsync<int>(connection, sql, new
            {
                Username = username,
                Email = email
            });

        return id;
    }

    // Update
    public async Task<bool> UpdateUserAsync(int id, string email, bool isActive)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.MicrosoftSql);

        var sql = @"UPDATE Users
                SET Email = @Email,
                    IsActive = @IsActive
                WHERE Id = @Id";

        var affected = await _dapperWarpper
            .ExecuteAsync(connection, sql, new
            {
                Id = id,
                Email = email,
                IsActive = isActive
            });

        return affected > 0;
    }

    // Delete
    public async Task<bool> DeleteUserAsync(int id)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.MicrosoftSql);

        var sql = @"DELETE FROM Users WHERE Id = @Id";

        var affected = await _dapperWarpper
            .ExecuteAsync(connection, sql, new { Id = id });

        return affected > 0;
    }

    // Exists Check
    public async Task<bool> IsEmailExistsAsync(string email)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.MicrosoftSql);

        var sql = @"SELECT 1 FROM Users WHERE Email = @Email";

        var result = await _dapperWarpper
            .QueryFirstOrDefaultAsync<int?>(connection, sql, new { Email = email });

        return result.HasValue;
    }

    // Pagination Example
    public async Task<IEnumerable<UserInfo>> GetUsersPagedAsync(int page, int pageSize)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.MicrosoftSql);

        var sql = @"
        SELECT Id, Username, Email, CreatedAt, IsActive
        FROM Users
        ORDER BY Id
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

        return await _dapperWarpper.QueryAsync<UserInfo>(connection, sql, new
        {
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
    }

    // Return Id (PostgreSQL)
    public async Task<int> CreateUserPostgresqlAsync(string username, string email)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.PostgreSql);

        var sql = @"
        INSERT INTO users (username, email, created_at, is_active)
        VALUES (@Username, @Email, NOW(), true)
        RETURNING id;";

        var id = await _dapperWarpper
            .QueryFirstOrDefaultAsync<int>(connection, sql, new
            {
                Username = username,
                Email = email
            });

        return id;
    }

    // Exists Check(PostgreSQL)
    public async Task<bool> IsEmailExistsPostgresqlAsync(string email)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.PostgreSql);

        var sql = @"SELECT EXISTS(
                    SELECT 1
                    FROM users
                    WHERE email = @Email
                );";

        return await _dapperWarpper
            .QueryFirstOrDefaultAsync<bool>(connection, sql, new { Email = email });
    }

    // Pagination(PostgreSQL)
    public async Task<IEnumerable<UserInfo>> GetUsersPagedPostgresqlAsync(int page, int pageSize)
    {
        using var connection = _connectionFactory
            .GetDatabaseInstance(DatabaseProvider.PostgreSql);

        var sql = @"
        SELECT id, username, email, created_at, is_active
        FROM users
        ORDER BY id
        LIMIT @PageSize
        OFFSET @Offset;";

        return await _dapperWarpper.QueryAsync<UserInfo>(connection, sql, new
        {
            PageSize = pageSize,
            Offset = (page - 1) * pageSize
        });
    }
}
