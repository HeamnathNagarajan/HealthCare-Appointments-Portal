using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Intefaces
{
    public interface IPatientRepository
    {
        void AddPatient(Patient patient);

        Patient? GetPatientById(Guid patientId);

        List<Patient> GetAllPatients();
    }
}
