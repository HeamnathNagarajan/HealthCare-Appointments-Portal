using HealthcareApp.Data;
using HealthcareApp.Dtos;
using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;
using HealthcareApp.Repositories;
using HealthcareApp.Repositories.Implementations;
using HealthcareApp.Services;
using HealthcareApp.Services.Implementations;
using HealthcareApp.Utilities;
using Microsoft.Extensions.DependencyInjection;


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

using var provider = services.BuildServiceProvider();

var patientService = provider.GetRequiredService<IPatientService>();
var doctorService = provider.GetRequiredService<IDoctorService>();
var appointmentService = provider.GetRequiredService<IAppointmentService>();
var healthRecordService = provider.GetRequiredService<IHealthRecordService>();

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
    Console.WriteLine("10. Exit");
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
                RegisterPatient(patientService);
                break;

            case 2:
                AddDoctor(doctorService);
                break;

            case 3:
                SearchDoctorsBySpecialisation(doctorService);
                break;

            case 4:
                BookAppointment(patientService, doctorService, appointmentService);
                break;

            case 5:
                ViewAppointmentsForPatient(appointmentService);
                break;

            case 6:
                ConfirmOrCancelAppointment(appointmentService);
                break;

            case 7:
                CompleteAppointmentAndAddRecord(appointmentService, healthRecordService);
                break;

            case 8:
                ViewHealthHistory(healthRecordService);
                break;

            case 9:
                ManageSystemTime();
                break;

            case 10:
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

// =====================================================
// Case 1: Register Patient
// =====================================================

static void RegisterPatient(IPatientService patientService)
{
    Console.WriteLine("\n========== Register New Patient ==========");

    string fullName = Validations.ReadRequiredString("Enter full name: ", "Full name", 2);

    DateTime dateOfBirth = Validations.ReadDateOfBirth();

    Gender selectedGender = Validations.ReadEnumChoice<Gender>("gender");

    string phoneNumber = Validations.ReadPhoneNumber();

    string email = Validations.ReadEmail();

    string insuranceId = Validations.ReadRequiredString("Enter insurance ID: ", "Insurance ID", 1);

    var patient = new Patient
    {
        FullName = fullName,
        DateOfBirth = dateOfBirth,
        Gender = selectedGender,
        PhoneNumber = phoneNumber,
        Email = email,
        InsuranceId = insuranceId
    };

    Patient registeredPatient = patientService.RegisterPatient(patient);

    Console.WriteLine("\nPatient registered successfully.");
    Console.WriteLine(registeredPatient.GetProfileSummary());
}

// =====================================================
// Case 2: Add Doctor
// =====================================================

static void AddDoctor(IDoctorService doctorService)
{
    Console.WriteLine("\n========== Add New Doctor ==========");

    string fullName = Validations.ReadRequiredString("Enter full name: ", "Full name", 2);

    Specialisation selectedSpecialisation = Validations.ReadEnumChoice<Specialisation>("specialisation");

    int yearsOfExperience = Validations.ReadIntInRange(
        "Enter years of experience: ",
        "Years of experience",
        0,
        70
    );

    decimal consultationFee = Validations.ReadDecimalInRange(
        "Enter consultation fee: ",
        "Consultation fee",
        0,
        100000
    );

    List<DayOfWeek> offDays = SelectTwoOffDays();

    var doctor = new Doctor
    {
        FullName = fullName,
        Specialisation = selectedSpecialisation,
        YearsOfExperience = yearsOfExperience,
        ConsultationFee = consultationFee,
        IsActive = true,
        OffDays = offDays
    };

    Doctor addedDoctor = doctorService.AddDoctor(doctor);

    Console.WriteLine("\nDoctor added successfully.");
    Console.WriteLine($"{addedDoctor.GetDoctorSummary()} | Off Days: {string.Join(", ", addedDoctor.OffDays)}");
}

static List<DayOfWeek> SelectTwoOffDays()
{
    var selectedOffDays = new List<DayOfWeek>();

    Console.WriteLine("\nSelect exactly two off-duty days:");

    var days = Enum.GetValues<DayOfWeek>();

    while (selectedOffDays.Count < 2)
    {
        Console.WriteLine();

        for (int i = 0; i < days.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {days[i]}");
        }

        int dayChoice = Validations.ReadIntInRange(
            $"Choose off-duty day {selectedOffDays.Count + 1}: ",
            "Off-duty day",
            1,
            days.Length
        );

        DayOfWeek selectedDay = days[dayChoice - 1];

        if (selectedOffDays.Contains(selectedDay))
        {
            Console.WriteLine("This day has already been selected. Choose another day.");
            continue;
        }

        selectedOffDays.Add(selectedDay);
    }

    return selectedOffDays;
}

// =====================================================
// Case 3: Search Doctors By Specialisation
// =====================================================

static void SearchDoctorsBySpecialisation(IDoctorService doctorService)
{
    Console.WriteLine("\n========== Search Doctors By Specialisation ==========");

    Specialisation selectedSpecialisation = Validations.ReadEnumChoice<Specialisation>("specialisation");

    List<Doctor> doctors = doctorService.SearchDoctorsBySpecialisation(selectedSpecialisation);

    if (doctors.Count == 0)
    {
        Console.WriteLine($"\nNo active doctors available for specialisation: {selectedSpecialisation}");
        return;
    }

    Console.WriteLine($"\nActive doctors available for specialisation: {selectedSpecialisation}");

    foreach (Doctor doctor in doctors)
    {
        Console.WriteLine(doctor.GetDoctorSummary());
    }
}

// =====================================================
// Case 4: Book Appointment
// =====================================================

static void BookAppointment(
    IPatientService patientService,
    IDoctorService doctorService,
    IAppointmentService appointmentService)
{
    Console.WriteLine("\n========== Book Appointment ==========");

    int patientId = Validations.ReadPositiveInt("Enter Patient ID: ", "Patient ID");

    // If the patient does not exist, PatientNotFoundException is thrown
    // and caught by the main menu catch block.
    patientService.GetPatientById(patientId);

    List<Doctor> matchingDoctors;
    Specialisation selectedSpecialisation;

    while (true)
    {
        Console.WriteLine("\nSelect the department/specialisation you want to visit:");

        selectedSpecialisation = Validations.ReadEnumChoice<Specialisation>("specialisation");

        matchingDoctors = doctorService.SearchDoctorsBySpecialisation(selectedSpecialisation);

        if (matchingDoctors.Count == 0)
        {
            Console.WriteLine($"\nNo active doctors available for {selectedSpecialisation}.");
            Console.WriteLine("Please select another specialisation.");
            continue;
        }

        break;
    }

    Console.WriteLine($"\nAvailable doctors for {selectedSpecialisation}:");

    foreach (Doctor doctor in matchingDoctors)
    {
        Console.WriteLine(doctor.GetDoctorSummary());
    }

    int doctorId;

    while (true)
    {
        doctorId = Validations.ReadPositiveInt("Enter Doctor ID from the above list: ", "Doctor ID");

        bool doctorExistsInDisplayedList = matchingDoctors.Any(d => d.DoctorId == doctorId);

        if (!doctorExistsInDisplayedList)
        {
            Console.WriteLine("Please choose a Doctor ID from the displayed list.");
            continue;
        }

        break;
    }

    DateTime appointmentDate = Validations.ReadDate("Enter appointment date (yyyy-MM-dd): ");

    while (appointmentDate.Date < SystemTime.Now.Date)
    {
        Console.WriteLine("Appointment date cannot be in the past.");
        appointmentDate = Validations.ReadDate("Enter appointment date again (yyyy-MM-dd): ");
    }

    List<TimeSpan> availableSlots = appointmentService.GetAvailableSlotsForDoctor(doctorId, appointmentDate);

    if (availableSlots.Count == 0)
    {
        Console.WriteLine("No available slots for this doctor on the selected date.");
        return;
    }

    Console.WriteLine("\nAvailable time slots:");

    for (int i = 0; i < availableSlots.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {TimeSlots.FormatSlot(availableSlots[i])}");
    }

    int slotChoice = Validations.ReadIntInRange(
        "Choose a slot option: ",
        "Slot option",
        1,
        availableSlots.Count
    );

    TimeSpan selectedSlotStartTime = availableSlots[slotChoice - 1];

    Appointment appointment = appointmentService.BookAppointment(
        patientId,
        doctorId,
        appointmentDate,
        selectedSlotStartTime
    );

    Console.WriteLine("\nAppointment booked successfully.");
    Console.WriteLine(appointment.GetDetails());
}

// =====================================================
// Case 5: View Appointments For Patient
// =====================================================

static void ViewAppointmentsForPatient(IAppointmentService appointmentService)
{
    Console.WriteLine("\n========== View Appointments For Patient ==========");

    int patientId = Validations.ReadPositiveInt("Enter Patient ID: ", "Patient ID");

    List<AppointmentDto> appointments =
        appointmentService.GetAppointmentSummariesByPatient(patientId);

    if (appointments.Count == 0)
    {
        Console.WriteLine("No appointments found for this patient.");
        return;
    }

    Console.WriteLine("\nAppointments:");

    foreach (AppointmentDto appointment in appointments)
    {
        Console.WriteLine(appointment.GetDetails());
    }
}

// =====================================================
// Case 6: Confirm Or Cancel Appointment
// =====================================================

static void ConfirmOrCancelAppointment(IAppointmentService appointmentService)
{
    Console.WriteLine("\n========== Confirm Or Cancel Appointment ==========");

    int patientId = Validations.ReadPositiveInt("Enter Patient ID: ", "Patient ID");

    List<AppointmentDto> pendingAppointments =
        appointmentService.GetPendingAppointmentSummariesByPatient(patientId);

    if (pendingAppointments.Count == 0)
    {
        Console.WriteLine("No pending appointments found for this patient.");
        return;
    }

    Console.WriteLine("\nPending Appointments:");

    foreach (AppointmentDto appointment in pendingAppointments)
    {
        Console.WriteLine(appointment.GetDetails());
    }

    Console.WriteLine("\nEnter 0 to cancel and return to the main menu.");

    int appointmentId = Validations.ReadIntInRange(
        "Enter Appointment ID: ",
        "Appointment ID",
        0,
        int.MaxValue
    );

    if (appointmentId == 0)
    {
        Console.WriteLine("Returning to main menu...");
        return;
    }

    bool appointmentExistsInList =
        pendingAppointments.Any(a => a.AppointmentId == appointmentId);

    while (!appointmentExistsInList)
    {
        Console.WriteLine("Please choose an Appointment ID from the displayed pending appointments.");
        Console.WriteLine("Enter 0 to cancel and return to the main menu.");

        appointmentId = Validations.ReadIntInRange(
            "Enter Appointment ID: ",
            "Appointment ID",
            0,
            int.MaxValue
        );

        if (appointmentId == 0)
        {
            Console.WriteLine("Returning to main menu...");
            return;
        }

        appointmentExistsInList =
            pendingAppointments.Any(a => a.AppointmentId == appointmentId);
    }

    Console.WriteLine("\n1. Confirm appointment");
    Console.WriteLine("2. Cancel appointment");
    Console.WriteLine("0. Return to main menu");

    int actionChoice = Validations.ReadIntInRange(
        "Choose an option: ",
        "Action choice",
        0,
        2
    );

    if (actionChoice == 0)
    {
        Console.WriteLine("Returning to main menu...");
        return;
    }

    if (actionChoice == 1)
    {
        Appointment confirmedAppointment =
            appointmentService.ConfirmAppointment(appointmentId);

        Console.WriteLine("\nAppointment confirmed successfully.");
        Console.WriteLine(confirmedAppointment.GetDetails());
    }
    else
    {
        string reason = Validations.ReadRequiredString(
            "Enter cancellation reason: ",
            "Cancellation reason",
            1
        );

        Appointment cancelledAppointment =
            appointmentService.CancelAppointment(appointmentId, reason);

        Console.WriteLine("\nAppointment cancelled successfully.");
        Console.WriteLine(cancelledAppointment.GetDetails());
    }
}
// =====================================================
// Case 7: Complete Appointment And Add Health Record
// =====================================================

static void CompleteAppointmentAndAddRecord(
    IAppointmentService appointmentService,
    IHealthRecordService healthRecordService)
{
    Console.WriteLine("\n========== Complete Appointment And Add Health Record ==========");

    int doctorId = Validations.ReadPositiveInt("Enter Doctor ID: ", "Doctor ID");

    List<AppointmentDto> todayConfirmedAppointments =
        appointmentService.GetTodayConfirmedAppointmentSummariesByDoctor(doctorId);

    if (todayConfirmedAppointments.Count == 0)
    {
        Console.WriteLine("No confirmed appointments found for this doctor today.");
        return;
    }

    Console.WriteLine($"\nConfirmed Appointments For Today ({SystemTime.Now:yyyy-MM-dd}):");

    foreach (AppointmentDto appointment in todayConfirmedAppointments)
    {
        Console.WriteLine(appointment.GetDetails());
    }

    Console.WriteLine("\nEnter 0 to cancel and return to the main menu.");

    int appointmentId = Validations.ReadIntInRange(
        "Enter Appointment ID: ",
        "Appointment ID",
        0,
        int.MaxValue
    );

    if (appointmentId == 0)
    {
        Console.WriteLine("Returning to main menu...");
        return;
    }

    bool appointmentExistsInList =
        todayConfirmedAppointments.Any(a => a.AppointmentId == appointmentId);

    while (!appointmentExistsInList)
    {
        Console.WriteLine("Please choose an Appointment ID from the displayed confirmed appointments.");
        Console.WriteLine("Enter 0 to cancel and return to the main menu.");

        appointmentId = Validations.ReadIntInRange(
            "Enter Appointment ID: ",
            "Appointment ID",
            0,
            int.MaxValue
        );

        if (appointmentId == 0)
        {
            Console.WriteLine("Returning to main menu...");
            return;
        }

        appointmentExistsInList =
            todayConfirmedAppointments.Any(a => a.AppointmentId == appointmentId);
    }

    Console.WriteLine("\nEnter Health Record Details");

    string diagnosis = Validations.ReadRequiredString("Diagnosis: ", "Diagnosis", 1);

    string prescription = Validations.ReadRequiredString("Prescription: ", "Prescription", 1);

    Console.Write("Notes: ");
    string notes = Console.ReadLine();

    Appointment completedAppointment =
        appointmentService.CompleteAppointment(appointmentId);

    Console.WriteLine("\nAppointment marked as completed.");
    Console.WriteLine(completedAppointment.GetDetails());

    HealthRecord record = healthRecordService.AddRecord(
        appointmentId,
        diagnosis,
        prescription,
        notes
    );

    Console.WriteLine("\nHealth record added successfully.");
    Console.WriteLine(record.GetSummary());
}

// =====================================================
// Case 8: View Health History
// =====================================================

static void ViewHealthHistory(IHealthRecordService healthRecordService)
{
    Console.WriteLine("\n========== View Health History ==========");

    int patientId = Validations.ReadPositiveInt("Enter Patient ID: ", "Patient ID");

    List<HealthRecordDto> records =
        healthRecordService.GetRecordSummariesByPatient(patientId);

    Console.WriteLine("\nHealth History:");

    foreach (HealthRecordDto record in records)
    {
        Console.WriteLine(record.GetSummary());
    }
}
// =====================================================
// Case 9: Manage System Time
// =====================================================

static void ManageSystemTime()
{
    Console.WriteLine("\n========== System Time Management ==========");
    Console.WriteLine("1. Set custom time");
    Console.WriteLine("2. Reset to current time");

    int input = Validations.ReadIntInRange(
        "Choose option: ",
        "System time option",
        1,
        2
    );

    switch (input)
    {
        case 1:
            Console.Write("Enter custom date/time (yyyy-MM-dd HH:mm): ");
            string customInput = Console.ReadLine();

            while (!DateTime.TryParse(customInput, out DateTime _))
            {
                Console.WriteLine("Invalid date/time format.");
                Console.Write("Enter again (yyyy-MM-dd HH:mm): ");
                customInput = Console.ReadLine();
            }

            DateTime customTime = DateTime.Parse(customInput);
            SystemTime.SetCustomTime(customTime);
            Console.WriteLine($"Custom time set: {SystemTime.Now}");
            break;

        case 2:
            SystemTime.Reset();
            Console.WriteLine($"System time reset: {SystemTime.Now}");
            break;
    }
}


