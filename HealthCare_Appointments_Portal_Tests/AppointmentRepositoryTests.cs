using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;

namespace HealthCare_Appointment_Portal.Tests
{
    public class AppointmentRepositoryTests
    {
        private readonly DataStore _dataStore;

        private readonly AppointmentRepository _repository;

        public AppointmentRepositoryTests()
        {
            _dataStore = new DataStore();

            _repository =
                new AppointmentRepository(
                    _dataStore);
        }

        // Add Appointment Success
        [Fact]
        public void AddAppointment_ValidAppointment_ShouldAddAppointment()
        {
            // Arrange
            Appointment appointment = CreateAppointment();

            // Act
            _repository.AddAppointment(
                appointment);

            // Assert
            Assert.Single(
                _dataStore.Appointments);

            Assert.Equal(
                appointment.AppointmentId,
                _dataStore
                    .Appointments[0]
                    .AppointmentId);
        }

        // Get Appointment By Existing Id
        [Fact]
        public void GetAppointmentById_ExistingId_ShouldReturnAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            // Act
            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                appointment.AppointmentId,
                result?.AppointmentId);
        }

        // Get Appointment By Invalid Id
        [Fact]
        public void GetAppointmentById_InvalidId_ShouldReturnNull()
        {
            // Act
            Appointment? result =
                _repository.GetAppointmentById(
                    45);

            // Assert
            Assert.Null(
                result);
        }

        // Get All Appointments
        [Fact]
        public void GetAllAppointments_ShouldReturnAllAppointments()
        {
            // Arrange
            _dataStore.Appointments.AddRange(
            [
                CreateAppointment(),
                CreateAppointment()
            ]);

            // Act
            List<Appointment> result =
                _repository.GetAllAppointments();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Appointments Empty
        [Fact]
        public void GetAllAppointments_EmptyList_ShouldReturnEmpty()
        {
            // Act
            List<Appointment> result =
                _repository.GetAllAppointments();

            // Assert
            Assert.Empty(
                result);
        }

        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnPatientAppointments()
        {
            Patient patient1 = new() { PatientId = 1 };
            Patient patient2 = new() { PatientId = 2 };

            _dataStore.Appointments.AddRange(
            [
                new Appointment
        {
            Patient = patient1,
            Doctor = new Doctor(),
            ScheduledDate = new DateOnly(2026,5,10)
        },

        new Appointment
        {
            Patient = patient2,
            Doctor = new Doctor(),
            ScheduledDate = new DateOnly(2026,5,11)
        }
            ]);

            List<Appointment> result =
                _repository.GetAppointmentsByPatient(1);

            Assert.Single(result);
        }

        [Fact]
        public void GetAppointmentsByPatient_NoMatch_ShouldReturnEmpty()
        {
            List<Appointment> result =
                _repository.GetAppointmentsByPatient(999);

            Assert.Empty(result);
        }

        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnDoctorAppointments()
        {
            Doctor doctor1 = new() { DoctorId = 1 };
            Doctor doctor2 = new() { DoctorId = 2 };

            _dataStore.Appointments.AddRange(
            [
                new Appointment
        {
            Patient = new Patient(),
            Doctor = doctor1,
            ScheduledDate = new DateOnly(2026,5,10)
        },

        new Appointment
        {
            Patient = new Patient(),
            Doctor = doctor2,
            ScheduledDate = new DateOnly(2026,5,11)
        }
            ]);

            List<Appointment> result =
                _repository.GetAppointmentsByDoctor(1);

            Assert.Single(result);
        }

        [Fact]
        public void GetAppointmentsByDoctor_NoMatch_ShouldReturnEmpty()
        {
            List<Appointment> result =
                _repository.GetAppointmentsByDoctor(999);

            Assert.Empty(result);
        }

        [Fact]
        public void GetUpcomingAppointments_ShouldReturnConfirmedAppointments()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Confirmed;

            appointment.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(1));

            _dataStore.Appointments.Add(
                appointment);

            List<Appointment> result =
                _repository.GetUpcomingAppointments();

            Assert.Single(result);
        }

        [Fact]
        public void GetUpcomingAppointments_PastDate_ShouldIgnoreAppointment()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Confirmed;

            appointment.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(-1));

            _dataStore.Appointments.Add(
                appointment);

            List<Appointment> result =
                _repository.GetUpcomingAppointments();

            Assert.Empty(result);
        }

        [Fact]
        public void GetUpcomingAppointments_Pending_ShouldIgnoreAppointment()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Pending;

            appointment.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(1));

            _dataStore.Appointments.Add(
                appointment);

            List<Appointment> result =
                _repository.GetUpcomingAppointments();

            Assert.Empty(result);
        }

        [Fact]
        public void GetCompletedAppointments_ShouldReturnCompletedAppointments()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Status =
                AppointmentStatus.Completed;

            _dataStore.Appointments.Add(
                appointment);

            List<Appointment> result =
                _repository.GetCompletedAppointments();

            Assert.Single(result);
        }

        [Fact]
        public void GetCompletedAppointments_Empty_ShouldReturnEmpty()
        {
            List<Appointment> result =
                _repository.GetCompletedAppointments();

            Assert.Empty(result);
        }

        [Fact]
        public void GetConflictingAppointment_ShouldReturnAppointment()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Doctor.DoctorId = 1;

            _dataStore.Appointments.Add(
                appointment);

            Appointment? result =
                _repository.GetConflictingAppointment(
                    1,
                    appointment.ScheduledDate,
                    appointment.TimeSlot);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetConflictingAppointment_NoConflict_ShouldReturnNull()
        {
            Appointment? result =
                _repository.GetConflictingAppointment(
                    1,
                    new DateOnly(2026, 5, 10),
                    new TimeOnly(10, 0));

            Assert.Null(result);
        }

        [Fact]
        public void GetConflictingAppointment_CancelledAppointment_ShouldIgnore()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.Doctor.DoctorId = 1;

            appointment.Status =
                AppointmentStatus.Cancelled;

            _dataStore.Appointments.Add(
                appointment);

            Appointment? result =
                _repository.GetConflictingAppointment(
                    1,
                    appointment.ScheduledDate,
                    appointment.TimeSlot);

            Assert.Null(result);
        }

        // Update Existing Appointment
        [Fact]
        public void UpdateAppointment_ExistingAppointment_ShouldUpdateDetails()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            Patient updatedPatient = new()
            {
                FullName =
                    "Updated Patient",

                DateOfBirth =
                    new DateOnly(
                        2001,
                        4,
                        22),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9999999999",

                Email =
                    "updated@gmail.com",

                InsuranceId =
                    "INS101"
            };

            Doctor updatedDoctor = new()
            {
                FullName =
                    "Updated Doctor",

                Specialisation =
                    Specialisation.Neurology,

                YearsOfExperience =
                    10,

                ConsultationFee =
                    2000,

                IsActive =
                    true
            };

            Appointment updatedAppointment = new()
            {
                AppointmentId =
                    appointment.AppointmentId,

                Patient =
                    updatedPatient,

                Doctor =
                    updatedDoctor,

                ScheduledDate =
                    new DateOnly(
                        2026,
                        5,
                        20),

                TimeSlot =
                    new TimeOnly(
                        11,
                        30),

                Status =
                    AppointmentStatus.Confirmed,

                CancellationReason =
                    "Updated"
            };

            // Act
            _repository.UpdateAppointment(
                updatedAppointment);

            // Assert
            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            Assert.NotNull(
                result);

            Assert.Equal(
                "Updated Patient",
                result?.Patient.FullName);

            Assert.Equal(
                "Updated Doctor",
                result?.Doctor.FullName);

            Assert.Equal(
                AppointmentStatus.Confirmed,
                result?.Status);
        }

        // Get Appointments By Patient Ordered
        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnOrderedByDate()
        {
            Patient patient =
                new()
                {
                    PatientId = 1
                };

            _dataStore.Appointments.AddRange(
            [
                new Appointment
        {
            Patient = patient,
            Doctor = CreateAppointment().Doctor,
            ScheduledDate =
                new DateOnly(2026, 12, 1),
            TimeSlot =
                new TimeOnly(10, 0)
        },

        new Appointment
        {
            Patient = patient,
            Doctor = CreateAppointment().Doctor,
            ScheduledDate =
                new DateOnly(2026, 1, 1),
            TimeSlot =
                new TimeOnly(10, 0)
        }
            ]);

            List<Appointment> result =
                _repository.GetAppointmentsByPatient(1);

            Assert.Equal(
                new DateOnly(2026, 1, 1),
                result[0].ScheduledDate);
        }

        // Get Appointments By Doctor Ordered
        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnOrderedByDate()
        {
            Doctor doctor =
                new()
                {
                    DoctorId = 1
                };

            _dataStore.Appointments.AddRange(
            [
                new Appointment
        {
            Patient =
                CreateAppointment().Patient,
            Doctor = doctor,
            ScheduledDate =
                new DateOnly(2026, 12, 1),
            TimeSlot =
                new TimeOnly(10, 0)
        },

        new Appointment
        {
            Patient =
                CreateAppointment().Patient,
            Doctor = doctor,
            ScheduledDate =
                new DateOnly(2026, 1, 1),
            TimeSlot =
                new TimeOnly(10, 0)
        }
            ]);

            List<Appointment> result =
                _repository.GetAppointmentsByDoctor(1);

            Assert.Equal(
                new DateOnly(2026, 1, 1),
                result[0].ScheduledDate);
        }

        // Upcoming Appointments Ordered
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnOrderedByDate()
        {
            Appointment appointment1 =
                CreateAppointment();

            appointment1.Status =
                AppointmentStatus.Confirmed;

            appointment1.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(5));

            Appointment appointment2 =
                CreateAppointment();

            appointment2.Status =
                AppointmentStatus.Confirmed;

            appointment2.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(1));

            _dataStore.Appointments.AddRange(
            [
                appointment1,
        appointment2
            ]);

            List<Appointment> result =
                _repository.GetUpcomingAppointments();

            Assert.Equal(
                appointment2.ScheduledDate,
                result[0].ScheduledDate);
        }

        // Update Null Patient And Doctor
        [Fact]
        public void UpdateAppointment_NullPatientAndDoctor_ShouldKeepExistingValues()
        {
            Appointment appointment =
                CreateAppointment();

            _dataStore.Appointments.Add(
                appointment);

            Appointment updated =
                new()
                {
                    AppointmentId =
                        appointment.AppointmentId,

                    Patient = null!,

                    Doctor = null!,

                    Status =
                        AppointmentStatus.Completed
                };

            _repository.UpdateAppointment(
                updated);

            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            Assert.NotNull(result);

            Assert.Equal(
                appointment.Patient,
                result!.Patient);

            Assert.Equal(
                appointment.Doctor,
                result.Doctor);
        }

        // Update Empty Cancellation Reason
        [Fact]
        public void UpdateAppointment_EmptyCancellationReason_ShouldKeepExistingValue()
        {
            Appointment appointment =
                CreateAppointment();

            appointment.CancellationReason =
                "Old Reason";

            _dataStore.Appointments.Add(
                appointment);

            Appointment updated =
                new()
                {
                    AppointmentId =
                        appointment.AppointmentId,

                    Patient =
                        appointment.Patient,

                    Doctor =
                        appointment.Doctor,

                    CancellationReason = ""
                };

            _repository.UpdateAppointment(
                updated);

            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            Assert.Equal(
                "Old Reason",
                result?.CancellationReason);
        }

        // Update Default Date And Time
        [Fact]
        public void UpdateAppointment_DefaultDateAndTime_ShouldKeepExistingValues()
        {
            Appointment appointment =
                CreateAppointment();

            _dataStore.Appointments.Add(
                appointment);

            Appointment updated =
                new()
                {
                    AppointmentId =
                        appointment.AppointmentId,

                    Patient =
                        appointment.Patient,

                    Doctor =
                        appointment.Doctor,

                    ScheduledDate = default,

                    TimeSlot = default,

                    Status =
                        AppointmentStatus.Completed
                };

            _repository.UpdateAppointment(
                updated);

            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            Assert.Equal(
                appointment.ScheduledDate,
                result?.ScheduledDate);

            Assert.Equal(
                appointment.TimeSlot,
                result?.TimeSlot);
        }

        // Partial Update Appointment
        [Fact]
        public void UpdateAppointment_PartialUpdate_ShouldUpdateOnlyProvidedFields()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            Appointment updatedAppointment = new()
            {
                AppointmentId =
                    appointment.AppointmentId,

                Patient =
                    appointment.Patient,

                Doctor =
                    appointment.Doctor,

                Status =
                    AppointmentStatus.Completed
            };

            // Act
            _repository.UpdateAppointment(
                updatedAppointment);

            // Assert
            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            Assert.NotNull(
                result);

            // Updated Field
            Assert.Equal(
                AppointmentStatus.Completed,
                result?.Status);

            // Existing Fields Unchanged
            Assert.Equal(
                appointment.Patient.FullName,
                result?.Patient.FullName);

            Assert.Equal(
                appointment.Doctor.FullName,
                result?.Doctor.FullName);
        }

        // Update Invalid Appointment
        [Fact]
        public void UpdateAppointment_InvalidId_ShouldNotUpdate()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            // Act
            _repository.UpdateAppointment(
                appointment);

            // Assert
            Assert.Empty(
                _dataStore.Appointments);
        }

        // Delete Existing Appointment
        [Fact]
        public void DeleteAppointmentById_ExistingId_ShouldRemoveAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            // Act
            _repository.DeleteAppointmentById(
                appointment.AppointmentId);

            // Assert
            Assert.Empty(
                _dataStore.Appointments);
        }

        // Delete Invalid Appointment
        [Fact]
        public void DeleteAppointmentById_InvalidId_ShouldNotRemoveAnything()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            // Act
            _repository.DeleteAppointmentById(
                32);

            // Assert
            Assert.Single(
                _dataStore.Appointments);
        }

        // Helper Method
        private static Appointment CreateAppointment()
        {
            Patient patient = new()
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

            Doctor doctor = new()
            {
                FullName =
                    "Dr Arun",

                Specialisation =
                    Specialisation.Cardiology,

                YearsOfExperience =
                    5,

                ConsultationFee =
                    1000,

                IsActive =
                    true
            };

            return new Appointment
            {
                Patient =
                    patient,

                Doctor =
                    doctor,

                ScheduledDate =
                    new DateOnly(
                        2026,
                        5,
                        10),

                TimeSlot =
                    new TimeOnly(
                        10,
                        0),

                Status =
                    AppointmentStatus.Pending
            };
        }
    }
}