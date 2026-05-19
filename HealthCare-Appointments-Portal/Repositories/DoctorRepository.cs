using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DataStore _dataStore;

        // Dependency Injection
        public DoctorRepository(
            DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void AddDoctor(Doctor doctor)
        {
            _dataStore.Doctors.Add(doctor);
        }

        public Doctor? GetDoctorById(Guid doctorId)
        {
            return _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId == doctorId);
        }

        public List<Doctor> GetAllDoctors()
        {
            return _dataStore.Doctors;
        }

        public void UpdateDoctor(Doctor updatedDoctor)
        {
            Doctor? existingDoctor = _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId == updatedDoctor.DoctorId);

            if (existingDoctor != null)
            {

                existingDoctor.FullName =
                    string.IsNullOrWhiteSpace(updatedDoctor.FullName)
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
        public void DeleteDoctorById(Guid doctorId)
        {

            Doctor? doctor = _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId == doctorId);

            if (doctor != null)
            {

                _dataStore.Doctors.Remove(doctor);
            }
        }

    }
}
