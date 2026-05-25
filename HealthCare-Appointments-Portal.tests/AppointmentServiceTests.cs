using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Services;
using HealthCare_Appointments_Portal.Exceptions;
using Moq;
using Xunit;

namespace HealthCare_Appointments_Portal.Tests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _mockRepo;
        private readonly AppointmentService _service;

        private readonly Patient _patient;
        private readonly Doctor _doctor;

        public AppointmentServiceTests()
        {
            _mockRepo = new Mock<IAppointmentRepository>();
            _service = new AppointmentService(_mockRepo.Object);

            _patient = new Patient
            {
                PatientId = Guid.NewGuid(),
                FullName = "Test Patient"
            };

            _doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FullName = "Test Doctor",
                IsActive = true,
                Appointments = new List<Appointment>()
            };
        }

        private static Appointment CreateAppointment(Patient patient, Doctor doctor, DateOnly date, TimeOnly time)
        {
            return new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                TimeSlot = time,
                Status = AppointmentStatus.Pending
            };
        }

        // ✅ Happy path
        [Fact]
        public void BookAppointment_ValidSlot_ShouldCreateAppointment()
        {
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var time = new TimeOnly(10, 0);

            _mockRepo.Setup(r => r.GetAllAppointments())
                     .Returns(new List<Appointment>());

            var result = _service.BookAppointment(_patient, _doctor, date, time);

            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Pending, result.Status);

            _mockRepo.Verify(r => r.AddAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        // ✅ Past date
        [Fact]
        public void BookAppointment_PastDate_ShouldThrowPastDateException()
        {
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

            Action act = () => _service.BookAppointment(_patient, _doctor, date, new TimeOnly(10, 0));

            Assert.Throws<PastDateException>(act);
        }

        // ✅ Doctor unavailable (>=10 appointments)
        [Fact]
        public void BookAppointment_DoctorUnavailable_ShouldThrowException()
        {
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

            var doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FullName = "Test Doctor",
                Appointments = new List<Appointment>()
            };

            // ✅ Make doctor unavailable
            for (int i = 0; i < 10; i++)
            {
                doctor.Appointments.Add(new Appointment
                {
                    AppointmentId = Guid.NewGuid(),
                    Patient = _patient,
                    Doctor = doctor,
                    ScheduledDate = date,
                    TimeSlot = new TimeOnly(9 + i, 0),
                    Status = AppointmentStatus.Pending
                });
            }

            _mockRepo.Setup(r => r.GetAllAppointments())
                     .Returns(new List<Appointment>());

            Action act = () => _service.BookAppointment(_patient, doctor, date, new TimeOnly(10, 0));

            Assert.Throws<DoctorUnavailableException>(act);
        }

        // ✅ Slot conflict
        [Fact]
        public void BookAppointment_SlotAlreadyTaken_ShouldThrowConflictException()
        {
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var time = new TimeOnly(11, 0);

            var existing = CreateAppointment(_patient, _doctor, date, time);

            _mockRepo.Setup(r => r.GetAllAppointments())
                     .Returns(new List<Appointment> { existing });

            Action act = () => _service.BookAppointment(_patient, _doctor, date, time);

            Assert.Throws<AppointmentConflictException>(act);
        }

        // ✅ Cancel success
        [Fact]
        public void CancelAppointment_ExistingId_ShouldUpdateStatusToCancelled()
        {
            var appt = CreateAppointment(_patient, _doctor,
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                new TimeOnly(12, 0));

            _mockRepo.Setup(r => r.GetAppointmentById(appt.AppointmentId))
                     .Returns(appt);

            _service.CancelAppointment(appt.AppointmentId, "Test reason");

            Assert.Equal(AppointmentStatus.Cancelled, appt.Status);
        }

        // ✅ FIXED (Sonar issue resolved)
        [Fact]
        public void CancelAppointment_InvalidId_ShouldDoNothing()
        {
            var id = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetAppointmentById(id))
                     .Returns((Appointment?)null);

            _service.CancelAppointment(id, "Reason");

            _mockRepo.Verify(r => r.GetAppointmentById(id), Times.Once);
        }

        // ✅ Get by patient
        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnOnlyPatientAppointments()
        {
            var otherPatient = new Patient { PatientId = Guid.NewGuid() };

            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

            var list = new List<Appointment>
            {
                CreateAppointment(_patient, _doctor, date, new TimeOnly(9, 0)),
                CreateAppointment(otherPatient, _doctor, date, new TimeOnly(10, 0))
            };

            _mockRepo.Setup(r => r.GetAllAppointments()).Returns(list);

            var result = _service.GetAppointmentsByPatient(_patient.PatientId);

            Assert.Single(result);
        }

        // ✅ Get by doctor
        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnDoctorAppointments()
        {
            var otherDoctor = new Doctor { DoctorId = Guid.NewGuid() };

            var list = new List<Appointment>
            {
                CreateAppointment(_patient, _doctor, DateOnly.FromDateTime(DateTime.Now), new TimeOnly(9, 0)),
                CreateAppointment(_patient, otherDoctor, DateOnly.FromDateTime(DateTime.Now), new TimeOnly(10, 0))
            };

            _mockRepo.Setup(r => r.GetAllAppointments()).Returns(list);

            var result = _service.GetAppointmentsByDoctor(_doctor.DoctorId);

            Assert.Single(result);
        }

        // ✅ Upcoming
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnOnlyFutureAppointments()
        {
            var future = CreateAppointment(_patient, _doctor,
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                new TimeOnly(10, 0));

            var past = CreateAppointment(_patient, _doctor,
                DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                new TimeOnly(11, 0));

            var completed = CreateAppointment(_patient, _doctor,
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                new TimeOnly(12, 0));
            completed.Status = AppointmentStatus.Completed;

            var list = new List<Appointment> { future, past, completed };

            _mockRepo.Setup(r => r.GetAllAppointments()).Returns(list);

            var result = _service.GetUpcomingAppointments();

            Assert.Single(result);
        }

        // ✅ Completed
        [Fact]
        public void GetCompletedAppointments_ShouldReturnOnlyCompleted()
        {
            var appt1 = CreateAppointment(_patient, _doctor,
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                new TimeOnly(10, 0));
            appt1.Status = AppointmentStatus.Completed;

            var appt2 = CreateAppointment(_patient, _doctor,
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                new TimeOnly(11, 0));

            var list = new List<Appointment> { appt1, appt2 };

            _mockRepo.Setup(r => r.GetAllAppointments()).Returns(list);

            var result = _service.GetCompletedAppointments();

            Assert.Single(result);
        }
    }
}
