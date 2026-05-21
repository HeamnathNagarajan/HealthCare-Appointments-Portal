using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IPatientService
    {
        // Add new patient
        void AddPatient(Patient patient);

        // Get all patients
        List<Patient> GetAllPatients();
    }
}