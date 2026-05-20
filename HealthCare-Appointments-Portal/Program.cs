using System.ComponentModel.DataAnnotations;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare_Appointment_Portal;

public class Program
{
    public static void Main(string[] args)
    {
        // Dependency Injection Container
        ServiceCollection services = new();

        // Register Data Store
        services.AddSingleton<DataStore>();

        // Register Repositories
        services.AddScoped<IPatientRepository,
            PatientRepository>();

        services.AddScoped<IDoctorRepository,
            DoctorRepository>();

        services.AddScoped<IAppointmentRepository,
            AppointmentRepository>();

        services.AddScoped<IHealthRecordRepository,
            HealthRecordRepository>();

        // Register Services
        services.AddScoped<IPatientService,
            PatientService>();

        services.AddScoped<IDoctorService,
            DoctorService>();

        services.AddScoped<IAppointmentService,
            AppointmentService>();

        services.AddScoped<IHealthRecordService,
            HealthRecordService>();

        // Build Service Provider
        ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        // Resolve Services
        IPatientService patientService =
            serviceProvider
            .GetRequiredService<IPatientService>();

        IDoctorService doctorService =
            serviceProvider
            .GetRequiredService<IDoctorService>();

        IAppointmentService appointmentService =
            serviceProvider
            .GetRequiredService<IAppointmentService>();

        IHealthRecordService healthRecordService =
            serviceProvider
            .GetRequiredService<IHealthRecordService>();

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine(
                "\n===== HEALTHCARE APPOINTMENT PORTAL =====");

            Console.WriteLine(
                "1. Register Patient");

            Console.WriteLine(
                "2. Add Doctor");

            Console.WriteLine(
                "3. Search Doctors By Specialisation");

            Console.WriteLine(
                "4. Book Appointment");

            Console.WriteLine(
                "5. View Patient Appointments");

            Console.WriteLine(
                "6. Confirm / Cancel / Complete Appointment");

            Console.WriteLine(
                "7. Add Health Record");

            Console.WriteLine(
                "8. View Patient Health Records");

            Console.WriteLine(
                "9. Exit");

            Console.Write(
                "\nEnter Choice: ");

            int choice =
                Convert.ToInt32(
                    Console.ReadLine());

            try
            {
                switch (choice)
                {
                    // Register Patient
                    case 1:

                        Patient patient = new();

                        patient.FullName =
                            ReadValidatedProperty(
                                "Enter Full Name: ",
                                nameof(Patient.FullName),
                                patient);

                        patient.DateOfBirth =
                            ReadValidDate(
                                "Enter DOB (yyyy-MM-dd): ");

                        patient.Gender =
                            ReadValidEnum<Gender>(
                                "Enter Gender Choice: ");

                        patient.PhoneNumber =
                            ReadValidatedProperty(
                                "Enter Phone Number: ",
                                nameof(Patient.PhoneNumber),
                                patient);

                        patient.Email =
                            ReadValidatedProperty(
                                "Enter Email: ",
                                nameof(Patient.Email),
                                patient);

                        patient.InsuranceId =
                            ReadValidatedProperty(
                                "Enter Insurance Id: ",
                                nameof(Patient.InsuranceId),
                                patient);

                        patientService
                            .AddPatient(patient);

                        Console.WriteLine(
                            "\nPatient Registered Successfully.");

                        Console.WriteLine(
                            patient.GetProfileSummary());

                        break;

                    // Add Doctor
                    case 2:

                        Doctor doctor = new();

                        doctor.FullName =
                            ReadValidatedProperty(
                                "Enter Full Name: ",
                                nameof(Doctor.FullName),
                                doctor);

                        doctor.Specialisation =
                            ReadValidEnum<Specialisation>(
                                "Enter Specialisation Choice: ");

                        doctor.YearsOfExperience =
                            Convert.ToInt32(
                                ReadInput(
                                    "Enter Years Of Experience: "));

                        doctor.ConsultationFee =
                            Convert.ToDecimal(
                                ReadInput(
                                    "Enter Consultation Fee: "));

                        doctor.IsActive = true;

                        if (!ValidateModel(doctor))
                        {
                            break;
                        }

                        doctorService
                            .AddDoctor(doctor);

                        Console.WriteLine(
                            "\nDoctor Added Successfully.");

                        Console.WriteLine(
                            doctor.GetDoctorSummary());

                        break;

                    // Search Doctors By Specialisation
                    case 3:

                        Specialisation selectedSpecialisation =
                            ReadValidEnum<Specialisation>(
                                "Enter Specialisation Choice: ");

                        List<Doctor> doctors =
                            doctorService
                            .GetDoctorsBySpecialisation(
                                selectedSpecialisation);

                        if (!doctors.Any())
                        {
                            Console.WriteLine(
                                "No Doctors Found.");
                        }
                        else
                        {
                            Console.WriteLine(
                                "\nAvailable Doctors:");

                            foreach (Doctor d in doctors)
                            {
                                Console.WriteLine(
                                    d.GetDoctorSummary());
                            }
                        }

                        break;

                    // Book Appointment
                    case 4:

                        string patientEmail =
                            ReadInput(
                                "Enter Patient Email: ");

                        Patient? existingPatient =
                            patientService
                            .GetAllPatients()
                            .FirstOrDefault(p =>
                                p.Email ==
                                patientEmail);

                        if (existingPatient == null)
                        {
                            Console.WriteLine(
                                "Patient Not Found.");

                            break;
                        }

                        Specialisation selectedAppointmentSpecialisation =
                            ReadValidEnum<Specialisation>(
                                "Enter Specialisation Choice: ");

                        List<Doctor> availableDoctors =
                            doctorService
                            .GetDoctorsBySpecialisation(
                                selectedAppointmentSpecialisation);

                        if (!availableDoctors.Any())
                        {
                            Console.WriteLine(
                                "No Doctors Available.");

                            break;
                        }

                        Console.WriteLine(
                            "\nAvailable Doctors:");

                        foreach (Doctor d in availableDoctors)
                        {
                            Console.WriteLine(
                                d.GetDoctorSummary());
                        }

                        Guid doctorId =
                            Guid.Parse(
                                ReadInput(
                                    "Enter Doctor Id: "));

                        Doctor? existingDoctor =
                            doctorService
                            .GetDoctorById(
                                doctorId);

                        Appointment appointment =
                            appointmentService
                            .BookAppointment(
                                existingPatient,
                                existingDoctor!,

                                ReadValidDate(
                                    "Enter Appointment Date (yyyy-MM-dd): "),

                                ReadValidTime(
                                    "Enter Time Slot (HH:mm): "));

                        Console.WriteLine(
                            "\nAppointment Booked Successfully.");

                        Console.WriteLine(
                            appointment.GetDetails());

                        break;

                    // View Patient Appointments
                    case 5:

                        string appointmentEmail =
                            ReadInput(
                                "Enter Patient Email: ");

                        Patient? appointmentPatient =
                            patientService
                            .GetAllPatients()
                            .FirstOrDefault(p =>
                                p.Email ==
                                appointmentEmail);

                        if (appointmentPatient == null)
                        {
                            Console.WriteLine(
                                "Patient Not Found.");

                            break;
                        }

                        List<Appointment> appointments =
                            appointmentService
                            .GetAppointmentsByPatient(
                                appointmentPatient.PatientId);

                        if (!appointments.Any())
                        {
                            Console.WriteLine(
                                "No Appointments Found.");
                        }
                        else
                        {
                            foreach (Appointment a in appointments)
                            {
                                Console.WriteLine(
                                    a.GetDetails());
                            }
                        }

                        break;

                    // Confirm / Cancel / Complete Appointment
                    case 6:

                        List<Appointment> allAppointments =
                            appointmentService
                            .GetAllAppointments();

                        if (!allAppointments.Any())
                        {
                            Console.WriteLine(
                                "No Appointments Available.");

                            break;
                        }

                        Console.WriteLine(
                            "\nAppointments:");

                        foreach (Appointment a in allAppointments)
                        {
                            Console.WriteLine(
                                a.GetDetails());
                        }

                        Guid appointmentId =
                            Guid.Parse(
                                ReadInput(
                                    "\nEnter Appointment Id: "));

                        Console.WriteLine(
                            "\n1. Confirm Appointment");

                        Console.WriteLine(
                            "2. Cancel Appointment");

                        Console.WriteLine(
                            "3. Complete Appointment");

                        int option =
                            Convert.ToInt32(
                                ReadInput(
                                    "Enter Choice: "));

                        switch (option)
                        {
                            case 1:

                                appointmentService
                                    .ConfirmAppointment(
                                        appointmentId);

                                Console.WriteLine(
                                    "Appointment Confirmed.");

                                break;

                            case 2:

                                string reason =
                                    ReadInput(
                                        "Enter Cancellation Reason: ");

                                appointmentService
                                    .CancelAppointment(
                                        appointmentId,
                                        reason);

                                Console.WriteLine(
                                    "Appointment Cancelled.");

                                break;

                            case 3:

                                appointmentService
                                    .CompleteAppointment(
                                        appointmentId);

                                Console.WriteLine(
                                    "Appointment Completed.");

                                break;

                            default:

                                Console.WriteLine(
                                    "Invalid Choice.");

                                break;
                        }

                        break;

                    // Add Health Record
                    case 7:

                        List<Appointment> completedAppointments =
                            appointmentService
                            .GetAllAppointments()
                            .Where(a =>
                                a.Status ==
                                AppointmentStatus.Completed)
                            .ToList();

                        if (!completedAppointments.Any())
                        {
                            Console.WriteLine(
                                "No Completed Appointments Found.");

                            break;
                        }

                        Console.WriteLine(
                            "\nCompleted Appointments:");

                        foreach (Appointment a in completedAppointments)
                        {
                            Console.WriteLine(
                                a.GetDetails());
                        }

                        Guid appointmentRecordId =
                            Guid.Parse(
                                ReadInput(
                                    "\nEnter Appointment Id: "));

                        Appointment? appointmentRecord =
                            appointmentService
                            .GetAppointmentById(
                                appointmentRecordId);

                        if (appointmentRecord != null)
                        {
                            HealthRecord record = new()
                            {
                                Patient =
                                    appointmentRecord.Patient,

                                Doctor =
                                    appointmentRecord.Doctor,

                                VisitDate =
                                    appointmentRecord.ScheduledDate
                            };

                            record.Diagnosis =
                                ReadValidatedProperty(
                                    "Enter Diagnosis: ",
                                    nameof(HealthRecord.Diagnosis),
                                    record);

                            record.Prescription =
                                ReadValidatedProperty(
                                    "Enter Prescription: ",
                                    nameof(HealthRecord.Prescription),
                                    record);

                            record.Notes =
                                ReadInput(
                                    "Enter Notes: ");

                            healthRecordService
                                .AddRecord(record);

                            Console.WriteLine(
                                "\nHealth Record Added Successfully.");

                            Console.WriteLine(
                                record.GetSummary());
                        }

                        break;

                    // View Patient Health Records
                    case 8:

                        string healthEmail =
                            ReadInput(
                                "Enter Patient Email: ");

                        Patient? healthPatient =
                            patientService
                            .GetAllPatients()
                            .FirstOrDefault(p =>
                                p.Email ==
                                healthEmail);

                        if (healthPatient == null)
                        {
                            Console.WriteLine(
                                "Patient Not Found.");

                            break;
                        }

                        List<HealthRecord> records =
                            healthRecordService
                            .GetRecordsByPatient(
                                healthPatient.PatientId);

                        if (!records.Any())
                        {
                            Console.WriteLine(
                                "No Health Records Found.");
                        }
                        else
                        {
                            foreach (HealthRecord record in records)
                            {
                                Console.WriteLine(
                                    record.GetSummary());
                            }
                        }

                        break;

                    // Exit Application
                    case 9:

                        exit = true;

                        Console.WriteLine(
                            "Application Closed.");

                        break;

                    default:

                        Console.WriteLine(
                            "Invalid Choice.");

                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error: {ex.Message}");
            }
        }
    }

    // Read Console Input
    public static string ReadInput(
        string message)
    {
        Console.Write(message);

        return Console.ReadLine()
            ?? string.Empty;
    }

    // Read Validated Date
    public static DateOnly ReadValidDate(
        string message)
    {
        while (true)
        {
            string input =
                ReadInput(message);

            bool isValidDate =
                DateOnly.TryParseExact(
                    input,
                    "yyyy-MM-dd",
                    out DateOnly date);

            if (isValidDate)
            {
                return date;
            }

            Console.WriteLine(
                "Invalid Date Format. Please enter in yyyy-MM-dd format.");
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
                    out TimeOnly time);

            if (isValidTime)
            {
                return time;
            }

            Console.WriteLine(
                "Invalid Time Format. Please enter in HH:mm format.");
        }
    }

    // Read Validated Enum Choice
    public static T ReadValidEnum<T>(
        string message)
        where T : struct, Enum
    {
        while (true)
        {
            Console.WriteLine(
                $"\nAvailable {typeof(T).Name}s:");

            T[] values =
                Enum.GetValues<T>();

            for (int i = 0;
                i < values.Length;
                i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {values[i]}");
            }

            string input =
                ReadInput(message);

            // Number Choice
            if (int.TryParse(
                input,
                out int choice))
            {
                if (choice >= 1 &&
                    choice <= values.Length)
                {
                    return values[
                        choice - 1];
                }

                Console.WriteLine(
                    "Invalid Choice.");

                continue;
            }

            // Enum Name Choice
            bool isValidEnum =
                Enum.TryParse<T>(
                    input,
                    true,
                    out T result);

            if (isValidEnum)
            {
                return result;
            }

            Console.WriteLine(
                "Invalid Choice.");
        }
    }

    // Validate Single Property Using DataAnnotations
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
                typeof(T).GetProperty(
                    propertyName);

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
                new();

            bool isValid =
                Validator.TryValidateProperty(
                    input,
                    context,
                    results);

            if (isValid)
            {
                return input;
            }

            foreach (ValidationResult error in results)
            {
                Console.WriteLine(
                    error.ErrorMessage);
            }
        }
    }

    // Validate Full Model
    public static bool ValidateModel<T>(
        T model)
    {
        List<ValidationResult> results =
            new();

        ValidationContext context =
            new(model!);

        bool isValid =
            Validator.TryValidateObject(
                model!,
                context,
                results,
                true);

        if (!isValid)
        {
            foreach (ValidationResult error in results)
            {
                Console.WriteLine(
                    error.ErrorMessage);
            }
        }

        return isValid;
    }
}