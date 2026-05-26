using HealthcareApp.Utilities;

namespace HealthcareApp.Controllers
{
    public class SystemTimeController
    {
        public void ManageSystemTime()
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
                    Console.Write("Enter custom date (yyyy-MM-dd): ");
                    string? customInput = Console.ReadLine();

                    while (!DateOnly.TryParse(customInput, out DateOnly _))
                    {
                        Console.WriteLine("Invalid date format.");
                        Console.Write("Enter again (yyyy-MM-dd): ");
                        customInput = Console.ReadLine();
                    }

                    DateOnly customDate = DateOnly.Parse(customInput);
                    SystemTime.SetCustomTime(customDate);

                    Console.WriteLine($"Custom date set: {SystemTime.Now:yyyy-MM-dd}");
                    break;

                case 2:
                    SystemTime.Reset();
                    Console.WriteLine($"System date reset: {SystemTime.Now:yyyy-MM-dd}");
                    break;
            }
        }
    }
}