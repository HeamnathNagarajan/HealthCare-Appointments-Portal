using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Services;
using Moq;

namespace HealthCare_Appointments_Portal.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository>
            _mockRepository;

        private readonly Mock<IAppointmentRepository>
            _mockAppointmentRepository;

        private readonly DoctorService
            _doctorService;

        public DoctorServiceTests()
        {
            _mockRepository = new();

            _mockAppointmentRepository = new();

            _doctorService =
                new DoctorService(
                    _mockRepository.Object,
                    _mockAppointmentRepository.Object);
        }

        // Add Doctor Success
        [Fact]
        public void AddDoctor_ValidDoctor_ShouldAddDoctor()
        {
            Doctor doctor = CreateDoctor();

            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(new List<Doctor>());

            _doctorService.AddDoctor(doctor);

            _mockRepository.Verify(
                r => r.AddDoctor(doctor),
                Times.Once);
        }

        // Duplicate Doctor
        [Fact]
        public void AddDoctor_DuplicateDoctor_ShouldThrowException()
        {
            Doctor doctor = CreateDoctor();

            List<Doctor> doctors =
            [
                doctor
            ];

            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(doctors);

            Assert.Throws<
                DuplicateDoctorException>(() =>
                    _doctorService.AddDoctor(
                        doctor));
        }

        // Same Name Different Specialisation
        [Fact]
        public void AddDoctor_SameNameDifferentSpecialisation_ShouldAddDoctor()
        {
            Doctor existingDoctor =
                CreateDoctor();

            Doctor newDoctor =
                CreateDoctor();

            newDoctor.Specialisation =
                Specialisation.Neurology;

            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(
                [
                    existingDoctor
                ]);

            _doctorService.AddDoctor(
                newDoctor);

            _mockRepository.Verify(
                r => r.AddDoctor(newDoctor),
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
                    r.GetDoctorById(1))
                .Returns(doctor);

            Doctor? result =
                _doctorService.GetDoctorById(1);

            Assert.NotNull(result);

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
                    _doctorService.GetDoctorById(10));
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
                .Setup(r => r.GetAllDoctors())
                .Returns(doctors);

            List<Doctor> result =
                _doctorService.GetAllDoctors();

            Assert.Equal(2, result.Count);
        }

        // Get Empty Doctors
        [Fact]
        public void GetAllDoctors_Empty_ShouldReturnEmpty()
        {
            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(new List<Doctor>());

            List<Doctor> result =
                _doctorService.GetAllDoctors();

            Assert.Empty(result);
        }

        // Get Doctors By Specialisation
        [Fact]
        public void GetDoctorsBySpecialisation_ShouldReturnDoctors()
        {
            List<Doctor> doctors =
            [
                CreateDoctor()
            ];

            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(doctors);

            List<Doctor> result =
                _doctorService
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Single(result);
        }

        // No Match Specialisation
        [Fact]
        public void GetDoctorsBySpecialisation_NoMatch_ShouldThrowException()
        {
            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(new List<Doctor>());

            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService
                    .GetDoctorsBySpecialisation(
                        Specialisation.Cardiology));
        }

        // Inactive Doctor
        [Fact]
        public void GetDoctorsBySpecialisation_InactiveDoctor_ShouldThrowException()
        {
            Doctor doctor =
                CreateDoctor();

            doctor.IsActive = false;

            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(
                [
                    doctor
                ]);

            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService
                    .GetDoctorsBySpecialisation(
                        Specialisation.Cardiology));
        }

        // Multiple Doctors
        [Fact]
        public void GetDoctorsBySpecialisation_MultipleDoctors_ShouldReturnAll()
        {
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
                .Setup(r => r.GetAllDoctors())
                .Returns(doctors);

            List<Doctor> result =
                _doctorService
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Equal(2, result.Count);
        }

        // Get Available Doctors
        [Fact]
        public void GetAvailableDoctors_ShouldReturnActiveDoctors()
        {
            Doctor activeDoctor =
                CreateDoctor();

            Doctor inactiveDoctor =
                CreateDoctor();

            inactiveDoctor.IsActive = false;

            List<Doctor> doctors =
            [
                activeDoctor,
                inactiveDoctor
            ];

            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(doctors);

            List<Doctor> result =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Single(result);
        }

        // Multiple Active Doctors
        [Fact]
        public void GetAvailableDoctors_MultipleActive_ShouldReturnAll()
        {
            Doctor doctor1 =
                CreateDoctor();

            Doctor doctor2 =
                CreateDoctor();

            doctor2.DoctorId = 2;

            _mockRepository
                .Setup(r => r.GetAllDoctors())
                .Returns(
                [
                    doctor1,
                    doctor2
                ]);

            List<Doctor> result =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Equal(2, result.Count);
        }

        // Update Doctor Success
        [Fact]
        public void UpdateDoctor_ShouldUpdateDoctor()
        {
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(1))
                .Returns(doctor);

            _doctorService.UpdateDoctor(
                doctor);

            _mockRepository.Verify(
                r => r.UpdateDoctor(doctor),
                Times.Once);
        }

        // Update Invalid Doctor
        [Fact]
        public void UpdateDoctor_InvalidDoctor_ShouldThrowException()
        {
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(1))
                .Returns((Doctor?)null);

            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService.UpdateDoctor(
                        doctor));
        }

        // Delete Doctor Success
        [Fact]
        public void DeleteDoctor_ShouldDeleteDoctor()
        {
            Doctor doctor =
                CreateDoctor();

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(1))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(new List<Appointment>());

            _doctorService.DeleteDoctorById(1);

            _mockRepository.Verify(
                r => r.DeleteDoctorById(1),
                Times.Once);
        }

        // Delete Invalid Doctor
        [Fact]
        public void DeleteDoctor_InvalidId_ShouldThrowException()
        {
            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(
                        It.IsAny<int>()))
                .Returns((Doctor?)null);

            Assert.Throws<
                DoctorNotFoundException>(() =>
                    _doctorService.DeleteDoctorById(99));
        }

        // Confirmed Appointment
        [Fact]
        public void DeleteDoctor_ConfirmedAppointment_ShouldThrowException()
        {
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Confirmed);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(1))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(
                [
                    appointment
                ]);

            Assert.Throws<
                DoctorDeletionException>(() =>
                    _doctorService.DeleteDoctorById(1));
        }

        // Pending Appointment
        [Fact]
        public void DeleteDoctor_PendingAppointment_ShouldCancelAndDelete()
        {
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Pending);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(1))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(
                [
                    appointment
                ]);

            _doctorService.DeleteDoctorById(1);

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _mockAppointmentRepository.Verify(
                r => r.UpdateAppointment(
                    appointment),
                Times.Once);

            _mockRepository.Verify(
                r => r.DeleteDoctorById(1),
                Times.Once);
        }

        // Completed Appointment
        [Fact]
        public void DeleteDoctor_CompletedAppointment_ShouldDeleteDoctor()
        {
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Completed);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(1))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(
                [
                    appointment
                ]);

            _doctorService.DeleteDoctorById(1);

            _mockRepository.Verify(
                r => r.DeleteDoctorById(1),
                Times.Once);
        }

        // Cancelled Appointment
        [Fact]
        public void DeleteDoctor_CancelledAppointment_ShouldDeleteDoctor()
        {
            Doctor doctor =
                CreateDoctor();

            Appointment appointment =
                CreateAppointment(
                    doctor,
                    AppointmentStatus.Cancelled);

            _mockRepository
                .Setup(r =>
                    r.GetDoctorById(1))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(
                [
                    appointment
                ]);

            _doctorService.DeleteDoctorById(1);

            _mockRepository.Verify(
                r => r.DeleteDoctorById(1),
                Times.Once);
        }

        // Multiple Pending Appointments
        [Fact]
        public void DeleteDoctor_MultiplePendingAppointments_ShouldUpdateAll()
        {
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
                    r.GetDoctorById(1))
                .Returns(doctor);

            _mockAppointmentRepository
                .Setup(r =>
                    r.GetAllAppointments())
                .Returns(
                [
                    appointment1,
                    appointment2
                ]);

            _doctorService.DeleteDoctorById(1);

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