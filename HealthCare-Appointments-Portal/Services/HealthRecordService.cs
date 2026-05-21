
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{
    public class HealthRecordService
        : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;

        // Dependency Injection
        public HealthRecordService(
            IHealthRecordRepository repository)
        {
            _repository = repository;
        }

        // Add new health record
        public void AddRecord(HealthRecord record)
        {
            _repository.AddRecord(record);
        }

        // Get records by patient
        // Ordered by VisitDate descending
        public List<HealthRecord> GetRecordsByPatient(
            Guid patientId)
        {
            return _repository.GetAllRecords()
                .Where(r =>
                    r.Patient.PatientId == patientId)
                .OrderByDescending(r =>
                    r.VisitDate)
                .ToList();
        }

        // Get records by doctor
        // Ordered by VisitDate descending
        public List<HealthRecord> GetRecordsByDoctor(
            Guid doctorId)
        {
            return _repository.GetAllRecords()
                .Where(r =>
                    r.Doctor.DoctorId == doctorId)
                .OrderByDescending(r =>
                    r.VisitDate)
                .ToList();
        }
    }
}