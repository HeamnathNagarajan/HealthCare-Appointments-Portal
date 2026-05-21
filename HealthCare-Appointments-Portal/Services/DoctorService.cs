using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{
    public class DoctorService
        : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        // Dependency Injection
        public DoctorService(
            IDoctorRepository repository)
        {
            _repository = repository;
        }

        // Add Doctor
        public void AddDoctor(Doctor doctor)
        {
            _repository.AddDoctor(doctor);
        }

        // Get All Doctors
        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }

        // Get Doctors By Specialisation
        public List<Doctor> GetDoctorsBySpecialisation(
            Specialisation specialisation)
        {
            return _repository
                .GetAllDoctors()
                .Where(d =>
                    d.Specialisation ==
                    specialisation)
                .ToList();
        }
    }
}