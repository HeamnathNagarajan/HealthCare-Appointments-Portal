using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        // Add Doctor
        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            _repository.AddDoctor(doctor);
        }

        // Get Doctor By Id
        public Doctor? GetDoctorById(Guid doctorId)
        {
            return _repository.GetDoctorById(doctorId);
        }

        // Get All Doctors
        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }

        // Update Doctor
        public void UpdateDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            _repository.UpdateDoctor(doctor);
        }

        // Delete Doctor
        public void DeleteDoctorById(Guid doctorId)
        {
            var doctor = _repository.GetDoctorById(doctorId);

            if (doctor == null)
                throw new KeyNotFoundException("Doctor not found.");

            _repository.DeleteDoctorById(doctorId);
        }
    }
}
