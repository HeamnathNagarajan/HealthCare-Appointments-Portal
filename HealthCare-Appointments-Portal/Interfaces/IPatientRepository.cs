using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{

    public interface IPatientRepository
    {

        void AddPatient(Patient patient);

        Patient? GetPatientById(int patientId);

        List<Patient> GetAllPatients();

        void UpdatePatient(Patient updatedPatient);

        void DeletePatientById(int patientId);
    }
}
