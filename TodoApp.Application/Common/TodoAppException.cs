using System;

namespace TodoApp.Application.Common
{
    public class TodoAppException : Exception
    {
        public TodoAppException(string message) : base(message)
        {

        }
    }
}
