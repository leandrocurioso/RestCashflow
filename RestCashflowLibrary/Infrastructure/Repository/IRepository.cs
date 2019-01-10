using Microsoft.Extensions.Configuration;

namespace RestCashflowLibrary.Infrastructure.Repository
{
    public interface IRepository
    {
        IConfiguration Configuration { get; set; }
    }
}
