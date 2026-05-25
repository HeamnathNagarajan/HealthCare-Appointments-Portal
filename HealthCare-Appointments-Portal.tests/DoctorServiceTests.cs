using Xunit;
using Moq;
using HealthCare_Appointments_Portal.Services;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _mockRepo;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _mockRepo = new Mock<IDoctorRepository>();
            _service = new DoctorService(_mockRepo.Object);
        }

        [Fact]
        public void AddDoctor_ValidDoctor_ShouldCallRepository()
        {
            var doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FullName = "Dr Test"
            };

            _service.AddDoctor(doctor);

            _mockRepo.Verify(r => r.AddDoctor(doctor), Times.Once);
        }

        [Fact]
        public void AddDoctor_NullDoctor_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.AddDoctor(null!));
        }

        [Fact]
        public void GetDoctorById_ShouldReturnDoctor()
        {
            var doctorId = Guid.NewGuid();
            var doctor = new Doctor { DoctorId = doctorId };

            _mockRepo.Setup(r => r.GetDoctorById(doctorId)).Returns(doctor);

            var result = _service.GetDoctorById(doctorId);

            Assert.NotNull(result);
            Assert.Equal(doctorId, result!.DoctorId);
        }

        [Fact]
        public void GetAllDoctors_ShouldReturnList()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorId = Guid.NewGuid() },
                new Doctor { DoctorId = Guid.NewGuid() }
            };

            _mockRepo.Setup(r => r.GetAllDoctors()).Returns(doctors);

            var result = _service.GetAllDoctors();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void UpdateDoctor_ValidDoctor_ShouldCallRepository()
        {
            var doctor = new Doctor { DoctorId = Guid.NewGuid() };

            _service.UpdateDoctor(doctor);

            _mockRepo.Verify(r => r.UpdateDoctor(doctor), Times.Once);
        }

        [Fact]
        public void UpdateDoctor_NullDoctor_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _service.UpdateDoctor(null!));
        }

        [Fact]
        public void DeleteDoctor_ExistingDoctor_ShouldDelete()
        {
            var doctorId = Guid.NewGuid();
            var doctor = new Doctor { DoctorId = doctorId };

            _mockRepo.Setup(r => r.GetDoctorById(doctorId)).Returns(doctor);

            _service.DeleteDoctorById(doctorId);

            _mockRepo.Verify(r => r.DeleteDoctorById(doctorId), Times.Once);
        }

        [Fact]
        public void DeleteDoctor_NotFound_ShouldThrow()
        {
            var doctorId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetDoctorById(doctorId))
                     .Returns((Doctor?)null);

            Assert.Throws<KeyNotFoundException>(() => _service.DeleteDoctorById(doctorId));
        }
    }
}
