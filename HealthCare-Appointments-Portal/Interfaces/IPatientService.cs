using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IPatientService
    {
        // Add New Patient
        void AddPatient(Patient patient);

        // Get Patient By Id
        Patient? GetPatientById(int patientId);

        // Get All Patients
        List<Patient> GetAllPatients();

        // Get Patient By Email
        Patient? GetPatientByEmail(string email);

        // Update Existing Patient
        void UpdatePatient(Patient updatedPatient);

        // Delete Patient By Id
        void DeletePatientById(int patientId);
    }
}