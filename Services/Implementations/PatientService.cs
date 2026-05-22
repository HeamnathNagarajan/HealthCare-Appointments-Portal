using HealthcareApp.Models;
using HealthcareApp.Repositories;
using HealthcareApp.Utilities;

namespace HealthcareApp.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public Patient RegisterPatient(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.FullName))
                throw new ArgumentException("Patient full name is required.");

            if (patient.DateOfBirth.Date > SystemTime.Now.Date)
                throw new ArgumentException("Date of birth cannot be in the future.");

            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
                throw new ArgumentException("Phone number is required.");

            if (string.IsNullOrWhiteSpace(patient.Email))
                throw new ArgumentException("Email is required.");

            patient.CreatedDate = SystemTime.Now;

            _patientRepository.Add(patient);

            return patient;
        }

        public Patient GetPatientById(int patientId)
        {
            return _patientRepository.GetById(patientId);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientRepository.GetAll();
        }

        public Patient UpdatePatient(Patient patient)
        {
            _patientRepository.Update(patient);

            return patient;
        }
    }
}