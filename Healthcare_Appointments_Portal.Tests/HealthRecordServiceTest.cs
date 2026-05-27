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

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(new List<HealthRecord>());

            // Act
            _healthRecordService
                .AddRecord(record);

            // Assert
            _mockRepository.Verify(r =>
                r.AddRecord(record),
                Times.Once);
        }


        // Add Record Repository Verification
        [Fact]
        public void AddRecord_ShouldCallRepositoryOnce()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(new List<HealthRecord>());

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
            Assert.NotNull(result);

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
                        It.IsAny<int>()))
                .Returns((HealthRecord?)null);

            // Act & Assert
            Assert.Throws<
                HealthRecordNotFoundException>(() =>
                    _healthRecordService
                    .GetRecordById(23));
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
            Assert.Empty(result);
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
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take Rest"
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
            Assert.Single(result);
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
                .GetRecordsByPatient(1);

            // Assert
            Assert.Empty(result);
        }

        // Get Records By Patient Unmatched
        [Fact]
        public void GetRecordsByPatient_UnmatchedPatient_ShouldReturnEmpty()
        {
            // Arrange
            List<HealthRecord> records =
            [
                CreateHealthRecord()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(records);

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetRecordsByPatient(999);

            // Assert
            Assert.Empty(result);
        }

        // Get Records By Patient Ordered Descending
        [Fact]
        public void GetRecordsByPatient_ShouldReturnOrderedRecords()
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
                        new DateOnly(2024, 1, 1),
                    Diagnosis = "Cold",
                    Prescription = "Tablet",
                    Notes = "Rest"
                },

                new HealthRecord
                {
                    Patient = patient,
                    Doctor = CreateDoctor(),
                    VisitDate =
                        new DateOnly(2025, 1, 1),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest"
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
            Assert.Equal(
                new DateOnly(2025, 1, 1),
                result[0].VisitDate);
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
                    Diagnosis = "Cold",
                    Prescription = "Tablet",
                    Notes = "Daily"
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
            Assert.Single(result);
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
                .GetRecordsByDoctor(1);

            // Assert
            Assert.Empty(result);
        }

        // Get Records By Doctor Unmatched
        [Fact]
        public void GetRecordsByDoctor_UnmatchedDoctor_ShouldReturnEmpty()
        {
            // Arrange
            List<HealthRecord> records =
            [
                CreateHealthRecord()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(records);

            // Act
            List<HealthRecord> result =
                _healthRecordService
                .GetRecordsByDoctor(999);

            // Assert
            Assert.Empty(result);
        }

        // Get Records By Doctor Ordered Descending
        [Fact]
        public void GetRecordsByDoctor_ShouldReturnOrderedRecords()
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
                        new DateOnly(2024, 1, 1),
                    Diagnosis = "Cold",
                    Prescription = "Tablet",
                    Notes = "Rest"
                },

                new HealthRecord
                {
                    Patient = CreatePatient(),
                    Doctor = doctor,
                    VisitDate =
                        new DateOnly(2025, 1, 1),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest"
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
            Assert.Equal(
                new DateOnly(2025, 1, 1),
                result[0].VisitDate);
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
                .UpdateRecord(record);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateRecord(record),
                Times.Once);
        }

        // Update Repository Verification
        [Fact]
        public void UpdateRecord_ShouldCallRepositoryOnce()
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
                .UpdateRecord(record);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateRecord(record),
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
                    .UpdateRecord(record));
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

        // Delete Repository Verification
        [Fact]
        public void DeleteRecord_ShouldCallRepositoryOnce()
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
                        It.IsAny<int>()))
                .Returns((HealthRecord?)null);

            // Act & Assert
            Assert.Throws<
                HealthRecordNotFoundException>(() =>
                    _healthRecordService
                    .DeleteRecordById(87));
        }

        // Create Record From Appointment
        [Fact]
        public void CreateRecordFromAppointment_ShouldCreateHealthRecord()
        {
            // Arrange
            Appointment appointment =
                new()
                {
                    Patient =
                        CreatePatient(),

                    Doctor =
                        CreateDoctor(),

                    ScheduledDate =
                        DateOnly.FromDateTime(
                            DateTime.Now)
                };

            // Act
            HealthRecord result =
                _healthRecordService
                .CreateRecordFromAppointment(
                    appointment);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(
                appointment.Patient,
                result.Patient);

            Assert.Equal(
                appointment.Doctor,
                result.Doctor);

            Assert.Equal(
                appointment.ScheduledDate,
                result.VisitDate);
        }

        // Add Duplicate Record Should Throw Exception
        [Fact]
        public void AddRecord_DuplicateAppointmentId_ShouldThrowException()
        {
            // Arrange
            HealthRecord existingRecord =
                CreateHealthRecord();

            existingRecord.AppointmentId = 1;

            HealthRecord newRecord =
                CreateHealthRecord();

            newRecord.AppointmentId = 1;

            List<HealthRecord> records =
            [
                existingRecord
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(records);

            // Act & Assert
            Assert.Throws<
                DuplicateHealthRecordException>(() =>
                    _healthRecordService
                    .AddRecord(newRecord));
        }

        // Add Record Unique AppointmentId Should Add Record
        [Fact]
        public void AddRecord_UniqueAppointmentId_ShouldAddRecord()
        {
            // Arrange
            HealthRecord existingRecord =
                CreateHealthRecord();

            existingRecord.AppointmentId = 1;

            HealthRecord newRecord =
                CreateHealthRecord();

            newRecord.AppointmentId = 2;

            List<HealthRecord> records =
            [
                existingRecord
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllRecords())
                .Returns(records);

            // Act
            _healthRecordService
                .AddRecord(newRecord);

            // Assert
            _mockRepository.Verify(r =>
                r.AddRecord(newRecord),
                Times.Once);
        }

        // Helper Method
        private static Patient CreatePatient()
        {
            return new Patient
            {
                PatientId = 1,
                FullName = "Ragu",
                DateOfBirth =
                    new DateOnly(2001, 4, 22),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "ragu@gmail.com",
                InsuranceId = "INS101"
            };
        }

        // Helper Method
        private static Doctor CreateDoctor()
        {
            return new Doctor
            {
                DoctorId = 1,
                FullName = "Dr Ragu",
                Specialisation =
                    Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 1000,
                IsActive = true
            };
        }

        // Helper Method
        private static HealthRecord CreateHealthRecord()
        {
            return new HealthRecord
            {
                RecordId = 1,

                AppointmentId = 1,

                Patient =
                    CreatePatient(),

                Doctor =
                    CreateDoctor(),

                VisitDate =
                    DateOnly.FromDateTime(
                        DateTime.Now),

                Diagnosis = "Fever",

                Prescription =
                    "Paracetamol",

                Notes = "Take Rest"
            };
        }
    }
}