using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;

        // Dependency Injection
        public HealthRecordService(IHealthRecordRepository repository)
        {
            _repository = repository;
        }

        // Add Record
        public void AddRecord(HealthRecord record)
        {
            ArgumentNullException.ThrowIfNull(record); 

            _repository.AddRecord(record);
        }

        // Get All Records
        public List<HealthRecord> GetAllRecords()
        {
            return _repository.GetAllRecords();
        }

        // Get Records By Patient
        public List<HealthRecord> GetRecordsByPatient(Guid patientId)
        {
            return _repository
                .GetAllRecords()
                .Where(r => r.Patient.PatientId == patientId)
                .ToList();
        }
    }
}
