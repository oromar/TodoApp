using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Api.Common
{
    public class ModelStateFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var state = context.ModelState;
            if (!state.IsValid) 
                throw new TodoApiException(state.Values.SelectMany(a => a.Errors).FirstOrDefault()?.ErrorMessage);
            await next();
        }
    }
}
