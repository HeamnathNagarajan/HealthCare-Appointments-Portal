
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Services;
using Moq;

namespace HealthCare_Appointments_Portal.Tests
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
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns(new List<Appointment>());

            Appointment result =
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                    new TimeOnly(10, 0));

            Assert.NotNull(result);

            Assert.Equal(
                AppointmentStatus.Pending,
                result.Status);

            _mockRepository.Verify(
                r => r.AddAppointment(
                    It.IsAny<Appointment>()),
                Times.Once);
        }

        // Past Date
        [Fact]
        public void BookAppointment_PastDate_ShouldThrowException()
        {
            Assert.Throws<PastDateException>(() =>
                _appointmentService
                .BookAppointment(
                    CreatePatient(),
                    CreateDoctor(),
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(-1)),
                    new TimeOnly(10, 0)));
        }

        // Past Time
        [Fact]
        public void BookAppointment_PastTime_ShouldThrowException()
        {
            Assert.Throws<PastTimeSlotException>(() =>
                _appointmentService
                .BookAppointment(
                    CreatePatient(),
                    CreateDoctor(),
                    DateOnly.FromDateTime(DateTime.Now),
                    TimeOnly.FromDateTime(
                        DateTime.Now.AddHours(-1))));
        }

        // Doctor Unavailable
        [Fact]
        public void BookAppointment_DoctorUnavailable_ShouldThrowException()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            for (int i = 0; i < 10; i++)
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

            Assert.Throws<DoctorUnavailableException>(() =>
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                    new TimeOnly(10, 0)));
        }

        // Conflict
        [Fact]
        public void BookAppointment_Conflict_ShouldThrowException()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
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
                };

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns([appointment]);

            Assert.Throws<AppointmentConflictException>(() =>
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    appointment.ScheduledDate,
                    appointment.TimeSlot));
        }

        // Cancelled Appointment Allow
        [Fact]
        public void BookAppointment_CancelledAppointment_ShouldAllowBooking()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            Appointment cancelled =
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
                        AppointmentStatus.Cancelled
                };

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns([cancelled]);

            Appointment result =
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    cancelled.ScheduledDate,
                    cancelled.TimeSlot);

            Assert.NotNull(result);
        }

        // Get Appointment
        [Fact]
        public void GetAppointmentById_ShouldReturnAppointment()
        {
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            Appointment? result =
                _appointmentService
                .GetAppointmentById(
                    appointment.AppointmentId);

            Assert.NotNull(result);
        }

        // Invalid Appointment
        [Fact]
        public void GetAppointmentById_Invalid_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        It.IsAny<int>()))
                .Returns((Appointment?)null);

            Assert.Throws<AppointmentNotFoundException>(() =>
                _appointmentService
                .GetAppointmentById(100));
        }

        // Get All
        [Fact]
        public void GetAllAppointments_ShouldReturnAppointments()
        {
            List<Appointment> appointments =
            [
                CreateAppointment(),
                CreateAppointment()
            ];

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetAllAppointments();

            Assert.Equal(2, result.Count);
        }

        // Patient Appointments Ordered
        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnOrderedAppointments()
        {
            Patient patient =
                CreatePatient();

            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = patient,
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                        new DateOnly(2026,12,30),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                },

                new Appointment
                {
                    Patient = patient,
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                        new DateOnly(2026,1,1),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                }
            ];

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByPatient(
                    patient.PatientId);

            Assert.Equal(
                new DateOnly(2026, 1, 1),
                result[0].ScheduledDate);
        }

        // Doctor Appointments Ordered
        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnOrderedAppointments()
        {
            Doctor doctor =
                CreateDoctor();

            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = CreatePatient(),
                    Doctor = doctor,
                    ScheduledDate =
                        new DateOnly(2026,12,30),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                },

                new Appointment
                {
                    Patient = CreatePatient(),
                    Doctor = doctor,
                    ScheduledDate =
                        new DateOnly(2026,1,1),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                }
            ];

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByDoctor(
                    doctor.DoctorId);

            Assert.Equal(
                new DateOnly(2026, 1, 1),
                result[0].ScheduledDate);
        }

        // Upcoming
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnConfirmed()
        {
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
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            Assert.Single(result);
        }

        // Ignore Past Upcoming
        [Fact]
        public void GetUpcomingAppointments_Past_ShouldIgnore()
        {
            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = CreatePatient(),
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(-1)),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Confirmed
                }
            ];

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            Assert.Empty(result);
        }

        // Completed
        [Fact]
        public void GetCompletedAppointments_ShouldReturnCompleted()
        {
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
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetCompletedAppointments();

            Assert.Single(result);
        }

        // Confirm Appointment
        [Fact]
        public void ConfirmAppointment_ShouldConfirm()
        {
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            _appointmentService
                .ConfirmAppointment(
                    appointment.AppointmentId);

            Assert.Equal(
                AppointmentStatus.Confirmed,
                appointment.Status);

            _mockRepository.Verify(
                r => r.UpdateAppointment(
                    appointment),
                Times.Once);
        }

        // Cancel Appointment
        [Fact]
        public void CancelAppointment_ShouldCancel()
        {
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            _appointmentService
                .CancelAppointment(
                    appointment.AppointmentId,
                    "Reason");

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _mockRepository.Verify(
                r => r.UpdateAppointment(
                    appointment),
                Times.Once);
        }

        // Complete Appointment
        [Fact]
        public void CompleteAppointment_ShouldComplete()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Confirmed;

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

            _mockRepository.Verify(
                r => r.UpdateAppointment(
                    appointment),
                Times.Once);
        }

        // Update Appointment
        [Fact]
        public void UpdateAppointment_ShouldUpdate()
        {
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            _appointmentService
                .UpdateAppointment(
                    appointment);

            _mockRepository.Verify(
                r => r.UpdateAppointment(
                    appointment),
                Times.Once);
        }

        // Delete Pending
        [Fact]
        public void DeleteAppointment_Pending_ShouldThrow()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Pending;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            Assert.Throws<AppointmentDeletionException>(() =>
                _appointmentService
                .DeleteAppointmentById(
                    appointment.AppointmentId));
        }

        // Delete Confirmed
        [Fact]
        public void DeleteAppointment_Confirmed_ShouldThrow()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Confirmed;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            Assert.Throws<AppointmentDeletionException>(() =>
                _appointmentService
                .DeleteAppointmentById(
                    appointment.AppointmentId));
        }

        // Delete Completed
        [Fact]
        public void DeleteAppointment_Completed_ShouldDelete()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Completed;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            _appointmentService
                .DeleteAppointmentById(
                    appointment.AppointmentId);

            _mockRepository.Verify(
                r => r.DeleteAppointmentById(
                    appointment.AppointmentId),
                Times.Once);
        }

        // Delete Cancelled
        [Fact]
        public void DeleteAppointment_Cancelled_ShouldDelete()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Cancelled;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            _appointmentService
                .DeleteAppointmentById(
                    appointment.AppointmentId);

            _mockRepository.Verify(
                r => r.DeleteAppointmentById(
                    appointment.AppointmentId),
                Times.Once);
        }

        // Helpers
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

        private static Appointment CreateAppointment()
        {
            return new Appointment
            {
                AppointmentId = 1,
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
        // Confirm Appointment Invalid Id
        [Fact]
        public void ConfirmAppointment_InvalidId_ShouldThrowException()
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
                    .ConfirmAppointment(999));
        }

        // Cancel Appointment Invalid Id
        [Fact]
        public void CancelAppointment_InvalidId_ShouldThrowException()
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
                    .CancelAppointment(
                        999,
                        "Reason"));
        }

        // Complete Appointment Invalid Id
        [Fact]
        public void CompleteAppointment_InvalidId_ShouldThrowException()
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
                    .CompleteAppointment(999));
        }

        // Update Appointment Invalid Id
        [Fact]
        public void UpdateAppointment_InvalidId_ShouldThrowException()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns((Appointment?)null);

            // Act & Assert
            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .UpdateAppointment(
                        appointment));
        }

        // Get Appointments By Patient Empty
        [Fact]
        public void GetAppointmentsByPatient_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByPatient(1);

            // Assert
            Assert.Empty(result);
        }

        // Get Appointments By Doctor Empty
        [Fact]
        public void GetAppointmentsByDoctor_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByDoctor(1);

            // Assert
            Assert.Empty(result);
        }

        // Upcoming Empty
        [Fact]
        public void GetUpcomingAppointments_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            // Act
            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            // Assert
            Assert.Empty(result);
        }

        // Get Completed Empty
        [Fact]
        public void GetCompletedAppointments_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            // Act
            List<Appointment> result =
                _appointmentService
                .GetCompletedAppointments();

            // Assert
            Assert.Empty(result);
        }

        // Book Appointment Same Day Future Time
        [Fact]
        public void BookAppointment_SameDayFutureTime_ShouldBookAppointment()
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

            TimeOnly futureTime =
                TimeOnly.FromDateTime(
                    DateTime.Now.AddHours(2));

            // Act
            Appointment result =
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    DateOnly.FromDateTime(DateTime.Now),
                    futureTime);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(
                AppointmentStatus.Pending,
                result.Status);
        }

        // Get Upcoming ShouldIgnoreCancelled
        [Fact]
        public void GetUpcomingAppointments_Cancelled_ShouldIgnore()
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
                AppointmentStatus.Cancelled
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
            Assert.Empty(result);
        }
        [Fact]
        public void DeleteAppointment_InvalidId_ShouldThrowException()
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
                 .DeleteAppointmentById(999));

        }

        [Fact]
        public void GetAppointmentsByPatient_ShouldFilterCorrectPatient()
        {
            // Arrange
            Patient patient1 = CreatePatient();

            Patient patient2 =
            new Patient
            {
                PatientId = 2,
                FullName = "Kumar",
                DateOfBirth =
                new DateOnly(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "kumar@gmail.com",
                InsuranceId = "INS102"
            };

            List<Appointment> appointments =
            [
                new Appointment
                {
                    Patient = patient1,
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                         DateOnly.FromDateTime(
                         DateTime.Now.AddDays(1)),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                 },

                new Appointment
                {
                    Patient = patient2,
                    Doctor = CreateDoctor(),
                    ScheduledDate =
                        DateOnly.FromDateTime(
                        DateTime.Now.AddDays(2)),
                    TimeSlot =
                        new TimeOnly(11,0),
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
                    patient1.PatientId);

            // Assert
            Assert.Single(result);

        }

        [Fact]
        public void GetAppointmentsByDoctor_ShouldFilterCorrectDoctor()
        {
            // Arrange
            Doctor doctor1 = CreateDoctor();

            Doctor doctor2 =
            new Doctor
            {
                DoctorId = 2,
                FullName = "Dr Kumar",
                Specialisation =
                    Specialisation.Neurology,
                YearsOfExperience = 7,
                ConsultationFee = 1500,
                IsActive = true
            };

            List<Appointment> appointments =
            [
                new Appointment
                {
                   Patient = CreatePatient(),
                   Doctor = doctor1,
                    ScheduledDate =
                        DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                    TimeSlot =
                        new TimeOnly(10,0),
                    Status =
                        AppointmentStatus.Pending
                },

                new Appointment
                {
                    Patient = CreatePatient(),
                    Doctor = doctor2,
                    ScheduledDate =
                        DateOnly.FromDateTime(
                        DateTime.Now.AddDays(2)),
                    TimeSlot =
                        new TimeOnly(11,0),
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
                    doctor1.DoctorId);

            // Assert
            Assert.Single(result);

        }

        [Fact]
        public void GetUpcomingAppointments_ShouldIgnorePending()
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
                .GetUpcomingAppointments();

            // Assert
            Assert.Empty(result);

        }
        // Get Appointment By Id Exact Verification
        [Fact]
        public void GetAppointmentById_ShouldCallRepositoryOnce()
        {
            // Arrange
            Appointment appointment = CreateAppointment();

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
            Assert.Equal(
                appointment.AppointmentId,
                result!.AppointmentId);

            _mockRepository.Verify(
                r => r.GetAppointmentById(
                    appointment.AppointmentId),
                Times.Once);
        }

        // Get All Appointments Empty
        [Fact]
        public void GetAllAppointments_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns(new List<Appointment>());

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAllAppointments();

            // Assert
            Assert.NotNull(result);

            Assert.Empty(result);
        }

        // Get Upcoming Appointments Ordered
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnOrderedAppointments()
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
                    DateTime.Now.AddDays(5)),
            TimeSlot =
                new TimeOnly(10,0),
            Status =
                AppointmentStatus.Confirmed
        },

        new Appointment
        {
            Patient = CreatePatient(),
            Doctor = CreateDoctor(),
            ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(1)),
            TimeSlot =
                new TimeOnly(11,0),
            Status =
                AppointmentStatus.Confirmed
        }
            ];

            _mockRepository
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.True(
                result[0].ScheduledDate <
                result[1].ScheduledDate);
        }

        // Get Appointments By Patient No Matching Patient
        [Fact]
        public void GetAppointmentsByPatient_NoMatchingPatient_ShouldReturnEmpty()
        {
            // Arrange
            List<Appointment> appointments =
            [
                new Appointment
        {
            Patient = new Patient
            {
                PatientId = 99,
                FullName = "Other"
            },
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
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByPatient(1);

            // Assert
            Assert.Empty(result);
        }

        // Get Appointments By Doctor No Matching Doctor
        [Fact]
        public void GetAppointmentsByDoctor_NoMatchingDoctor_ShouldReturnEmpty()
        {
            // Arrange
            List<Appointment> appointments =
            [
                new Appointment
        {
            Patient = CreatePatient(),
            Doctor = new Doctor
            {
                DoctorId = 99,
                FullName = "Other Doctor"
            },
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
                .Setup(r => r.GetAllAppointments())
                .Returns(appointments);

            // Act
            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByDoctor(1);

            // Assert
            Assert.Empty(result);
        }


        // Confirm Non Pending Appointment
        [Fact]
        public void ConfirmAppointment_NonPending_ShouldThrowException()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Completed;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act & Assert
            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .ConfirmAppointment(
                        appointment.AppointmentId));
        }

        // Cancel Completed Appointment
        [Fact]
        public void CancelAppointment_Completed_ShouldThrowException()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Completed;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act & Assert
            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .CancelAppointment(
                        appointment.AppointmentId,
                        "Reason"));
        }

        // Cancel Cancelled Appointment
        [Fact]
        public void CancelAppointment_AlreadyCancelled_ShouldThrowException()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Cancelled;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act & Assert
            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .CancelAppointment(
                        appointment.AppointmentId,
                        "Reason"));
        }

        // Complete Pending Appointment
        [Fact]
        public void CompleteAppointment_Pending_ShouldThrowException()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Pending;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act & Assert
            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .CompleteAppointment(
                        appointment.AppointmentId));
        }

        // Complete Cancelled Appointment
        [Fact]
        public void CompleteAppointment_Cancelled_ShouldThrowException()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Cancelled;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            // Act & Assert
            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .CompleteAppointment(
                        appointment.AppointmentId));
        }
    }
}