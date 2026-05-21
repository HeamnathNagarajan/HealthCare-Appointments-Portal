using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Enums;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IDoctorService
    {
        // Add new doctor
        void AddDoctor(Doctor doctor);

        // Get all doctors
        List<Doctor> GetAllDoctors();

        // Search doctors by specialisation
        List<Doctor> GetDoctorsBySpecialisation(
            Specialisation specialisation);
    }
}