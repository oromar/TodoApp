using System;

namespace TodoApp.Api.Common
{
    public class TodoApiException : Exception
    {
        public TodoApiException(string message) : base(message)
        {

        }
    }
}
