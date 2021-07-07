using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Validators
{
    public class IdValidator : ValidationAttribute
    {
        public IdValidator()
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            if (!(value is Guid))
                return false;
            if (value is Guid id && Guid.Empty == id)
                return false;
            return true;
        }
    }
}
