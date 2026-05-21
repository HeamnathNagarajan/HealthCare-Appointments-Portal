using HealthCare_Appointments_portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IAppointmentRepository
    {
        void AddAppointment(Appointment appointment);

        Appointment? GetAppointmentById(Guid appointmentId);

        List<Appointment> GetAllAppointments();

        void UpdateAppointment(Appointment updatedAppointment);

        void DeleteAppointmentById(Guid appointmentId);
    }
}