using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{

    public interface IHealthRecordService
    {

        // Add New Health Record
        void AddRecord(
            HealthRecord record);

        // Get Health Record By Id
        HealthRecord? GetRecordById(
            int recordId);

        // Get All Health Records
        List<HealthRecord> GetAllRecords();

        // Get Records By Patient
        List<HealthRecord> GetRecordsByPatient(
            int patientId);

        // Get Records By Doctor
        List<HealthRecord> GetRecordsByDoctor(
            int doctorId);

        // Update Existing Health Record
        void UpdateRecord(
            HealthRecord updatedRecord);

        // Delete Health Record By Id
        void DeleteRecordById(
            int recordId);

        // Create Health Record From Appointment
        public HealthRecord CreateRecordFromAppointment(
            Appointment appointment);
    }
}