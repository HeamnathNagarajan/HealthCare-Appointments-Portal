using HealthcareApp.Controllers;
using HealthcareApp.Data;
using HealthcareApp.Exceptions;
using HealthcareApp.Repositories;
using HealthcareApp.Repositories.Implementations;
using HealthcareApp.Services;
using HealthcareApp.Services.Implementations;
using HealthcareApp.Utilities;
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

using var provider = services.BuildServiceProvider();

var patientController = provider.GetRequiredService<PatientController>();
var doctorController = provider.GetRequiredService<DoctorController>();
var appointmentController = provider.GetRequiredService<AppointmentController>();
var healthRecordController = provider.GetRequiredService<HealthRecordController>();

bool exit = false;

Action[] menuActions =
{
    patientController.RegisterPatient,
    doctorController.AddDoctor,
    doctorController.SearchDoctorsBySpecialisation,
    appointmentController.BookAppointment,
    appointmentController.ViewAppointmentsForPatient,
    appointmentController.ConfirmOrCancelAppointment,
    healthRecordController.CompleteAppointmentAndAddRecord,
    healthRecordController.ViewHealthHistory,
    SystemTimeController.ManageSystemTime,
    doctorController.ManageDoctorOffDays,
    ExitApplication
};

while (!exit)
{
    DisplayMenu();

    int choice = Validations.ReadIntInRange(
        "Choose an option: ",
        "Menu option",
        1,
        menuActions.Length
    );

    try
    {
        menuActions[choice-1]();
    }
    catch (Exception ex)
    {
        Console.WriteLine(GetErrorMessage(ex));
    }
}

void ExitApplication()
{
    exit = true;
    Console.WriteLine("Exiting application...");
}

static void DisplayMenu()
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
}

static string GetErrorMessage(Exception ex)
{
    return ex switch
    {
        PatientNotFoundException => $"Error: {ex.Message}",
        DoctorNotFoundException => $"Error: {ex.Message}",
        AppointmentNotFoundException => $"Error: {ex.Message}",
        PastDateException => $"Error: {ex.Message}",
        AppointmentConflictException => $"Error: {ex.Message}",
        DoctorUnavailableException => $"Error: {ex.Message}",
        InvalidAppointmentStatusException => $"Error: {ex.Message}",
        InvalidHealthRecordException => $"Error: {ex.Message}",
        DuplicateHealthRecordException => $"Error: {ex.Message}",
        NoHealthRecordsFoundException => $"Error: {ex.Message}",
        ArgumentException => $"Invalid input: {ex.Message}",
        _ => $"Unexpected error: {ex.Message}"
    };
}

