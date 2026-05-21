using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IHealthRecordService
    {
        void AddRecord(HealthRecord record);

        List<HealthRecord> GetRecordsByPatient(
            Guid patientId);

        List<HealthRecord> GetRecordsByDoctor(
            Guid doctorId);
    }
}