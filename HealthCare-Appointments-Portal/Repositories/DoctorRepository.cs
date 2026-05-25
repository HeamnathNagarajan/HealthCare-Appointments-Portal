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

        public void UpdateDoctor(Doctor doctor)
        {
            Doctor? existingDoctor = _dataStore.Doctors
                .FirstOrDefault(d =>
                    d.DoctorId == doctor.DoctorId);

            if (existingDoctor != null)
            {

                existingDoctor.FullName =
                    string.IsNullOrWhiteSpace(doctor.FullName)
                    ? existingDoctor.FullName
                    : doctor.FullName;

                existingDoctor.Specialisation =
                    doctor.Specialisation == default
                    ? existingDoctor.Specialisation
                    : doctor.Specialisation;

                existingDoctor.YearsOfExperience =
                    doctor.YearsOfExperience == 0
                    ? existingDoctor.YearsOfExperience
                    : doctor.YearsOfExperience;

                existingDoctor.ConsultationFee =
                    doctor.ConsultationFee == 0
                    ? existingDoctor.ConsultationFee
                    : doctor.ConsultationFee;

                existingDoctor.IsActive =
                    doctor.IsActive;
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
