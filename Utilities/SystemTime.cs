using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Utilities
{
    public static class SystemTime
    {
        private static DateOnly? _customDate;

        public static DateOnly Now
        {
            get
            {
                return _customDate ?? DateOnly.FromDateTime(DateTime.Now);
            }
        }

        public static void SetCustomTime(DateOnly customDate)
        {
            _customDate = customDate;
        }

        public static void Reset()
        {
            _customDate = null;
        }
    }

}
