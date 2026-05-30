using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests
{
    public partial class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository>
            _mockRepository;

        private readonly Mock<IAppointmentRepository>
            _mockAppointmentRepository;

        private readonly DoctorService
            _doctorService;

        public DoctorServiceTests()
        {
            _mockRepository =
                new Mock<IDoctorRepository>();

            _mockAppointmentRepository =
                new Mock<IAppointmentRepository>();

            _doctorService =
                new DoctorService(
                    _mockRepository.Object,
                    _mockAppointmentRepository.Object);
        }

        // Add Doctor Success
        [Fact]
        public void AddDoctor_ValidDoctor_ShouldAddDoctor()
        {
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorByNameAndSpecialisation(
                        doctor.FullName,
                        doctor.Specialisation))
                .Returns((Doctor?)null);

            _doctorService.AddDoctor(
                doctor);

            _mockRepository.Verify(
                r => r.AddDoctor(
                    doctor),
                Times.Once);
        }


        // Get Doctor By Id Success
        [Fact]
        public void GetDoctorById_ExistingId_ShouldReturnDoctor()
        {
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            Doctor? result =
                _doctorService.GetDoctorById(
                    doctor.DoctorId);

            Assert.NotNull(
                result);

            Assert.Equal(
                doctor.DoctorId,
                result?.DoctorId);
        }

        // Get Doctor Invalid Id
        [Fact]
        public void GetDoctorById_InvalidId_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        It.IsAny<int>()))
                .Returns((Doctor?)null);

            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService.GetDoctorById(
                        999));
        }

        // Get All Doctors
        [Fact]
        public void GetAllDoctors_ShouldReturnDoctors()
        {
            List<Doctor> doctors =
            [
                CreateDoctor(),
                CreateDoctor()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(doctors);

            List<Doctor> result =
                _doctorService.GetAllDoctors();

            Assert.Equal(
                2,
                result.Count);
        }

        // Get Empty Doctors
        [Fact]
        public void GetAllDoctors_Empty_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(new List<Doctor>());

            List<Doctor> result =
                _doctorService.GetAllDoctors();

            Assert.Empty(
                result);
        }

        // Get Doctors By Specialisation
        [Fact]
        public void GetDoctorsBySpecialisation_ShouldReturnDoctors()
        {
            // Arrange
            List<Doctor> doctors =
            [
                CreateDoctor()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetDoctorsBySpecialisation(
                        Specialisation.Cardiology))
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Single(result);
        }

        // No Match Specialisation
        [Fact]
        public void GetDoctorsBySpecialisation_NoMatch_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetDoctorsBySpecialisation(
                        Specialisation.Cardiology))
                .Returns(new List<Doctor>());

            // Act & Assert
            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService
                    .GetDoctorsBySpecialisation(
                        Specialisation.Cardiology));
        }

        // Multiple Doctors Same Specialisation
        [Fact]
        public void GetDoctorsBySpecialisation_MultipleDoctors_ShouldReturnAll()
        {
            // Arrange
            Doctor doctor1 =
                CreateDoctor();

            Doctor doctor2 =
                CreateDoctor();

            doctor2.DoctorId = 2;

            List<Doctor> doctors =
            [
                doctor1,
        doctor2
            ];

            _mockRepository
                .Setup(r =>
                    r.GetDoctorsBySpecialisation(
                        Specialisation.Cardiology))
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get Available Doctors
        [Fact]
        public void GetAvailableDoctors_ShouldReturnActiveDoctors()
        {
            // Arrange
            Doctor activeDoctor =
                CreateDoctor();

            List<Doctor> doctors =
            [
                activeDoctor
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAvailableDoctorsBySpecialisation(
                        Specialisation.Cardiology))
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Single(result);

            Assert.True(
                result[0].IsActive);
        }

        // Multiple Active Doctors
        [Fact]
        public void GetAvailableDoctors_MultipleActive_ShouldReturnAll()
        {
            // Arrange
            Doctor doctor1 =
                CreateDoctor();

            Doctor doctor2 =
                CreateDoctor();

            doctor2.DoctorId = 2;

            List<Doctor> doctors =
            [
                doctor1,
        doctor2
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAvailableDoctorsBySpecialisation(
                        Specialisation.Cardiology))
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // No Available Doctors
        [Fact]
        public void GetAvailableDoctors_NoDoctors_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAvailableDoctorsBySpecialisation(
                        Specialisation.Cardiology))
                .Returns(new List<Doctor>());

            // Act
            List<Doctor> result =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Empty(result);
        }

        // Update Doctor Success
        [Fact]
        public void UpdateDoctor_ShouldUpdateDoctor()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            // Act
            _doctorService.UpdateDoctor(
                doctor);

            // Assert
            _mockRepository.Verify(
                r => r.UpdateDoctor(
                    doctor),
                Times.Once);
        }

        // Update Invalid Doctor
        [Fact]
        public void UpdateDoctor_InvalidDoctor_ShouldThrowException()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns((Doctor?)null);

            // Act & Assert
            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService
                    .UpdateDoctor(
                        doctor));
        }

        // Delete Doctor Success
        [Fact]
        public void DeleteDoctor_ShouldDeleteDoctor()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(
                        doctor.DoctorId))
                .Returns(new List<Appointment>());

            // Act
            _doctorService.DeleteDoctorById(
                doctor.DoctorId);

            // Assert
            _mockRepository.Verify(
                r => r.DeleteDoctorById(
                    doctor.DoctorId),
                Times.Once);
        }

        // Delete Invalid Doctor
        [Fact]
        public void DeleteDoctor_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        It.IsAny<int>()))
                .Returns((Doctor?)null);

            // Act & Assert
            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService
                    .DeleteDoctorById(
                        999));
        }

        // Confirmed Appointment
        [Fact]
        public void DeleteDoctor_ConfirmedAppointment_ShouldThrowException()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Confirmed);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(
                        doctor.DoctorId))
                .Returns(
                [
                    appointment
                ]);

            // Act & Assert
            Assert.Throws<
                DoctorDeletionException>(() =>
                    _doctorService
                    .DeleteDoctorById(
                        doctor.DoctorId));
        }

        // Pending Appointment
        [Fact]
        public void DeleteDoctor_PendingAppointment_ShouldCancelAndDelete()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Pending);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(
                        doctor.DoctorId))
                .Returns(
                [
                    appointment
                ]);

            // Act
            _doctorService.DeleteDoctorById(
                doctor.DoctorId);

            // Assert
            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _mockAppointmentRepository.Verify(
                r => r.UpdateAppointment(
                    appointment),
                Times.Once);

            _mockRepository.Verify(
                r => r.DeleteDoctorById(
                    doctor.DoctorId),
                Times.Once);
        }

        // Completed Appointment
        [Fact]
        public void DeleteDoctor_CompletedAppointment_ShouldDeleteDoctor()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Completed);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(
                        doctor.DoctorId))
                .Returns(
                [
                    appointment
                ]);

            // Act
            _doctorService.DeleteDoctorById(
                doctor.DoctorId);

            // Assert
            _mockRepository.Verify(
                r => r.DeleteDoctorById(
                    doctor.DoctorId),
                Times.Once);
        }

        // Cancelled Appointment
        [Fact]
        public void DeleteDoctor_CancelledAppointment_ShouldDeleteDoctor()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Cancelled);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(
                        doctor.DoctorId))
                .Returns(
                [
                    appointment
                ]);

            // Act
            _doctorService.DeleteDoctorById(
                doctor.DoctorId);

            // Assert
            _mockRepository.Verify(
                r => r.DeleteDoctorById(
                    doctor.DoctorId),
                Times.Once);
        }

        // Multiple Pending Appointments
        [Fact]
        public void DeleteDoctor_MultiplePendingAppointments_ShouldUpdateAll()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            Appointment appointment1 =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Pending);

            Appointment appointment2 =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Pending);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        doctor.DoctorId))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAppointmentsByDoctor(
                        doctor.DoctorId))
                .Returns(
                [
                    appointment1,
            appointment2
                ]);

            // Act
            _doctorService.DeleteDoctorById(
                doctor.DoctorId);

            // Assert
            _mockAppointmentRepository.Verify(
                r => r.UpdateAppointment(
                    It.IsAny<Appointment>()),
                Times.Exactly(2));
        }

        // Helper Doctor
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

        // Helper Patient
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

        // Helper Appointment
        private static Appointment CreateAppointment(
            Doctor doctor,
            AppointmentStatus status)
        {
            return new Appointment
            {
                AppointmentId = 1,
                Patient = CreatePatient(),
                Doctor = doctor,
                ScheduledDate =
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                TimeSlot =
                    new TimeOnly(10, 0),
                Status = status
            };
        }
    }
}