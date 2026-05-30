using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Enums;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IDoctorRepository
    {
        void AddDoctor(Doctor doctor);

        Doctor? GetDoctorById(int doctorId);

        List<Doctor> GetAllDoctors();

        Doctor? GetDoctorByNameAndSpecialisation(
            string fullName,
            Specialisation specialisation);

        List<Doctor> GetDoctorsBySpecialisation(
            Specialisation specialisation);

        List<Doctor>
            GetAvailableDoctorsBySpecialisation(
                Specialisation specialisation);

        void UpdateDoctor(Doctor doctor);

        void DeleteDoctorById(int doctorId);
    }
}