using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IHealthRecordRepository
    {
        void AddRecord(
            HealthRecord record);

        List<HealthRecord>
            GetAllRecords();

        HealthRecord?
            GetRecordById(
                int recordId);

        bool RecordExists(
            int appointmentId);

        List<HealthRecord>
            GetRecordsByPatient(
                int patientId);

        List<HealthRecord>
            GetRecordsByDoctor(
                int doctorId);

        List<int>
            GetRecordedAppointmentIds();

        void UpdateRecord(
            HealthRecord updatedRecord);

        void DeleteRecordById(
            int recordId);
    }
}