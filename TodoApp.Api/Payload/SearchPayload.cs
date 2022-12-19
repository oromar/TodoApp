using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Payload
{
    public class SearchPayload : PaginationPayload
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Criteria is Required to Search Todos.")]
        public string Criteria { get; set; }
    }
}
