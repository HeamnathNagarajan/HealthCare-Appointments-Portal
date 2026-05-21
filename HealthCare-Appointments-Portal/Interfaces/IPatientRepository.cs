using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IPatientRepository
    {
        void AddPatient(Patient patient);

        Patient? GetPatientById(Guid patientId);

        List<Patient> GetAllPatients();

        void UpdatePatient(Patient updatedPatient);

        void DeletePatientById(Guid patientId);
    }
}