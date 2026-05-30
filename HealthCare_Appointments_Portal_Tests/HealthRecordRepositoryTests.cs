using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;

namespace HealthCare_Appointment_Portal.Tests
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

        // Record Exists
        [Fact]
        public void RecordExists_ExistingAppointment_ShouldReturnTrue()
        {
            HealthRecord record =
                CreateHealthRecord();

            record.AppointmentId = 1;

            _dataStore.HealthRecords
                .Add(record);

            bool result =
                _repository.RecordExists(1);

            Assert.True(result);
        }

        // Record Does Not Exist
        [Fact]
        public void RecordExists_InvalidAppointment_ShouldReturnFalse()
        {
            bool result =
                _repository.RecordExists(99);

            Assert.False(result);
        }

        // Get Records By Patient
        [Fact]
        public void GetRecordsByPatient_ShouldReturnPatientRecords()
        {
            Patient patient1 =
                CreateHealthRecord()
                .Patient;

            patient1.PatientId = 1;

            Patient patient2 =
                CreateHealthRecord()
                .Patient;

            patient2.PatientId = 2;

            _dataStore.HealthRecords.AddRange(
            [
                new HealthRecord
        {
            RecordId = 1,
            AppointmentId = 1,
            Patient = patient1,
            Doctor = CreateHealthRecord().Doctor,
            VisitDate =
                new DateOnly(2026,5,20),
            Diagnosis = "A"
        },

        new HealthRecord
        {
            RecordId = 2,
            AppointmentId = 2,
            Patient = patient2,
            Doctor = CreateHealthRecord().Doctor,
            VisitDate =
                new DateOnly(2026,5,21),
            Diagnosis = "B"
        }
            ]);

            List<HealthRecord> result =
                _repository.GetRecordsByPatient(1);

            Assert.Single(result);
        }

        // Get Records By Doctor
        [Fact]
        public void GetRecordsByDoctor_ShouldReturnDoctorRecords()
        {
            Doctor doctor1 =
                CreateHealthRecord()
                .Doctor;

            doctor1.DoctorId = 1;

            Doctor doctor2 =
                CreateHealthRecord()
                .Doctor;

            doctor2.DoctorId = 2;

            _dataStore.HealthRecords.AddRange(
            [
                new HealthRecord
        {
            RecordId = 1,
            AppointmentId = 1,
            Patient = CreateHealthRecord().Patient,
            Doctor = doctor1,
            VisitDate =
                new DateOnly(2026,5,20)
        },

        new HealthRecord
        {
            RecordId = 2,
            AppointmentId = 2,
            Patient = CreateHealthRecord().Patient,
            Doctor = doctor2,
            VisitDate =
                new DateOnly(2026,5,21)
        }
            ]);

            List<HealthRecord> result =
                _repository.GetRecordsByDoctor(1);

            Assert.Single(result);
        }

        // Get Records By Doctor Empty
        [Fact]
        public void GetRecordsByDoctor_NoMatch_ShouldReturnEmpty()
        {
            List<HealthRecord> result =
                _repository.GetRecordsByDoctor(999);

            Assert.Empty(result);
        }

        // Get Recorded Appointment Ids
        [Fact]
        public void GetRecordedAppointmentIds_ShouldReturnIds()
        {
            _dataStore.HealthRecords.AddRange(
            [
                new HealthRecord
        {
            RecordId = 1,
            AppointmentId = 10,
            Patient = CreateHealthRecord().Patient,
            Doctor = CreateHealthRecord().Doctor,
            VisitDate = new DateOnly(2026, 5, 20),
            Diagnosis = "A",
            Prescription = "B",
            Notes = "C"
        },

        new HealthRecord
        {
            RecordId = 2,
            AppointmentId = 20,
            Patient = CreateHealthRecord().Patient,
            Doctor = CreateHealthRecord().Doctor,
            VisitDate = new DateOnly(2026, 5, 21),
            Diagnosis = "D",
            Prescription = "E",
            Notes = "F"
        }
            ]);

            List<int> result =
                _repository.GetRecordedAppointmentIds();

            Assert.Equal(2, result.Count);

            Assert.Contains(10, result);

            Assert.Contains(20, result);
        }

        // Get Recorded Appointment Ids Empty
        [Fact]
        public void GetRecordedAppointmentIds_Empty_ShouldReturnEmpty()
        {
            List<int> result =
                _repository.GetRecordedAppointmentIds();

            Assert.Empty(result);
        }

        // Get Records By Patient Empty
        [Fact]
        public void GetRecordsByPatient_NoMatch_ShouldReturnEmpty()
        {
            List<HealthRecord> result =
                _repository.GetRecordsByPatient(999);

            Assert.Empty(result);
        }


        [Fact]
        public void GetRecordsByPatient_ShouldReturnRecordsInDescendingVisitDateOrder()
        {
            Patient patient = CreateHealthRecord().Patient;
            patient.PatientId = 1;

            _dataStore.HealthRecords.AddRange(
            [
                new HealthRecord
        {
            RecordId = 1,
            AppointmentId = 1,
            Patient = patient,
            Doctor = CreateHealthRecord().Doctor,
            VisitDate = new DateOnly(2026, 5, 10)
        },

        new HealthRecord
        {
            RecordId = 2,
            AppointmentId = 2,
            Patient = patient,
            Doctor = CreateHealthRecord().Doctor,
            VisitDate = new DateOnly(2026, 5, 20)
        }
            ]);

            List<HealthRecord> result =
                _repository.GetRecordsByPatient(1);

            Assert.Equal(2, result.Count);

            Assert.Equal(
                new DateOnly(2026, 5, 20),
                result[0].VisitDate);

            Assert.Equal(
                new DateOnly(2026, 5, 10),
                result[1].VisitDate);
        }

        [Fact]
        public void GetRecordsByDoctor_ShouldReturnRecordsInDescendingVisitDateOrder()
        {
            Doctor doctor = CreateHealthRecord().Doctor;
            doctor.DoctorId = 1;

            _dataStore.HealthRecords.AddRange(
            [
                new HealthRecord
        {
            RecordId = 1,
            AppointmentId = 1,
            Patient = CreateHealthRecord().Patient,
            Doctor = doctor,
            VisitDate = new DateOnly(2026, 5, 10)
        },

        new HealthRecord
        {
            RecordId = 2,
            AppointmentId = 2,
            Patient = CreateHealthRecord().Patient,
            Doctor = doctor,
            VisitDate = new DateOnly(2026, 5, 20)
        }
            ]);

            List<HealthRecord> result =
                _repository.GetRecordsByDoctor(1);

            Assert.Equal(2, result.Count);

            Assert.Equal(
                new DateOnly(2026, 5, 20),
                result[0].VisitDate);

            Assert.Equal(
                new DateOnly(2026, 5, 10),
                result[1].VisitDate);
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

        [Fact]
        public void UpdateRecord_DefaultVisitDate_ShouldKeepExistingVisitDate()
        {
            // Arrange
            HealthRecord record = CreateHealthRecord();

            _dataStore.HealthRecords.Add(record);

            DateOnly originalDate = record.VisitDate;

            HealthRecord updatedRecord = new()
            {
                RecordId = record.RecordId,
                Patient = record.Patient,
                Doctor = record.Doctor,
                VisitDate = default
            };

            // Act
            _repository.UpdateRecord(updatedRecord);

            // Assert
            HealthRecord? result =
                _repository.GetRecordById(record.RecordId);

            Assert.NotNull(result);

            Assert.Equal(
                originalDate,
                result!.VisitDate);
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