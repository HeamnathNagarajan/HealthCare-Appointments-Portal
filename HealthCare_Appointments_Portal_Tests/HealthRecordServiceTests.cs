using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository>
            _mockRepository;

        private readonly HealthRecordService
            _healthRecordService;

        public HealthRecordServiceTests()
        {
            _mockRepository =
                new Mock<IHealthRecordRepository>();

            _healthRecordService =
                new HealthRecordService(
                    _mockRepository.Object);
        }

        // Add Record Success
        [Fact]
        public void AddRecord_ValidRecord_ShouldAddRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            // Act
            _healthRecordService
                .AddRecord(record);

            // Assert
            _mockRepository.Verify(r =>
                r.AddRecord(record),
                Times.Once);
        }

        // Get Record By Existing Id
        [Fact]
        public void GetRecordById_ExistingId_ShouldReturnRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _mockRepository
                .Setup(r =>
                    r.GetRecordById(
                        record.RecordId))
                .Returns(record);

            // Act
            HealthRecord? result =
                _healthRecordService
                .GetRecordById(
                    record.RecordId);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                record.RecordId,
                result?.RecordId);
        }

        // Get Record By Invalid Id
        [Fact]
        public void GetRecordById_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetRecordById(
                        It.IsAny<Guid>()))
                .Returns((HealthRecord?)null);

            // Act & Assert
            Assert.Throws<
                HealthRecordNotFoundException>(() =>
                    _healthRecordService
                    .GetRecordById(
                        Guid.NewGuid()));
        }

        // Get All Records
        [Fact]
        public void GetAllRecords_ShouldReturnAllRecords()
        {
            // Arrange
            List<HealthRecord> records =
            [
                CreateHealthRecord(),
                CreateHealthRecord()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(records);

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetAllRecords();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Records Empty
        [Fact]
        public void GetAllRecords_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(new List<HealthRecord>());

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetAllRecords();

            // Assert
            Assert.Empty(
                result);
        }

        // Get Records By Patient
        [Fact]
        public void GetRecordsByPatient_ValidPatient_ShouldReturnRecords()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            List<HealthRecord> records =
            [
                new HealthRecord
                {
                    Patient = patient,
                    Doctor = CreateDoctor(),
                    VisitDate =
                        DateOnly.FromDateTime(
                            DateTime.Now),
                    Diagnosis =
                        "Fever",
                    Prescription =
                        "Paracetamol",
                    Notes =
                        "Take Rest"
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(records);

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetRecordsByPatient(
                    patient.PatientId);

            // Assert
            Assert.Single(
                result);
        }

        // Get Records By Patient Empty
        [Fact]
        public void GetRecordsByPatient_NoRecords_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(new List<HealthRecord>());

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetRecordsByPatient(
                    Guid.NewGuid());

            // Assert
            Assert.Empty(
                result);
        }

        // Get Records By Doctor
        [Fact]
        public void GetRecordsByDoctor_ValidDoctor_ShouldReturnRecords()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            List<HealthRecord> records =
            [
                new HealthRecord
                {
                    Patient = CreatePatient(),
                    Doctor = doctor,
                    VisitDate =
                        DateOnly.FromDateTime(
                            DateTime.Now),
                    Diagnosis =
                        "Cold",
                    Prescription =
                        "Tablet",
                    Notes =
                        "Daily"
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(records);

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetRecordsByDoctor(
                    doctor.DoctorId);

            // Assert
            Assert.Single(
                result);
        }

        // Get Records By Doctor Empty
        [Fact]
        public void GetRecordsByDoctor_NoRecords_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(new List<HealthRecord>());

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetRecordsByDoctor(
                    Guid.NewGuid());

            // Assert
            Assert.Empty(
                result);
        }

        // Update Existing Record
        [Fact]
        public void UpdateRecord_ExistingRecord_ShouldUpdateRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _mockRepository
                .Setup(r =>
                    r.GetRecordById(
                        record.RecordId))
                .Returns(record);

            // Act
            _healthRecordService
                .UpdateRecord(
                    record);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateRecord(
                    record),
                Times.Once);
        }

        // Update Invalid Record
        [Fact]
        public void UpdateRecord_InvalidId_ShouldThrowException()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _mockRepository
                .Setup(r =>
                    r.GetRecordById(
                        record.RecordId))
                .Returns((HealthRecord?)null);

            // Act & Assert
            Assert.Throws<
                HealthRecordNotFoundException>(() =>
                    _healthRecordService
                    .UpdateRecord(
                        record));
        }

        // Delete Existing Record
        [Fact]
        public void DeleteRecordById_ExistingId_ShouldDeleteRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _mockRepository
                .Setup(r =>
                    r.GetRecordById(
                        record.RecordId))
                .Returns(record);

            // Act
            _healthRecordService
                .DeleteRecordById(
                    record.RecordId);

            // Assert
            _mockRepository.Verify(r =>
                r.DeleteRecordById(
                    record.RecordId),
                Times.Once);
        }

        // Delete Invalid Record
        [Fact]
        public void DeleteRecordById_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetRecordById(
                        It.IsAny<Guid>()))
                .Returns((HealthRecord?)null);

            // Act & Assert
            Assert.Throws<
                HealthRecordNotFoundException>(() =>
                    _healthRecordService
                    .DeleteRecordById(
                        Guid.NewGuid()));
        }

        // Helper Method
        private static Patient CreatePatient()
        {
            return new Patient
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
        }

        // Helper Method
        private static Doctor CreateDoctor()
        {
            return new Doctor
            {
                FullName =
                    "Dr Ragu",

                Specialisation =
                    Specialisation.Cardiology,

                YearsOfExperience =
                    5,

                ConsultationFee =
                    1000,

                IsActive =
                    true
            };
        }

        // Helper Method
        private static HealthRecord CreateHealthRecord()
        {
            return new HealthRecord
            {
                Patient =
                    CreatePatient(),

                Doctor =
                    CreateDoctor(),

                VisitDate =
                    DateOnly.FromDateTime(
                        DateTime.Now),

                Diagnosis =
                    "Fever",

                Prescription =
                    "Paracetamol",

                Notes =
                    "Take Rest"
            };
        }
    }
}