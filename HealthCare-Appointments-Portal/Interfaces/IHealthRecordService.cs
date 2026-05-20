using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{

    public interface IHealthRecordService
    {

        // Add New Health Record
        void AddRecord(
            HealthRecord record);

        // Get Health Record By Id
        HealthRecord? GetRecordById(
            Guid recordId);

        // Get All Health Records
        List<HealthRecord> GetAllRecords();

        // Get Records By Patient
        List<HealthRecord> GetRecordsByPatient(
            Guid patientId);

        // Get Records By Doctor
        List<HealthRecord> GetRecordsByDoctor(
            Guid doctorId);

        // Update Existing Health Record
        void UpdateRecord(
            HealthRecord updatedRecord);

        // Delete Health Record By Id
        void DeleteRecordById(
            Guid recordId);
    }
}
