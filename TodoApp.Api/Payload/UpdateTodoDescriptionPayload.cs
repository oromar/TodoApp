using System.ComponentModel.DataAnnotations;
using TodoApp.Domain.Entities;

namespace TodoApp.Api.Payload
{
    public class UpdateTodoDescriptionPayload
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required.")]
        [MaxLength(Todo.MAX_LENGTH_DESCRIPTION, ErrorMessage = "Title should have max 100 characters.")]
        public string Description { get; set; }
    }
}
