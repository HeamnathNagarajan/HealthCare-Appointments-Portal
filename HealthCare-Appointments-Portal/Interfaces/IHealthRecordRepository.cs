using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{

    public interface IHealthRecordRepository
    {

        void AddRecord(HealthRecord record);

        List<HealthRecord> GetAllRecords();

        HealthRecord? GetRecordById(int recordId);

        void UpdateRecord(HealthRecord updatedRecord);

        void DeleteRecordById(int recordId);
    }
}
