using HealthcareApp.Controllers;
using HealthcareApp.Data;
using HealthcareApp.Exceptions;
using HealthcareApp.Repositories;
using HealthcareApp.Repositories.Implementations;
using HealthcareApp.Services;
using HealthcareApp.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

var services = new ServiceCollection();

// Dependency Injection setup
services.AddSingleton<DataStore>();

services.AddSingleton<IPatientRepository, PatientRepository>();
services.AddSingleton<IDoctorRepository, DoctorRepository>();
services.AddSingleton<IAppointmentRepository, AppointmentRepository>();
services.AddSingleton<IHealthRecordRepository, HealthRecordRepository>();

services.AddSingleton<IPatientService, PatientService>();
services.AddSingleton<IDoctorService, DoctorService>();
services.AddSingleton<IAppointmentService, AppointmentService>();
services.AddSingleton<IHealthRecordService, HealthRecordService>();

services.AddSingleton<PatientController>();
services.AddSingleton<DoctorController>();
services.AddSingleton<AppointmentController>();
services.AddSingleton<HealthRecordController>();
services.AddSingleton<SystemTimeController>();

using var provider = services.BuildServiceProvider();

var patientController = provider.GetRequiredService<PatientController>();
var doctorController = provider.GetRequiredService<DoctorController>();
var appointmentController = provider.GetRequiredService<AppointmentController>();
var healthRecordController = provider.GetRequiredService<HealthRecordController>();
var systemTimeController = provider.GetRequiredService<SystemTimeController>();

bool exit = false;

while (!exit)
{
    Console.WriteLine("\n========== Healthcare Appointment Portal ==========");
    Console.WriteLine("1. Register a new patient");
    Console.WriteLine("2. Add a new doctor");
    Console.WriteLine("3. Search doctors by specialisation");
    Console.WriteLine("4. Book an appointment for a patient");
    Console.WriteLine("5. View all appointments for a patient");
    Console.WriteLine("6. Confirm or cancel an appointment");
    Console.WriteLine("7. Complete appointment and add health record");
    Console.WriteLine("8. View health history for a patient");
    Console.WriteLine("9. Manage system time");
    Console.WriteLine("10. Manage Doctor Off Days");
    Console.WriteLine("11. Exit");
    Console.Write("Choose an option: ");

    bool isValidChoice = int.TryParse(Console.ReadLine(), out int choice);

    if (!isValidChoice)
    {
        Console.WriteLine("Invalid input. Please enter a number.");
        continue;
    }

    try
    {
        switch (choice)
        {
            case 1:
                patientController.RegisterPatient();
                break;

            case 2:
                doctorController.AddDoctor();
                break;

            case 3:
                doctorController.SearchDoctorsBySpecialisation();
                break;

            case 4:
                appointmentController.BookAppointment();
                break;

            case 5:
                appointmentController.ViewAppointmentsForPatient();
                break;

            case 6:
                appointmentController.ConfirmOrCancelAppointment();
                break;

            case 7:
                healthRecordController.CompleteAppointmentAndAddRecord();
                break;

            case 8:
                healthRecordController.ViewHealthHistory();
                break;

            case 9:
                systemTimeController.ManageSystemTime();
                break;

            case 10:
                doctorController.ManageDoctorOffDays();
                break;
            case 11:
                exit = true;
                Console.WriteLine("Exiting application...");
                break;

            default:
                Console.WriteLine("Invalid option. Please choose between 1 and 10.");
                break;
        }
    }
    catch (PatientNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (DoctorNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (AppointmentNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (PastDateException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (AppointmentConflictException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (DoctorUnavailableException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (InvalidAppointmentStatusException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (InvalidHealthRecordException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (DuplicateHealthRecordException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (NoHealthRecordsFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Invalid input: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}
