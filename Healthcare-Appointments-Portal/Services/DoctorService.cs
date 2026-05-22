using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Services
{

    public class DoctorService : IDoctorService
    {

        private readonly IDoctorRepository _doctorRepository;

        // Dependency Injection
        public DoctorService(
            IDoctorRepository doctorRepository)
        {

            _doctorRepository = doctorRepository;
        }

        // Add New Doctor
        public void AddDoctor(Doctor doctor)
        {

            Doctor? existingDoctor =
                _doctorRepository
                .GetAllDoctors()
                .FirstOrDefault(d =>
                    d.FullName == doctor.FullName &&
                    d.Specialisation ==
                    doctor.Specialisation);

            if (existingDoctor != null)
            {

                throw new DuplicateDoctorException();
            }

            _doctorRepository.AddDoctor(doctor);
        }

        // Get Doctor By Id
        public Doctor? GetDoctorById(
            int doctorId)
        {

            Doctor? doctor =
                _doctorRepository
                .GetDoctorById(doctorId);

            if (doctor == null)
            {

                throw new DoctorNotFoundException();
            }

            return doctor;
        }

        // Get All Doctors
        public List<Doctor> GetAllDoctors()
        {

            return _doctorRepository
                .GetAllDoctors();
        }

        // Search Doctors By Specialisation
        public List<Doctor>
            GetDoctorsBySpecialisation(
                Specialisation specialisation)
        {

            List<Doctor> doctors =
                _doctorRepository
                .GetAllDoctors()
                .Where(d =>
                d.Specialisation ==
                specialisation &&
                d.IsActive)
             .ToList();

            if (!doctors.Any())
            {
                throw new DoctorNotFoundException();
            }

            return doctors;

        }

        // Get Available Doctors By Specialisation
        public List<Doctor> GetAvailableDoctorsBySpecialisation(
                Specialisation specialisation)
        {

            return _doctorRepository
                .GetAllDoctors()
                .Where(d =>
                    d.Specialisation ==
                    specialisation &&
                    d.IsActive)
                .ToList();
        }

        // Update Existing Doctor
        public void UpdateDoctor(
            Doctor updatedDoctor)
        {

            Doctor? existingDoctor =
                _doctorRepository
                .GetDoctorById(
                    updatedDoctor.DoctorId);

            if (existingDoctor == null)
            {

                throw new DoctorNotFoundException();
            }

            _doctorRepository
                .UpdateDoctor(updatedDoctor);
        }

        // Delete Doctor By Id
        public void DeleteDoctorById(
            int doctorId)
        {

            Doctor? doctor =
                _doctorRepository
                .GetDoctorById(doctorId);

            if (doctor == null)
            {

                throw new DoctorNotFoundException();
            }

            _doctorRepository
                .DeleteDoctorById(doctorId);
        }
    }
}