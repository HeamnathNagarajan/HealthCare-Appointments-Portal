using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Controllers
{
    [ExcludeFromCodeCoverage]
    public class PatientController
    {
        private readonly IPatientService _patientService;

        public PatientController(
            IPatientService patientService)
        {
            _patientService =
                patientService;
        }

        // Register Patient
        public void RegisterPatient()
        {
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

            _patientService
                .AddPatient(patient);

            Console.WriteLine(
                ConsoleConstants
                .PatientRegisteredSuccessfully);

            UtilityHelper.DisplayPatientTable(
                new List<Patient>
                {
                    patient
                });
        }

        // View Patient By Id
        public void GetPatientById()
        {
            int patientId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants.EnterPatientId);

            Patient patient =
                _patientService
                .GetPatientById(
                    patientId)!;

            UtilityHelper.DisplayPatientTable(
                new List<Patient>
                {
                    patient
                });
        }

        // View All Patients
        public void GetAllPatients()
        {
            List<Patient> patients =
                _patientService
                .GetAllPatients();

            if (patients.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoPatientsFound);

                return;
            }

            UtilityHelper.DisplayPatientTable(
                patients);
        }

        // Get Patient By Email
        public void GetPatientByEmail()
        {
            string email =
                UtilityHelper
                .ReadInput(
                    ConsoleConstants
                    .EnterPatientEmail);

            Patient patient =
                _patientService
                .GetPatientByEmail(
                    email)!;

            UtilityHelper.DisplayPatientTable(
                new List<Patient>
                {
                    patient
                });
        }

        // Update Patient
        public void UpdatePatient()
        {
            int patientId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterPatientId);

            Patient existingPatient =
                _patientService
                .GetPatientById(
                    patientId)!;

            Console.WriteLine(
                ConsoleConstants
                .CurrentPatientDetails);

            UtilityHelper.DisplayPatientTable(
                new List<Patient>
                {
                    existingPatient
                });

            Patient updatedPatient = new()
            {
                PatientId =
                    existingPatient.PatientId,

                FullName =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants.FullNameLabel,
                        existingPatient.FullName),

                DateOfBirth =
                    UtilityHelper
                    .ReadOptionalDate(
                        ConsoleConstants.DateOfBirthLabel,
                        existingPatient.DateOfBirth),

                Gender =
                    UtilityHelper
                    .ReadOptionalEnum<Gender>(
                        ConsoleConstants.GenderLabel,
                        existingPatient.Gender),

                PhoneNumber =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants.PhoneNumberLabel,
                        existingPatient.PhoneNumber),

                Email =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants.EmailLabel,
                        existingPatient.Email),

                InsuranceId =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants.InsuranceIdLabel,
                        existingPatient.InsuranceId)
            };

            _patientService
                .UpdatePatient(
                    updatedPatient);

            Console.WriteLine(
                ConsoleConstants
                .PatientUpdatedSuccessfully);

            Console.WriteLine(
                ConsoleConstants
                .UpdatedPatientDetails);

            Patient updatedPatientDetails =
                _patientService
                .GetPatientById(
                    patientId)!;

            UtilityHelper.DisplayPatientTable(
                new List<Patient>
                {
                    updatedPatientDetails
                });
        }

        // Delete Patient
        public void DeletePatient()
        {
            int patientId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterPatientId);

            _patientService
                .DeletePatientById(
                    patientId);

            Console.WriteLine(
                ConsoleConstants
                .PatientDeletedSuccessfully);
        }

        // View Patient Appointments
        public void ViewAppointments(
            IAppointmentService appointmentService)
        {
            string patientEmail =
                UtilityHelper
                .ReadInput(
                    ConsoleConstants
                    .EnterPatientEmail);

            Patient patient =
                _patientService
                .GetPatientByEmail(
                    patientEmail)!;

            List<Appointment> appointments =
                appointmentService
                .GetAppointmentsByPatient(
                    patient.PatientId);

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoAppointmentsFound);

                return;
            }

            UtilityHelper.DisplayAppointmentTable(
                appointments);
        }

        // View Patient Health Records
        public void ViewHealthRecords(
            IHealthRecordService healthRecordService)
        {
            string patientEmail =
                UtilityHelper
                .ReadInput(
                    ConsoleConstants
                    .EnterPatientEmail);

            Patient patient =
                _patientService
                .GetPatientByEmail(
                    patientEmail)!;

            List<HealthRecord> records =
                healthRecordService
                .GetRecordsByPatient(
                    patient.PatientId);

            if (records.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoHealthRecordsFound);

                return;
            }

            UtilityHelper.DisplayHealthRecordTable(
                records);
        }
    }
}