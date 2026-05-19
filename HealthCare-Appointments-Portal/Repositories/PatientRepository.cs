using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Intefaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataStore _dataStore;


        // Dependency Injection
        public PatientRepository(DataStore dataStore)
        {

            _dataStore = dataStore;

        }
        
        public void AddPatient(Patient patient)
        {
            _dataStore.Patients.Add(patient);
        }

        public Patient? GetPatientById(Guid patientId)
        {
            return _dataStore.Patients
                .FirstOrDefault(p =>
                    p.PatientId == patientId);
        }

        public List<Patient> GetAllPatients()
        {
            return _dataStore.Patients;
        }
    }
}
