using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;
using Xunit;

namespace HealthCare_Appointments_Portal.Tests
{
    public class DoctorRepositoryTests
    {
        private readonly DataStore _dataStore;

        private readonly DoctorRepository _repository;

        public DoctorRepositoryTests()
        {
            _dataStore = new DataStore();

            _repository = new DoctorRepository(_dataStore);

            // Seed data
            _dataStore.Doctors.Add(
                new Doctor
                {
                    DoctorId = Guid.Parse(
                        "11111111-1111-1111-1111-111111111111"),

                    FullName = "Dr John",

                    Specialisation = Specialisation.Cardiology,

                    YearsOfExperience = 10,

                    ConsultationFee = 500,

                    IsActive = true
                });
        }

        [Fact]
        public void AddDoctor_ShouldAddDoctorSuccessfully()
        {
            // Arrange
            Doctor doctor = new()
            {
                FullName = "Dr Strange",

                Specialisation = Specialisation.Neurology,

                YearsOfExperience = 15,

                ConsultationFee = 1000,

                IsActive = true
            };

            // Act
            _repository.AddDoctor(doctor);

            // Assert
            Assert.Contains(
                _dataStore.Doctors,
                d => d.FullName == "Dr Strange");
        }

        [Fact]
        public void UpdateDoctor_ShouldUpdateOnlyProvidedValues()
        {
            // Arrange
            Doctor updatedDoctor = new()
            {
                DoctorId = Guid.Parse(
                    "11111111-1111-1111-1111-111111111111"),

                ConsultationFee = 900
            };

            // Act
            _repository.UpdateDoctor(updatedDoctor);

            Doctor? doctor =
                _repository.GetDoctorById(
                    updatedDoctor.DoctorId);

            // Assert
            Assert.NotNull(doctor);

            Assert.Equal(900, doctor.ConsultationFee);

            // Existing values should remain unchanged
            Assert.Equal("Dr John", doctor.FullName);

            Assert.Equal(
                Specialisation.Cardiology,
                doctor.Specialisation);

            Assert.Equal(10,
                doctor.YearsOfExperience);

        }

        [Fact]
        public void GetDoctorById_ShouldReturnDoctor()
        {
            // Arrange
            Guid doctorId = Guid.Parse(
                "11111111-1111-1111-1111-111111111111");

            // Act
            Doctor? doctor = _repository.GetDoctorById(doctorId);

            // Assert
            Assert.NotNull(doctor);
            Assert.Equal("Dr John", doctor.FullName);
            Assert.Equal(500, doctor.ConsultationFee);
        }

        [Fact]
        public void GetAllDoctors_ShouldReturnAllDoctors()
        {
            // Arrange
            _dataStore.Doctors.Add(new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FullName = "Dr Smith",
                Specialisation = Specialisation.Dermatology,
                YearsOfExperience = 8,
                ConsultationFee = 400,
                IsActive = true
            });

            //Act
            List<Doctor> doctors = _repository.GetAllDoctors();

            // Assert
            Assert.NotNull(doctors);
            Assert.True(doctors.Count >= 2);
        }

        [Fact]
        public void DeleteDoctorById_ShouldRemoveDoctor()
        {
            // Arrange
            Guid doctorId = Guid.Parse(
                "11111111-1111-1111-1111-111111111111");

            // Act
            _repository.DeleteDoctorById(doctorId);

            Doctor? doctor = _repository.GetDoctorById(doctorId);

            // Assert
            Assert.Null(doctor);
        }


    }
}
