using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Interfaces;
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

        public void UpdatePatient(Patient updatedPatient)
        {
            Patient? existingPatient =
                _dataStore.Patients
                .FirstOrDefault(p =>
                    p.PatientId ==
                    updatedPatient.PatientId);

            if (existingPatient != null)
            {
                existingPatient.FullName =
                    string.IsNullOrWhiteSpace(
                        updatedPatient.FullName)
                    ? existingPatient.FullName
                    : updatedPatient.FullName;

                existingPatient.DateOfBirth =
                    updatedPatient.DateOfBirth == default
                    ? existingPatient.DateOfBirth
                    : updatedPatient.DateOfBirth;

                existingPatient.Gender =
                    updatedPatient.Gender == default
                    ? existingPatient.Gender
                    : updatedPatient.Gender;

                existingPatient.PhoneNumber =
                    string.IsNullOrWhiteSpace(
                        updatedPatient.PhoneNumber)
                    ? existingPatient.PhoneNumber
                    : updatedPatient.PhoneNumber;

                existingPatient.Email =
                    string.IsNullOrWhiteSpace(
                        updatedPatient.Email)
                    ? existingPatient.Email
                    : updatedPatient.Email;

                existingPatient.InsuranceId =
                    string.IsNullOrWhiteSpace(
                        updatedPatient.InsuranceId)
                    ? existingPatient.InsuranceId
                    : updatedPatient.InsuranceId;
            }
        }

        public void DeletePatientById(Guid patientId)
        {
            Patient? patient =
                _dataStore.Patients
                .FirstOrDefault(p =>
                    p.PatientId == patientId);

            if (patient != null)
            {
                _dataStore.Patients.Remove(patient);
            }
        }
    }
}