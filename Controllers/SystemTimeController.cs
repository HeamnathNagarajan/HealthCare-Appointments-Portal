using HealthcareApp.Utilities;
using System.Globalization;

namespace HealthcareApp.Controllers
{
    public static class SystemTimeController
    {
        private const string DateFormat = "yyyy-MM-dd";

        public static void ManageSystemTime()
        {
            Console.WriteLine("\n========== System Time Management ==========");
            Console.WriteLine("1. Set custom date");
            Console.WriteLine("2. Reset to current date");

            int input = Validations.ReadIntInRange(
                "Choose option: ",
                "System time option",
                1,
                2
            );

            switch (input)
            {
                case 1:
                    SetCustomDate();
                    break;

                case 2:
                    ResetSystemDate();
                    break;
            }
        }

        private static void SetCustomDate()
        {
            Console.Write($"Enter custom date ({DateFormat}): ");
            string customInput = Console.ReadLine() ?? string.Empty;

            DateOnly customDate;

            while (!DateOnly.TryParseExact(
                       customInput,
                       DateFormat,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out customDate))
            {
                Console.WriteLine($"Invalid date format. Use {DateFormat}.");
                Console.Write($"Enter again ({DateFormat}): ");
                customInput = Console.ReadLine() ?? string.Empty;
            }

            SystemTime.SetCustomTime(customDate);

            Console.WriteLine($"Custom date set: {SystemTime.Now:yyyy-MM-dd}");
        }

        private static void ResetSystemDate()
        {
            SystemTime.Reset();

            Console.WriteLine($"System date reset: {SystemTime.Now:yyyy-MM-dd}");
        }
    }
}