using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{
    public interface IDoctorService
    {
        void AddDoctor(Doctor doctor);

        Doctor? GetDoctorById(Guid doctorId);

        List<Doctor> GetAllDoctors();

        void UpdateDoctor(Doctor doctor);

        void DeleteDoctorById(Guid doctorId);
    }
}