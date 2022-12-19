using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TodoApp.Api.Payload;
using TodoApp.Application.Commands;
using TodoApp.Application.Queries;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;
using TodoApp.Infra.Mediator;

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly IMediatorHandler handler;
        private readonly ITodoQueriesService queries;

        public TodosController(IMediatorHandler handler, ITodoQueriesService queries)
        {
            this.handler = handler;
            this.queries = queries;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationViewModel<TodoViewModel>>> Get(PaginationPayload payload) =>
            Ok(await queries.ListAll(payload.Page, payload.Limit));

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoViewModel>> Get(IdPayload payload) =>
            Ok(await queries.GetById(payload.Id));

        [HttpGet("uncomplete")]
        public async Task<ActionResult<PaginationViewModel<TodoViewModel>>> GetUncompleted(PaginationPayload payload) =>
            Ok(await queries.ListUncomplete(payload.Page, payload.Limit));

        [HttpPost]
        public async Task<ActionResult<TodoViewModel>> Post([FromBody] AddTodoPayload payload) =>
            Created(string.Empty, await handler.Send(new AddTodoCommand(payload.Title, payload.Description, payload.Completed)));

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoViewModel>> Put(Guid id, [FromBody] UpdateTodoDescriptionPayload payload) =>
            Ok(await handler.Send(new UpdateTodoDescriptionCommand(id, payload.Description)));

        [HttpPut("toggleCompleted/{id}")]
        public async Task<ActionResult<TodoViewModel>> ToggleComplete(IdPayload payload) =>
            Ok(await handler.Send(new ToggleCompletedCommand(payload.Id)));

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(IdPayload payload)
        {
            await handler.Send(new DeleteTodoCommand(payload.Id));
            return NoContent();
        }
    }
}
