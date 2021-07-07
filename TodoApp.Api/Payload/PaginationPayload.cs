using System;
using System.ComponentModel.DataAnnotations;
using TodoApp.Application.Queries;

namespace TodoApp.Api.Payload
{
    public class PaginationPayload
    {
        [Required(ErrorMessage = "Page number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Page number should be equal or greather 1")]
        public int? Page { get; set; }
        [Required(ErrorMessage = "Limit of rows number is required.")]
        [Range(1, PaginationQueryBase.MAX_LIMIT_OF_ROWS, ErrorMessage = "Limit of rows number should be between 1 and 100.")]
        public int? Limit { get; set; }
    }
}
