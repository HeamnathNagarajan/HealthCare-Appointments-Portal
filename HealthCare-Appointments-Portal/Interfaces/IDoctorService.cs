using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Enums;

namespace HealthCare_Appointment_Portal.Interfaces
{

    public interface IDoctorService
    {

        // Add New Doctor
        void AddDoctor(Doctor doctor);

        // Get Doctor By Id
        Doctor? GetDoctorById(int doctorId);

        // Get All Doctors
        List<Doctor> GetAllDoctors();

        // Get Available Doctors By Specialisation
        List<Doctor> GetAvailableDoctorsBySpecialisation(
             Specialisation specialisation);

        // Search Doctors By Specialisation
        List<Doctor> GetDoctorsBySpecialisation(
            Specialisation specialisation);

        // Update Existing Doctor
        void UpdateDoctor(Doctor updatedDoctor);

        // Delete Doctor By Id
        void DeleteDoctorById(int doctorId);
    }
}
