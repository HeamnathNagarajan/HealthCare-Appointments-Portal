using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;
using HealthCare_Appointments_Portal.Services;
using HealthCare_Appointments_Portal.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare_Appointments_Portal;

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

        // Resolve DataStore
        DataStore dataStore =
            serviceProvider
            .GetRequiredService<DataStore>();

        // Seed Dummy Data
        DataSeeder.Seed(dataStore);

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine(
                ConsoleConstants.ApplicationTitle);

            Console.WriteLine(
                ConsoleConstants.RegisterPatient);

            Console.WriteLine(
                ConsoleConstants.AddDoctor);

            Console.WriteLine(
                ConsoleConstants.SearchDoctors);

            Console.WriteLine(
                ConsoleConstants.BookAppointment);

            Console.WriteLine(
                ConsoleConstants.ViewAppointments);

            Console.WriteLine(
                ConsoleConstants.ManageAppointments);

            Console.WriteLine(
                ConsoleConstants.AddHealthRecord);

            Console.WriteLine(
                ConsoleConstants.ViewHealthRecords);

            Console.WriteLine(
                ConsoleConstants.Exit);

            int choice =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants.EnterChoice);

            try
            {
                switch (choice)
                {
                    // Register Patient
                    case 1:

                        Patient patient = new();

                        patient.FullName =
                            UtilityHelper
                            .ReadValidatedProperty(
                                ConsoleConstants.EnterFullName,
                                nameof(Patient.FullName),
                                patient);

                        patient.DateOfBirth =
                            UtilityHelper
                            .ReadValidDate(
                                ConsoleConstants.EnterDob,
                                nameof(Patient.DateOfBirth),
                                patient);

                        patient.Gender =
                            UtilityHelper
                            .ReadValidEnum<Gender>(
                                ConsoleConstants.EnterGenderChoice);

                        patient.PhoneNumber =
                            UtilityHelper
                            .ReadValidatedProperty(
                                ConsoleConstants.EnterPhoneNumber,
                                nameof(Patient.PhoneNumber),
                                patient);

                        patient.Email =
                            UtilityHelper
                            .ReadValidatedProperty(
                                ConsoleConstants.EnterEmail,
                                nameof(Patient.Email),
                                patient);

                        patient.InsuranceId =
                            UtilityHelper
                            .ReadValidatedProperty(
                                ConsoleConstants.EnterInsuranceId,
                                nameof(Patient.InsuranceId),
                                patient);

                        patientService
                            .AddPatient(patient);

                        Console.WriteLine(
                            ConsoleConstants
                            .PatientRegisteredSuccessfully);

                        Console.WriteLine(
                            patient.GetProfileSummary());

                        break;

                    // Add Doctor
                    case 2:

                        Doctor doctor = new();

                        doctor.FullName =
                            UtilityHelper
                            .ReadValidatedProperty(
                                ConsoleConstants.EnterFullName,
                                nameof(Doctor.FullName),
                                doctor);

                        doctor.Specialisation =
                            UtilityHelper
                            .ReadValidEnum<Specialisation>(
                                ConsoleConstants.EnterSpecialisationChoice);

                        doctor.YearsOfExperience =
                            UtilityHelper
                            .ReadValidInt(
                                ConsoleConstants.EnterYearsOfExperience);

                        doctor.ConsultationFee =
                            UtilityHelper
                            .ReadValidDecimal(
                                ConsoleConstants.EnterConsultationFee);

                        doctor.IsActive = true;

                        if (!UtilityHelper
                            .ValidateModel(doctor))
                        {
                            break;
                        }

                        doctorService
                            .AddDoctor(doctor);

                        Console.WriteLine(
                            ConsoleConstants
                            .DoctorAddedSuccessfully);

                        Console.WriteLine(
                            doctor.GetDoctorSummary());

                        break;

                    // Search Doctors
                    case 3:

                        Specialisation specialisation =
                            UtilityHelper
                            .ReadValidEnum<Specialisation>(
                                ConsoleConstants.EnterSpecialisationChoice);

                        List<Doctor> doctors =
                            doctorService
                            .GetDoctorsBySpecialisation(
                                specialisation);

                        Console.WriteLine(
                            ConsoleConstants
                            .AvailableDoctors);

                        foreach (Doctor d in doctors)
                        {
                            Console.WriteLine(
                                d.GetDoctorSummary());
                        }

                        break;

                    // Book Appointment
                    case 4:

                        string patientEmail =
                            UtilityHelper
                            .ReadInput(
                                ConsoleConstants
                                .EnterPatientEmail);

                        Patient? existingPatient =
                            patientService
                            .GetPatientByEmail(
                                patientEmail);

                        Specialisation selectedAppointmentSpecialisation =
                            UtilityHelper
                            .ReadValidEnum<Specialisation>(
                                ConsoleConstants.EnterSpecialisationChoice);

                        List<Doctor> availableDoctors =
                            doctorService
                            .GetDoctorsBySpecialisation(
                                selectedAppointmentSpecialisation);

                        if (!availableDoctors.Any())
                        {
                            Console.WriteLine(
                                ConsoleConstants
                                .NoDoctorsAvailable);

                            break;
                        }

                        Console.WriteLine(
                            ConsoleConstants
                            .AvailableDoctors);

                        foreach (Doctor d in availableDoctors)
                        {
                            Console.WriteLine(
                                d.GetDoctorSummary());
                        }

                        int doctorId =
                            UtilityHelper
                            .ReadValidInt(
                                ConsoleConstants.EnterDoctorId);

                        Doctor? existingDoctor =
                            doctorService
                            .GetDoctorById(
                                doctorId);

                        // Temporary Appointment Object
                        Appointment appointmentModel = new()
                        {
                            Patient =
                                existingPatient!,

                            Doctor =
                                existingDoctor!
                        };

                        DateOnly scheduledDate =
                            UtilityHelper
                            .ReadValidDate(
                                ConsoleConstants.EnterAppointmentDate,
                                nameof(Appointment.ScheduledDate),
                                appointmentModel);

                        TimeOnly timeSlot =
                            UtilityHelper
                            .ReadValidTime(
                                ConsoleConstants.EnterTimeSlot);

                        Appointment appointment =
                            appointmentService
                            .BookAppointment(
                                existingPatient!,
                                existingDoctor!,
                                scheduledDate,
                                timeSlot);

                        Console.WriteLine(
                            ConsoleConstants
                            .AppointmentBookedSuccessfully);

                        Console.WriteLine(
                            appointment.GetDetails());

                        break;

                    // View Patient Appointments
                    case 5:

                        string appointmentEmail =
                            UtilityHelper
                            .ReadInput(
                                ConsoleConstants
                                .EnterPatientEmail);

                        Patient? appointmentPatient =
                            patientService
                            .GetPatientByEmail(
                                appointmentEmail);

                        List<Appointment> appointments =
                            appointmentService
                            .GetAppointmentsByPatient(
                                appointmentPatient!
                                .PatientId);

                        if (!appointments.Any())
                        {
                            Console.WriteLine(
                                ConsoleConstants
                                .NoAppointmentsFound);
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

                    // Confirm Cancel Complete Appointment
                    case 6:

                        List<Appointment> allAppointments =
                            appointmentService
                            .GetAllAppointments();

                        if (!allAppointments.Any())
                        {
                            Console.WriteLine(
                                ConsoleConstants
                                .NoAppointmentsAvailable);

                            break;
                        }

                        Console.WriteLine(
                            ConsoleConstants
                            .Appointments);

                        foreach (Appointment a in allAppointments)
                        {
                            Console.WriteLine(
                                a.GetDetails());
                        }

                        int appointmentId =
                            int.Parse(
                                UtilityHelper
                                .ReadInput(
                                    ConsoleConstants
                                    .EnterAppointmentId));

                        Console.WriteLine(
                            ConsoleConstants
                            .ConfirmAppointment);

                        Console.WriteLine(
                            ConsoleConstants
                            .CancelAppointment);

                        Console.WriteLine(
                            ConsoleConstants
                            .CompleteAppointment);

                        int option =
                            UtilityHelper
                            .ReadValidInt(
                                ConsoleConstants
                                .EnterChoice);

                        switch (option)
                        {
                            case 1:

                                appointmentService
                                    .ConfirmAppointment(
                                        appointmentId);

                                Console.WriteLine(
                                    ConsoleConstants
                                    .AppointmentConfirmedSuccessfully);

                                break;

                            case 2:

                                string reason =
                                    UtilityHelper
                                    .ReadInput(
                                        ConsoleConstants
                                        .EnterCancellationReason);

                                appointmentService
                                    .CancelAppointment(
                                        appointmentId,
                                        reason);

                                Console.WriteLine(
                                    ConsoleConstants
                                    .AppointmentCancelledSuccessfully);

                                break;

                            case 3:

                                appointmentService
                                    .CompleteAppointment(
                                        appointmentId);

                                Console.WriteLine(
                                    ConsoleConstants
                                    .AppointmentCompletedSuccessfully);

                                break;

                            default:

                                Console.WriteLine(
                                    ConsoleConstants
                                    .InvalidChoice);

                                break;
                        }

                        break;

                    // Add Health Record
                    case 7:

                        List<Appointment> completedAppointments =
                            appointmentService
                            .GetCompletedAppointments();

                        if (!completedAppointments.Any())
                        {
                            Console.WriteLine(
                                ConsoleConstants
                                .NoCompletedAppointmentsFound);

                            break;
                        }

                        Console.WriteLine(
                            ConsoleConstants
                            .CompletedAppointments);

                        foreach (Appointment a in completedAppointments)
                        {
                            Console.WriteLine(
                                a.GetDetails());
                        }

                        int appointmentRecordId =
                            int.Parse(
                                UtilityHelper
                                .ReadInput(
                                    ConsoleConstants
                                    .EnterAppointmentId));

                        Appointment? appointmentRecord =
                            appointmentService
                            .GetAppointmentById(
                                appointmentRecordId);

                        if (appointmentRecord != null)
                        {
                            HealthRecord record =
                                healthRecordService
                                .CreateRecordFromAppointment(
                                    appointmentRecord);

                            record.Diagnosis =
                                UtilityHelper
                                .ReadValidatedProperty(
                                    ConsoleConstants.EnterDiagnosis,
                                    nameof(HealthRecord.Diagnosis),
                                    record);

                            record.Prescription =
                                UtilityHelper
                                .ReadValidatedProperty(
                                    ConsoleConstants.EnterPrescription,
                                    nameof(HealthRecord.Prescription),
                                    record);

                            record.Notes =
                                UtilityHelper
                                .ReadInput(
                                    ConsoleConstants
                                    .EnterNotes);

                            healthRecordService
                                .AddRecord(record);

                            Console.WriteLine(
                                ConsoleConstants
                                .HealthRecordAddedSuccessfully);

                            Console.WriteLine(
                                record.GetSummary());
                        }

                        break;

                    // View Patient Health Records
                    case 8:

                        string healthEmail =
                            UtilityHelper
                            .ReadInput(
                                ConsoleConstants
                                .EnterPatientEmail);

                        Patient? healthPatient =
                            patientService
                            .GetPatientByEmail(
                                healthEmail);

                        List<HealthRecord> records =
                            healthRecordService
                            .GetRecordsByPatient(
                                healthPatient!
                                .PatientId);

                        if (!records.Any())
                        {
                            Console.WriteLine(
                                ConsoleConstants
                                .NoHealthRecordsFound);
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

                    
                    // Exit
                    case 9:

                        exit = true;

                        Console.WriteLine(
                            ConsoleConstants
                            .ApplicationClosed);

                        break;

                    default:

                        Console.WriteLine(
                            ConsoleConstants
                            .InvalidChoice);

                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .InvalidInputFormat);
            }
            catch (OverflowException)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .InvalidRange);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .InputCannotBeEmpty);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.OperationFailed}{ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.RecordNotFound}{ex.Message}");
            }
            catch (DuplicatePatientException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.DuplicatePatient}{ex.Message}");
            }
            catch (DuplicateDoctorException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.DuplicateDoctor}{ex.Message}");
            }
            catch (PatientNotFoundException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.PatientError}{ex.Message}");
            }
            catch (DoctorNotFoundException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.DoctorError}{ex.Message}");
            }
            catch (AppointmentNotFoundException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.AppointmentError}{ex.Message}");
            }
            catch (AppointmentConflictException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.AppointmentConflict}{ex.Message}");
            }
            catch (DoctorUnavailableException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.DoctorUnavailable}{ex.Message}");
            }
            catch (PastDateException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.PastDateError}{ex.Message}");
            }
            catch (HealthRecordNotFoundException ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.HealthRecordError}{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"{ConsoleConstants.UnexpectedError}{ex.Message}");
            }
        }
    }
}