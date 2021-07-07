using System.ComponentModel.DataAnnotations;
using TodoApp.Domain.Entities;

namespace TodoApp.Api.Payload
{
    public class AddTodoPayload
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        [MaxLength(Todo.MAX_LENGTH_TITLE, ErrorMessage = "Title should have max 30 characters.")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required.")]
        [MaxLength(Todo.MAX_LENGTH_DESCRIPTION, ErrorMessage = "Description should have max 100 characters.")]
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
