using HealthcareApp.Enums;
using HealthcareApp.Models;

namespace HealthcareApp.Repositories
{
    public interface IAppointmentRepository
    {
        void Add(Appointment appointment);

        Appointment GetById(int appointmentId);

        List<Appointment> GetAll();

        List<Appointment> GetByPatientId(int patientId);

        List<Appointment> GetByDoctorId(int doctorId);

        List<Appointment> GetByStatus(AppointmentStatus status);

        void Update(Appointment appointment);
    }
}