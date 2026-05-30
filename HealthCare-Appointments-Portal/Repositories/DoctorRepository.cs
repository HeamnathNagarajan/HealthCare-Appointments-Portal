using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Enums;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class DoctorRepository
        : IDoctorRepository
    {
        private readonly
            DataStore _dataStore;

        // Dependency Injection
        public DoctorRepository(
            DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // Add New Doctor
        public void AddDoctor(
            Doctor doctor)
        {
            _dataStore.Doctors
                .Add(doctor);
        }

        // Get Doctor By Id
        public Doctor? GetDoctorById(
            int doctorId)
        {
            return _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId ==
                    doctorId);
        }

        // Get All Doctors
        public List<Doctor>
            GetAllDoctors()
        {
            return _dataStore.Doctors
                .ToList();
        }

        // Get Doctor By Name And Specialisation
        public Doctor?
            GetDoctorByNameAndSpecialisation(
                string fullName,
                Specialisation specialisation)
        {
            return _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.FullName.Equals(
                        fullName,
                        StringComparison
                            .OrdinalIgnoreCase)
                    &&
                    d.Specialisation ==
                    specialisation);
        }

        // Get Doctors By Specialisation
        public List<Doctor>
            GetDoctorsBySpecialisation(
                Specialisation specialisation)
        {
            return _dataStore.Doctors
                .Where(d =>
                    d.Specialisation ==
                    specialisation)
                .ToList();
        }

        // Get Available Doctors By Specialisation
        public List<Doctor>
            GetAvailableDoctorsBySpecialisation(
                Specialisation specialisation)
        {
            return _dataStore.Doctors
                .Where(d =>
                    d.Specialisation ==
                    specialisation
                    &&
                    d.IsActive)
                .ToList();
        }

        // Update Existing Doctor
        public void UpdateDoctor(
            Doctor doctor)
        {
            Doctor? existingDoctor =
                _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId ==
                    doctor.DoctorId);

            if (existingDoctor != null)
            {
                existingDoctor.FullName =
                    string.IsNullOrWhiteSpace(
                        doctor.FullName)
                    ? existingDoctor.FullName
                    : doctor.FullName;

                existingDoctor.Specialisation =
                    doctor.Specialisation
                        == default
                    ? existingDoctor
                        .Specialisation
                    : doctor.Specialisation;

                existingDoctor
                    .YearsOfExperience =
                    doctor.YearsOfExperience
                        == 0
                    ? existingDoctor
                        .YearsOfExperience
                    : doctor
                        .YearsOfExperience;

                existingDoctor
                    .ConsultationFee =
                    doctor.ConsultationFee
                        == 0
                    ? existingDoctor
                        .ConsultationFee
                    : doctor
                        .ConsultationFee;

                existingDoctor.IsActive =
                    doctor.IsActive;
            }
        }

        // Delete Doctor By Id
        public void DeleteDoctorById(
            int doctorId)
        {
            Doctor? doctor =
                _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId ==
                    doctorId);

            if (doctor != null)
            {
                _dataStore.Doctors
                    .Remove(doctor);
            }
        }
    }
}