using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Exceptions
{
    public class DuplicateHealthRecordException: Exception
    {
        public DuplicateHealthRecordException(string message)
            : base(message)
        {
        }
    }
}

