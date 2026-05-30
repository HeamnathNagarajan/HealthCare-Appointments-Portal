using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;

namespace HealthCare_Appointment_Portal.Tests
{
    public class PatientRepositoryTests
    {
        private readonly DataStore _dataStore;

    private readonly PatientRepository _repository;

        public PatientRepositoryTests()
        {
            _dataStore = new DataStore();

            _repository =
                new PatientRepository(
                    _dataStore);
        }

        [Fact]
        public void AddPatient_ValidPatient_ShouldAddPatient()
        {
            Patient patient = CreatePatient();

            _repository.AddPatient(patient);

            Assert.Single(_dataStore.Patients);

            Assert.Equal(
                patient.FullName,
                _dataStore.Patients[0].FullName);
        }

        [Fact]
        public void GetPatientById_ExistingId_ShouldReturnPatient()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            Assert.NotNull(result);

            Assert.Equal(
                patient.PatientId,
                result?.PatientId);
        }

        [Fact]
        public void GetPatientById_InvalidId_ShouldReturnNull()
        {
            Patient? result =
                _repository.GetPatientById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnAllPatients()
        {
            _dataStore.Patients.AddRange(
            [
                CreatePatient(),
            CreatePatient()
            ]);

            List<Patient> result =
                _repository.GetAllPatients();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAllPatients_EmptyList_ShouldReturnEmpty()
        {
            List<Patient> result =
                _repository.GetAllPatients();

            Assert.Empty(result);
        }

        [Fact]
        public void GetPatientByEmail_ExistingEmail_ShouldReturnPatient()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient? result =
                _repository.GetPatientByEmail(
                    patient.Email);

            Assert.NotNull(result);

            Assert.Equal(
                patient.Email,
                result?.Email);
        }

        [Fact]
        public void GetPatientByEmail_CaseInsensitive_ShouldReturnPatient()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient? result =
                _repository.GetPatientByEmail(
                    patient.Email.ToUpper());

            Assert.NotNull(result);

            Assert.Equal(
                patient.Email,
                result?.Email);
        }

        [Fact]
        public void GetPatientByEmail_InvalidEmail_ShouldReturnNull()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient? result =
                _repository.GetPatientByEmail(
                    "invalid@gmail.com");

            Assert.Null(result);
        }

        [Fact]
        public void GetPatientByEmail_EmptyStore_ShouldReturnNull()
        {
            Patient? result =
                _repository.GetPatientByEmail(
                    "test@gmail.com");

            Assert.Null(result);
        }

        [Fact]
        public void UpdatePatient_ExistingPatient_ShouldUpdateDetails()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient updatedPatient = new()
            {
                PatientId = patient.PatientId,
                FullName = "Updated Name",
                DateOfBirth =
                    new DateOnly(2000, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "9999999999",
                Email = "updated@gmail.com",
                InsuranceId = "NEW101"
            };

            _repository.UpdatePatient(
                updatedPatient);

            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            Assert.NotNull(result);

            Assert.Equal(
                "Updated Name",
                result?.FullName);

            Assert.Equal(
                "9999999999",
                result?.PhoneNumber);

            Assert.Equal(
                "updated@gmail.com",
                result?.Email);

            Assert.Equal(
                "NEW101",
                result?.InsuranceId);

            Assert.Equal(
                Gender.Female,
                result?.Gender);
        }

        [Fact]
        public void UpdatePatient_PartialUpdate_ShouldUpdateOnlyProvidedFields()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient updatedPatient = new()
            {
                PatientId = patient.PatientId,
                FullName = "Updated Ragu"
            };

            _repository.UpdatePatient(
                updatedPatient);

            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            Assert.NotNull(result);

            Assert.Equal(
                "Updated Ragu",
                result?.FullName);

            Assert.Equal(
                patient.PhoneNumber,
                result?.PhoneNumber);

            Assert.Equal(
                patient.Email,
                result?.Email);

            Assert.Equal(
                patient.InsuranceId,
                result?.InsuranceId);
        }

        [Fact]
        public void UpdatePatient_ShouldNotOverwriteWithEmptyStrings()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient updatedPatient = new()
            {
                PatientId = patient.PatientId,
                FullName = "",
                PhoneNumber = "",
                Email = "",
                InsuranceId = ""
            };

            _repository.UpdatePatient(
                updatedPatient);

            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            Assert.Equal(
                patient.FullName,
                result?.FullName);

            Assert.Equal(
                patient.PhoneNumber,
                result?.PhoneNumber);

            Assert.Equal(
                patient.Email,
                result?.Email);

            Assert.Equal(
                patient.InsuranceId,
                result?.InsuranceId);
        }

        [Fact]
        public void UpdatePatient_ShouldNotOverwriteWithWhitespace()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient updatedPatient = new()
            {
                PatientId = patient.PatientId,
                FullName = " ",
                PhoneNumber = " ",
                Email = " ",
                InsuranceId = " "
            };

            _repository.UpdatePatient(
                updatedPatient);

            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            Assert.Equal(
                patient.FullName,
                result?.FullName);

            Assert.Equal(
                patient.PhoneNumber,
                result?.PhoneNumber);
        }

        [Fact]
        public void UpdatePatient_DefaultValues_ShouldKeepExistingValues()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            Patient updatedPatient = new()
            {
                PatientId = patient.PatientId
            };

            _repository.UpdatePatient(
                updatedPatient);

            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            Assert.Equal(
                patient.DateOfBirth,
                result?.DateOfBirth);

            Assert.Equal(
                patient.Gender,
                result?.Gender);
        }

        [Fact]
        public void UpdatePatient_InvalidId_ShouldNotUpdate()
        {
            Patient updatedPatient = new()
            {
                PatientId = 999,
                FullName = "Updated"
            };

            _repository.UpdatePatient(
                updatedPatient);

            Assert.Empty(
                _dataStore.Patients);
        }

        [Fact]
        public void DeletePatientById_ExistingId_ShouldRemovePatient()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            _repository.DeletePatientById(
                patient.PatientId);

            Assert.Empty(
                _dataStore.Patients);
        }

        [Fact]
        public void DeletePatientById_InvalidId_ShouldNotRemoveAnything()
        {
            Patient patient = CreatePatient();

            _dataStore.Patients.Add(patient);

            _repository.DeletePatientById(
                999);

            Assert.Single(
                _dataStore.Patients);
        }

        [Fact]
        public void DeletePatientById_EmptyStore_ShouldNotThrow()
        {
            _repository.DeletePatientById(1);

            Assert.Empty(
                _dataStore.Patients);
        }

        private static Patient CreatePatient()
        {
            return new Patient
            {
                FullName = "Ragu",
                DateOfBirth =
                    new DateOnly(2001, 4, 22),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "ragu@gmail.com",
                InsuranceId = "INS101"
            };
        }
    }
}
