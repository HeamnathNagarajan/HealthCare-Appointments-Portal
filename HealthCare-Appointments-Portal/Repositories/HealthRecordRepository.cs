using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class HealthRecordRepository
        : IHealthRecordRepository
    {
        private readonly
            DataStore _dataStore;

        // Dependency Injection
        public HealthRecordRepository(
            DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // Add New Health Record
        public void AddRecord(
            HealthRecord record)
        {
            _dataStore.HealthRecords
                .Add(record);
        }

        // Get All Health Records
        public List<HealthRecord>
            GetAllRecords()
        {
            return _dataStore.HealthRecords
                .ToList();
        }

        // Get Health Record By Id
        public HealthRecord?
            GetRecordById(
                int recordId)
        {
            return _dataStore.HealthRecords
                .FirstOrDefault(r =>
                    r.RecordId ==
                    recordId);
        }

        // Check Record Exists
        public bool RecordExists(
            int appointmentId)
        {
            return _dataStore.HealthRecords
                .Any(r =>
                    r.AppointmentId ==
                    appointmentId);
        }

        // Get Records By Patient
        public List<HealthRecord>
            GetRecordsByPatient(
                int patientId)
        {
            return _dataStore.HealthRecords
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
            return _dataStore.HealthRecords
                .Where(r =>
                    r.Doctor.DoctorId ==
                    doctorId)
                .OrderByDescending(r =>
                    r.VisitDate)
                .ToList();
        }

        // Get Recorded Appointment Ids
        public List<int>
            GetRecordedAppointmentIds()
        {
            return _dataStore.HealthRecords
                .Select(r =>
                    r.AppointmentId)
                .ToList();
        }

        // Update Existing Health Record
        public void UpdateRecord(
            HealthRecord updatedRecord)
        {
            HealthRecord? existingRecord =
                _dataStore.HealthRecords
                .FirstOrDefault(r =>
                    r.RecordId ==
                    updatedRecord.RecordId);

            if (existingRecord != null)
            {
                existingRecord.Diagnosis =
                    string.IsNullOrWhiteSpace(
                        updatedRecord.Diagnosis)
                    ? existingRecord.Diagnosis
                    : updatedRecord.Diagnosis;

                existingRecord.Prescription =
                    string.IsNullOrWhiteSpace(
                        updatedRecord.Prescription)
                    ? existingRecord.Prescription
                    : updatedRecord.Prescription;

                existingRecord.Notes =
                    string.IsNullOrWhiteSpace(
                        updatedRecord.Notes)
                    ? existingRecord.Notes
                    : updatedRecord.Notes;

                existingRecord.VisitDate =
                    updatedRecord.VisitDate
                        == default
                    ? existingRecord.VisitDate
                    : updatedRecord.VisitDate;
            }
        }

        // Delete Health Record By Id
        public void DeleteRecordById(
            int recordId)
        {
            HealthRecord? record =
                _dataStore.HealthRecords
                .FirstOrDefault(r =>
                    r.RecordId ==
                    recordId);

            if (record != null)
            {
                _dataStore.HealthRecords
                    .Remove(record);
            }
        }
    }
}