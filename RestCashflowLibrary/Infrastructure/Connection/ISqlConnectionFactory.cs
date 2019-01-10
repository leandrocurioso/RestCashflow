using System.Data;

namespace RestCashflowLibrary.Infrastructure.Connection
{
    public interface ISqlConnectionFactory
    {
        IDbConnection Fabricate();
    }
}
