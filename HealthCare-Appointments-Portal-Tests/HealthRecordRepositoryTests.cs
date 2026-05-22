using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;

namespace HealthCare_Appointments_Portal.Tests
{
    public class HealthRecordRepositoryTests
    {
        private readonly DataStore _dataStore;

        private readonly HealthRecordRepository _repository;

        public HealthRecordRepositoryTests()
        {
            _dataStore = new DataStore();

            _repository =
                new HealthRecordRepository(
                    _dataStore);
        }

        // Add Health Record Success
        [Fact]
        public void AddRecord_ValidRecord_ShouldAddRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            // Act
            _repository.AddRecord(
                record);

            // Assert
            Assert.Single(
                _dataStore.HealthRecords);

            Assert.Equal(
                record.RecordId,
                _dataStore
                    .HealthRecords[0]
                    .RecordId);
        }

        // Get Record By Existing Id
        [Fact]
        public void GetRecordById_ExistingId_ShouldReturnRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _dataStore
                .HealthRecords
                .Add(record);

            // Act
            HealthRecord? result =
                _repository.GetRecordById(
                    record.RecordId);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                record.RecordId,
                result?.RecordId);
        }

        // Get Record By Invalid Id
        [Fact]
        public void GetRecordById_InvalidId_ShouldReturnNull()
        {
            // Act
            HealthRecord? result =
                _repository.GetRecordById(
                    32);

            // Assert
            Assert.Null(
                result);
        }

        // Get All Records
        [Fact]
        public void GetAllRecords_ShouldReturnAllRecords()
        {
            // Arrange
            _dataStore.HealthRecords.AddRange(
            [
                CreateHealthRecord(),
                CreateHealthRecord()
            ]);

            // Act
            List<HealthRecord> result =
                _repository.GetAllRecords();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Records Empty
        [Fact]
        public void GetAllRecords_EmptyList_ShouldReturnEmpty()
        {
            // Act
            List<HealthRecord> result =
                _repository.GetAllRecords();

            // Assert
            Assert.Empty(
                result);
        }

        // Update Existing Record
        [Fact]
        public void UpdateRecord_ExistingRecord_ShouldUpdateDetails()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _dataStore
                .HealthRecords
                .Add(record);

            HealthRecord updatedRecord = new()
            {
                RecordId =
                    record.RecordId,

                Patient =
                    record.Patient,

                Doctor =
                    record.Doctor,

                VisitDate =
                    new DateOnly(
                        2026,
                        6,
                        10),

                Diagnosis =
                    "Updated Diagnosis",

                Prescription =
                    "Updated Prescription",

                Notes =
                    "Updated Notes"
            };

            // Act
            _repository.UpdateRecord(
                updatedRecord);

            // Assert
            HealthRecord? result =
                _repository.GetRecordById(
                    record.RecordId);

            Assert.NotNull(
                result);

            Assert.Equal(
                "Updated Diagnosis",
                result?.Diagnosis);

            Assert.Equal(
                "Updated Prescription",
                result?.Prescription);

            Assert.Equal(
                "Updated Notes",
                result?.Notes);
        }

        // Partial Update Record
        [Fact]
        public void UpdateRecord_PartialUpdate_ShouldUpdateOnlyProvidedFields()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _dataStore
                .HealthRecords
                .Add(record);

            HealthRecord updatedRecord = new()
            {
                RecordId =
                    record.RecordId,

                Patient =
                    record.Patient,

                Doctor =
                    record.Doctor,

                Diagnosis =
                    "Updated Diagnosis"
            };

            // Act
            _repository.UpdateRecord(
                updatedRecord);

            // Assert
            HealthRecord? result =
                _repository.GetRecordById(
                    record.RecordId);

            Assert.NotNull(
                result);

            // Updated Field
            Assert.Equal(
                "Updated Diagnosis",
                result?.Diagnosis);

            // Existing Fields Unchanged
            Assert.Equal(
                record.Prescription,
                result?.Prescription);

            Assert.Equal(
                record.Notes,
                result?.Notes);
        }

        // Update Invalid Record
        [Fact]
        public void UpdateRecord_InvalidId_ShouldNotUpdate()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            // Act
            _repository.UpdateRecord(
                record);

            // Assert
            Assert.Empty(
                _dataStore.HealthRecords);
        }

        // Delete Existing Record
        [Fact]
        public void DeleteRecordById_ExistingId_ShouldRemoveRecord()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _dataStore
                .HealthRecords
                .Add(record);

            // Act
            _repository.DeleteRecordById(
                record.RecordId);

            // Assert
            Assert.Empty(
                _dataStore.HealthRecords);
        }

        // Delete Invalid Record
        [Fact]
        public void DeleteRecordById_InvalidId_ShouldNotRemoveAnything()
        {
            // Arrange
            HealthRecord record =
                CreateHealthRecord();

            _dataStore
                .HealthRecords
                .Add(record);

            // Act
            _repository.DeleteRecordById(
                67);

            // Assert
            Assert.Single(
                _dataStore.HealthRecords);
        }

        // Helper Method
        private static HealthRecord CreateHealthRecord()
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

            return new HealthRecord
            {
                Patient =
                    patient,

                Doctor =
                    doctor,

                VisitDate =
                    new DateOnly(
                        2026,
                        5,
                        20),

                Diagnosis =
                    "Fever",

                Prescription =
                    "Paracetamol",

                Notes =
                    "Take Rest"
            };
        }
    }
}