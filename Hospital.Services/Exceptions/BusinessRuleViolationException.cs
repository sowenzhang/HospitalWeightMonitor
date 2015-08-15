using System;

namespace Hospital.Services.Exceptions
{
    public class BusinessRuleViolationException : Exception
    {
        public BusinessRuleViolationException(string incorrectTaskStatus)
            : base(incorrectTaskStatus)
        {
        }
    }
}