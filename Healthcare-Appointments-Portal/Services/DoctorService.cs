using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Services
{

    public class DoctorService : IDoctorService
    {

        private readonly IDoctorRepository _doctorRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        // Dependency Injection
        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository)
        {

            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
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

            Console.WriteLine(
                doctor.GetScheduleSummary());

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

            if (doctors.Count == 0)
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

            List<Appointment> doctorAppointments =
                _appointmentRepository
                .GetAllAppointments()
                .Where(a => a.Doctor.DoctorId == doctorId)
                .ToList();

            bool hasConfirmedAppointments =
                doctorAppointments
                .Any(a =>
                a.Status ==
                AppointmentStatus.Confirmed);

            if (hasConfirmedAppointments)
            {
                throw new DoctorDeletionException();
            }

            // Cancel Pending Appointments
            List<Appointment> pendingAppointments =
                doctorAppointments
                .Where(a =>
                a.Status ==
                AppointmentStatus.Pending)
                .ToList();

            foreach (Appointment appointment in pendingAppointments)
            {

                appointment.Cancel(Constants.DoctorRemovedFromSystem);
                _appointmentRepository.UpdateAppointment(appointment);
            }

            _doctorRepository
                .DeleteDoctorById(doctorId);
        }
    }
}