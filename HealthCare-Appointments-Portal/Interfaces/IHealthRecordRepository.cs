using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
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
