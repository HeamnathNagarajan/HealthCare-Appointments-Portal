using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Data;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;

namespace HealthcareApp.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly List<Patient> _patients;
        private int _nextId;

        public PatientRepository(DataStore dataStore)
        {
            _patients = dataStore.Patients;

            _nextId = _patients.Any()
                ? _patients.Max(p => p.PatientId) + 1
                : 1;
        }

        public void Add(Patient patient)
        {
            patient.PatientId = _nextId++;
            _patients.Add(patient);
        }

        public Patient GetById(int patientId)
        {
            var patient = _patients.FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null)
                throw new PatientNotFoundException(patientId);

            return patient;
        }

        public List<Patient> GetAll()
        {
            return _patients.ToList();
        }

        public void Update(Patient patient)
        {
            var existingPatient = GetById(patient.PatientId);

            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceId = patient.InsuranceId;
            existingPatient.CreatedDate = patient.CreatedDate;
        }
    }
}
