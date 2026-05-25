using Xunit;
using Moq;
using HealthCare_Appointments_Portal.Services;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare.Tests
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _mockRepo;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _mockRepo = new Mock<IHealthRecordRepository>();
            _service = new HealthRecordService(_mockRepo.Object);
        }

        private static HealthRecord CreateValidRecord()
        {
            return new HealthRecord
            {
                Patient = new Patient { PatientId = Guid.NewGuid() },
                Doctor = new Doctor { DoctorId = Guid.NewGuid() },
                Appointment = new Appointment
                {
                    AppointmentId = Guid.NewGuid(),
                    Patient = new Patient { PatientId = Guid.NewGuid() },
                    Doctor = new Doctor { DoctorId = Guid.NewGuid() }
                }
            };
        }

        [Fact]
        public void AddRecord_ValidRecord_ShouldCallRepository()
        {
            var record = CreateValidRecord();

            _service.AddRecord(record);

            _mockRepo.Verify(r => r.AddRecord(record), Times.Once);
        }

        [Fact]
        public void AddRecord_NullRecord_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.AddRecord(null!));
        }

        [Fact]
        public void GetAllRecords_ShouldReturnAllRecords()
        {
            var records = new List<HealthRecord>
            {
                CreateValidRecord(),
                CreateValidRecord()
            };

            _mockRepo.Setup(r => r.GetAllRecords()).Returns(records);

            var result = _service.GetAllRecords();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetRecordsByPatient_ShouldReturnMatchingRecords()
        {
            var patientId = Guid.NewGuid();

            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    Patient = new Patient { PatientId = patientId },
                    Doctor = new Doctor { DoctorId = Guid.NewGuid() },
                    Appointment = new Appointment
                    {
                        AppointmentId = Guid.NewGuid(),
                        Patient = new Patient { PatientId = patientId },
                        Doctor = new Doctor { DoctorId = Guid.NewGuid() }
                    }
                },
                CreateValidRecord()
            };

            _mockRepo.Setup(r => r.GetAllRecords()).Returns(records);

            var result = _service.GetRecordsByPatient(patientId);

            Assert.Single(result);
        }

        [Fact]
        public void GetRecordsByPatient_NoMatch_ShouldReturnEmptyList()
        {
            var records = new List<HealthRecord>
            {
                CreateValidRecord()
            };

            _mockRepo.Setup(r => r.GetAllRecords()).Returns(records);

            var result = _service.GetRecordsByPatient(Guid.NewGuid());

            Assert.Empty(result);
        }
    }
}