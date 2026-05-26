using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HealthcareApp.Utilities
{
    public static class Validations
    {
        public static string ReadRequiredString(string prompt, string fieldName, int minLength = 1)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input) || input.Trim().Length < minLength)
            {
                Console.WriteLine($"{fieldName} is required and must be at least {minLength} character(s).");
                Console.Write("Enter again: ");
                input = Console.ReadLine();
            }

            return input.Trim();
        }

        public static int ReadPositiveInt(string prompt, string fieldName)
        {
            Console.Write(prompt);
            bool isValid = int.TryParse(Console.ReadLine(), out int value);

            while (!isValid || value <= 0)
            {
                Console.WriteLine($"{fieldName} must be a positive number.");
                Console.Write("Enter again: ");
                isValid = int.TryParse(Console.ReadLine(), out value);
            }

            return value;
        }

        public static int ReadIntInRange(string prompt, string fieldName, int min, int max)
        {
            Console.Write(prompt);
            bool isValid = int.TryParse(Console.ReadLine(), out int value);

            while (!isValid || value < min || value > max)
            {
                Console.WriteLine($"{fieldName} must be between {min} and {max}.");
                Console.Write("Enter again: ");
                isValid = int.TryParse(Console.ReadLine(), out value);
            }

            return value;
        }

        public static decimal ReadDecimalInRange(string prompt, string fieldName, decimal min, decimal max)
        {
            Console.Write(prompt);
            bool isValid = decimal.TryParse(Console.ReadLine(), out decimal value);

            while (!isValid || value < min || value > max)
            {
                Console.WriteLine($"{fieldName} must be between {min} and {max}.");
                Console.Write("Enter again: ");
                isValid = decimal.TryParse(Console.ReadLine(), out value);
            }

            return value;
        }

        public static DateOnly ReadDate(string prompt)
        {
            Console.Write(prompt);
            bool isValid = DateOnly.TryParse(Console.ReadLine(), out DateOnly date);

            while (!isValid)
            {
                Console.WriteLine("Invalid date format.");
                Console.Write("Enter again: ");
                isValid = DateOnly.TryParse(Console.ReadLine(), out date);
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
                age--;

            return age;
        }

        public static string ReadPhoneNumber()
        {
            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(phone) ||
                   phone.Length != 10 ||
                   !phone.All(char.IsDigit))
            {
                Console.WriteLine("Phone number must be exactly 10 digits.");
                Console.Write("Enter again: ");
                phone = Console.ReadLine();
            }

            return phone.Trim();
        }

        public static string ReadEmail()
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            while (string.IsNullOrWhiteSpace(email) ||
                   !Regex.IsMatch(
                        email,
                        pattern,
                        RegexOptions.None,
                        TimeSpan.FromMilliseconds(250)))
            {
                Console.WriteLine("Invalid email format.");
                Console.Write("Enter again: ");
                email = Console.ReadLine();
            }

            return email.Trim();
        }

        public static TEnum ReadEnumChoice<TEnum>(string title) where TEnum : struct, Enum
        {
            Console.WriteLine($"Select {title}:");

            var values = Enum.GetValues<TEnum>();

            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {values[i]}");
            }

            int choice = ReadIntInRange("Choose option: ", title, 1, values.Length);

            return values[choice - 1];
        }
    }
}
