using System.Threading.Tasks;

namespace RestCashflowLibrary.Infrastructure.Queue
{
    public interface IQueue
    {
        Task Publish(object data);
    }
}
