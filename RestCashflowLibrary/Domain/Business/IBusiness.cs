using Microsoft.Extensions.Configuration;

namespace RestCashflowLibrary.Domain.Business
{
    public interface IBusiness
    {
        IConfiguration Configuration { get; set; }
    }
}
