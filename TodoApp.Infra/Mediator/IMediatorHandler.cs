using MediatR;

namespace TodoApp.Infra.Mediator
{
    public interface IMediatorHandler
    {
        Task Send(IRequest request);
        Task<T> Send(IRequest<T> request);
    }
}