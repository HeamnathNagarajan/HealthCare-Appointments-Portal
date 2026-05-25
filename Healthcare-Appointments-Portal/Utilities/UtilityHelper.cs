using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HealthCare_Appointment_Portal.Utilities
{
    public static class UtilityHelper
    {
        // Read Console Input
        public static string ReadInput(
            string message)
        {
            Console.Write(message);

            return Console.ReadLine()
                ?? string.Empty;
        }

        // Read Validated Date
        public static DateOnly ReadValidDate<T>(
            string message,
            string propertyName,
            T model)
        {
            while (true)
            {
                string input =
                    ReadInput(message);

                bool isValidDate =
                    DateOnly.TryParseExact(
                        input,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateOnly date);

                if (!isValidDate)
                {
                    Console.WriteLine(
                        "Invalid Date Format. Please enter in yyyy-MM-dd format.");

                    continue;
                }

                var property =
                    typeof(T)
                    .GetProperty(propertyName);

                property?.SetValue(
                    model,
                    date);

                ValidationContext context =
                    new(model!)
                    {
                        MemberName =
                            propertyName
                    };

                List<ValidationResult> results =
                    [];

                bool isValid =
                    Validator.TryValidateProperty(
                        date,
                        context,
                        results);

                if (isValid)
                {
                    return date;
                }

                Console.WriteLine(
                    results[0]
                    .ErrorMessage);
            }
        }

        // Read Validated Time
        public static TimeOnly ReadValidTime(
            string message)
        {
            while (true)
            {
                string input =
                    ReadInput(message);

                bool isValidTime =
                    TimeOnly.TryParseExact(
                        input,
                        "HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out TimeOnly time);

                if (isValidTime)
                {
                    return time;
                }

                Console.WriteLine(
                    "Invalid Time Format. Please enter in HH:mm format.");
            }
        }

        // Read Validated Property
        public static string ReadValidatedProperty<T>(
            string message,
            string propertyName,
            T model)
        {
            while (true)
            {
                Console.Write(message);

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                var property =
                    typeof(T)
                    .GetProperty(propertyName);

                property?.SetValue(
                    model,
                    input);

                ValidationContext context =
                    new(model!)
                    {
                        MemberName =
                            propertyName
                    };

                List<ValidationResult> results =
                    [];

                bool isValid =
                    Validator.TryValidateProperty(
                        input,
                        context,
                        results);

                if (isValid)
                {
                    return input;
                }

                Console.WriteLine(
                    results[0]
                    .ErrorMessage);
            }
        }

        // Validate Complete Model
        public static bool ValidateModel<T>(
            T model)
        {
            ValidationContext context =
                new(model!);

            List<ValidationResult> results =
                [];

            bool isValid =
                Validator.TryValidateObject(
                    model!,
                    context,
                    results,
                    true);

            if (!isValid)
            {
                foreach (ValidationResult error
                    in results)
                {
                    Console.WriteLine(
                        error.ErrorMessage);
                }
            }

            return isValid;
        }

        // Read Enum Value
        public static T ReadValidEnum<T>(
            string message)
            where T : struct, Enum
        {
            T[] values =
                Enum.GetValues<T>();

            Console.WriteLine(
                $"\nAvailable {typeof(T).Name}s:");

            for (int i = 0;
                i < values.Length;
                i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {values[i]}");
            }

            while (true)
            {
                Console.Write(message);

                bool isValid =
                    int.TryParse(
                        Console.ReadLine(),
                        out int choice);

                if (isValid &&
                    choice >= 1 &&
                    choice <= values.Length)
                {
                    return values[
                        choice - 1];
                }

                Console.WriteLine(
                    "Invalid Choice.");
            }
        }

        // Read Valid Integer
        public static int ReadValidInt(
            string message)
        {
            while (true)
            {
                string input =
                    ReadInput(message);

                bool isValid =
                    int.TryParse(
                        input,
                        out int value);

                if (isValid)
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid Number Format.");
            }
        }

        // Read Valid Decimal
        public static decimal ReadValidDecimal(
            string message)
        {
            while (true)
            {
                string input =
                    ReadInput(message);

                bool isValid =
                    decimal.TryParse(
                        input,
                        out decimal value);

                if (isValid)
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid Decimal Format.");
            }
        }

        // OPTIONAL UPDATE HELPER METHODS
        // Press Enter = keep existing value
        // Enter new value = update value

        // Read Optional String
        public static string ReadOptionalString(
            string label,
            string? currentValue)
        {
            currentValue ??= string.Empty;

            Console.Write(
                $"{label} Current [{currentValue}] - Enter new value or press Enter to keep: ");

            string input =
                Console.ReadLine()
                ?? string.Empty;

            return string.IsNullOrWhiteSpace(input)
                ? currentValue
                : input.Trim();
        }

        // Read Optional Validated String Property
        public static string ReadOptionalValidatedProperty<T>(
            string label,
            string propertyName,
            T model,
            string? currentValue)
        {
            currentValue ??= string.Empty;

            while (true)
            {
                Console.Write(
                    $"{label} Current [{currentValue}] - Enter new value or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                var property =
                    typeof(T)
                    .GetProperty(propertyName);

                object? oldValue =
                    property?.GetValue(model);

                property?.SetValue(
                    model,
                    input);

                ValidationContext context =
                    new(model!)
                    {
                        MemberName =
                            propertyName
                    };

                List<ValidationResult> results =
                    [];

                bool isValid =
                    Validator.TryValidateProperty(
                        input,
                        context,
                        results);

                if (isValid)
                {
                    return input.Trim();
                }

                property?.SetValue(
                    model,
                    oldValue);

                Console.WriteLine(
                    results[0]
                    .ErrorMessage);
            }
        }

        // Read Optional Integer
        public static int ReadOptionalInt(
            string label,
            int currentValue)
        {
            while (true)
            {
                Console.Write(
                    $"{label} Current [{currentValue}] - Enter new value or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                bool isValid =
                    int.TryParse(
                        input,
                        out int value);

                if (isValid)
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid Number Format.");
            }
        }

        // Read Optional Decimal
        public static decimal ReadOptionalDecimal(
            string label,
            decimal currentValue)
        {
            while (true)
            {
                Console.Write(
                    $"{label} Current [{currentValue}] - Enter new value or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                bool isValid =
                    decimal.TryParse(
                        input,
                        out decimal value);

                if (isValid)
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid Decimal Format.");
            }
        }


        // Read Optional Date
        public static DateOnly ReadOptionalDate(
            string label,
            DateOnly currentValue)
        {
            while (true)
            {
                Console.Write(
                    $"{label} Current [{currentValue:yyyy-MM-dd}] - Enter new date yyyy-MM-dd or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                bool isValid =
                    DateOnly.TryParseExact(
                        input,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateOnly value);

                if (isValid)
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid Date Format. Please enter in yyyy-MM-dd format.");
            }
        }

        // Read Optional Validated Date
        public static DateOnly ReadOptionalDate<T>(
            string label,
            string propertyName,
            T model,
            DateOnly currentValue)
        {
            while (true)
            {
                Console.Write(
                    $"{label} Current [{currentValue:yyyy-MM-dd}] - Enter new date yyyy-MM-dd or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                bool isValidDate =
                    DateOnly.TryParseExact(
                        input,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateOnly date);

                if (!isValidDate)
                {
                    Console.WriteLine(
                        "Invalid Date Format. Please enter in yyyy-MM-dd format.");

                    continue;
                }

                var property =
                    typeof(T)
                    .GetProperty(propertyName);

                object? oldValue =
                    property?.GetValue(model);

                property?.SetValue(
                    model,
                    date);

                ValidationContext context =
                    new(model!)
                    {
                        MemberName =
                            propertyName
                    };

                List<ValidationResult> results =
                    [];

                bool isValid =
                    Validator.TryValidateProperty(
                        date,
                        context,
                        results);

                if (isValid)
                {
                    return date;
                }

                property?.SetValue(
                    model,
                    oldValue);

                Console.WriteLine(
                    results[0]
                    .ErrorMessage);
            }
        }

        // Read Optional Time
        public static TimeOnly ReadOptionalTime(
            string label,
            TimeOnly currentValue)
        {
            while (true)
            {
                Console.Write(
                    $"{label} Current [{currentValue:HH:mm}] - Enter new time HH:mm or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                bool isValid =
                    TimeOnly.TryParseExact(
                        input,
                        "HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out TimeOnly value);

                if (isValid)
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid Time Format. Please enter in HH:mm format.");
            }
        }

        // Read Optional Boolean
        public static bool ReadOptionalBool(
            string label,
            bool currentValue)
        {
            while (true)
            {
                Console.Write(
                    $"{label} Current [{currentValue}] - Enter true/false or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                bool isValid =
                    bool.TryParse(
                        input,
                        out bool value);

                if (isValid)
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid Boolean Format. Please enter true or false.");
            }
        }

        // Read Optional Enum
        public static T ReadOptionalEnum<T>(
            string label,
            T currentValue)
            where T : struct, Enum
        {
            T[] values =
                Enum.GetValues<T>();

            Console.WriteLine(
                $"\n{label}");

            Console.WriteLine(
                $"Current Value: {currentValue}");

            Console.WriteLine(
                $"Available {typeof(T).Name}s:");

            for (int i = 0;
                i < values.Length;
                i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {values[i]}");
            }

            while (true)
            {
                Console.Write(
                    "Enter new choice or press Enter to keep: ");

                string input =
                    Console.ReadLine()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentValue;
                }

                bool isValid =
                    int.TryParse(
                        input,
                        out int choice);

                if (isValid &&
                    choice >= 1 &&
                    choice <= values.Length)
                {
                    return values[
                        choice - 1];
                }

                Console.WriteLine(
                    "Invalid Choice.");
            }
        }
    }
}