using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Repositories;
using HealthCare_Appointments_portal.Models;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Services;

using Microsoft.Extensions.DependencyInjection;

// =============================
// Dependency Injection
// =============================

ServiceCollection services = new();

// Register DataStore
services.AddSingleton<DataStore>();

// Patient DI
services.AddSingleton<IPatientRepository, PatientRepository>();

services.AddSingleton<IPatientService, PatientService>();

// Doctor DI
services.AddSingleton<IDoctorRepository, DoctorRepository>();

services.AddSingleton<IDoctorService, DoctorService>();

// Appointment DI
services.AddSingleton<IAppointmentRepository, AppointmentRepository>();

services.AddSingleton<IAppointmentService, AppointmentService>();

// Build Provider
ServiceProvider provider = services.BuildServiceProvider();

IPatientService patientService =
    provider.GetRequiredService<IPatientService>();

IDoctorService doctorService =
    provider.GetRequiredService<IDoctorService>();

IAppointmentService appointmentService =
    provider.GetRequiredService<IAppointmentService>();

// =============================
// Console Menu
// =============================

bool isRunning = true;

while (isRunning)
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine(" HEALTHCARE APPOINTMENT PORTAL ");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Register Patient");
    Console.WriteLine("2. Add Doctor");
    Console.WriteLine("3. Search Doctors By Specialisation");
    Console.WriteLine("4. Book Appointment");
    Console.WriteLine("5. View Patient Appointments");
    Console.WriteLine("6. Confirm Or Cancel Appointment");
    Console.WriteLine("7. Exit");

    Console.Write("\nEnter Choice: ");

    string? choice = Console.ReadLine();

    Console.Clear();

    switch (choice)
    {
        // Register Patient
        case "1":

            Patient patient = new Patient();

            Console.WriteLine("=== Register Patient ===\n");

            Console.Write("Full Name: ");

            patient.FullName =
                Console.ReadLine() ?? string.Empty;

            Console.Write(
                "Date Of Birth (yyyy-mm-dd): ");

            patient.DateOfBirth =
                DateOnly.Parse(
                    Console.ReadLine() ?? string.Empty);

            Console.WriteLine("\nSelect Gender:");

            Gender[] genders =
                Enum.GetValues<Gender>();

            for (int i = 0; i < genders.Length; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {genders[i]}");
            }

            Console.Write("\nChoice: ");

            int genderChoice = Convert.ToInt32(Console.ReadLine());

            patient.Gender = genders[genderChoice - 1];

            Console.Write("Phone Number: ");

            patient.PhoneNumber = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");

            patient.Email = Console.ReadLine() ?? string.Empty;

            Console.Write("Insurance Id: ");

            patient.InsuranceId = Console.ReadLine() ?? string.Empty;

            patientService.AddPatient(patient);

            Console.WriteLine("\nPatient Registered Successfully!\n");

            Console.WriteLine(patient.GetProfileSummary());

            Pause();

            break;

        // Add Doctor
        case "2":

            Doctor doctor = new Doctor();

            Console.WriteLine("=== Add Doctor ===\n");

            Console.Write("Full Name: ");

            doctor.FullName =
                Console.ReadLine() ?? string.Empty;

            Console.WriteLine(
                "\nSelect Specialisation:");

            Specialisation[] specialisations =
                Enum.GetValues<Specialisation>();

            for (int i = 0; i < specialisations.Length; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {specialisations[i]}");
            }

            Console.Write("\nChoice: ");

            int specialisationChoice =
                Convert.ToInt32(
                    Console.ReadLine());

            doctor.Specialisation =
                specialisations[specialisationChoice - 1];

            Console.Write(
                "Years Of Experience: ");

            doctor.YearsOfExperience =
                Convert.ToInt32(
                    Console.ReadLine());

            Console.Write(
                "Consultation Fee: ");

            doctor.ConsultationFee =
                Convert.ToDecimal(
                    Console.ReadLine());

            doctor.IsActive = true;

            doctorService.AddDoctor(doctor);

            Console.WriteLine(
                "\nDoctor Added Successfully!");

            Pause();

            break;

        // Search Doctors By Specialisation
        case "3":

            Console.WriteLine(
                "=== Search Doctors By Specialisation ===\n");

            Specialisation[] allSpecialisations =
                Enum.GetValues<Specialisation>();

            for (int i = 0; i < allSpecialisations.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {allSpecialisations[i]}");
            }

            Console.Write("\nSelect Specialisation: ");

            int specialityChoice = Convert.ToInt32(Console.ReadLine());

            Specialisation selectedSpecialisation =
                allSpecialisations[specialityChoice - 1];

            List<Doctor> filteredDoctors =
                doctorService.GetDoctorsBySpecialisation(
                    selectedSpecialisation);

            Console.WriteLine(
                $"\nDoctors In {selectedSpecialisation}\n");

            if (filteredDoctors.Count == 0)
            {
                Console.WriteLine(
                    "No doctors found.");
            }
            else
            {
                foreach (Doctor d in filteredDoctors)
                {
                    Console.WriteLine($"ID: {d.DoctorId}");

                    Console.WriteLine($"Name: {d.FullName}");

                    Console.WriteLine($"Experience: {d.YearsOfExperience} years");

                    Console.WriteLine($"Consultation Fee: {d.ConsultationFee}");

                    Console.WriteLine($"Active: {d.IsActive}");

                    Console.WriteLine("--------------------------------");
                }
            }

            Pause();

            break;

        // Book Appointment
        case "4":

            Console.WriteLine(
                "=== Book Appointment ===\n");

            List<Patient> allPatients =
                patientService.GetAllPatients();

            if (allPatients.Count == 0)
            {
                Console.WriteLine(
                    "No patients available.");

                Pause();

                break;
            }

            Console.WriteLine("Select Patient:\n");

            for (int i = 0; i < allPatients.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {allPatients[i].FullName}");
            }

            Console.Write("\nChoice: ");

            int patientChoice =
                Convert.ToInt32(Console.ReadLine());

            Patient selectedPatient =
                allPatients[patientChoice - 1];

            Console.WriteLine(
                "\nSelect Doctor:\n");

            List<Doctor> allDoctors =
                doctorService.GetAllDoctors();

            if (allDoctors.Count == 0)
            {
                Console.WriteLine(
                    "No doctors available.");

                Pause();

                break;
            }

            for (int i = 0; i < allDoctors.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. " +
                    $"{allDoctors[i].FullName} " +
                    $"({allDoctors[i].Specialisation})");
            }

            Console.Write("\nChoice: ");

            int doctorChoice =
                Convert.ToInt32(Console.ReadLine());

            Doctor selectedDoctor =
                allDoctors[doctorChoice - 1];

            Console.Write(
                "\nAppointment Date (yyyy-mm-dd): ");

            DateOnly appointmentDate =
                DateOnly.Parse(
                    Console.ReadLine() ?? string.Empty);

            Console.Write(
                "Time Slot (HH:mm): ");

            TimeOnly timeSlot =
                TimeOnly.Parse(
                    Console.ReadLine() ?? string.Empty);

            try
            {
                Appointment appointment =
                    appointmentService.BookAppointment(
                        selectedPatient,
                        selectedDoctor,
                        appointmentDate,
                        timeSlot);

                Console.WriteLine(
                    "\nAppointment Booked Successfully!\n");

                Console.WriteLine(
                    appointment.GetDetails());
            }
            catch (PastDateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DoctorUnavailableException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (AppointmentConflictException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Pause();

            break;
        // View Patient Appointments
        case "5":

            Console.WriteLine(
                "=== View Patient Appointments ===\n");

            List<Patient> patientList =
                patientService.GetAllPatients();

            if (patientList.Count == 0)
            {
                Console.WriteLine(
                    "No patients found.");

                Pause();

                break;
            }

            Console.WriteLine(
                "Select Patient:\n");

            for (int i = 0; i < patientList.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. " +
                    $"{patientList[i].FullName}");
            }

            Console.Write("\nChoice: ");

            int patientSelection =
                Convert.ToInt32(
                    Console.ReadLine());

            Patient chosenPatient =
                patientList[patientSelection - 1];

            List<Appointment> appointments =
                appointmentService.GetAppointmentsByPatient(
                    chosenPatient.PatientId);

            Console.WriteLine(
                $"\nAppointments For " +
                $"{chosenPatient.FullName}\n");

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    "No appointments found.");
            }
            else
            {
                foreach (Appointment appointment in appointments)
                {
                    Console.WriteLine(
                        $"Appointment ID: " +
                        $"{appointment.AppointmentId}");

                    Console.WriteLine(
                        $"Doctor: " +
                        $"{appointment.Doctor.FullName}");

                    Console.WriteLine(
                        $"Specialisation: " +
                        $"{appointment.Doctor.Specialisation}");

                    Console.WriteLine(
                        $"Date: " +
                        $"{appointment.ScheduledDate}");

                    Console.WriteLine(
                        $"Time: " +
                        $"{appointment.TimeSlot}");

                    Console.WriteLine(
                        $"Status: " +
                        $"{appointment.Status}");

                    Console.WriteLine(
                        "--------------------------------");
                }
            }

            Pause();

            break;
        // Confirm Or Cancel Appointment
        case "6":

            Console.WriteLine(
                "=== Confirm Or Cancel Appointment ===\n");

            List<Appointment> allAppointments =
                appointmentService.GetAllAppointments();

            if (allAppointments.Count == 0)
            {
                Console.WriteLine(
                    "No appointments found.");

                Pause();

                break;
            }

            for (int i = 0; i < allAppointments.Count; i++)
            {
                Appointment appointment =
                    allAppointments[i];

                Console.WriteLine(
                    $"{i + 1}. " +
                    $"{appointment.Patient.FullName} | " +
                    $"{appointment.Doctor.FullName} | " +
                    $"{appointment.ScheduledDate} | " +
                    $"{appointment.TimeSlot} | " +
                    $"{appointment.Status}");
            }

            Console.Write("\nSelect Appointment: ");

            int appointmentChoice =
                Convert.ToInt32(
                    Console.ReadLine());

            Appointment selectedAppointment =
                allAppointments[appointmentChoice - 1];

            Console.WriteLine("\n1. Confirm");
            Console.WriteLine("2. Cancel");

            Console.Write("\nChoice: ");

            int actionChoice =
                Convert.ToInt32(
                    Console.ReadLine());

            if (actionChoice == 1)
            {
                appointmentService.ConfirmAppointment(
                    selectedAppointment.AppointmentId);

                Console.WriteLine(
                    "\nAppointment Confirmed Successfully!");
            }
            else if (actionChoice == 2)
            {
                Console.Write(
                    "\nEnter Cancellation Reason: ");

                string reason =
                    Console.ReadLine() ?? string.Empty;

                appointmentService.CancelAppointment(
                    selectedAppointment.AppointmentId,
                    reason);

                Console.WriteLine(
                    "\nAppointment Cancelled Successfully!");
            }
            else
            {
                Console.WriteLine(
                    "\nInvalid Choice.");
            }

            Pause();

            break;
        case "7":

            isRunning = false;

            break;

        default:

            Console.WriteLine("Invalid Choice");

            Pause();

            break;
    }
}

// Helper Method
static void Pause()
{
    Console.WriteLine(
        "\nPress Any Key To Continue...");

    Console.ReadKey();
}