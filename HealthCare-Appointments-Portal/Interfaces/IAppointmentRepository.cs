using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
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
