using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Intefaces;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Services;
using Moq;
using Xunit;

namespace HealthCare.Tests
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockRepo;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _mockRepo = new Mock<IPatientRepository>();
            _service = new PatientService(_mockRepo.Object);
        }

        private static Patient CreatePatient(string email = "test@test.com")
        {
            return new Patient
            {
                PatientId = Guid.NewGuid(),
                FullName = "Test User",
                Email = email,
                PhoneNumber = "1234567890",
                Gender = "Male"
            };
        }

        [Fact]
        public void AddPatient_ValidPatient_ShouldAdd()
        {
            var patient = CreatePatient();

            _mockRepo.Setup(r => r.GetAllPatients())
                     .Returns(new List<Patient>());

            _service.AddPatient(patient);

            _mockRepo.Verify(r => r.AddPatient(patient), Times.Once);
        }

        [Fact]
        public void AddPatient_Null_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _service.AddPatient(null!));
        }

        [Fact]
        public void AddPatient_DuplicateEmail_ShouldThrowException()
        {
            var existingPatient = CreatePatient("test@test.com");
            var newPatient = CreatePatient("test@test.com");

            _mockRepo.Setup(r => r.GetAllPatients())
                     .Returns(new List<Patient> { existingPatient });

            Assert.Throws<DuplicatePatientException>(() => _service.AddPatient(newPatient));
        }

        [Fact]
        public void AddPatient_ShouldNormalizeEmail()
        {
            var patient = CreatePatient("  TEST@TEST.COM  ");

            _mockRepo.Setup(r => r.GetAllPatients())
                     .Returns(new List<Patient>());

            _service.AddPatient(patient);

            Assert.Equal("test@test.com", patient.Email);
        }

        [Fact]
        public void GetPatientById_ShouldReturnPatient()
        {
            var patient = CreatePatient();
            var id = patient.PatientId;

            _mockRepo.Setup(r => r.GetPatientById(id))
                     .Returns(patient);

            var result = _service.GetPatientById(id);

            Assert.NotNull(result);
            Assert.Equal(id, result!.PatientId);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnList()
        {
            var patients = new List<Patient>
            {
                CreatePatient(),
                CreatePatient()
            };

            _mockRepo.Setup(r => r.GetAllPatients())
                     .Returns(patients);

            var result = _service.GetAllPatients();

            Assert.Equal(2, result.Count);
        }
    }
}
