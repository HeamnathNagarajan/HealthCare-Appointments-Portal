using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests
{
    public partial class AppointmentServiceTests
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

        [Fact]
        public void BookAppointment_ValidData_ShouldCreateAppointment()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetConflictingAppointment(
                        doctor.DoctorId,
                        It.IsAny<DateOnly>(),
                        It.IsAny<TimeOnly>()))
                .Returns((Appointment?)null);

            Appointment result =
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    doctor.Specialisation,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                    new TimeOnly(10, 0));

            Assert.NotNull(result);

            Assert.Equal(
                AppointmentStatus.Pending,
                result.Status);
        }

        [Fact]
        public void BookAppointment_PastDate_ShouldThrowException()
        {
            Assert.Throws<PastDateException>(() =>
                _appointmentService.BookAppointment(
                    CreatePatient(),
                    CreateDoctor(),
                    Specialisation.Cardiology,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(-1)),
                    new TimeOnly(10, 0)));
        }

        [Fact]
        public void BookAppointment_PastTime_ShouldThrowException()
        {
            Assert.Throws<PastTimeSlotException>(() =>
                _appointmentService.BookAppointment(
                    CreatePatient(),
                    CreateDoctor(),
                    Specialisation.Cardiology,
                    DateOnly.FromDateTime(DateTime.Now),
                    TimeOnly.FromDateTime(
                        DateTime.Now.AddHours(-1))));
        }

        [Fact]
        public void BookAppointment_Conflict_ShouldThrowException()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetConflictingAppointment(
                        doctor.DoctorId,
                        appointment.ScheduledDate,
                        appointment.TimeSlot))
                .Returns(appointment);

            Assert.Throws<
                AppointmentConflictException>(() =>
                    _appointmentService
                    .BookAppointment(
                        patient,
                        doctor,
                        doctor.Specialisation,
                        appointment.ScheduledDate,
                        appointment.TimeSlot));
        }

        // Get Appointment By Id
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

            Assert.Equal(
                appointment.AppointmentId,
                result?.AppointmentId);
        }

        // Invalid Appointment Id
        [Fact]
        public void GetAppointmentById_Invalid_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        It.IsAny<int>()))
                .Returns((Appointment?)null);

            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .GetAppointmentById(
                        999));
        }

        // Verify Repository Called
        [Fact]
        public void GetAppointmentById_ShouldCallRepositoryOnce()
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

            _mockRepository.Verify(
                r => r.GetAppointmentById(
                    appointment.AppointmentId),
                Times.Once);
        }

        // Get All Appointments
        [Fact]
        public void GetAllAppointments_ShouldReturnAppointments()
        {
            List<Appointment> appointments =
            [
                CreateAppointment(),
                CreateAppointment()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetAllAppointments();

            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Empty
        [Fact]
        public void GetAllAppointments_Empty_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            List<Appointment> result =
                _appointmentService
                .GetAllAppointments();

            Assert.Empty(result);
        }
        // Get Appointments By Patient Ordered
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
                new DateOnly(2026, 12, 30),
            TimeSlot =
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Pending
        },

        new Appointment
        {
            Patient = patient,
            Doctor = CreateDoctor(),
            ScheduledDate =
                new DateOnly(2026, 1, 1),
            TimeSlot =
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Pending
        }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentsByPatient(
                        patient.PatientId))
                .Returns(
                    appointments
                    .OrderBy(a =>
                        a.ScheduledDate)
                    .ToList());

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByPatient(
                    patient.PatientId);

            Assert.Equal(
                new DateOnly(2026, 1, 1),
                result[0].ScheduledDate);
        }

        // Get Appointments By Patient Empty
        [Fact]
        public void GetAppointmentsByPatient_Empty_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentsByPatient(1))
                .Returns(new List<Appointment>());

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByPatient(1);

            Assert.Empty(result);
        }

        // Get Appointments By Patient No Match
        [Fact]
        public void GetAppointmentsByPatient_NoMatchingPatient_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentsByPatient(1))
                .Returns(new List<Appointment>());

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByPatient(1);

            Assert.Empty(result);
        }

        // Get Appointments By Doctor Ordered
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
                new DateOnly(2026, 12, 30),
            TimeSlot =
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Pending
        },

        new Appointment
        {
            Patient = CreatePatient(),
            Doctor = doctor,
            ScheduledDate =
                new DateOnly(2026, 1, 1),
            TimeSlot =
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Pending
        }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(
                        doctor.DoctorId))
                .Returns(
                    appointments
                    .OrderBy(a =>
                        a.ScheduledDate)
                    .ToList());

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByDoctor(
                    doctor.DoctorId);

            Assert.Equal(
                new DateOnly(2026, 1, 1),
                result[0].ScheduledDate);
        }

        // Get Appointments By Doctor Empty
        [Fact]
        public void GetAppointmentsByDoctor_Empty_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(1))
                .Returns(new List<Appointment>());

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByDoctor(1);

            Assert.Empty(result);
        }

        // Get Appointments By Doctor No Match
        [Fact]
        public void GetAppointmentsByDoctor_NoMatchingDoctor_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(1))
                .Returns(new List<Appointment>());

            List<Appointment> result =
                _appointmentService
                .GetAppointmentsByDoctor(1);

            Assert.Empty(result);
        }
        // Get Upcoming Appointments
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
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Confirmed
        }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetUpcomingAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            Assert.Single(result);
        }

        // Get Upcoming Empty
        [Fact]
        public void GetUpcomingAppointments_Empty_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetUpcomingAppointments())
                .Returns(new List<Appointment>());

            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            Assert.Empty(result);
        }

        // Get Upcoming Ordered
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnOrderedAppointments()
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
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Confirmed
        },

        new Appointment
        {
            Patient = CreatePatient(),
            Doctor = CreateDoctor(),
            ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(5)),
            TimeSlot =
                new TimeOnly(11, 0),
            Status =
                AppointmentStatus.Confirmed
        }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetUpcomingAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetUpcomingAppointments();

            Assert.Equal(
                2,
                result.Count);
        }

        // Get Completed Appointments
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
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Completed
        }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetCompletedAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetCompletedAppointments();

            Assert.Single(result);
        }

        // Get Completed Empty
        [Fact]
        public void GetCompletedAppointments_Empty_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetCompletedAppointments())
                .Returns(new List<Appointment>());

            List<Appointment> result =
                _appointmentService
                .GetCompletedAppointments();

            Assert.Empty(result);
        }

        // Multiple Completed Appointments
        [Fact]
        public void GetCompletedAppointments_Multiple_ShouldReturnAll()
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
                new TimeOnly(10, 0),
            Status =
                AppointmentStatus.Completed
        },

        new Appointment
        {
            Patient = CreatePatient(),
            Doctor = CreateDoctor(),
            ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(2)),
            TimeSlot =
                new TimeOnly(11, 0),
            Status =
                AppointmentStatus.Completed
        }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetCompletedAppointments())
                .Returns(appointments);

            List<Appointment> result =
                _appointmentService
                .GetCompletedAppointments();

            Assert.Equal(
                2,
                result.Count);
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

        // Confirm Appointment Invalid Id
        [Fact]
        public void ConfirmAppointment_InvalidId_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        It.IsAny<int>()))
                .Returns((Appointment?)null);

            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .ConfirmAppointment(999));
        }

        // Confirm Non Pending Appointment
        [Fact]
        public void ConfirmAppointment_NonPending_ShouldThrowException()
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

            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .ConfirmAppointment(
                        appointment.AppointmentId));
        }

        // Cancel Appointment
        [Fact]
        public void CancelAppointment_ShouldCancel()
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

        // Cancel Appointment Invalid Id
        [Fact]
        public void CancelAppointment_InvalidId_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        It.IsAny<int>()))
                .Returns((Appointment?)null);

            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .CancelAppointment(
                        999,
                        "Reason"));
        }

        // Cancel Completed Appointment
        [Fact]
        public void CancelAppointment_Completed_ShouldThrowException()
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

            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .CancelAppointment(
                        appointment.AppointmentId,
                        "Reason"));
        }

        // Cancel Already Cancelled
        [Fact]
        public void CancelAppointment_AlreadyCancelled_ShouldThrowException()
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

            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .CancelAppointment(
                        appointment.AppointmentId,
                        "Reason"));
        }

        // Complete Appointment
        [Fact]
        public void CompleteAppointment_ShouldComplete()
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

            _appointmentService
                .CompleteAppointment(
                    appointment.AppointmentId);

            Assert.Equal(
                AppointmentStatus.Completed,
                appointment.Status);

            _mockRepository.Verify(
                r => r.UpdateAppointment(
                    appointment),
                Times.Once);
        }

        // Complete Appointment Invalid Id
        [Fact]
        public void CompleteAppointment_InvalidId_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        It.IsAny<int>()))
                .Returns((Appointment?)null);

            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .CompleteAppointment(999));
        }

        // Complete Pending Appointment
        [Fact]
        public void CompleteAppointment_Pending_ShouldThrowException()
        {
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

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
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Cancelled;

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns(appointment);

            Assert.Throws<
                InvalidAppointmentStatusException>(() =>
                    _appointmentService
                    .CompleteAppointment(
                        appointment.AppointmentId));
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

        // Update Appointment Invalid Id
        [Fact]
        public void UpdateAppointment_InvalidId_ShouldThrowException()
        {
            Appointment appointment =
                CreateAppointment();

            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        appointment.AppointmentId))
                .Returns((Appointment?)null);

            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .UpdateAppointment(
                        appointment));
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

            Assert.Throws<
                AppointmentDeletionException>(() =>
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

            Assert.Throws<
                AppointmentDeletionException>(() =>
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

        // Delete Invalid Id
        [Fact]
        public void DeleteAppointment_InvalidId_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAppointmentById(
                        It.IsAny<int>()))
                .Returns((Appointment?)null);

            Assert.Throws<
                AppointmentNotFoundException>(() =>
                    _appointmentService
                    .DeleteAppointmentById(
                        999));
        }

        // Invalid Doctor Specialisation
        [Fact]
        public void BookAppointment_InvalidDoctorSpecialisation_ShouldThrowException()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            Assert.Throws<
                InvalidDoctorSpecialisationException>(() =>
                    _appointmentService
                    .BookAppointment(
                        patient,
                        doctor,
                        Specialisation.Neurology,
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(1)),
                        new TimeOnly(10, 0)));
        }

        // Advance Booking Limit
        [Fact]
        public void BookAppointment_MoreThanSixMonths_ShouldThrowException()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            Assert.Throws<
                AdvanceBookingLimitException>(() =>
                    _appointmentService
                    .BookAppointment(
                        patient,
                        doctor,
                        doctor.Specialisation,
                        DateOnly.FromDateTime(
                            DateTime.Now.AddMonths(7)),
                        new TimeOnly(10, 0)));
        }

        // Same Day Future Time
        [Fact]
        public void BookAppointment_SameDayFutureTime_ShouldBookAppointment()
        {
            Patient patient =
                CreatePatient();

            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetConflictingAppointment(
                        doctor.DoctorId,
                        It.IsAny<DateOnly>(),
                        It.IsAny<TimeOnly>()))
                .Returns((Appointment?)null);

            Appointment result =
                _appointmentService
                .BookAppointment(
                    patient,
                    doctor,
                    doctor.Specialisation,
                    DateOnly.FromDateTime(
                        DateTime.Now),
                    TimeOnly.FromDateTime(
                        DateTime.Now.AddHours(2)));

            Assert.NotNull(result);
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
                    CreateAppointment());
            }

            Assert.Throws<
                DoctorUnavailableException>(() =>
                    _appointmentService
                    .BookAppointment(
                        patient,
                        doctor,
                        doctor.Specialisation,
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(1)),
                        new TimeOnly(10, 0)));
        }

        // Helper Methods
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
    }
}