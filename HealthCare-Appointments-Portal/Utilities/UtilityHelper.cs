using HealthCare_Appointments_Portal.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HealthCare_Appointments_Portal.Utilities
{
    [ExcludeFromCodeCoverage]
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
        public static void DisplayPatientTable(
            List<Patient> patients)
        {
            if (patients.Count == 0)
            {
                Console.WriteLine(
                    "No Patients Found.");

                return;
            }

            int idWidth =
                Math.Max(5,
                patients.Max(p =>
                    p.PatientId.ToString().Length));

            int nameWidth =
                Math.Max(20,
                patients.Max(p =>
                    p.FullName.Length));

            int ageWidth = 5;

            int phoneWidth =
                Math.Max(12,
                patients.Max(p =>
                    p.PhoneNumber.Length));

            int emailWidth =
                Math.Max(25,
                patients.Max(p =>
                    p.Email.Length));

            string border =
                "+" + new string('-', idWidth + 2) +
                "+" + new string('-', nameWidth + 2) +
                "+" + new string('-', ageWidth + 2) +
                "+" + new string('-', phoneWidth + 2) +
                "+" + new string('-', emailWidth + 2) +
                "+";

            Console.WriteLine(border);

            Console.WriteLine(
                "| " + "ID".PadRight(idWidth) +
                " | " + "Name".PadRight(nameWidth) +
                " | " + "Age".PadRight(ageWidth) +
                " | " + "Phone".PadRight(phoneWidth) +
                " | " + "Email".PadRight(emailWidth) +
                " |");

            Console.WriteLine(border);

            foreach (Patient patient in patients)
            {
                int age =
                    DateTime.Now.Year -
                    patient.DateOfBirth.Year;

                Console.WriteLine(
                "| " + patient.PatientId.ToString().PadRight(idWidth) +
                " | " + patient.FullName.PadRight(nameWidth) +
                " | " + age.ToString().PadRight(ageWidth) +
                " | " + patient.PhoneNumber.PadRight(phoneWidth) +
                " | " + patient.Email.PadRight(emailWidth) +
                " |");

                Console.WriteLine(border);
            }
        }

        public static void DisplayDoctorTable(
    List<Doctor> doctors)
        {
            if (doctors.Count == 0)
            {
                Console.WriteLine(
                    "No Doctors Found.");

                return;
            }

            int idWidth = 5;
            int nameWidth =
                Math.Max(
                    20,
                    doctors.Max(d =>
                        d.FullName.Length));

            int specialisationWidth =
                Math.Max(
                    15,
                    doctors.Max(d =>
                        d.Specialisation
                        .ToString()
                        .Length));

            int experienceWidth = 10;
            int feeWidth = 10;
            int statusWidth = 12;

            string border =
                "+" + new string('-', idWidth + 2) +
                "+" + new string('-', nameWidth + 2) +
                "+" + new string('-', specialisationWidth + 2) +
                "+" + new string('-', experienceWidth + 2) +
                "+" + new string('-', feeWidth + 2) +
                "+" + new string('-', statusWidth + 2) +
                "+";

            Console.WriteLine(border);

            Console.WriteLine(
                "| " + "ID".PadRight(idWidth) +
                " | " + "Name".PadRight(nameWidth) +
                " | " + "Specialisation".PadRight(specialisationWidth) +
                " | " + "Experience".PadRight(experienceWidth) +
                " | " + "Fee".PadRight(feeWidth) +
                " | " + "Status".PadRight(statusWidth) +
                " |");

            Console.WriteLine(border);

            foreach (Doctor doctor in doctors)
            {
                string status =
                    doctor.IsActive
                        ? "Available"
                        : "Unavailable";

                Console.WriteLine(
                    "| " + doctor.DoctorId
                        .ToString()
                        .PadRight(idWidth) +

                    " | " + doctor.FullName
                        .PadRight(nameWidth) +

                    " | " + doctor.Specialisation
                        .ToString()
                        .PadRight(specialisationWidth) +

                    " | " + doctor.YearsOfExperience
                        .ToString()
                        .PadRight(experienceWidth) +

                    " | " + doctor.ConsultationFee
                        .ToString()
                        .PadRight(feeWidth) +

                    " | " + status
                        .PadRight(statusWidth) +

                    " |");
            }

            Console.WriteLine(border);
        }

        public static void DisplayAppointmentTable(
                List<Appointment> appointments)
        {
            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    "No Appointments Found.");

                return;
            }

            int idWidth = 5;

            int patientWidth =
                Math.Max(
                    20,
                    appointments.Max(a =>
                        a.Patient.FullName.Length));

            int doctorWidth =
                Math.Max(
                    20,
                    appointments.Max(a =>
                        a.Doctor.FullName.Length));

            int dateWidth = 12;

            int timeWidth = 10;

            int statusWidth =
                Math.Max(
                    12,
                    appointments.Max(a =>
                        a.Status.ToString().Length));

            string border =
                "+" + new string('-', idWidth + 2) +
                "+" + new string('-', patientWidth + 2) +
                "+" + new string('-', doctorWidth + 2) +
                "+" + new string('-', dateWidth + 2) +
                "+" + new string('-', timeWidth + 2) +
                "+" + new string('-', statusWidth + 2) +
                "+";

            Console.WriteLine(border);

            Console.WriteLine(
                "| " + "ID".PadRight(idWidth) +
                " | " + "Patient".PadRight(patientWidth) +
                " | " + "Doctor".PadRight(doctorWidth) +
                " | " + "Date".PadRight(dateWidth) +
                " | " + "Time".PadRight(timeWidth) +
                " | " + "Status".PadRight(statusWidth) +
                " |");

            Console.WriteLine(border);

            foreach (Appointment appointment
                in appointments)
            {
                Console.WriteLine(
                    "| " + appointment.AppointmentId
                        .ToString()
                        .PadRight(idWidth) +

                    " | " + appointment.Patient.FullName
                        .PadRight(patientWidth) +

                    " | " + appointment.Doctor.FullName
                        .PadRight(doctorWidth) +

                    " | " + appointment.ScheduledDate
                        .ToString()
                        .PadRight(dateWidth) +

                    " | " + appointment.TimeSlot
                        .ToString()
                        .PadRight(timeWidth) +

                    " | " + appointment.Status
                        .ToString()
                        .PadRight(statusWidth) +

                    " |");
            }

            Console.WriteLine(border);
        }

        public static void DisplayHealthRecordTable(
                List<HealthRecord> records)
        {
            if (records.Count == 0)
            {
                Console.WriteLine(
                    "No Health Records Found.");

                return;
            }

            int idWidth = 5;

            int dateWidth = 12;

            int patientWidth =
                Math.Max(
                    20,
                    records.Max(r =>
                        r.Patient.FullName.Length));

            int doctorWidth =
                Math.Max(
                    20,
                    records.Max(r =>
                        r.Doctor.FullName.Length));

            int diagnosisWidth =
                Math.Max(
                    20,
                    records.Max(r =>
                        r.Diagnosis.Length));

            int prescriptionWidth =
                Math.Max(
                    20,
                    records.Max(r =>
                        r.Prescription.Length));

            int notesWidth =
                Math.Max(
                    20,
                    records.Max(r =>
                        (r.Notes ?? string.Empty).Length));

            string border =
                "+" + new string('-', idWidth + 2) +
                "+" + new string('-', dateWidth + 2) +
                "+" + new string('-', patientWidth + 2) +
                "+" + new string('-', doctorWidth + 2) +
                "+" + new string('-', diagnosisWidth + 2) +
                "+" + new string('-', prescriptionWidth + 2) +
                "+" + new string('-', notesWidth + 2) +
                "+";

            Console.WriteLine(border);

            Console.WriteLine(
                "| " + "ID".PadRight(idWidth) +
                " | " + "Visit Date".PadRight(dateWidth) +
                " | " + "Patient".PadRight(patientWidth) +
                " | " + "Doctor".PadRight(doctorWidth) +
                " | " + "Diagnosis".PadRight(diagnosisWidth) +
                " | " + "Prescription".PadRight(prescriptionWidth) +
                " | " + "Notes".PadRight(notesWidth) +
                " |");

            Console.WriteLine(border);

            foreach (HealthRecord record
                in records)
            {
                Console.WriteLine(
                    "| " + record.RecordId
                        .ToString()
                        .PadRight(idWidth) +

                    " | " + record.VisitDate
                        .ToString()
                        .PadRight(dateWidth) +

                    " | " + record.Patient.FullName
                        .PadRight(patientWidth) +

                    " | " + record.Doctor.FullName
                        .PadRight(doctorWidth) +

                    " | " + record.Diagnosis
                        .PadRight(diagnosisWidth) +

                    " | " + record.Prescription
                        .PadRight(prescriptionWidth) +

                    " | " + (record.Notes ?? string.Empty)
                        .PadRight(notesWidth) +

                    " |");
            }

            Console.WriteLine(border);
        }

    }
}