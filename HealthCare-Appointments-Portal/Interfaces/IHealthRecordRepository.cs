using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IHealthRecordRepository
    {
        void AddRecord(HealthRecord record);

        List<HealthRecord> GetAllRecords();

        HealthRecord? GetRecordById(Guid recordId);

        void UpdateRecord(HealthRecord updatedRecord);
        void DeleteRecordById(Guid recordId);
    }
}