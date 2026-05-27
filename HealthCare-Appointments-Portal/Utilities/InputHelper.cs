using System.Globalization;
using System.Text.RegularExpressions;

namespace HealthCare_Appointments_Portal.Utilities
{
    public static class InputHelper
    {
        private const string ChoicePrompt = "Choice: ";

        public static string ReadName(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.All(c => char.IsLetter(c) || c == ' ') &&
                    input.Trim().Length >= 3)
                {
                    return input.Trim();
                }

                Console.WriteLine(
                    "Invalid name. Only alphabets and spaces allowed (min 3 chars).");
            }
        }

        public static string ReadNonEmpty(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                var val = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(val))
                    return val.Trim();

                Console.WriteLine("Cannot be empty");
            }
        }

        public static int ReadValidInt(string msg)
        {
            while (true)
            {
                Console.Write(msg);

                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Invalid number");
            }
        }

        public static decimal ReadValidDecimal(string msg)
        {
            while (true)
            {
                Console.Write(msg);

                if (decimal.TryParse(Console.ReadLine(), out decimal value) &&
                    value >= 0)
                {
                    return value;
                }

                Console.WriteLine("Invalid decimal value");
            }
        }

        public static int ReadValidIndex(int max)
        {
            while (true)
            {
                int index = ReadValidInt(ChoicePrompt) - 1;

                if (index >= 0 && index < max)
                    return index;

                Console.WriteLine("Invalid selection");
            }
        }

        public static string ReadPhone()
        {
            while (true)
            {
                Console.Write("Phone (10 digits): ");
                var phone = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(phone) &&
                    phone.Length == 10 &&
                    phone.All(char.IsDigit))
                {
                    return phone;
                }

                Console.WriteLine("Invalid phone number");
            }
        }

        public static string ReadEmail()
        {
            while (true)
            {
                Console.Write("Email: ");
                var email = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(email) &&
                    Regex.IsMatch(
                        email,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                        RegexOptions.None,
                        TimeSpan.FromMilliseconds(500)))
                {
                    return email;
                }

                Console.WriteLine("Invalid email format");
            }
        }

        public static DateOnly ReadDOB()
        {
            while (true)
            {
                Console.Write("DOB (yyyy-MM-dd): ");

                if (DateOnly.TryParseExact(
                        Console.ReadLine(),
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dob) &&
                    dob <= DateOnly.FromDateTime(DateTime.Now))
                {
                    return dob;
                }

                Console.WriteLine("Invalid DOB");
            }
        }

        public static DateOnly ReadFutureDate()
        {
            while (true)
            {
                Console.Write("Enter Date (yyyy-MM-dd): ");

                if (DateOnly.TryParseExact(
                        Console.ReadLine(),
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var date) &&
                    date >= DateOnly.FromDateTime(DateTime.Now))
                {
                    return date;
                }

                Console.WriteLine("Invalid future date");
            }
        }

        public static TimeOnly ReadValidTime(DateOnly date)
        {
            while (true)
            {
                Console.Write("Enter Time (HH:mm): ");

                if (TimeOnly.TryParseExact(
                        Console.ReadLine(), 
                        "HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var time))
                {
                    // Check only if appointment date is today
                    if (date == DateOnly.FromDateTime(DateTime.Now))
                    {
                        var currentTime = TimeOnly.FromDateTime(DateTime.Now);

                        if (time <= currentTime)
                        {
                            Console.WriteLine("Time cannot be in the past");
                            continue;
                        }
                    }

                    return time;
                }

                Console.WriteLine("Invalid time format");
            }
        }
    
        public static string ReadCleanText(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.Trim().Length >= 3)
                {
                    return input.Trim();
                }

                Console.WriteLine("Enter at least 3 characters.");
            }
        }

        public static string ReadOptionalText(string message)
        {
            Console.Write(message);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public static bool ReadYesNo(string message)
        {
            while (true)
            {
                Console.Write($"{message} (Y/N): ");
                var input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "Y")
                    return true;

                if (input == "N")
                    return false;

                Console.WriteLine("Enter Y or N only.");
            }
        }

        public static int ReadPositiveInt(string msg)
        {
            while (true)
            {
                Console.Write(msg);

                if (int.TryParse(Console.ReadLine(), out int value) && value >= 0)
                    return value;

                Console.WriteLine("Enter a valid positive number");
            }
        }


        public static int ReadExperience(string msg)
        {
            while (true)
            {
                Console.Write(msg);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    if (value >= 0 && value <= 50)
                        return value;

                    Console.WriteLine(
                        "Experience must be between 0 and 50 years.");
                }
                else
                {
                    Console.WriteLine("Invalid number");
                }
            }
        }


        public static decimal ReadConsultationFee(string msg)
        {
            while (true)
            {
                Console.Write(msg);

                if (decimal.TryParse(Console.ReadLine(), out decimal fee))
                {
                    if (fee >= 100 && fee <= 10000)
                        return fee;

                    Console.WriteLine(
                        "Consultation fee must be between 100 and 10000.");
                }
                else
                {
                    Console.WriteLine("Invalid fee");
                }
            }
        }

    }
}
