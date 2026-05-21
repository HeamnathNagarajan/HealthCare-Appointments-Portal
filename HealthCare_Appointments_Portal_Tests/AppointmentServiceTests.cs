using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository>
            _mockRepository;

        private readonly AppointmentService
            _appointmentService;

        public AppointmentServiceTests()
        {
            _mockRepository =
                new Mock<IAppointmentRepository>();

            _appointmentService =
                new AppointmentService(
                    _mockRepository.Object);
        }

        // Book Appointment Success
        [Fact]
        public void BookAppointment_ValidData_ShouldCreateAppointment()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            // Act
            Appointment result =
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                    new TimeOnly(10, 0));

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                AppointmentStatus.Pending,
                result.Status);

            _mockRepository.Verify(r =>
                r.AddAppointment(
                    It.IsAny<Appointment>()),
                Times.Once);
        }

        // Book Appointment Past Date
        [Fact]
        public void BookAppointment_PastDate_ShouldThrowException()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            // Act & Assert
            Assert.Throws<
                PastDateException>(() =>
                    _appointmentService
                    .BookAppointment(
                        patient,
                        doctor,
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(-1)),
                        new TimeOnly(10, 0)));
        }

        // Book Appointment Doctor Unavailable
        [Fact]
        public void BookAppointment_DoctorUnavailable_ShouldThrowException()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            for (int i = 0;
                i < 10;
                i++)
            {
                doctor.Appointments.Add(
                    new Appointment
                    {
                        Patient = patient,
                        Doctor = doctor,
                        ScheduledDate =
                            DateOnly.FromDateTime(
                                DateTime.Now.AddDays(1)),
                        TimeSlot =
                            new TimeOnly(10, 0),
                        Status =
                            AppointmentStatus.Confirmed
                    });
            }

            // Act & Assert
            Assert.Throws<
                DoctorUnavailableException>(() =>
                    _appointmentService
                    .BookAppointment(
                        patient,
                        doctor,
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(1)),
                        new TimeOnly(10, 0)));
        }

        // Book Appointment Conflict
        [Fact]
        public void BookAppointment_Conflict_ShouldThrowException()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            Appointment existingAppointment =
                new()
                {
                    Patient = patient,
                    Doctor = doctor,
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
                    r.GetAllAppointments())
                .Returns(
                [
                    existingAppointment
                ]);

            // Act & Assert
            Assert.Throws<
                AppointmentConflictException>(() =>
                    _appointmentService
                    .BookAppointment(
                        patient,
                        doctor,
                        existingAppointment.ScheduledDate,
                        existingAppointment.TimeSlot));
        }

        // Get Appointment By Existing Id
        [Fact]
        public void GetAppointmentById_ExistingId_ShouldReturnAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act
            Appointment? result =
                _appointmentService
                .GetAppointmentById(
                    appointment.AppointmentId);

            // Assert
            Assert.NotNull(
                result);
        }

        // Get Appointment By Invalid Id
        [Fact]
        public void GetAppointmentById_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        It.IsAny<int>()))
                .Returns((Appointment?)null);

            // Act & Assert
            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .GetAppointmentById(
                       99));
        }

        // Get All Appointments
        [Fact]
        public void GetAllAppointments_ShouldReturnAppointments()
        {
            // Arrange
            List<Appointment> appointments =
            [
                CreateAppointment(),
                CreateAppointment()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAllAppointments();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get Appointments By Patient
        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnPatientAppointments()
        {
            // Arrange
            Patient patient =
                CreatePatient();

            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = patient,
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(1)),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByPatient(
                    patient.PatientId);

            // Assert
            Assert.Single(
                result);
        }

        // Get Appointments By Doctor
        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnDoctorAppointments()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = CreatePatient(),
                    Doctor = doctor,
                    ScheduledDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(1)),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByDoctor(
                    doctor.DoctorId);

            // Assert
            Assert.Single(
                result);
        }

        // Get Upcoming Appointments
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnConfirmedAppointments()
        {
            // Arrange
            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = CreatePatient(),
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(1)),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Confirmed
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            // Assert
            Assert.Single(
                result);
        }

        // Get Completed Appointments
        [Fact]
        public void GetCompletedAppointments_ShouldReturnCompletedAppointments()
        {
            // Arrange
            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = CreatePatient(),
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(1)),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Completed
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetCompletedAppointments();

            // Assert
            Assert.Single(
                result);
        }

        // Confirm Appointment
        [Fact]
        public void ConfirmAppointment_ExistingAppointment_ShouldUpdateStatus()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act
            _appointmentService
                .ConfirmAppointment(
                    appointment.AppointmentId);

            // Assert
            Assert.Equal(
                AppointmentStatus.Confirmed,
                appointment.Status);

            _mockRepository.Verify(r =>
                r.UpdateAppointment(
                    appointment),
                Times.Once);
        }

        // Cancel Appointment
        [Fact]
        public void CancelAppointment_ExistingAppointment_ShouldCancelAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act
            _appointmentService
                .CancelAppointment(
                    appointment.AppointmentId,
                    "Patient Request");

            // Assert
            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            Assert.Equal(
                "Patient Request",
                appointment.CancellationReason);
        }

        // Complete Appointment
        [Fact]
        public void CompleteAppointment_ExistingAppointment_ShouldCompleteAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act
            _appointmentService
                .CompleteAppointment(
                    appointment.AppointmentId);

            // Assert
            Assert.Equal(
                AppointmentStatus.Completed,
                appointment.Status);
        }

        // Update Existing Appointment
        [Fact]
        public void UpdateAppointment_ExistingAppointment_ShouldUpdateAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act
            _appointmentService
                .UpdateAppointment(
                    appointment);

            // Assert
            _mockRepository.Verify(r =>
                r.UpdateAppointment(
                    appointment),
                Times.Once);
        }

        // Delete Existing Appointment
        [Fact]
        public void DeleteAppointmentById_ExistingId_ShouldDeleteAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act
            _appointmentService
                .DeleteAppointmentById(
                    appointment.AppointmentId);

            // Assert
            _mockRepository.Verify(r =>
                r.DeleteAppointmentById(
                    appointment.AppointmentId),
                Times.Once);
        }

        // Helper Methods
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

        private static Doctor CreateDoctor()
        {
            return new Doctor
            {
                FullName = "Dr Ragu",
                Specialisation =
                    Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 1000,
                IsActive = true
            };
        }

        private static Appointment CreateAppointment()
        {
            return new Appointment
            {
                Patient = CreatePatient(),
                Doctor = CreateDoctor(),
                ScheduledDate =
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                TimeSlot =
                    new TimeOnly(10, 0),
                Status =
                    AppointmentStatus.Pending
            };
        }
    }
}