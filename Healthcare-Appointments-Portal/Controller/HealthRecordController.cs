using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Controllers
{
    [ExcludeFromCodeCoverage]
    public class HealthRecordController
    {
        private readonly IHealthRecordService _healthRecordService;

        private readonly IAppointmentService _appointmentService;

        public HealthRecordController(
            IHealthRecordService healthRecordService,
            IAppointmentService appointmentService)
        {
            _healthRecordService =
                healthRecordService;

            _appointmentService =
                appointmentService;
        }

        // Add Health Record
        public void AddHealthRecord()
        {
            List<Appointment> completedAppointments =
                _appointmentService
                .GetCompletedAppointments();

            if (completedAppointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoCompletedAppointmentsFound);

                return;
            }

            Console.WriteLine(
                ConsoleConstants
                .CompletedAppointments);

            UtilityHelper.DisplayAppointmentTable(
                completedAppointments);

            int appointmentId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterAppointmentId);

            Appointment appointmentRecord =
                _appointmentService
                .GetAppointmentById(
                    appointmentId)!;

            HealthRecord record =
                _healthRecordService
                .CreateRecordFromAppointment(
                    appointmentRecord);

            record.Diagnosis =
                UtilityHelper
                .ReadValidatedProperty(
                    ConsoleConstants
                    .EnterDiagnosis,
                    nameof(HealthRecord.Diagnosis),
                    record);

            record.Prescription =
                UtilityHelper
                .ReadValidatedProperty(
                    ConsoleConstants
                    .EnterPrescription,
                    nameof(HealthRecord.Prescription),
                    record);

            record.Notes =
                UtilityHelper
                .ReadInput(
                    ConsoleConstants
                    .EnterNotes);

            _healthRecordService
                .AddRecord(record);

            Console.WriteLine(
                ConsoleConstants
                .HealthRecordAddedSuccessfully);

            UtilityHelper.DisplayHealthRecordTable(
                new List<HealthRecord>
                {
                    record
                });
        }

        // Get Health Record By Id
        public void GetHealthRecordById()
        {
            int recordId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterRecordId);

            HealthRecord record =
                _healthRecordService
                .GetRecordById(
                    recordId)!;

            UtilityHelper.DisplayHealthRecordTable(
                new List<HealthRecord>
                {
                    record
                });
        }

        // View All Health Records
        public void GetAllHealthRecords()
        {
            List<HealthRecord> records =
                _healthRecordService
                .GetAllRecords();

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

        // Get Records By Doctor
        public void GetRecordsByDoctor()
        {
            int doctorId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterDoctorId);

            List<HealthRecord> records =
                _healthRecordService
                .GetRecordsByDoctor(
                    doctorId);

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

        // Update Health Record
        public void UpdateHealthRecord()
        {
            int recordId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterRecordId);

            HealthRecord existingRecord =
                _healthRecordService
                .GetRecordById(
                    recordId)!;

            Console.WriteLine(
                ConsoleConstants
                .CurrentHealthRecordDetails);

            UtilityHelper.DisplayHealthRecordTable(
                new List<HealthRecord>
                {
                    existingRecord
                });

            HealthRecord updatedRecord = new()
            {
                RecordId =
                    existingRecord.RecordId,

                AppointmentId =
                    existingRecord.AppointmentId,

                Patient =
                    existingRecord.Patient,

                Doctor =
                    existingRecord.Doctor,

                Diagnosis =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants
                        .DiagnosisLabel,
                        existingRecord.Diagnosis),

                Prescription =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants
                        .PrescriptionLabel,
                        existingRecord.Prescription),

                Notes =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants
                        .NotesLabel,
                        existingRecord.Notes
                        ?? string.Empty),

                VisitDate =
                    UtilityHelper
                    .ReadOptionalDate(
                        ConsoleConstants
                        .VisitDateLabel,
                        existingRecord.VisitDate)
            };

            _healthRecordService
                .UpdateRecord(
                    updatedRecord);

            Console.WriteLine(
                ConsoleConstants
                .HealthRecordUpdatedSuccessfully);

            Console.WriteLine(
                ConsoleConstants
                .UpdatedHealthRecordDetails);

            HealthRecord updatedRecordDetails =
                _healthRecordService
                .GetRecordById(
                    recordId)!;

            UtilityHelper.DisplayHealthRecordTable(
                new List<HealthRecord>
                {
                    updatedRecordDetails
                });
        }

        // Delete Health Record
        public void DeleteHealthRecord()
        {
            int recordId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterRecordId);

            _healthRecordService
                .DeleteRecordById(
                    recordId);

            Console.WriteLine(
                ConsoleConstants
                .HealthRecordDeletedSuccessfully);
        }
    }
}