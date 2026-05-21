using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Repositories;

namespace HealthCare_Appointments_Portal.Tests
{
    public class PatientRepositoryTests
    {
        private readonly DataStore _dataStore;
        private readonly PatientRepository _repository;

        public PatientRepositoryTests()
        {
            _dataStore = new DataStore
            {
                Patients = new List<Patient>()
            };

            _repository = new PatientRepository(_dataStore);
        }

        [Fact]
        public void AddPatient_ShouldAddPatient()
        {
            // Arrange
            var patient = new Patient
            {
                FullName = "Elon Musk",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = Enums.Gender.Male,
                PhoneNumber = "1234567890",
                Email = "elonmusk@example.com",
                InsuranceId = "INS123"
            };

            // Act
            _repository.AddPatient(patient);

            // Assert
            Assert.Single(_dataStore.Patients);
            Assert.Equal("Elon Musk", _dataStore.Patients[0].FullName);
        }

        [Fact]
        public void GetPatientById_ShouldReturnPatient_WhenExists()
        {
            // Arrange
            var patient = new Patient
            {
                FullName = "Alice",
                DateOfBirth = new DateOnly(1995, 5, 5),
                Gender = Enums.Gender.Female,
                PhoneNumber = "9876543210",
                Email = "alice@example.com",
                InsuranceId = "INS456"
            };

            _dataStore.Patients.Add(patient);

            // Act
            var result = _repository.GetPatientById(patient.PatientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice", result.FullName);
        }

        [Fact]
        public void GetPatientById_ShouldReturnNull_WhenNotFound()
        {
            // Act
            var result = _repository.GetPatientById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnAll()
        {
            // Arrange
            _dataStore.Patients.AddRange(new List<Patient>
            {
                new Patient
                {
                    FullName = "A",
                    DateOfBirth = new DateOnly(2000,1,1),
                    Gender = Enums.Gender.Male,
                    PhoneNumber = "1111111111",
                    Email = "a@test.com",
                    InsuranceId = "INS1"
                },
                new Patient
                {
                    FullName = "B",
                    DateOfBirth = new DateOnly(2001,1,1),
                    Gender = Enums.Gender.Female,
                    PhoneNumber = "2222222222",
                    Email = "b@test.com",
                    InsuranceId = "INS2"
                }
            });

            // Act
            var result = _repository.GetAllPatients();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}