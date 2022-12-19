using MediatR;
using System.Threading.Tasks;

namespace TodoApp.Infra.Mediator
{
    public interface IMediatorHandler
    {
        Task Send(IRequest request);
        Task<T> Send<T>(IRequest<T> request);
    }
}