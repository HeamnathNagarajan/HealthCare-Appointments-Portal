
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{
    public class PatientService
        : IPatientService
    {
        private readonly IPatientRepository _repository;

        // Dependency Injection
        public PatientService(
            IPatientRepository repository)
        {
            _repository = repository;
        }

        // Add Patient
        public void AddPatient(Patient patient)
        {
            _repository.AddPatient(patient);
        }

        // Get All Patients
        public List<Patient> GetAllPatients()
        {
            return _repository.GetAllPatients();
        }
    }
}