using OriginalExample.Common.Constants;
using System.Data;

namespace OriginalExample.Interfaces.IData;

public interface IDbConnectionFactory
{
    IDbConnection GetDatabaseInstance(DatabaseProvider databaseProvider);
}
