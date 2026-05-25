using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Intefaces;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        // Dependency Injection
        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }
        public void AddPatient(Patient patient)
        { 
            ArgumentNullException.ThrowIfNull(patient);

            var existingPatients = _repository.GetAllPatients();

            // EMAIL CHECK
            bool isDuplicate = existingPatients.Any(p =>
                !string.IsNullOrWhiteSpace(p.Email) &&
                !string.IsNullOrWhiteSpace(patient.Email) &&
                p.Email.Trim().Equals(patient.Email.Trim(), StringComparison.OrdinalIgnoreCase));

            if (isDuplicate)
            {
                throw new DuplicatePatientException();
            }

            // Normalize email before saving
            patient.Email = patient.Email.Trim().ToLower();

            _repository.AddPatient(patient);
        }

        // Get Patient By Id
        public Patient? GetPatientById(Guid patientId)
        {
            return _repository.GetPatientById(patientId);
        }

        // Get All Patients
        public List<Patient> GetAllPatients()
        {
            return _repository.GetAllPatients();
        }
    }
}
