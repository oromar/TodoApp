using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Api.Common
{
    public class ModelStateFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
            }
            else
            {
                var firstErrorMessage = context.ModelState.Values.SelectMany(a => a.Errors).FirstOrDefault()?.ErrorMessage;
                throw new TodoApiException(firstErrorMessage);
            }
        }
    }
}