using Microsoft.Extensions.Configuration;

namespace RestCashflowLibrary.Domain.Service
{
    public interface IService
    {
        IConfiguration Configuration { get; set; }
    }
}
