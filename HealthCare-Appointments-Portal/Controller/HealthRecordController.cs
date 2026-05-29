using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Utilities;

namespace HealthCare_Appointments_Portal.Controllers
{
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

            foreach (Appointment appointment
                in completedAppointments)
            {
                Console.WriteLine(
                    appointment
                    .GetDetails());
            }

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

            Console.WriteLine(
                record
                .GetSummary());
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

            Console.WriteLine(
                record
                .GetSummary());
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

            foreach (HealthRecord record
                in records)
            {
                Console.WriteLine(
                    record
                    .GetSummary());
            }
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

            foreach (HealthRecord record
                in records)
            {
                Console.WriteLine(
                    record
                    .GetSummary());
            }
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

            Console.WriteLine(
                existingRecord
                .GetSummary());

            HealthRecord updatedRecord = new()
            {
                RecordId =
                    existingRecord.RecordId,

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

            Console.WriteLine(
                _healthRecordService
                .GetRecordById(
                    recordId)!
                .GetSummary());
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