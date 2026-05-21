using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Repositories
{

    public class DoctorRepository : IDoctorRepository
    {

        private readonly DataStore _dataStore;

        // Dependency Injection
        public DoctorRepository(DataStore dataStore)
        {

            _dataStore = dataStore;
        }

        // Add New Doctor
        public void AddDoctor(Doctor doctor)
        {

            _dataStore.Doctors.Add(doctor);
        }

        // Get Doctor By Id
        public Doctor? GetDoctorById(int doctorId)
        {

            return _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId == doctorId);
        }

        // Get All Doctors
        public List<Doctor> GetAllDoctors()
        {

            return _dataStore.Doctors
                .ToList();
        }

        // Update Existing Doctor
        public void UpdateDoctor(Doctor updatedDoctor)
        {

            Doctor? existingDoctor =
                _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId ==
                    updatedDoctor.DoctorId);

            if (existingDoctor != null)
            {

                existingDoctor.FullName =
                    string.IsNullOrWhiteSpace(
                        updatedDoctor.FullName)
                    ? existingDoctor.FullName
                    : updatedDoctor.FullName;

                existingDoctor.Specialisation =
                    updatedDoctor.Specialisation == default
                    ? existingDoctor.Specialisation
                    : updatedDoctor.Specialisation;

                existingDoctor.YearsOfExperience =
                    updatedDoctor.YearsOfExperience == 0
                    ? existingDoctor.YearsOfExperience
                    : updatedDoctor.YearsOfExperience;

                existingDoctor.ConsultationFee =
                    updatedDoctor.ConsultationFee == 0
                    ? existingDoctor.ConsultationFee
                    : updatedDoctor.ConsultationFee;

                existingDoctor.IsActive =
                    updatedDoctor.IsActive;
            }
        }

        // Delete Doctor By Id
        public void DeleteDoctorById(int doctorId)
        {

            Doctor? doctor =
                _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId == doctorId);

            if (doctor != null)
            {

                _dataStore.Doctors.Remove(doctor);
            }
        }
    }
}
