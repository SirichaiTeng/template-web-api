using OriginalExample.Constants;
using System.Data;

namespace OriginalExample.Interfaces.IData;

public interface IDbConnectionFactory
{
    IDbConnection GetDatebaseInstance(DatabaseProvider databaseProvider);
}
