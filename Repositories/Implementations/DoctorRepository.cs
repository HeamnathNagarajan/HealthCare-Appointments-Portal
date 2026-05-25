using HealthcareApp.Data;
using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly List<Doctor> _doctors;
        private int _nextId;

        public DoctorRepository(DataStore dataStore)
        {
            _doctors = dataStore.Doctors;

            _nextId = _doctors.Count == 0
                ? 1
                : _doctors.Max(d => d.DoctorId) + 1;
        }

        public void Add(Doctor doctor)
        {
            doctor.DoctorId = _nextId++;
            _doctors.Add(doctor);
        }

        public Doctor GetById(int doctorId)
        {
            var doctor = _doctors.FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor == null)
                throw new DoctorNotFoundException(doctorId);

            return doctor;
        }

        public List<Doctor> GetAll()
        {
            return _doctors.ToList();
        }

        public List<Doctor> GetBySpecialisation(Specialisation specialisation)
        {
            return _doctors
                .Where(d => d.Specialisation == specialisation)
                .ToList();
        }

        public void Update(Doctor doctor)
        {
            var existingDoctor = GetById(doctor.DoctorId);

            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;
            existingDoctor.OffDays = doctor.OffDays.ToList();
        }
    }
}
