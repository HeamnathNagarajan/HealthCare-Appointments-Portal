using System.Globalization;
using System.Net.Mail;

namespace HealthcareApp.Utilities
{
    public static class Validations
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string RetryPrompt = "Enter again: ";

        public static string ReadRequiredString(string prompt, string fieldName, int minLength = 1)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            while (string.IsNullOrWhiteSpace(input) || input.Trim().Length < minLength)
            {
                Console.WriteLine($"{fieldName} is required and must be at least {minLength} character(s).");
                Console.Write(RetryPrompt);
                input = Console.ReadLine() ?? string.Empty;
            }

            return input.Trim();
        }

        public static int ReadPositiveInt(string prompt, string fieldName)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            bool isValid = int.TryParse(
                input,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int value
            );

            while (!isValid || value <= 0)
            {
                Console.WriteLine($"{fieldName} must be a positive number.");
                Console.Write(RetryPrompt);

                input = Console.ReadLine() ?? string.Empty;

                isValid = int.TryParse(
                    input,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out value
                );
            }

            return value;
        }

        public static int ReadIntInRange(string prompt, string fieldName, int min, int max)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            bool isValid = int.TryParse(
                input,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int value
            );

            while (!isValid || value < min || value > max)
            {
                Console.WriteLine($"{fieldName} must be between {min} and {max}.");
                Console.Write(RetryPrompt);

                input = Console.ReadLine() ?? string.Empty;

                isValid = int.TryParse(
                    input,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out value
                );
            }

            return value;
        }

        public static decimal ReadDecimalInRange(string prompt, string fieldName, decimal min, decimal max)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            bool isValid = decimal.TryParse(
                input,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal value
            );

            while (!isValid || value < min || value > max)
            {
                Console.WriteLine($"{fieldName} must be between {min} and {max}.");
                Console.Write(RetryPrompt);

                input = Console.ReadLine() ?? string.Empty;

                isValid = decimal.TryParse(
                    input,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out value
                );
            }

            return value;
        }

        public static DateOnly ReadDate(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            bool isValid = DateOnly.TryParseExact(
                input,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly date
            );

            while (!isValid)
            {
                Console.WriteLine($"Invalid date format. Use {DateFormat}.");
                Console.Write(RetryPrompt);

                input = Console.ReadLine() ?? string.Empty;

                isValid = DateOnly.TryParseExact(
                    input,
                    DateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out date
                );
            }

            return date;
        }

        public static DateOnly ReadDateOfBirth()
        {
            DateOnly dob = ReadDate("Enter date of birth (yyyy-MM-dd): ");

            while (dob >= SystemTime.Now)
            {
                Console.WriteLine("Date of birth must be before the current system date.");
                dob = ReadDate("Enter date of birth again (yyyy-MM-dd): ");
            }

            int age = CalculateAge(dob);

            while (age > 120)
            {
                Console.WriteLine("Age cannot be greater than 120 years.");
                dob = ReadDate("Enter date of birth again (yyyy-MM-dd): ");
                age = CalculateAge(dob);
            }

            return dob;
        }

        public static int CalculateAge(DateOnly dateOfBirth)
        {
            DateOnly currentDate = SystemTime.Now;

            int age = currentDate.Year - dateOfBirth.Year;

            if (dateOfBirth > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public static string ReadPhoneNumber()
        {
            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine() ?? string.Empty;

            while (string.IsNullOrWhiteSpace(phone) ||
                   phone.Length != 10 ||
                   !phone.All(char.IsDigit))
            {
                Console.WriteLine("Phone number must be exactly 10 digits.");
                Console.Write(RetryPrompt);
                phone = Console.ReadLine() ?? string.Empty;
            }

            return phone.Trim();
        }

        public static string ReadEmail()
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine() ?? string.Empty;

            while (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email format.");
                Console.Write(RetryPrompt);
                email = Console.ReadLine() ?? string.Empty;
            }

            return email.Trim();
        }

        public static TEnum ReadEnumChoice<TEnum>(string title) where TEnum : struct, Enum
        {
            Console.WriteLine($"Select {title}:");

            TEnum[] values = Enum.GetValues<TEnum>();

            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {values[i]}");
            }

            int choice = ReadIntInRange("Choose option: ", title, 1, values.Length);

            return values[choice - 1];
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > 254)
            {
                return false;
            }

            try
            {
                MailAddress mailAddress = new MailAddress(email);

                return mailAddress.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}