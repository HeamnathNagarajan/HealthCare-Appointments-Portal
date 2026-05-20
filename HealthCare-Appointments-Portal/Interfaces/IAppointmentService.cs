using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{

    public interface IAppointmentService
    {

        // Book New Appointment
        Appointment BookAppointment(
            Patient patient,
            Doctor doctor,
            DateOnly date,
            TimeOnly slot);

        // Get Appointment By Id
        Appointment? GetAppointmentById(
            Guid appointmentId);

        // Get All Appointments
        List<Appointment> GetAllAppointments();

        // Get Appointments By Patient
        List<Appointment> GetAppointmentsByPatient(
            Guid patientId);

        // Get Appointments By Doctor
        List<Appointment> GetAppointmentsByDoctor(
            Guid doctorId);

        // Get Upcoming Appointments
        List<Appointment> GetUpcomingAppointments();

        // Get Completed Appointments
        List<Appointment> GetCompletedAppointments();

        // Confirm Appointment
        void ConfirmAppointment(
            Guid appointmentId);

        // Cancel Appointment
        void CancelAppointment(
            Guid appointmentId,
            string reason);

        // Complete Appointment
        void CompleteAppointment(Guid appointmentId);

        // Update Existing Appointment
        void UpdateAppointment(
            Appointment updatedAppointment);

        // Delete Appointment By Id
        void DeleteAppointmentById(
            Guid appointmentId);
    }
}
