using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;

namespace HealthCare_Appointment_Portal.Tests
{
    public class DoctorRepositoryTests
    {
        private readonly DataStore _dataStore;

        private readonly DoctorRepository _repository;

        public DoctorRepositoryTests()
        {
            _dataStore = new DataStore();

            _repository =
                new DoctorRepository(
                    _dataStore);
        }

        // Add Doctor Success
        [Fact]
        public void AddDoctor_ValidDoctor_ShouldAddDoctor()
        {
            // Arrange
            Doctor doctor = new()
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

            // Act
            _repository.AddDoctor(
                doctor);

            // Assert
            Assert.Single(
                _dataStore.Doctors);

            Assert.Equal(
                "Dr Ragu",
                _dataStore
                    .Doctors[0]
                    .FullName);
        }

        // Get Doctor By Existing Id
        [Fact]
        public void GetDoctorById_ExistingId_ShouldReturnDoctor()
        {
            // Arrange
            Doctor doctor = new()
            {
                FullName =
                    "Dr Arun",

                Specialisation =
                    Specialisation.Neurology,

                YearsOfExperience =
                    8,

                ConsultationFee =
                    1500,

                IsActive =
                    true
            };

            _dataStore
                .Doctors
                .Add(doctor);

            // Act
            Doctor? result =
                _repository.GetDoctorById(
                    doctor.DoctorId);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                doctor.DoctorId,
                result?.DoctorId);
        }

        // Get Doctor By Name And Specialisation
        [Fact]
        public void GetDoctorByNameAndSpecialisation_ExistingDoctor_ShouldReturnDoctor()
        {
            Doctor doctor = new()
            {
                FullName = "Dr Ragu",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 1000,
                IsActive = true
            };

            _dataStore.Doctors.Add(doctor);

            Doctor? result =
                _repository
                .GetDoctorByNameAndSpecialisation(
                    "Dr Ragu",
                    Specialisation.Cardiology);

            Assert.NotNull(result);

            Assert.Equal(
                doctor.FullName,
                result?.FullName);
        }

        // Get Doctor By Name And Specialisation Ignore Case
        [Fact]
        public void GetDoctorByNameAndSpecialisation_CaseInsensitive_ShouldReturnDoctor()
        {
            Doctor doctor = new()
            {
                FullName = "Dr Ragu",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 1000,
                IsActive = true
            };

            _dataStore.Doctors.Add(doctor);

            Doctor? result =
                _repository
                .GetDoctorByNameAndSpecialisation(
                    "DR RAGU",
                    Specialisation.Cardiology);

            Assert.NotNull(result);
        }

        // Get Doctor By Name And Specialisation Invalid
        [Fact]
        public void GetDoctorByNameAndSpecialisation_Invalid_ShouldReturnNull()
        {
            Doctor? result =
                _repository
                .GetDoctorByNameAndSpecialisation(
                    "Unknown",
                    Specialisation.Cardiology);

            Assert.Null(result);
        }

        // Get Doctors By Specialisation
        [Fact]
        public void GetDoctorsBySpecialisation_ShouldReturnMatchingDoctors()
        {
            _dataStore.Doctors.AddRange(
            [
                new Doctor
        {
            FullName = "Doctor 1",
            Specialisation = Specialisation.Cardiology,
            YearsOfExperience = 5,
            ConsultationFee = 1000,
            IsActive = true
        },

        new Doctor
        {
            FullName = "Doctor 2",
            Specialisation = Specialisation.Neurology,
            YearsOfExperience = 5,
            ConsultationFee = 1000,
            IsActive = true
        }
            ]);

            List<Doctor> result =
                _repository
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Single(result);
        }

        // Get Doctors By Specialisation No Match
        [Fact]
        public void GetDoctorsBySpecialisation_NoMatch_ShouldReturnEmpty()
        {
            List<Doctor> result =
                _repository
                .GetDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Empty(result);
        }

        // Get Available Doctors By Specialisation
        [Fact]
        public void GetAvailableDoctorsBySpecialisation_ShouldReturnOnlyActiveDoctors()
        {
            _dataStore.Doctors.AddRange(
            [
                new Doctor
        {
            FullName = "Active Doctor",
            Specialisation = Specialisation.Cardiology,
            YearsOfExperience = 5,
            ConsultationFee = 1000,
            IsActive = true
        },

        new Doctor
        {
            FullName = "Inactive Doctor",
            Specialisation = Specialisation.Cardiology,
            YearsOfExperience = 5,
            ConsultationFee = 1000,
            IsActive = false
        }
            ]);

            List<Doctor> result =
                _repository
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Single(result);

            Assert.True(result[0].IsActive);
        }

        // Get Available Doctors By Specialisation No Match
        [Fact]
        public void GetAvailableDoctorsBySpecialisation_NoMatch_ShouldReturnEmpty()
        {
            List<Doctor> result =
                _repository
                .GetAvailableDoctorsBySpecialisation(
                    Specialisation.Cardiology);

            Assert.Empty(result);
        }

        // Get Doctor By Invalid Id
        [Fact]
        public void GetDoctorById_InvalidId_ShouldReturnNull()
        {
            // Act
            Doctor? result =
                _repository.GetDoctorById(
                    43);

            // Assert
            Assert.Null(
                result);
        }

        // Get All Doctors
        [Fact]
        public void GetAllDoctors_ShouldReturnAllDoctors()
        {
            // Arrange
            _dataStore.Doctors.AddRange(
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
                        10,

                    ConsultationFee =
                        2000,

                    IsActive =
                        true
                }
            ]);

            // Act
            List<Doctor> result =
                _repository.GetAllDoctors();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Doctors Empty
        [Fact]
        public void GetAllDoctors_EmptyList_ShouldReturnEmpty()
        {
            // Act
            List<Doctor> result =
                _repository.GetAllDoctors();

            // Assert
            Assert.Empty(
                result);
        }

        // Update Existing Doctor
        [Fact]
        public void UpdateDoctor_ExistingDoctor_ShouldUpdateDetails()
        {
            // Arrange
            Doctor doctor = new()
            {
                FullName =
                    "Old Doctor",

                Specialisation =
                    Specialisation.Cardiology,

                YearsOfExperience =
                    5,

                ConsultationFee =
                    1000,

                IsActive =
                    true
            };

            _dataStore
                .Doctors
                .Add(doctor);

            Doctor updatedDoctor = new()
            {
                DoctorId =
                    doctor.DoctorId,

                FullName =
                    "Updated Doctor",

                Specialisation =
                    Specialisation.Neurology,

                YearsOfExperience =
                    12,

                ConsultationFee =
                    2500,

                IsActive =
                    false
            };

            // Act
            _repository.UpdateDoctor(
                updatedDoctor);

            // Assert
            Doctor? result =
                _repository.GetDoctorById(
                    doctor.DoctorId);

            Assert.NotNull(
                result);

            Assert.Equal(
                "Updated Doctor",
                result?.FullName);

            Assert.Equal(
                Specialisation.Neurology,
                result?.Specialisation);

            Assert.Equal(
                12,
                result?.YearsOfExperience);

            Assert.Equal(
                2500,
                result?.ConsultationFee);

            Assert.False(
                result!.IsActive);
        }

        // Partial Update Doctor
        [Fact]
        public void UpdateDoctor_PartialUpdate_ShouldUpdateOnlyProvidedFields()
        {
            // Arrange
            Doctor doctor = new()
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

            _dataStore
                .Doctors
                .Add(doctor);

            Doctor updatedDoctor = new()
            {
                DoctorId =
                    doctor.DoctorId,

                FullName =
                    "Updated Dr Ragu"
            };

            // Act
            _repository.UpdateDoctor(
                updatedDoctor);

            // Assert
            Doctor? result =
                _repository.GetDoctorById(
                    doctor.DoctorId);

            Assert.NotNull(
                result);

            // Updated Field
            Assert.Equal(
                "Updated Dr Ragu",
                result?.FullName);

            // Existing Fields Unchanged
            Assert.Equal(
                Specialisation.Cardiology,
                result?.Specialisation);

            Assert.Equal(
                5,
                result?.YearsOfExperience);

            Assert.Equal(
                1000,
                result?.ConsultationFee);
        }

        // Update Invalid Doctor
        [Fact]
        public void UpdateDoctor_InvalidId_ShouldNotUpdate()
        {
            // Arrange
            Doctor updatedDoctor = new()
            {
                DoctorId =
                    67,

                FullName =
                    "Updated Doctor"
            };

            // Act
            _repository.UpdateDoctor(
                updatedDoctor);

            // Assert
            Assert.Empty(
                _dataStore.Doctors);
        }

        // Delete Existing Doctor
        [Fact]
        public void DeleteDoctorById_ExistingId_ShouldRemoveDoctor()
        {
            // Arrange
            Doctor doctor = new()
            {
                FullName =
                    "Delete Doctor",

                Specialisation =
                    Specialisation.Cardiology,

                YearsOfExperience =
                    5,

                ConsultationFee =
                    1000,

                IsActive =
                    true
            };

            _dataStore
                .Doctors
                .Add(doctor);

            // Act
            _repository.DeleteDoctorById(
                doctor.DoctorId);

            // Assert
            Assert.Empty(
                _dataStore.Doctors);
        }

        // Delete Invalid Doctor
        [Fact]
        public void DeleteDoctorById_InvalidId_ShouldNotRemoveAnything()
        {
            // Arrange
            Doctor doctor = new()
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

            _dataStore
                .Doctors
                .Add(doctor);

            // Act
            _repository.DeleteDoctorById(
                67);

            // Assert
            Assert.Single(
                _dataStore.Doctors);
        }
    }
}