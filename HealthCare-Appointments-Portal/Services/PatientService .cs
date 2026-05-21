using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Exceptions;

namespace HealthCare_Appointment_Portal.Services
{

    public class PatientService : IPatientService
    {

        private readonly IPatientRepository _patientRepository;

        // Dependency Injection
        public PatientService(
            IPatientRepository patientRepository)
        {

            _patientRepository = patientRepository;
        }

        // Add New Patient
        public void AddPatient(Patient patient)
        {

            Patient? existingPatient =
                _patientRepository
                .GetAllPatients()
                .FirstOrDefault(p =>
                    p.Email == patient.Email);

            if (existingPatient != null)
            {

                throw new DuplicatePatientException();
            }

            _patientRepository.AddPatient(patient);
        }

        // Get Patient By Id
        public Patient? GetPatientById(int patientId)
        {

            Patient? patient =
                _patientRepository
                .GetPatientById(patientId);

            if (patient == null)
            {

                throw new PatientNotFoundException();
            }

            return patient;
        }

        // Get All Patients
        public List<Patient> GetAllPatients()
        {

            return _patientRepository
                .GetAllPatients();
        }

        // Get Patient By Email
        public Patient GetPatientByEmail(
            string email)
        {
            Patient? patient =
                _patientRepository
                .GetAllPatients()
                .FirstOrDefault(p =>
                    p.Email.Equals(
                        email,
                        StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            return patient;
        }

        // Update Existing Patient
        public void UpdatePatient(
            Patient updatedPatient)
        {

            Patient? existingPatient =
                _patientRepository
                .GetPatientById(
                    updatedPatient.PatientId);

            if (existingPatient == null)
            {

                throw new PatientNotFoundException();
            }

            _patientRepository
                .UpdatePatient(updatedPatient);
        }

        // Delete Patient By Id
        public void DeletePatientById(
            int patientId)
        {

            Patient? patient =
                _patientRepository
                .GetPatientById(patientId);

            if (patient == null)
            {

                throw new PatientNotFoundException();
            }

            _patientRepository
                .DeletePatientById(patientId);
        }
    }
}
