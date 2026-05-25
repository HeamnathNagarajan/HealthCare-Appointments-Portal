using HealthcareApp.Data;
using HealthcareApp.Models;

namespace HealthcareApp.Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly List<HealthRecord> _records;
        private int _nextId;

        public HealthRecordRepository(DataStore dataStore)
        {
            _records = dataStore.HealthRecords;

            _nextId = _records.Count==0
                ? 1:
                _records.Max(r => r.RecordId) + 1;
        }

        public void Add(HealthRecord record)
        {
            record.RecordId = _nextId++;
            _records.Add(record);
        }

        public List<HealthRecord> GetAll()
        {
            return _records.ToList();
        }

        public List<HealthRecord> GetByPatientId(int patientId)
        {
            return _records
                .Where(r => r.Patient.PatientId == patientId)
                .ToList();
        }

        public List<HealthRecord> GetByDoctorId(int doctorId)
        {
            return _records
                .Where(r => r.Doctor.DoctorId == doctorId)
                .ToList();
        }

        public List<HealthRecord> GetByAppointmentId(int appointmentId)
        {
            return _records
                .Where(r => r.AppointmentId == appointmentId)
                .ToList();
        }
    }
}