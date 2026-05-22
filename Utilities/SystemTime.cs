using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Utilities
{
    public static class SystemTime
    {
        private static DateTime? _customDateTime;

        public static DateTime Now
        {
            get
            {
                return _customDateTime ?? DateTime.Now;
            }
        }

        public static void SetCustomTime(DateTime customDateTime)
        {
            _customDateTime = customDateTime;
        }

        public static void Reset()
        {
            _customDateTime = null;
        }
    }

}
