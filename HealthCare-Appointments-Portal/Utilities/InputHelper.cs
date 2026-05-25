using System.Text.RegularExpressions;
using System.Globalization;

namespace HealthCare_Appointments_Portal.Utilities
{
    public static class InputHelper
    {
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

                Console.WriteLine("Invalid name. Only alphabets and spaces allowed (min 3 chars).");
            }
        }

        public static string ReadNonEmpty(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                var val = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(val))
                    return val;

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

        public static int ReadValidIndex(int max)
        {
            while (true)
            {
                int index = ReadValidInt("Choice: ") - 1;

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
                    return phone;

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

                Console.WriteLine("Invalid date");
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

                if (!string.IsNullOrWhiteSpace(input) && input.Trim().Length >= 3)
                    return input.Trim();

                Console.WriteLine("Enter at least 3 characters.");
            }
        }

        public static string ReadOptionalText(string message)
        {
            Console.Write(message);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
    }
}