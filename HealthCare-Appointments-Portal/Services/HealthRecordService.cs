
using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{

    public class HealthRecordService : IHealthRecordService
    {

        private readonly IHealthRecordRepository
            _healthRecordRepository;

        // Dependency Injection
        public HealthRecordService(
            IHealthRecordRepository
            healthRecordRepository)
        {

            _healthRecordRepository =
                healthRecordRepository;
        }

        // Add New Health Record
        public void AddRecord(
            HealthRecord record)
        {
            bool recordExists =
                _healthRecordRepository
                .GetAllRecords()
                .Any(r =>
                    r.AppointmentId ==
                    record.AppointmentId);

            if (recordExists)
            {
                throw new DuplicateHealthRecordException();
            }

            _healthRecordRepository
                .AddRecord(record);
        }

        // Get Health Record By Id
        public HealthRecord? GetRecordById(
            int recordId)
        {

            HealthRecord? record =
                _healthRecordRepository
                .GetRecordById(recordId);

            if (record == null)
            {

                throw new HealthRecordNotFoundException();
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
                .GetAllRecords()
                .Where(r =>
                    r.Patient.PatientId ==
                    patientId)
                .OrderByDescending(r =>
                    r.VisitDate)
                .ToList();
        }

        // Get Records By Doctor
        public List<HealthRecord>
            GetRecordsByDoctor(
                int doctorId)
        {

            return _healthRecordRepository
                .GetAllRecords()
                .Where(r =>
                    r.Doctor.DoctorId ==
                    doctorId)
                .OrderByDescending(r =>
                    r.VisitDate)
                .ToList();
        }

        // Update Existing Health Record
        public void UpdateRecord(
            HealthRecord updatedRecord)
        {

            HealthRecord? existingRecord =
                _healthRecordRepository
                .GetRecordById(
                    updatedRecord.RecordId);

            if (existingRecord == null)
            {

                throw new HealthRecordNotFoundException();
            }

            _healthRecordRepository
                .UpdateRecord(
                    updatedRecord);
        }

        // Delete Health Record By Id
        public void DeleteRecordById(
            int recordId)
        {

            HealthRecord? record =
                _healthRecordRepository
                .GetRecordById(recordId);

            if (record == null)
            {

                throw new HealthRecordNotFoundException();
            }

            _healthRecordRepository
                .DeleteRecordById(
                    recordId);
        }

        // Create Health Record From Appointment
        public HealthRecord CreateRecordFromAppointment(
            Appointment appointment)
        {
            return new HealthRecord
            {
                Patient =
                    appointment.Patient,

                Doctor =
                    appointment.Doctor,

                VisitDate =
                    appointment.ScheduledDate
            };
        }
    }
}