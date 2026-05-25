using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;
using HealthCare_Appointments_Portal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HealthCare_Appointments_Portal.Tests
{
    public class HealthRecordRepositoryTests
    {
        private readonly DataStore _dataStore;
        private readonly HealthRecordRepository _repository;
        private readonly TestData _testData;

        public HealthRecordRepositoryTests()
        {
            _dataStore = new DataStore();
            _repository = new HealthRecordRepository(_dataStore);
            _testData = new TestData();
        }

        [Fact]
        public void AddRecord_ShouldAddHealthRecord_ToDataStore()
        {
            // Arrange
            var record = _testData.HealthRecords.First();

            // Act
            _repository.AddRecord(record);
            var result = _repository.GetAllRecords();

            // Assert
            Assert.Single(result);
            Assert.Equal(record.RecordId, result.First().RecordId);
        }

        [Fact]
        public void AddRecord_ShouldAllowMultipleRecords()
        {
            // Arrange
            var records = _testData.HealthRecords;

            // Act
            foreach (var record in records)
            {
                _repository.AddRecord(record);
            }

            var result = _repository.GetAllRecords();

            // Assert
            Assert.Equal(records.Count, result.Count);
        }

        [Fact]
        public void GetAllRecords_ShouldReturnEmptyList_WhenNoRecordsExist()
        {
            // Act
            var result = _repository.GetAllRecords();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAllRecords_ShouldReturnCorrectData_AfterInsertions()
        {
            // Arrange
            var records = _testData.HealthRecords;

            foreach (var record in records)
            {
                _repository.AddRecord(record);
            }

            // Act
            var result = _repository.GetAllRecords();

            // Assert
            foreach (var record in records)
            {
                Assert.Contains(result, r => r.RecordId == record.RecordId);
            }
        }


        private class TestData
        {
            public List<Patient> Patients { get; set; }
            public List<Doctor> Doctors { get; set; }
            public List<HealthRecord> HealthRecords { get; set; }

            public TestData()
            {
                Patients = new List<Patient>
                {
                    new Patient
                    {
                        FullName = "John Doe",
                        DateOfBirth = new DateOnly(1990, 5, 10),
                        Gender = "Male",
                        PhoneNumber = "1234567890",
                        Email = "john.doe@test.com",
                        InsuranceId = "INS001"
                    },
                    new Patient
                    {
                        FullName = "Jane Smith",
                        DateOfBirth = new DateOnly(1985, 8, 20),
                        Gender = "Female",
                        PhoneNumber = "9876543210",
                        Email = "jane.smith@test.com",
                        InsuranceId = "INS002"
                    }
                };

                Doctors = new List<Doctor>
                {
                    new Doctor
                    {
                        FullName = "Dr. Gregory House",
                        Specialisation = Specialisation.InternalMedicine,
                        YearsOfExperience = 20,
                        ConsultationFee = 1000,
                        IsActive = true
                    },
                    new Doctor
                    {
                        FullName = "Dr. Meredith Grey",
                        Specialisation = Specialisation.GeneralSurgery,
                        YearsOfExperience = 12,
                        ConsultationFee = 800,
                        IsActive = true
                    }
                };

                HealthRecords = new List<HealthRecord>
                {
                    new HealthRecord
                    {
                        Patient = Patients[0],
                        Doctor = Doctors[0],
                        VisitDate = new DateOnly(2024, 1, 10),
                        Diagnosis = "Viral Fever",
                        Prescription = "Paracetamol",
                        Notes = "Recovering"
                    },
                    new HealthRecord
                    {
                        Patient = Patients[1],
                        Doctor = Doctors[1],
                        VisitDate = new DateOnly(2024, 2, 15),
                        Diagnosis = "Routine Checkup",
                        Prescription = "None",
                        Notes = "Healthy"
                    }
                };
            }
        }
    }
}
