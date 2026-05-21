using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly DataStore _dataStore;

        public HealthRecordRepository(DataStore data)
        {
            _dataStore = data;
        }

        public void AddRecord(HealthRecord record)
        {

            _dataStore.HealthRecords.Add(record);
        }

        public List<HealthRecord> GetAllRecords()
        {

            return _dataStore.HealthRecords;
        }

        public HealthRecord? GetRecordById(Guid recordId)
        {

            return _dataStore.HealthRecords
                .FirstOrDefault(r =>
                    r.RecordId == recordId);
        }

        public void UpdateRecord(HealthRecord updatedRecord)
        {

            HealthRecord? existingRecord =
                _dataStore.HealthRecords
                .FirstOrDefault(r =>
                    r.RecordId == updatedRecord.RecordId);

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
                    updatedRecord.VisitDate == default
                    ? existingRecord.VisitDate
                    : updatedRecord.VisitDate;
            }
        }

        public void DeleteRecordById(Guid recordId)
        {

            HealthRecord? record =
                _dataStore.HealthRecords
                .FirstOrDefault(r =>
                    r.RecordId == recordId);

            if (record != null)
            {
                _dataStore.HealthRecords.Remove(record);
            }
        }
    }
}