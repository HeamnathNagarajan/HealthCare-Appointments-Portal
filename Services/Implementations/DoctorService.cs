using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;
using HealthcareApp.Repositories;
using HealthcareApp.Repositories.Implementations;
using HealthcareApp.Services;

namespace HealthcareApp.Services.Implementations
{
        public class DoctorService : IDoctorService
        {
            private readonly IDoctorRepository _doctorRepository;
            private readonly IAppointmentRepository _appointmentRepository;

            public DoctorService(
                IDoctorRepository doctorRepository,
                IAppointmentRepository appointmentRepository)
            {
                _doctorRepository = doctorRepository;
                _appointmentRepository = appointmentRepository;
            }

            public Doctor AddDoctor(Doctor doctor)
            {
                ValidateDoctor(doctor);

                doctor.IsActive = true;

                if (doctor.OffDays == null)
                {
                    doctor.OffDays = new List<DateOnly>();
                }

                _doctorRepository.Add(doctor);

                return doctor;
            }

            public Doctor GetDoctorById(int doctorId)
            {
                return _doctorRepository.GetById(doctorId);
            }

            public List<Doctor> GetAllDoctors()
            {
                return _doctorRepository.GetAll();
            }

            public List<Doctor> SearchDoctorsBySpecialisation(Specialisation specialisation)
            {
                return _doctorRepository
                    .GetBySpecialisation(specialisation)
                    .Where(d => d.IsActive)
                    .ToList();
            }

            public Doctor UpdateDoctor(Doctor doctor)
            {
                ValidateDoctor(doctor);
                ValidateDoctorId(doctor.DoctorId);

                if (doctor.OffDays == null)
                {
                    doctor.OffDays = new List<DateOnly>();
                }

                _doctorRepository.Update(doctor);

                return doctor;
            }

            public List<DateOnly> GetOffDays(int doctorId)
            {
                Doctor doctor = _doctorRepository.GetById(doctorId);

                return doctor.OffDays
                    .OrderBy(date => date)
                    .ToList();
            }

            public Doctor AddOffDay(int doctorId, DateOnly offDay)
            {
                Doctor doctor = _doctorRepository.GetById(doctorId);

                if (doctor.OffDays == null)
                {
                    doctor.OffDays = new List<DateOnly>();
                }

                if (doctor.OffDays.Contains(offDay))
                {
                    throw new ArgumentException("This date is already marked as an off day.");
                }

                bool hasConfirmedAppointments = _appointmentRepository
                    .GetByDoctorId(doctorId)
                    .Count(appointment =>
                        appointment.ScheduledDate == offDay &&
                        appointment.Status == AppointmentStatus.Confirmed) > 0;

                if (hasConfirmedAppointments)
                {
                    throw new DoctorUnavailableException(
                        "Cannot mark this date as off because the doctor has confirmed appointments on this date.");
                }

                doctor.OffDays.Add(offDay);

                _doctorRepository.Update(doctor);

                return doctor;
            }

            public Doctor RemoveOffDay(int doctorId, DateOnly offDay)
            {
                Doctor doctor = _doctorRepository.GetById(doctorId);

                if (doctor.OffDays == null || !doctor.OffDays.Contains(offDay))
                {
                    throw new ArgumentException("This date is not currently marked as an off day.");
                }

                doctor.OffDays.Remove(offDay);

                _doctorRepository.Update(doctor);

                return doctor;
            }

            private static void ValidateDoctor(Doctor doctor)
            {
                if (doctor is null)
                {
                    throw new ArgumentException("Doctor details are required.");
                }

                if (string.IsNullOrWhiteSpace(doctor.FullName))
                {
                    throw new ArgumentException("Doctor full name is required.");
                }

                if (doctor.YearsOfExperience < 0)
                {
                    throw new ArgumentException("Years of experience cannot be negative.");
                }

                if (doctor.ConsultationFee < 0)
                {
                    throw new ArgumentException("Consultation fee cannot be negative.");
                }

                if (doctor.OffDays != null &&
                    doctor.OffDays.Count != doctor.OffDays.Distinct().Count())
                {
                    throw new ArgumentException("Doctor off-day dates must be different.");
                }
            }

            private static void ValidateDoctorId(int doctorId)
            {
                if (doctorId <= 0)
                {
                    throw new ArgumentException("Valid Doctor ID is required.");
                }
            }
        }
}

