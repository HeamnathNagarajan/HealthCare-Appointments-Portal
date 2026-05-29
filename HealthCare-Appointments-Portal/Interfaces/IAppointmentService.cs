using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
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
            int appointmentId);

        // Get All Appointments
        List<Appointment> GetAllAppointments();

        // Get Appointments By Patient
        List<Appointment> GetAppointmentsByPatient(
            int patientId);

        // Get Appointments By Doctor
        List<Appointment> GetAppointmentsByDoctor(
            int doctorId);

        // Get Upcoming Appointments
        List<Appointment> GetUpcomingAppointments();

        // Get Completed Appointments
        List<Appointment> GetCompletedAppointments();

        // Confirm Appointment
        void ConfirmAppointment(
            int appointmentId);

        // Cancel Appointment
        void CancelAppointment(
            int appointmentId,
            string reason);

        // Complete Appointment
        void CompleteAppointment(int appointmentId);

        // Update Existing Appointment
        void UpdateAppointment(
            Appointment updatedAppointment);

        // Delete Appointment By Id
        void DeleteAppointmentById(
            int appointmentId);
    }
}