using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Exceptions
{
    public class NoHealthRecordsFoundException: Exception
    {
        public NoHealthRecordsFoundException(string message)
            : base(message)
        {
        }
    }
}
