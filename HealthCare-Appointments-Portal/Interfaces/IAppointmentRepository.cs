using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IAppointmentRepository
    {
        void AddAppointment(
            Appointment appointment);

        Appointment? GetAppointmentById(
            int appointmentId);

        List<Appointment>
            GetAllAppointments();

        List<Appointment>
            GetAppointmentsByPatient(
                int patientId);

        List<Appointment>
            GetAppointmentsByDoctor(
                int doctorId);

        List<Appointment>
            GetUpcomingAppointments();

        List<Appointment>
            GetCompletedAppointments();

        Appointment?
            GetConflictingAppointment(
                int doctorId,
                DateOnly date,
                TimeOnly slot);

        void UpdateAppointment(
            Appointment updatedAppointment);

        void DeleteAppointmentById(
            int appointmentId);
    }
}