using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IPatientService
    {
        void AddPatient(Patient patient);

        Patient? GetPatientById(Guid patientId);

        List<Patient> GetAllPatients();
    }
}