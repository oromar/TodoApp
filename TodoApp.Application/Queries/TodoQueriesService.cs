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
        private readonly ITodoRepository repository;
        public TodoQueriesService(ITodoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PaginationViewModel<TodoViewModel>> ListAll(int page, int limit)
        {
            var offset = (page - 1) * limit;
            var total = repository.AsQueryable().Count();
            var todos = repository.AsQueryable().OrderBy(a => a.CreationDate).Skip(offset).Take(limit).ToList();
            var items = todos.Select(a => new TodoViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Completed = a.Completed,
                CompletedDate = a.Completed ? a.CompletedDate : (DateTime?)null
            })
            .ToList();
            return new PaginationViewModel<TodoViewModel>(items, total, page, limit);
        }

        public async Task<TodoViewModel> GetById(Guid id)
        {
            Expression<Func<Todo, bool>> predicate = a => a.Id == id;
            var todo = repository.AsQueryable().FirstOrDefault(predicate);
            if (todo != null)
                return new TodoViewModel
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Completed = todo.Completed,
                    CompletedDate = todo.Completed ? todo.CompletedDate : (DateTime?)null
                };
            throw new TodoAppException("Todo not found.");
        }

        public async Task<PaginationViewModel<TodoViewModel>> ListUncomplete(int page, int limit)
        {
            var offset = (page - 1) * limit;
            Expression<Func<Todo, bool>> predicate = a => !a.Completed;
            var total = repository.AsQueryable().Count(predicate);
            var todos = repository.AsQueryable().Where(predicate).OrderBy(a => a.CreationDate).Skip(offset).Take(limit).ToList();
            var items = todos.Select(a => new TodoViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
            })
            .ToList();
            return new PaginationViewModel<TodoViewModel>(items, total, page, limit);
        }

        public async Task<PaginationViewModel<TodoViewModel>> Search(string criteria, int page, int limit)
        {
            var offset = (page - 1) * limit;
            Expression<Func<Todo, bool>> predicate = a => a.Title.ToLower().Contains(criteria.ToLower()) || a.Description.ToLower().Contains(criteria.ToLower());
            var total = repository.AsQueryable().Count(predicate);
            var todos = repository.AsQueryable().OrderBy(a => a.CreationDate).Where(predicate).Skip(offset).Take(limit).ToList();
            var items = todos.Select(a => new TodoViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Completed = a.Completed,
                CompletedDate = a.Completed ? a.CompletedDate : (DateTime?)null
            })
            .ToList();
            return new PaginationViewModel<TodoViewModel>(items, total, page, limit);
        }
    }
}