using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.CommandHandlers;
using TodoApp.Domain.Data;
using TodoApp.Infra.Context;
using TodoApp.Infra.Repositories;

namespace TodoApp.Infra.Configuration
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
        {
            services.AddDbContext<TodoContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionStringName));
            });
            services.AddMediatR(typeof(TodoCommandHandler).Assembly);

            services.AddScoped<ITodoRepository, TodoRepository>();
        }
    }
}
