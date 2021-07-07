using System;
using System.ComponentModel.DataAnnotations;
using TodoApp.Api.Validators;

namespace TodoApp.Api.Payload
{
    public class IdPayload
    {
        [Required]
        [IdValidator(ErrorMessage = "Invalid Id.")]
        public Guid Id { get; set; }
    }
}
