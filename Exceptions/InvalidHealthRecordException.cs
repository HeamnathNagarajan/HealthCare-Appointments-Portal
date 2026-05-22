using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Exceptions
{
    public class InvalidHealthRecordException : Exception
    {
        public InvalidHealthRecordException(string message)
            : base(message)
        {
        }
    }
}
