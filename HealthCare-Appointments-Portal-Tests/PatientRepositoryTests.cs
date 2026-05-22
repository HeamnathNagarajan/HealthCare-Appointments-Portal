using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;

namespace HealthCare_Appointments_Portal.Tests
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

        // Add Patient Success
        [Fact]
        public void AddPatient_ValidPatient_ShouldAddPatient()
        {
            // Arrange
            Patient patient = new()
            {
                FullName = "Ragu",

                DateOfBirth =
                    new DateOnly(
                        2001,
                        4,
                        22),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9876543210",

                Email =
                    "ragu@gmail.com",

                InsuranceId =
                    "INS101"
            };

            // Act
            _repository.AddPatient(
                patient);

            // Assert
            Assert.Single(
                _dataStore.Patients);

            Assert.Equal(
                "Ragu",
                _dataStore
                    .Patients[0]
                    .FullName);
        }

        // Get Patient By Existing Id
        [Fact]
        public void GetPatientById_ExistingId_ShouldReturnPatient()
        {
            // Arrange
            Patient patient = new()
            {
                FullName = "Arun",

                DateOfBirth =
                    new DateOnly(
                        2000,
                        1,
                        1),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9999999999",

                Email =
                    "arun@gmail.com",

                InsuranceId =
                    "INS102"
            };

            _dataStore
                .Patients
                .Add(patient);

            // Act
            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                patient.PatientId,
                result?.PatientId);
        }

        // Get Patient By Invalid Id
        [Fact]
        public void GetPatientById_InvalidId_ShouldReturnNull()
        {
            // Act
            Patient? result =
                _repository.GetPatientById(
                    23);

            // Assert
            Assert.Null(
                result);
        }

        // Get All Patients
        [Fact]
        public void GetAllPatients_ShouldReturnAllPatients()
        {
            // Arrange
            _dataStore.Patients.AddRange(
            [
                new Patient
                {
                    FullName = "Ragu",

                    DateOfBirth =
                        new DateOnly(
                            2001,
                            4,
                            22),

                    Gender =
                        Gender.Male,

                    PhoneNumber =
                        "9876543210",

                    Email =
                        "ragu@gmail.com",

                    InsuranceId =
                        "INS101"
                },

                new Patient
                {
                    FullName = "Priya",

                    DateOfBirth =
                        new DateOnly(
                            2002,
                            5,
                            10),

                    Gender =
                        Gender.Female,

                    PhoneNumber =
                        "9876543211",

                    Email =
                        "priya@gmail.com",

                    InsuranceId =
                        "INS102"
                }
            ]);

            // Act
            List<Patient> result =
                _repository.GetAllPatients();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Patients Empty
        [Fact]
        public void GetAllPatients_EmptyList_ShouldReturnEmpty()
        {
            // Act
            List<Patient> result =
                _repository.GetAllPatients();

            // Assert
            Assert.Empty(
                result);
        }

        // Update Existing Patient
        [Fact]
        public void UpdatePatient_ExistingPatient_ShouldUpdateDetails()
        {
            // Arrange
            Patient patient = new()
            {
                FullName = "Old Name",

                DateOfBirth =
                    new DateOnly(
                        2000,
                        1,
                        1),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9999999999",

                Email =
                    "old@gmail.com",

                InsuranceId =
                    "OLD101"
            };

            _dataStore
                .Patients
                .Add(patient);

            Patient updatedPatient = new()
            {
                PatientId =
                    patient.PatientId,

                FullName =
                    "New Name",

                DateOfBirth =
                    new DateOnly(
                        2001,
                        2,
                        2),

                Gender =
                    Gender.Female,

                PhoneNumber =
                    "8888888888",

                Email =
                    "new@gmail.com",

                InsuranceId =
                    "NEW101"
            };

            // Act
            _repository.UpdatePatient(
                updatedPatient);

            // Assert
            Patient? result =
                _dataStore.Patients
                .FirstOrDefault(p =>
                    p.PatientId ==
                    patient.PatientId);

            Assert.NotNull(
                result);

            Assert.Equal(
                "New Name",
                result?.FullName);

            Assert.Equal(
                "8888888888",
                result?.PhoneNumber);

            Assert.Equal(
                "new@gmail.com",
                result?.Email);

            Assert.Equal(
                "NEW101",
                result?.InsuranceId);
        }

        // Partial Update Patient
        [Fact]
        public void UpdatePatient_PartialUpdate_ShouldUpdateOnlyProvidedFields()
        {
            // Arrange
            Patient patient = new()
            {
                FullName = "Ragu",

                DateOfBirth =
                    new DateOnly(
                        2001,
                        4,
                        22),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9876543210",

                Email =
                    "ragu@gmail.com",

                InsuranceId =
                    "INS101"
            };

            _dataStore
                .Patients
                .Add(patient);

            Patient updatedPatient = new()
            {
                PatientId =
                    patient.PatientId,

                FullName =
                    "Updated Ragu"
            };

            // Act
            _repository.UpdatePatient(
                updatedPatient);

            // Assert
            Patient? result =
                _repository.GetPatientById(
                    patient.PatientId);

            Assert.NotNull(
                result);

            // Updated Field
            Assert.Equal(
                "Updated Ragu",
                result?.FullName);

            // Existing Fields Unchanged
            Assert.Equal(
                "9876543210",
                result?.PhoneNumber);

            Assert.Equal(
                "ragu@gmail.com",
                result?.Email);

            Assert.Equal(
                "INS101",
                result?.InsuranceId);
        }

        // Update Invalid Patient
        [Fact]
        public void UpdatePatient_InvalidId_ShouldNotUpdate()
        {
            // Arrange
            Patient updatedPatient = new()
            {
                PatientId =
                    1,

                FullName =
                    "Updated"
            };

            // Act
            _repository.UpdatePatient(
                updatedPatient);

            // Assert
            Assert.Empty(
                _dataStore.Patients);
        }

        // Delete Existing Patient
        [Fact]
        public void DeletePatientById_ExistingId_ShouldRemovePatient()
        {
            // Arrange
            Patient patient = new()
            {
                FullName =
                    "Delete Test",

                DateOfBirth =
                    new DateOnly(
                        2000,
                        1,
                        1),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9999999999",

                Email =
                    "delete@gmail.com",

                InsuranceId =
                    "DEL101"
            };

            _dataStore
                .Patients
                .Add(patient);

            // Act
            _repository.DeletePatientById(
                patient.PatientId);

            // Assert
            Assert.Empty(
                _dataStore.Patients);
        }

        // Delete Invalid Patient
        [Fact]
        public void DeletePatientById_InvalidId_ShouldNotRemoveAnything()
        {
            // Arrange
            Patient patient = new()
            {
                FullName =
                    "Ragu",

                DateOfBirth =
                    new DateOnly(
                        2001,
                        4,
                        22),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9876543210",

                Email =
                    "ragu@gmail.com",

                InsuranceId =
                    "INS101"
            };

            _dataStore
                .Patients
                .Add(patient);

            // Act
            _repository.DeletePatientById(
                87);

            // Assert
            Assert.Single(
                _dataStore.Patients);
        }
    }
}