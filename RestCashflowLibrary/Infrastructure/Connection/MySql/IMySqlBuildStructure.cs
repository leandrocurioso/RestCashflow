using System;
using System.Threading.Tasks;

namespace RestCashflowLibrary.Infrastructure.Connection.MySql
{
    public interface IMySqlBuildStructure
    {
        Task CreateTables();
    }
}
