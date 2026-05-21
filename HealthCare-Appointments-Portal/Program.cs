using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Intefaces;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;
using HealthCare_Appointments_Portal.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<DataStore>();

// Repositories
services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IDoctorRepository, DoctorRepository>();
services.AddScoped<IPatientRepository, PatientRepository>();
services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

// Services
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IDoctorService, DoctorService>();
services.AddScoped<IPatientService, PatientService>();
services.AddScoped<IHealthRecordService, HealthRecordService>();

var provider = services.BuildServiceProvider();

var appointmentService = provider.GetRequiredService<IAppointmentService>();
var doctorService = provider.GetRequiredService<IDoctorService>();
var patientService = provider.GetRequiredService<IPatientService>();

bool exit = false;

while (!exit)
{
    Console.Clear();
    Console.WriteLine("==== HealthCare Appointment Portal ====");
    Console.WriteLine("1. Add Patient");
    Console.WriteLine("2. Add Doctor");
    Console.WriteLine("3. Book Appointment");
    Console.WriteLine("4. View Appointments by Doctor");
    Console.WriteLine("5. View Appointments by Patient");
    Console.WriteLine("6. Cancel Appointment");
    Console.WriteLine("7. Exit");
    Console.Write("Enter choice: ");

    int choice = int.Parse(Console.ReadLine()!);

    try
    {
        switch (choice)
        {
            case 1:
                AddPatient();
                break;

            case 2:
                AddDoctor();
                break;

            case 3:
                BookAppointment();
                break;

            case 4:
                ViewDoctorAppointments();
                break;

            case 5:
                ViewPatientAppointments();
                break;

            case 6:
                CancelAppointment();
                break;

            case 7:
                exit = true;
                break;

            default:
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine("\nPress Enter to continue...");
    Console.ReadLine();
}

//METHODS

void AddPatient()
{
    Console.Write("Enter Name: ");
    string name = Console.ReadLine()!;

    Console.Write("Enter Date of Birth (yyyy-MM-dd): ");
    DateOnly dob = DateOnly.Parse(Console.ReadLine()!);

    Console.Write("Enter Phone: ");
    string phone = Console.ReadLine()!;

    Console.Write("Enter Email: ");
    string email = Console.ReadLine()!;

    Patient patient = new()
    {
        PatientId = Guid.NewGuid(),
        FullName = name,
        DateOfBirth = dob,
        PhoneNumber = phone,
        Email = email
    };

    patientService.AddPatient(patient);

    Console.WriteLine("Patient Added Successfully!");
}

void AddDoctor()
{
    Console.Write("Enter Name: ");
    string name = Console.ReadLine()!;

    Console.WriteLine("Select Specialisation:");
    foreach (var spec in Enum.GetValues(typeof(Specialisation)))
    {
        Console.WriteLine($"{(int)spec} - {spec}");
    }

    int specChoice = int.Parse(Console.ReadLine()!);

    Console.Write("Years of Experience: ");
    int exp = int.Parse(Console.ReadLine()!);

    Console.Write("Consultation Fee: ");
    decimal fee = decimal.Parse(Console.ReadLine()!);

    Doctor doctor = new()
    {
        DoctorId = Guid.NewGuid(),
        FullName = name,
        Specialisation = (Specialisation)specChoice,
        YearsOfExperience = exp,
        ConsultationFee = fee,
        IsActive = true
    };

    doctorService.AddDoctor(doctor);

    Console.WriteLine("Doctor Added Successfully!");
}

void BookAppointment()
{
    var patients = patientService.GetAllPatients();
    var doctors = doctorService.GetAllDoctors();

    if (!patients.Any() || !doctors.Any())
    {
        Console.WriteLine("Add at least one patient and doctor first.");
        return;
    }

    Console.WriteLine("\nSelect Patient:");
    for (int i = 0; i < patients.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {patients[i].FullName}");
    }

    int pIndex = int.Parse(Console.ReadLine()!) - 1;
    var patient = patients[pIndex];

    Console.WriteLine("\nSelect Doctor:");
    for (int i = 0; i < doctors.Count; i++)
    {
        Console.WriteLine($"{i + 1}. Dr. {doctors[i].FullName} ({doctors[i].Specialisation})");
    }

    int dIndex = int.Parse(Console.ReadLine()!) - 1;
    var doctor = doctors[dIndex];

    Console.Write("Enter Date (yyyy-MM-dd): ");
    DateOnly date = DateOnly.Parse(Console.ReadLine()!);

    Console.Write("Enter Time (HH:mm): ");
    TimeOnly time = TimeOnly.Parse(Console.ReadLine()!);

    var appointment = appointmentService.BookAppointment(patient, doctor, date, time);
    appointment.Confirm();

    Console.WriteLine("\n Appointment Booked!");
    Console.WriteLine(appointment.GetDetails());
}

void ViewDoctorAppointments()
{
    var doctors = doctorService.GetAllDoctors();

    if (!doctors.Any())
    {
        Console.WriteLine("No doctors found.");
        return;
    }

    Console.WriteLine("\nSelect Doctor:");
    for (int i = 0; i < doctors.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {doctors[i].FullName}");
    }

    int index = int.Parse(Console.ReadLine()!) - 1;
    var doctor = doctors[index];

    var list = appointmentService.GetAppointmentsByDoctor(doctor.DoctorId);

    if (!list.Any())
    {
        Console.WriteLine("No appointments found.");
        return;
    }

    foreach (var a in list)
    {
        Console.WriteLine(a.GetDetails());
    }
}

void ViewPatientAppointments()
{
    var patients = patientService.GetAllPatients();

    if (!patients.Any())
    {
        Console.WriteLine("No patients found.");
        return;
    }

    Console.WriteLine("\nSelect Patient:");
    for (int i = 0; i < patients.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {patients[i].FullName}");
    }

    int index = int.Parse(Console.ReadLine()!) - 1;
    var patient = patients[index];

    var list = appointmentService.GetAppointmentsByPatient(patient.PatientId);

    if (!list.Any())
    {
        Console.WriteLine("No appointments found.");
        return;
    }

    foreach (var a in list)
    {
        Console.WriteLine(a.GetDetails());
    }
}

void CancelAppointment()
{
    var appointments = appointmentService.GetUpcomingAppointments();

    if (!appointments.Any())
    {
        Console.WriteLine("No upcoming appointments.");
        return;
    }

    Console.WriteLine("\nSelect Appointment:");
    for (int i = 0; i < appointments.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {appointments[i].GetDetails()}");
    }

    int index = int.Parse(Console.ReadLine()!) - 1;
    var selected = appointments[index];

    Console.Write("Enter reason: ");
    string reason = Console.ReadLine()!;

    appointmentService.CancelAppointment(selected.AppointmentId, reason);

    Console.WriteLine(" Appointment Cancelled");
}
