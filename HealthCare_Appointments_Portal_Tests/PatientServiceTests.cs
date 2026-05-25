using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository>
            _mockRepository;

        private readonly Mock<IAppointmentRepository>
            _mockAppointmentRepository;

        private readonly PatientService
            _patientService;

        public PatientServiceTests()
        {
            _mockRepository =
                new Mock<IPatientRepository>();

            _mockAppointmentRepository =
                new Mock<IAppointmentRepository>();

            _patientService =
                new PatientService(
                    _mockRepository.Object,
                    _mockAppointmentRepository.Object);
        }

        // Add Patient Success
        [Fact]
        public void AddPatient_ValidPatient_ShouldAddPatient()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            _mockRepository
                .Setup(r =>
                    r.GetAllPatients())
                .Returns(new List<Patient>());

            // Act
            _patientService.AddPatient(
                patient);

            // Assert
            _mockRepository.Verify(r =>
                r.AddPatient(patient),
                Times.Once);
        }

        // Add Duplicate Patient
        [Fact]
        public void AddPatient_DuplicateEmail_ShouldThrowException()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            List<Patient> patients =
            [
                patient
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllPatients())
                .Returns(patients);

            // Act & Assert
            Assert.Throws<
                DuplicatePatientException>(() =>
                    _patientService.AddPatient(
                        patient));
        }

        // Get Patient By Existing Id
        [Fact]
        public void GetPatientById_ExistingId_ShouldReturnPatient()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        patient.PatientId))
                .Returns(patient);

            // Act
            Patient? result =
                _patientService.GetPatientById(
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
        public void GetPatientById_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        It.IsAny<int>()))
                .Returns((Patient?)null);

            // Act & Assert
            Assert.Throws<
                PatientNotFoundException>(() =>
                    _patientService.GetPatientById(
                       322));
        }

        // Get All Patients
        [Fact]
        public void GetAllPatients_ShouldReturnAllPatients()
        {
            // Arrange
            List<Patient> patients =
            [
                CreatePatient(),
                CreatePatient()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllPatients())
                .Returns(patients);

            // Act
            List<Patient> result =
                _patientService.GetAllPatients();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Patients Empty
        [Fact]
        public void GetAllPatients_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllPatients())
                .Returns(new List<Patient>());

            // Act
            List<Patient> result =
                _patientService.GetAllPatients();

            // Assert
            Assert.Empty(
                result);
        }

        // Get Patient By Existing Email
        [Fact]
        public void GetPatientByEmail_ExistingEmail_ShouldReturnPatient()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            List<Patient> patients =
            [
                patient
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllPatients())
                .Returns(patients);

            // Act
            Patient? result =
                _patientService
                .GetPatientByEmail(
                    patient.Email);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                patient.Email,
                result?.Email);
        }

        // Get Patient By Invalid Email
        [Fact]
        public void GetPatientByEmail_InvalidEmail_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllPatients())
                .Returns(new List<Patient>());

            // Act & Assert
            Assert.Throws<
                PatientNotFoundException>(() =>
                    _patientService
                    .GetPatientByEmail(
                        "invalid@gmail.com"));
        }

        // Update Existing Patient
        [Fact]
        public void UpdatePatient_ExistingPatient_ShouldUpdatePatient()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        patient.PatientId))
                .Returns(patient);

            // Act
            _patientService.UpdatePatient(
                patient);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdatePatient(patient),
                Times.Once);
        }

        // Update Invalid Patient
        [Fact]
        public void UpdatePatient_InvalidId_ShouldThrowException()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        patient.PatientId))
                .Returns((Patient?)null);

            // Act & Assert
            Assert.Throws<
                PatientNotFoundException>(() =>
                    _patientService.UpdatePatient(
                        patient));
        }

        // Delete Existing Patient
        [Fact]
        public void DeletePatientById_ExistingId_ShouldDeletePatient()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            patient.PatientId = 1;

            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        patient.PatientId))
                .Returns(patient);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            // Act
            _patientService.DeletePatientById(
                patient.PatientId);

            // Assert
            _mockRepository.Verify(r =>
                r.DeletePatientById(
                    patient.PatientId),
                Times.Once);
        }

        // Delete Invalid Patient
        [Fact]
        public void DeletePatientById_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        It.IsAny<int>()))
                .Returns((Patient?)null);

            // Act & Assert
            Assert.Throws<
                PatientNotFoundException>(() =>
                    _patientService.DeletePatientById(
                        322));
        }

        // Delete Patient With Confirmed Appointment
        [Fact]
        public void DeletePatientById_ConfirmedAppointment_ShouldThrowException()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            patient.PatientId = 1;

            Appointment appointment = new()
            {
                AppointmentId = 1,

                Patient = patient,

                Doctor = CreateDoctor(),

                ScheduledDate =
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),

                TimeSlot =
                    new TimeOnly(10, 0),

                Status =
                    AppointmentStatus.Confirmed
            };

            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        patient.PatientId))
                .Returns(patient);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(
                [
                    appointment
                ]);

            // Act & Assert
            Assert.Throws<
                PatientDeletionException>(() =>
                    _patientService
                    .DeletePatientById(
                        patient.PatientId));
        }

        // Delete Patient With Pending Appointment
        [Fact]
        public void DeletePatientById_PendingAppointment_ShouldCancelAndDelete()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            patient.PatientId = 1;

            Appointment appointment = new()
            {
                AppointmentId = 1,

                Patient = patient,

                Doctor = CreateDoctor(),

                ScheduledDate =
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),

                TimeSlot =
                    new TimeOnly(10, 0),

                Status =
                    AppointmentStatus.Pending
            };

            _mockRepository
                .Setup(r =>
                    r.GetPatientById(
                        patient.PatientId))
                .Returns(patient);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(
                [
                    appointment
                ]);

            // Act
            _patientService
                .DeletePatientById(
                    patient.PatientId);

            // Assert
            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _mockAppointmentRepository
                .Verify(r =>
                    r.UpdateAppointment(
                        appointment),
                    Times.Once);

            _mockRepository
                .Verify(r =>
                    r.DeletePatientById(
                        patient.PatientId),
                    Times.Once);
        }

        // Helper Method
        private static Patient CreatePatient()
        {
            return new Patient
            {
                PatientId =
                    1,

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
                DoctorId = 1,

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
    }
}
