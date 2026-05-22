using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;

namespace HealthCare_Appointments_Portal.Tests
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
                    43);

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