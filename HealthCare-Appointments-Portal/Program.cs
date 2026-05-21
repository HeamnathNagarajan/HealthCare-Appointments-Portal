using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Utilities;
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
                ConsoleConstants.ManagementModules);

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

                    // MANAGEMENT MODULES
                    case 9:

                        bool managementExit = false;

                        while (!managementExit)
                        {
                            Console.WriteLine(
                                ConsoleConstants.ManagementModuleTitle);

                            Console.WriteLine(
                                ConsoleConstants.PatientManagement);

                            Console.WriteLine(
                                ConsoleConstants.DoctorManagement);

                            Console.WriteLine(
                                ConsoleConstants.AppointmentManagement);

                            Console.WriteLine(
                                ConsoleConstants.HealthRecordManagement);

                            Console.WriteLine(
                                ConsoleConstants.Back);

                            int managementChoice =
                                UtilityHelper
                                .ReadValidInt(
                                    ConsoleConstants.EnterChoice);

                            switch (managementChoice)
                            {
                                // PATIENT MANAGEMENT
                                case 1:

                                    Console.WriteLine(
                                        ConsoleConstants.PatientManagementTitle);

                                    Console.WriteLine(
                                        ConsoleConstants.GetPatientById);

                                    Console.WriteLine(
                                        ConsoleConstants.ViewAllPatients);

                                    Console.WriteLine(
                                        ConsoleConstants.GetPatientByEmail);

                                    Console.WriteLine(
                                        ConsoleConstants.UpdatePatient);

                                    Console.WriteLine(
                                        ConsoleConstants.DeletePatient);

                                    int patientChoice =
                                        UtilityHelper
                                        .ReadValidInt(
                                            ConsoleConstants.EnterChoice);

                                    switch (patientChoice)
                                    {
                                        case 1:

                                            int patientId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterPatientId);

                                            Patient selectedPatient =
                                                patientService
                                                .GetPatientById(
                                                    patientId)!;

                                            Console.WriteLine(
                                                selectedPatient
                                                .GetProfileSummary());

                                            break;

                                        case 2:

                                            List<Patient> patients =
                                                patientService
                                                .GetAllPatients();

                                            foreach (Patient p in patients)
                                            {
                                                Console.WriteLine(
                                                    p.GetProfileSummary());
                                            }

                                            break;

                                        case 3:

                                            string managementPatientEmail =
                                                UtilityHelper
                                                .ReadInput(
                                                    ConsoleConstants.EnterPatientEmail);

                                            Patient patientByEmail =
                                                patientService
                                                .GetPatientByEmail(
                                                    managementPatientEmail)!;

                                            Console.WriteLine(
                                                patientByEmail
                                                .GetProfileSummary());

                                            break;

                                        case 4:

                                            int updatePatientId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterPatientId);

                                            Patient existingUpdatePatient =
                                                patientService
                                                .GetPatientById(
                                                    updatePatientId)!;

                                            Console.WriteLine(ConsoleConstants.CurrentPatientDetails);
                                            Console.WriteLine(
                                                existingUpdatePatient.GetProfileSummary());

                                            Patient updatedPatient = new()
                                            {
                                                PatientId = existingUpdatePatient.PatientId,

                                                FullName = UtilityHelper.ReadOptionalString(
                                                    ConsoleConstants.FullNameLabel,
                                                    existingUpdatePatient.FullName),

                                                DateOfBirth = UtilityHelper.ReadOptionalDate(
                                                    ConsoleConstants.DateOfBirthLabel,
                                                    existingUpdatePatient.DateOfBirth),

                                                Gender = UtilityHelper.ReadOptionalEnum<Gender>(
                                                    ConsoleConstants.GenderLabel,
                                                    existingUpdatePatient.Gender),

                                                PhoneNumber = UtilityHelper.ReadOptionalString(
                                                    ConsoleConstants.PhoneNumberLabel,
                                                    existingUpdatePatient.PhoneNumber),

                                                Email = UtilityHelper.ReadOptionalString(
                                                    ConsoleConstants.EmailLabel,
                                                    existingUpdatePatient.Email),

                                                InsuranceId = UtilityHelper.ReadOptionalString(
                                                    ConsoleConstants.InsuranceIdLabel,
                                                    existingUpdatePatient.InsuranceId)
                                            };

                                            patientService
                                                .UpdatePatient(
                                                    updatedPatient);

                                            Console.WriteLine(
                                                ConsoleConstants.PatientUpdatedSuccessfully);

                                            Console.WriteLine(ConsoleConstants.UpdatedPatientDetails);
                                            Console.WriteLine(
                                                patientService
                                                .GetPatientById(updatePatientId)!
                                                .GetProfileSummary());

                                            break;

                                        case 5:

                                            int deletePatientId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterPatientId);

                                            patientService
                                                .DeletePatientById(
                                                    deletePatientId);

                                            Console.WriteLine(
                                                ConsoleConstants.PatientDeletedSuccessfully);

                                            break;

                                        default:

                                            Console.WriteLine(
                                                ConsoleConstants.InvalidChoice);

                                            break;
                                    }

                                    break;

                                // DOCTOR MANAGEMENT
                                case 2:

                                    Console.WriteLine(
                                       ConsoleConstants.DoctorManagementTitle);

                                    Console.WriteLine(
                                        ConsoleConstants.GetDoctorById);

                                    Console.WriteLine(
                                        ConsoleConstants.ViewAllDoctors);

                                    Console.WriteLine(
                                        ConsoleConstants.GetAvailableDoctors);

                                    Console.WriteLine(
                                        ConsoleConstants.UpdateDoctor);

                                    Console.WriteLine(
                                        ConsoleConstants.DeleteDoctor);

                                    int doctorChoice =
                                        UtilityHelper
                                        .ReadValidInt(
                                            ConsoleConstants.EnterChoice);

                                    switch (doctorChoice)
                                    {
                                        case 1:

                                            int managementDoctorId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterDoctorId);

                                            Doctor selectedDoctor =
                                                doctorService
                                                .GetDoctorById(
                                                    managementDoctorId)!;

                                            Console.WriteLine(
                                                selectedDoctor
                                                .GetDoctorSummary());

                                            break;

                                        case 2:

                                            List<Doctor> allDoctors =
                                                doctorService
                                                .GetAllDoctors();

                                            foreach (Doctor d in allDoctors)
                                            {
                                                Console.WriteLine(
                                                    d.GetDoctorSummary());
                                            }

                                            break;

                                        case 3:

                                            Specialisation managementSpecialisation =
                                                UtilityHelper
                                                .ReadValidEnum<Specialisation>(
                                                    ConsoleConstants.EnterSpecialisationChoice);

                                            List<Doctor> managementAvailableDoctors =
                                                doctorService
                                                .GetAvailableDoctorsBySpecialisation(
                                                    managementSpecialisation);

                                            foreach (Doctor d in managementAvailableDoctors)
                                            {
                                                Console.WriteLine(
                                                    d.GetDoctorSummary());
                                            }

                                            break;

                                        case 4:

                                            int updateDoctorId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterDoctorId);

                                            Doctor existingUpdateDoctor =
                                                doctorService
                                                .GetDoctorById(
                                                    updateDoctorId)!;

                                            Console.WriteLine(ConsoleConstants.CurrentDoctorDetails);
                                            Console.WriteLine(
                                                existingUpdateDoctor.GetDoctorSummary());

                                            Doctor updatedDoctor = new()
                                            {
                                                DoctorId = existingUpdateDoctor.DoctorId,

                                                FullName = UtilityHelper.ReadOptionalString(
                                                    ConsoleConstants.DoctorNameLabel,
                                                    existingUpdateDoctor.FullName),

                                                Specialisation = UtilityHelper.ReadOptionalEnum<Specialisation>(
                                                    ConsoleConstants.SpecialisationLabel,
                                                    existingUpdateDoctor.Specialisation),

                                                YearsOfExperience = UtilityHelper.ReadOptionalInt(
                                                    ConsoleConstants.YearsOfExperienceLabel,
                                                    existingUpdateDoctor.YearsOfExperience),

                                                ConsultationFee = UtilityHelper.ReadOptionalDecimal(
                                                    ConsoleConstants.ConsultationFeeLabel,
                                                    existingUpdateDoctor.ConsultationFee),

                                                IsActive = UtilityHelper.ReadOptionalBool(
                                                   ConsoleConstants.IsActiveLabel,
                                                    existingUpdateDoctor.IsActive)
                                            };

                                            doctorService
                                                .UpdateDoctor(
                                                    updatedDoctor);

                                            Console.WriteLine(
                                                ConsoleConstants.DoctorUpdatedSuccessfully);

                                            Console.WriteLine(ConsoleConstants.UpdatedDoctorDetails);
                                            Console.WriteLine(
                                                doctorService
                                                .GetDoctorById(updateDoctorId)!
                                                .GetDoctorSummary());

                                            break;

                                        case 5:

                                            int deleteDoctorId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterDoctorId);

                                            doctorService
                                                .DeleteDoctorById(
                                                    deleteDoctorId);

                                            Console.WriteLine(
                                                ConsoleConstants.DoctorDeletedSuccessfully);

                                            break;

                                        default:

                                            Console.WriteLine(
                                                ConsoleConstants.InvalidChoice);

                                            break;
                                    }

                                    break;

                                // APPOINTMENT MANAGEMENT
                                case 3:

                                    Console.WriteLine(
                                        ConsoleConstants.AppointmentManagementTitle);

                                    Console.WriteLine(
                                        ConsoleConstants.GetAppointmentById);

                                    Console.WriteLine(
                                        ConsoleConstants.ViewAllAppointments);

                                    Console.WriteLine(
                                        ConsoleConstants.GetAppointmentsByPatient);

                                    Console.WriteLine(
                                        ConsoleConstants.GetAppointmentsByDoctor);

                                    Console.WriteLine(
                                        ConsoleConstants.GetUpcomingAppointments);

                                    Console.WriteLine(
                                        ConsoleConstants.GetCompletedAppointments);

                                    Console.WriteLine(
                                        ConsoleConstants.UpdateAppointment);

                                    Console.WriteLine(
                                        ConsoleConstants.DeleteAppointment);

                                    int appointmentChoice =
                                        UtilityHelper
                                        .ReadValidInt(
                                            ConsoleConstants.EnterChoice);

                                    switch (appointmentChoice)
                                    {
                                        case 1:

                                            int managementAppointmentId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterAppointmentId);

                                            Appointment selectedAppointment =
                                                appointmentService
                                                .GetAppointmentById(
                                                    managementAppointmentId)!;

                                            Console.WriteLine(
                                                selectedAppointment
                                                .GetDetails());

                                            break;

                                        case 2:

                                            List<Appointment> managementAppointments =
                                                appointmentService
                                                .GetAllAppointments();

                                            foreach (Appointment a in managementAppointments)
                                            {
                                                Console.WriteLine(
                                                    a.GetDetails());
                                            }

                                            break;

                                        case 3:

                                            int appointmentPatientId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterPatientId);

                                            List<Appointment> patientAppointments =
                                                appointmentService
                                                .GetAppointmentsByPatient(
                                                    appointmentPatientId);

                                            foreach (Appointment a in patientAppointments)
                                            {
                                                Console.WriteLine(
                                                    a.GetDetails());
                                            }

                                            break;

                                        case 4:

                                            int appointmentDoctorId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterDoctorId);

                                            List<Appointment> doctorAppointments =
                                                appointmentService
                                                .GetAppointmentsByDoctor(
                                                    appointmentDoctorId);

                                            foreach (Appointment a in doctorAppointments)
                                            {
                                                Console.WriteLine(
                                                    a.GetDetails());
                                            }

                                            break;

                                        case 5:

                                            List<Appointment> upcomingAppointments =
                                                appointmentService
                                                .GetUpcomingAppointments();

                                            foreach (Appointment a in upcomingAppointments)
                                            {
                                                Console.WriteLine(
                                                    a.GetDetails());
                                            }

                                            break;

                                        case 6:

                                            List<Appointment> managementCompletedAppointments =
                                                appointmentService
                                                .GetCompletedAppointments();

                                            foreach (Appointment a in managementCompletedAppointments)
                                            {
                                                Console.WriteLine(
                                                    a.GetDetails());
                                            }

                                            break;

                                        case 7:

                                            int updateAppointmentId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterAppointmentId);

                                            Appointment existingUpdateAppointment =
                                                appointmentService
                                                .GetAppointmentById(
                                                    updateAppointmentId)!;

                                            Console.WriteLine(ConsoleConstants.CurrentAppointmentDetails);
                                            Console.WriteLine(
                                                existingUpdateAppointment.GetDetails());

                                            Appointment updatedAppointment = new()
                                            {
                                                AppointmentId = existingUpdateAppointment.AppointmentId,

                                                Patient = existingUpdateAppointment.Patient,

                                                Doctor = existingUpdateAppointment.Doctor,

                                                ScheduledDate = UtilityHelper.ReadOptionalDate(
                                                    ConsoleConstants.ScheduledDateLabel,
                                                    existingUpdateAppointment.ScheduledDate),

                                                TimeSlot = UtilityHelper.ReadOptionalTime(
                                                    ConsoleConstants.TimeSlotLabel,
                                                    existingUpdateAppointment.TimeSlot),

                                                Status = UtilityHelper.ReadOptionalEnum<AppointmentStatus>(
                                                    ConsoleConstants.AppointmentStatusLabel,
                                                    existingUpdateAppointment.Status),

                                                CancellationReason = UtilityHelper.ReadOptionalString(
                                                    ConsoleConstants.CancellationReasonLabel,
                                                    existingUpdateAppointment.CancellationReason ?? string.Empty)
                                            };

                                            appointmentService
                                                .UpdateAppointment(
                                                    updatedAppointment);

                                            Console.WriteLine(
                                                ConsoleConstants.AppointmentUpdatedSuccessfully);

                                            Console.WriteLine(ConsoleConstants.UpdatedAppointmentDetails);
                                            Console.WriteLine(
                                                appointmentService
                                                .GetAppointmentById(updateAppointmentId)!
                                                .GetDetails());

                                            break;

                                        case 8:

                                            int deleteAppointmentId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterAppointmentId);

                                            appointmentService
                                                .DeleteAppointmentById(
                                                    deleteAppointmentId);

                                            Console.WriteLine(
                                                ConsoleConstants.AppointmentDeletedSuccessfully);

                                            break;

                                        default:

                                            Console.WriteLine(
                                                ConsoleConstants.InvalidChoice);

                                            break;
                                    }

                                    break;

                                // HEALTH RECORD MANAGEMENT
                                case 4:

                                    Console.WriteLine(
                                        ConsoleConstants.HealthRecordManagementTitle);

                                    Console.WriteLine(
                                        ConsoleConstants.GetHealthRecordById);

                                    Console.WriteLine(
                                        ConsoleConstants.ViewAllHealthRecords);

                                    Console.WriteLine(
                                        ConsoleConstants.GetRecordsByDoctor);

                                    Console.WriteLine(
                                        ConsoleConstants.UpdateHealthRecord);

                                    Console.WriteLine(
                                        ConsoleConstants.DeleteHealthRecord);

                                    int healthChoice =
                                        UtilityHelper
                                        .ReadValidInt(
                                            ConsoleConstants.EnterChoice);

                                    switch (healthChoice)
                                    {
                                        case 1:

                                            int recordId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterRecordId);

                                            HealthRecord selectedRecord =
                                                healthRecordService
                                                .GetRecordById(
                                                    recordId)!;

                                            Console.WriteLine(
                                                selectedRecord
                                                .GetSummary());

                                            break;

                                        case 2:

                                            List<HealthRecord> allRecords =
                                                healthRecordService
                                                .GetAllRecords();

                                            foreach (HealthRecord r in allRecords)
                                            {
                                                Console.WriteLine(
                                                    r.GetSummary());
                                            }

                                            break;

                                        case 3:

                                            int recordDoctorId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterDoctorId);

                                            List<HealthRecord> doctorRecords =
                                                healthRecordService
                                                .GetRecordsByDoctor(
                                                    recordDoctorId);

                                            foreach (HealthRecord r in doctorRecords)
                                            {
                                                Console.WriteLine(
                                                    r.GetSummary());
                                            }

                                            break;

                                            case 4:

                                                int updateRecordId =
                                                    UtilityHelper
                                                    .ReadValidInt(
                                                        ConsoleConstants.EnterRecordId);

                                                HealthRecord existingUpdateRecord =
                                                    healthRecordService
                                                    .GetRecordById(
                                                        updateRecordId)!;

                                                Console.WriteLine(
                                                    ConsoleConstants.CurrentHealthRecordDetails);

                                                Console.WriteLine(
                                                    existingUpdateRecord
                                                    .GetSummary());

                                                HealthRecord updatedRecord = new()
                                                {
                                                    RecordId =
                                                        existingUpdateRecord.RecordId,

                                                    Patient =
                                                        existingUpdateRecord.Patient,

                                                    Doctor =
                                                        existingUpdateRecord.Doctor,

                                                    Diagnosis =
                                                        UtilityHelper
                                                        .ReadOptionalString(
                                                            ConsoleConstants.DiagnosisLabel,
                                                            existingUpdateRecord.Diagnosis),

                                                    Prescription =
                                                        UtilityHelper
                                                        .ReadOptionalString(
                                                            ConsoleConstants.PrescriptionLabel,
                                                            existingUpdateRecord.Prescription),

                                                    Notes =
                                                        UtilityHelper
                                                        .ReadOptionalString(
                                                            ConsoleConstants.NotesLabel,
                                                            existingUpdateRecord.Notes
                                                            ?? string.Empty),

                                                    VisitDate =
                                                        UtilityHelper
                                                        .ReadOptionalDate(
                                                            ConsoleConstants.VisitDateLabel,
                                                            existingUpdateRecord.VisitDate)
                                                };

                                                healthRecordService
                                                    .UpdateRecord(
                                                        updatedRecord);

                                                Console.WriteLine(
                                                    ConsoleConstants.HealthRecordUpdatedSuccessfully);

                                                Console.WriteLine(
                                                    ConsoleConstants.UpdatedHealthRecordDetails);

                                                Console.WriteLine(
                                                    healthRecordService
                                                    .GetRecordById(
                                                        updateRecordId)!
                                                    .GetSummary());

                                        break;


                                        case 5:

                                            int deleteRecordId =
                                                UtilityHelper
                                                .ReadValidInt(
                                                    ConsoleConstants.EnterRecordId);

                                            healthRecordService
                                                .DeleteRecordById(
                                                    deleteRecordId);

                                            Console.WriteLine(
                                                ConsoleConstants.HealthRecordDeletedSuccessfully);

                                            break;

                                        default:

                                            Console.WriteLine(
                                                ConsoleConstants.InvalidChoice);

                                            break;
                                    }

                                    break;

                                case 5:

                                    managementExit = true;

                                    break;

                                default:

                                    Console.WriteLine(
                                        ConsoleConstants.InvalidChoice);

                                    break;
                            }
                        }

                        break;

                    // Exit
                    case 10:

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
