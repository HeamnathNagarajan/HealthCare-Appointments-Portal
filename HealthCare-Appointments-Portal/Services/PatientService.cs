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

        // Add Patient
        public void AddPatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            var existingPatients = _repository.GetAllPatients();

            bool isDuplicate = existingPatients.Any(p =>
                (!string.IsNullOrWhiteSpace(p.Email) &&
                 !string.IsNullOrWhiteSpace(patient.Email) &&
                 p.Email.Trim().Equals(patient.Email.Trim(), StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(p.PhoneNumber) &&
                 !string.IsNullOrWhiteSpace(patient.PhoneNumber) &&
                 p.PhoneNumber.Trim() == patient.PhoneNumber.Trim()) ||

                (!string.IsNullOrWhiteSpace(p.FullName) &&
                 !string.IsNullOrWhiteSpace(patient.FullName) &&
                 p.FullName.Trim().Equals(patient.FullName.Trim(), StringComparison.OrdinalIgnoreCase)
                 && p.DateOfBirth == patient.DateOfBirth)
            );

            if (isDuplicate)
            {
                throw new DuplicatePatientException();
            }

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

        //Duplicate check
    }
}
