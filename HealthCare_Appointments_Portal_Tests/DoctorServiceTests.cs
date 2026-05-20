using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository>
            _mockRepository;

        private readonly DoctorService
            _doctorService;

        public DoctorServiceTests()
        {
            _mockRepository =
                new Mock<IDoctorRepository>();

            _doctorService =
                new DoctorService(
                    _mockRepository.Object);
        }

        // Add Doctor Success
        [Fact]
        public void AddDoctor_ValidDoctor_ShouldAddDoctor()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(new List<Doctor>());

            // Act
            _doctorService.AddDoctor(
                doctor);

            // Assert
            _mockRepository.Verify(r =>
                r.AddDoctor(doctor),
                Times.Once);
        }

        // Add Duplicate Doctor
        [Fact]
        public void AddDoctor_DuplicateDoctor_ShouldThrowException()
        {
            // Arrange
            Doctor doctor =
                CreateDoctor();

            List<Doctor> doctors =
            [
                doctor
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(doctors);

            // Act & Assert
            Assert.Throws<
                DuplicateDoctorException>(() =>
                    _doctorService.AddDoctor(
                        doctor));
        }

        // Add Doctor Same Name Different Specialisation
        [Fact]
        public void AddDoctor_SameNameDifferentSpecialisation_ShouldAddDoctor()
        {
            // Arrange
            Doctor existingDoctor =
                CreateDoctor();

            Doctor newDoctor =
                CreateDoctor();

            newDoctor.Specialisation =
                Specialisation.Neurology;

            List<Doctor> doctors =
            [
                existingDoctor
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(doctors);

            // Act
            _doctorService.AddDoctor(
                newDoctor);

            // Assert
            _mockRepository.Verify(r =>
                r.AddDoctor(newDoctor),
                Times.Once);
        }

        // Get Doctor By Existing Id
        [Fact]
        public void GetDoctorById_ExistingId_ShouldReturnDoctor()
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
            Doctor? result =
                _doctorService.GetDoctorById(
                    doctor.DoctorId);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                doctor.DoctorId,
                result?.DoctorId);
        }

        // Get Doctor By Invalid Id
        [Fact]
        public void GetDoctorById_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        It.IsAny<Guid>()))
                .Returns((Doctor?)null);

            // Act & Assert
            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService.GetDoctorById(
                        Guid.NewGuid()));
        }

        // Get All Doctors
        [Fact]
        public void GetAllDoctors_ShouldReturnAllDoctors()
        {
            // Arrange
            List<Doctor> doctors =
            [
                CreateDoctor(),
                CreateDoctor()
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService.GetAllDoctors();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Doctors Empty
        [Fact]
        public void GetAllDoctors_Empty_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(new List<Doctor>());

            // Act
            List<Doctor> result =
                _doctorService.GetAllDoctors();

            // Assert
            Assert.Empty(
                result);
        }

        // Get Doctors By Specialisation
        [Fact]
        public void GetDoctorsBySpecialisation_ValidSpecialisation_ShouldReturnDoctors()
        {
            // Arrange
            List<Doctor> doctors =
            [
                new Doctor
                {
                    FullName =
                        "Doctor One",

                    Specialisation =
                        Specialisation.Cardiology,

                    YearsOfExperience =
                        5,

                    ConsultationFee =
                        1000,

                    IsActive =
                        true
                },

                new Doctor
                {
                    FullName =
                        "Doctor Two",

                    Specialisation =
                        Specialisation.Neurology,

                    YearsOfExperience =
                        8,

                    ConsultationFee =
                        2000,

                    IsActive =
                        true
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Single(
                result);

            Assert.Equal(
                Specialisation.Cardiology,
                result[0].Specialisation);
        }

        // Get Doctors By Specialisation Empty
        [Fact]
        public void GetDoctorsBySpecialisation_NoMatch_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(new List<Doctor>());

            // Act
            List<Doctor> result =
                _doctorService
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Empty(
                result);
        }

        // Get Available Doctors By Specialisation
        [Fact]
        public void GetAvailableDoctorsBySpecialisation_ActiveDoctors_ShouldReturnDoctors()
        {
            // Arrange
            List<Doctor> doctors =
            [
                new Doctor
                {
                    FullName =
                        "Doctor One",

                    Specialisation =
                        Specialisation.Cardiology,

                    YearsOfExperience =
                        5,

                    ConsultationFee =
                        1000,

                    IsActive =
                        true
                },

                new Doctor
                {
                    FullName =
                        "Doctor Two",

                    Specialisation =
                        Specialisation.Cardiology,

                    YearsOfExperience =
                        7,

                    ConsultationFee =
                        1500,

                    IsActive =
                        false
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Single(
                result);

            Assert.True(
                result[0].IsActive);
        }

        // Get Available Doctors Empty
        [Fact]
        public void GetAvailableDoctorsBySpecialisation_NoActiveDoctors_ShouldReturnEmpty()
        {
            // Arrange
            List<Doctor> doctors =
            [
                new Doctor
                {
                    FullName =
                        "Doctor One",

                    Specialisation =
                        Specialisation.Cardiology,

                    YearsOfExperience =
                        5,

                    ConsultationFee =
                        1000,

                    IsActive =
                        false
                }
            ];

            _mockRepository
                .Setup(r =>
                    r.GetAllDoctors())
                .Returns(doctors);

            // Act
            List<Doctor> result =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            // Assert
            Assert.Empty(
                result);
        }

        // Update Existing Doctor
        [Fact]
        public void UpdateDoctor_ExistingDoctor_ShouldUpdateDoctor()
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
            _mockRepository.Verify(r =>
                r.UpdateDoctor(doctor),
                Times.Once);
        }

        // Update Invalid Doctor
        [Fact]
        public void UpdateDoctor_InvalidId_ShouldThrowException()
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
                    _doctorService.UpdateDoctor(
                        doctor));
        }

        // Delete Existing Doctor
        [Fact]
        public void DeleteDoctorById_ExistingId_ShouldDeleteDoctor()
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
            _doctorService.DeleteDoctorById(
                doctor.DoctorId);

            // Assert
            _mockRepository.Verify(r =>
                r.DeleteDoctorById(
                    doctor.DoctorId),
                Times.Once);
        }

        // Delete Invalid Doctor
        [Fact]
        public void DeleteDoctorById_InvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        It.IsAny<Guid>()))
                .Returns((Doctor?)null);

            // Act & Assert
            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService.DeleteDoctorById(
                        Guid.NewGuid()));
        }

        // Helper Method
        private static Doctor CreateDoctor()
        {
            return new Doctor
            {
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