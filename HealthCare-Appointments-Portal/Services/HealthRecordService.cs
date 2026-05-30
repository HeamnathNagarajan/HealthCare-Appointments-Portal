using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Services
{
    public class HealthRecordService
        : IHealthRecordService
    {
        private readonly
            IHealthRecordRepository
            _healthRecordRepository;

        private readonly
            IAppointmentRepository
            _appointmentRepository;

        // Dependency Injection
        public HealthRecordService(
            IHealthRecordRepository
                healthRecordRepository,

            IAppointmentRepository
                appointmentRepository)
        {
            _healthRecordRepository =
                healthRecordRepository;

            _appointmentRepository =
                appointmentRepository;
        }

        // Add New Health Record
        public void AddRecord(
            HealthRecord record)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    record.AppointmentId);

            // Check Appointment Exists
            if (appointment == null)
            {
                throw new
                    AppointmentNotFoundException();
            }

            // Allow Only Completed Appointment
            if (appointment.Status !=
                AppointmentStatus.Completed)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants
                        .CompletedHealthRecord);
            }

            // Prevent Duplicate Record
            bool recordExists =
                _healthRecordRepository
                .RecordExists(
                    record.AppointmentId);

            if (recordExists)
            {
                throw new
                    DuplicateHealthRecordException();
            }

            _healthRecordRepository
                .AddRecord(record);
        }

        // Get Health Record By Id
        public HealthRecord?
            GetRecordById(
                int recordId)
        {
            HealthRecord? record =
                _healthRecordRepository
                .GetRecordById(
                    recordId);

            if (record == null)
            {
                throw new
                    HealthRecordNotFoundException();
            }

            return record;
        }

        // Get All Health Records
        public List<HealthRecord>
            GetAllRecords()
        {
            return _healthRecordRepository
                .GetAllRecords();
        }

        // Get Records By Patient
        public List<HealthRecord>
            GetRecordsByPatient(
                int patientId)
        {
            return _healthRecordRepository
                .GetRecordsByPatient(
                    patientId);
        }

        // Get Records By Doctor
        public List<HealthRecord>
            GetRecordsByDoctor(
                int doctorId)
        {
            return _healthRecordRepository
                .GetRecordsByDoctor(
                    doctorId);
        }

        // Update Existing Record
        public void UpdateRecord(
            HealthRecord updatedRecord)
        {
            HealthRecord? existingRecord =
                _healthRecordRepository
                .GetRecordById(
                    updatedRecord.RecordId);

            if (existingRecord == null)
            {
                throw new
                    HealthRecordNotFoundException();
            }

            _healthRecordRepository
                .UpdateRecord(
                    updatedRecord);
        }

        // Delete Record By Id
        public void DeleteRecordById(
            int recordId)
        {
            HealthRecord? record =
                _healthRecordRepository
                .GetRecordById(
                    recordId);

            if (record == null)
            {
                throw new
                    HealthRecordNotFoundException();
            }

            _healthRecordRepository
                .DeleteRecordById(
                    recordId);
        }

        // Create Record From Appointment
        public HealthRecord
            CreateRecordFromAppointment(
                Appointment appointment)
        {
            return new HealthRecord
            {
                AppointmentId =
                    appointment.AppointmentId,

                Patient =
                    appointment.Patient,

                Doctor =
                    appointment.Doctor,

                VisitDate =
                    appointment.ScheduledDate
            };
        }

        // Get Completed Appointments
        // Without Health Record
        public List<Appointment>
            GetCompletedAppointmentsWithoutHealthRecord()
        {
            List<int>
                recordedAppointmentIds =
                _healthRecordRepository
                .GetRecordedAppointmentIds();

            return _appointmentRepository
                .GetCompletedAppointments()
                .Where(a =>
                    !recordedAppointmentIds
                    .Contains(
                        a.AppointmentId))
                .ToList();
        }
    }
}
