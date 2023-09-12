using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoApp.Application.Common;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;
using TodoApp.Domain.Data;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Queries
{
    public class TodoQueriesService : ITodoQueriesService
    {
        public const int MAX_ROWS_PER_PAGE = 100;
        private readonly ITodoQuery dbQuery;
        public TodoQueriesService(ITodoQuery dbQuery)
        {
            this.dbQuery = dbQuery;
        }

        public async Task<PaginationViewModel<TodoViewModel>> ListAll(int page, int limit)
        {
            return Search(page, limit);
        }

        public async Task<TodoViewModel> GetById(Guid id)
        {
            Expression<Func<Todo, bool>> predicate = a => a.Id == id;
            var todo = dbQuery.AsQueryable().FirstOrDefault(predicate);
            if (todo != null)
            {
                return new TodoViewModel
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Completed = todo.Completed,
                    CompletedDate = todo.Completed ? todo.CompletedDate : null
                };
            }
            throw new TodoAppException("Todo not found.");
        }

        public async Task<PaginationViewModel<TodoViewModel>> ListUncomplete(int page, int limit)
        {
            Expression<Func<Todo, bool>> predicate = a => !a.Completed;
            return Search(page, limit, predicate);
        }

        public async Task<PaginationViewModel<TodoViewModel>> Search(string criteria, int page, int limit)
        {
            Expression<Func<Todo, bool>> predicate = a => a.Title.ToLower().Contains(criteria.ToLower()) || a.Description.ToLower().Contains(criteria.ToLower());
            return Search(page, limit, predicate);
        }

        private PaginationViewModel<TodoViewModel> Search(int page, int limit, Expression<Func<Todo, bool>> predicate = null)
        {
            var offset = (page - 1) * limit;
            var queryable = dbQuery.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            var total = queryable.Count();
            var todos = queryable.OrderBy(a => a.CreationDate).Skip(offset).Take(limit).ToList();
            var items = todos.Select(a => new TodoViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Completed = a.Completed,
                CompletedDate = a.Completed ? a.CompletedDate : null
            })
            .ToList();
            return new PaginationViewModel<TodoViewModel>(items, total, page, limit);
        }
    }
}