using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(
            Patient patient,
            Doctor doctor,
            DateOnly date,
            TimeOnly slot);

        void CancelAppointment(
            Guid appointmentId,
            string reason);

        List<Appointment> GetAppointmentsByPatient(
            Guid patientId);

        List<Appointment> GetAppointmentsByDoctor(
            Guid doctorId);

        List<Appointment> GetUpcomingAppointments();

        List<Appointment> GetCompletedAppointments();

    }
}

