using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{

    public interface IDoctorRepository
    {

        void AddDoctor(Doctor doctor);

        Doctor? GetDoctorById(Guid doctorId);

        List<Doctor> GetAllDoctors();

        void UpdateDoctor(Doctor doctor);

        void DeleteDoctorById(Guid doctorId);
    }
}
