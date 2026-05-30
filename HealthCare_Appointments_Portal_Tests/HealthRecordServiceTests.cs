using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests
{
    public partial class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository>
            _mockRepository;

        private readonly Mock<IAppointmentRepository>
            _mockAppointmentRepository;

        private readonly HealthRecordService
            _healthRecordService;

        public HealthRecordServiceTests()
        {
            _mockRepository =
                new Mock<IHealthRecordRepository>();

            _mockAppointmentRepository =
                new Mock<IAppointmentRepository>();

            _healthRecordService =
                new HealthRecordService(
                    _mockRepository.Object,
                    _mockAppointmentRepository.Object);
        }

        // Add Record Success
        [Fact]
        public void AddRecord_ValidRecord_ShouldAddRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            Appointment appointment =
                new()
                {
                    AppointmentId =
                        record.AppointmentId,

                    Patient =
                        record.Patient,

                    Doctor =
                        record.Doctor,

                    Status =
                        AppointmentStatus.Completed
                };

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        record.AppointmentId))
                .Returns(appointment);

            _mockRepository
                .Setup(r =>
                    r.RecordExists(
                        record.AppointmentId))
                .Returns(false);

            // Act
            _healthRecordService
                .AddRecord(record);

            // Assert
            _mockRepository.Verify(
                r => r.AddRecord(
                    record),
                Times.Once);
        }

        // Invalid Appointment
        [Fact]
        public void AddRecord_InvalidAppointment_ShouldThrowException()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        record.AppointmentId))
                .Returns((Appointment?)null);

            // Act & Assert
            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _healthRecordService
                        .AddRecord(record));
        }

        // Non Completed Appointment
        [Fact]
        public void AddRecord_NonCompletedAppointment_ShouldThrowException()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            Appointment appointment =
                new()
                {
                    AppointmentId =
                        record.AppointmentId,

                    Patient =
                        record.Patient,

                    Doctor =
                        record.Doctor,

                    Status =
                        AppointmentStatus.Pending
                };

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        record.AppointmentId))
                .Returns(appointment);

            // Act & Assert
            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _healthRecordService
                        .AddRecord(record));
        }

        // Duplicate Record
        [Fact]
        public void AddRecord_DuplicateAppointmentId_ShouldThrowException()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            Appointment appointment =
                new()
                {
                    AppointmentId =
                        record.AppointmentId,

                    Patient =
                        record.Patient,

                    Doctor =
                        record.Doctor,

                    Status =
                        AppointmentStatus.Completed
                };

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        record.AppointmentId))
                .Returns(appointment);

            _mockRepository
                .Setup(r =>
                    r.RecordExists(
                        record.AppointmentId))
                .Returns(true);

            // Act & Assert
            Assert.Throws<
                DuplicateHealthRecordException>(() =>
                    _healthRecordService
                        .AddRecord(record));
        }

        // Unique Appointment Id
        [Fact]
        public void AddRecord_UniqueAppointmentId_ShouldAddRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            record.AppointmentId = 2;

            Appointment appointment =
                new()
                {
                    AppointmentId = 2,

                    Patient =
                        record.Patient,

                    Doctor =
                        record.Doctor,

                    Status =
                        AppointmentStatus.Completed
                };

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentById(2))
                .Returns(appointment);

            _mockRepository
                .Setup(r =>
                    r.RecordExists(2))
                .Returns(false);

            // Act
            _healthRecordService
                .AddRecord(record);

            // Assert
            _mockRepository.Verify(
                r => r.AddRecord(
                    record),
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
                        .GetRecordById(999));
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
                .Returns(
                    new List<HealthRecord>());

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
                CreateHealthRecord()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetRecordsByPatient(
                        patient.PatientId))
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
                    r.GetRecordsByPatient(1))
                .Returns(
                    new List<HealthRecord>());

            // Act
            List<HealthRecord> result =
                _healthRecordService
                    .GetRecordsByPatient(1);

            // Assert
            Assert.Empty(result);
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
                CreateHealthRecord()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetRecordsByDoctor(
                        doctor.DoctorId))
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
                    r.GetRecordsByDoctor(1))
                .Returns(
                    new List<HealthRecord>());

            // Act
            List<HealthRecord> result =
                _healthRecordService
                    .GetRecordsByDoctor(1);

            // Assert
            Assert.Empty(result);
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
            _mockRepository.Verify(
                r => r.UpdateRecord(
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
            _mockRepository.Verify(
                r => r.DeleteRecordById(
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
                        .DeleteRecordById(
                            999));
        }

        // Create Record From Appointment
        [Fact]
        public void CreateRecordFromAppointment_ShouldCreateHealthRecord()
        {
            // Arrange
            Appointment appointment =
                new()
                {
                    AppointmentId = 1,

                    Patient =
                        CreatePatient(),

                    Doctor =
                        CreateDoctor(),

                    ScheduledDate =
                        new DateOnly(
                            2026,
                            5,
                            20)
                };

            // Act
            HealthRecord result =
                _healthRecordService
                    .CreateRecordFromAppointment(
                        appointment);

            // Assert
            Assert.Equal(
                appointment.AppointmentId,
                result.AppointmentId);

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

        // Get Completed Appointments Without Health Record
        [Fact]
        public void GetCompletedAppointmentsWithoutHealthRecord_ShouldReturnAppointments()
        {
            // Arrange
            List<Appointment> appointments =
            [
                new Appointment
        {
            AppointmentId = 1,
            Patient = CreatePatient(),
            Doctor = CreateDoctor(),
            Status =
                AppointmentStatus.Completed
        },

        new Appointment
        {
            AppointmentId = 2,
            Patient = CreatePatient(),
            Doctor = CreateDoctor(),
            Status =
                AppointmentStatus.Completed
        }
            ];

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetCompletedAppointments())
                .Returns(appointments);

            _mockRepository
                .Setup(r =>
                    r.GetRecordedAppointmentIds())
                .Returns(
                [
                    1
                ]);

            // Act
            List<Appointment> result =
                _healthRecordService
                    .GetCompletedAppointmentsWithoutHealthRecord();

            // Assert
            Assert.Single(result);

            Assert.Equal(
                2,
                result[0].AppointmentId);
        }

        // Helper Method
        private static Patient CreatePatient()
        {
            return new Patient
            {
                PatientId = 1,

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
                    new DateOnly(
                        2026,
                        5,
                        20),

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