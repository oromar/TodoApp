using MediatR;
using System.Threading.Tasks;

namespace TodoApp.Infra.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {

        private IMediator mediator;
        public MediatorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Send(IRequest request)
        {
            await mediator.Send(request);
        }

        public async Task<T> Send<T>(IRequest<T> request)
        {
            return await mediator.Send(request);
        }
    }
}