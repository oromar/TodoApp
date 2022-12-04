using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TodoApp.Api.Payload;
using TodoApp.Application.Commands;
using TodoApp.Application.Queries;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly IMediator mediator;

        public TodosController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationViewModel<TodoViewModel>>> Get(PaginationPayload payload) =>
            Ok(await mediator.Send(new GetListTodosQuery(payload.Page.Value, payload.Limit.Value)));

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoViewModel>> Get(IdPayload payload) =>
            Ok(await mediator.Send(new GetTodoByIdQuery(payload.Id)));

        [HttpGet("uncomplete")]
        public async Task<ActionResult<PaginationViewModel<TodoViewModel>>> GetUncompleted(PaginationPayload payload) =>
            Ok(await mediator.Send(new GetUncompletedTodosQuery(payload.Page.Value, payload.Limit.Value)));

        [HttpPost]
        public async Task<ActionResult<TodoViewModel>> Post([FromBody] AddTodoPayload payload) =>
            Created(string.Empty, await mediator.Send(new AddTodoCommand(payload.Title, payload.Description, payload.Completed)));

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoViewModel>> Put(Guid id, [FromBody] UpdateTodoDescriptionPayload payload) =>
            Ok(await mediator.Send(new UpdateTodoDescriptionCommand(id, payload.Description)));

        [HttpPut("toggleCompleted/{id}")]
        public async Task<ActionResult<TodoViewModel>> ToggleComplete(IdPayload payload) =>
            Ok(await mediator.Send(new ToggleCompletedCommand(payload.Id)));

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(IdPayload payload)
        {
            await mediator.Send(new DeleteTodoCommand(payload.Id));
            return NoContent();
        }
    }
}
