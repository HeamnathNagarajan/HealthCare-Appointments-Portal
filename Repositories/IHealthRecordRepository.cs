using HealthcareApp.Models;

namespace HealthcareApp.Repositories
{
    public interface IHealthRecordRepository
    {
        void Add(HealthRecord record);

        List<HealthRecord> GetAll();

        List<HealthRecord> GetByPatientId(int patientId);

        List<HealthRecord> GetByDoctorId(int doctorId);

        List<HealthRecord> GetByAppointmentId(int appointmentId);
    }
}
