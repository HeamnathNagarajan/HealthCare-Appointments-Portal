using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;
using Xunit;

namespace HealthCare_Appointments_Portal.Tests
{
    public class AppointmentRepositoryTests
    {
        private readonly DataStore _dataStore;
        private readonly AppointmentRepository _repository;
        private readonly Guid _appointmentId;

        public AppointmentRepositoryTests()
        {
            _dataStore = new DataStore();
            _repository = new AppointmentRepository(_dataStore);

            _appointmentId = Guid.Parse(
                "22222222-2222-2222-2222-222222222222");

            // Seed data
            _dataStore.Appointments.Add(
                new Appointment
                {
                    AppointmentId = _appointmentId,

                    Patient = new Patient
                    {
                        PatientId = Guid.NewGuid(),
                        FullName = "John Doe"
                    },

                    Doctor = new Doctor
                    {
                        DoctorId = Guid.NewGuid(),
                        FullName = "Dr Smith"
                    },

                    ScheduledDate = DateOnly.FromDateTime(DateTime.Today),

                    TimeSlot = new TimeOnly(10, 0),

                    Status = AppointmentStatus.Pending,

                    CancellationReason = null!
                });
        }

        // GetAppointmentById

        [Fact]
        public void GetAppointmentById_ShouldReturnAppointment_WhenExists()
        {
            // Act
            Appointment? appointment =
                _repository.GetAppointmentById(_appointmentId);

            // Assert
            Assert.NotNull(appointment);

            Assert.Equal("John Doe",
                appointment.Patient.FullName);

            Assert.Equal("Dr Smith",
                appointment.Doctor.FullName);
        }

        [Fact]
        public void GetAppointmentById_ShouldReturnNull_WhenNotExists()
        {
            // Act
            Appointment? appointment =
                _repository.GetAppointmentById(Guid.NewGuid());

            // Assert
            Assert.Null(appointment);
        }

        // GetAllAppointments

        [Fact]
        public void GetAllAppointments_ShouldReturnAllAppointments()
        {
            var appointments = _repository.GetAllAppointments();

            Assert.Single(appointments);
        }

        [Fact]
        public void GetAllAppointments_ShouldReturnEmptyList_WhenNoData()
        {
            _dataStore.Appointments.Clear();

            var appointments = _repository.GetAllAppointments();

            Assert.Empty(appointments);
        }

        // UpdateAppointment

        [Fact]
        public void UpdateAppointment_ShouldUpdateOnlyProvidedValues()
        {
            Appointment updated = new()
            {
                AppointmentId = _appointmentId,

                Patient = null!,

                Doctor = new Doctor
                {
                    DoctorId = Guid.NewGuid(),
                    FullName = "Dr Strange"
                },

                Status = AppointmentStatus.Completed
            };

            // Act
            _repository.UpdateAppointment(updated);

            var appointment =
                _repository.GetAppointmentById(_appointmentId);

            // Assert
            Assert.NotNull(appointment);

            Assert.Equal("Dr Strange",
                appointment.Doctor.FullName);

            Assert.Equal(AppointmentStatus.Completed,
                appointment.Status);

            Assert.Equal("John Doe",
                appointment.Patient.FullName);
        }

        [Fact]
        public void UpdateAppointment_ShouldNotOverwriteWithNullOrDefaultValues()
        {
            Appointment updated = new()
            {
                AppointmentId = _appointmentId,

                Patient = null!,
                Doctor = null!,

                ScheduledDate = default,
                TimeSlot = default,
                CancellationReason = ""
            };

            // Act
            _repository.UpdateAppointment(updated);

            var appointment =
                _repository.GetAppointmentById(_appointmentId);

            // Assert
            Assert.NotNull(appointment);

            Assert.Equal("John Doe",
                appointment.Patient.FullName);

            Assert.Equal("Dr Smith",
                appointment.Doctor.FullName);

            Assert.Equal(new TimeOnly(10, 0),
                appointment.TimeSlot);
        }

        [Fact]
        public void UpdateAppointment_ShouldDoNothing_WhenAppointmentNotFound()
        {
            Appointment updated = new()
            {
                AppointmentId = Guid.NewGuid(),

                Patient = new Patient
                {
                    PatientId = Guid.NewGuid(),
                    FullName = "Test Patient"
                },

                Doctor = new Doctor
                {
                    DoctorId = Guid.NewGuid(),
                    FullName = "Test Doctor"
                },

                Status = AppointmentStatus.Pending
            };

            int initialCount = _dataStore.Appointments.Count;

            // Act
            _repository.UpdateAppointment(updated);

            // Assert
            Assert.Equal(initialCount,
                _dataStore.Appointments.Count);
        }

        [Fact]
        public void UpdateAppointment_ShouldUpdateCancellationReason_WhenValid()
        {
            Appointment updated = new()
            {
                AppointmentId = _appointmentId,

                Patient = new Patient
                {
                    PatientId = Guid.NewGuid(),
                    FullName = "Dummy"
                },

                Doctor = new Doctor
                {
                    DoctorId = Guid.NewGuid(),
                    FullName = "Dummy"
                },

                CancellationReason = "Patient requested cancellation"
            };

            // Act
            _repository.UpdateAppointment(updated);

            var appointment =
                _repository.GetAppointmentById(_appointmentId);

            // Assert
            Assert.NotNull(appointment);

            Assert.Equal(
                "Patient requested cancellation",
                appointment.CancellationReason);
        }
        [Fact]
        public void AddAppointment_ShouldAddAppointmentSuccessfully()
        {
            // Arrange
            Guid newId = Guid.NewGuid();

            Appointment newAppointment = new()
            {
                AppointmentId = newId,

                Patient = new Patient
                {
                    PatientId = Guid.NewGuid(),
                    FullName = "Alice"
                },

                Doctor = new Doctor
                {
                    DoctorId = Guid.NewGuid(),
                    FullName = "Dr Brown"
                },

                ScheduledDate = DateOnly.FromDateTime(DateTime.Today),

                TimeSlot = new TimeOnly(11, 0),

                Status = AppointmentStatus.Pending,

                CancellationReason = null!
            };

            // Act
            _repository.AddAppointment(newAppointment);

            // Assert
            var result = _repository.GetAppointmentById(newId);

            Assert.NotNull(result);
            Assert.Equal("Alice", result.Patient.FullName);
            Assert.Equal("Dr Brown", result.Doctor.FullName);
        }
        [Fact]
        public void DeleteAppointmentById_ShouldRemoveAppointment_WhenExists()
        {
            // Act
            _repository.DeleteAppointmentById(_appointmentId);

            // Assert
            var result = _repository.GetAppointmentById(_appointmentId);

            Assert.Null(result);
            Assert.Empty(_dataStore.Appointments);
        }
        [Fact]
        public void DeleteAppointmentById_ShouldDoNothing_WhenNotExists()
        {
            // Arrange
            Guid nonExistingId = Guid.NewGuid();
            int initialCount = _dataStore.Appointments.Count;

            // Act
            _repository.DeleteAppointmentById(nonExistingId);

            // Assert
            Assert.Equal(initialCount, _dataStore.Appointments.Count);
        }

    }
}
