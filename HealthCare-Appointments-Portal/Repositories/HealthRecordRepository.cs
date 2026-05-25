using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Repositories
{
    public class HealthRecordRepository: IHealthRecordRepository
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
    }
}
