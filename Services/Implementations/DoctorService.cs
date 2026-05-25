using HealthcareApp.Enums;
using HealthcareApp.Models;
using HealthcareApp.Repositories;

namespace HealthcareApp.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public Doctor AddDoctor(Doctor doctor)
        {
            ValidateDoctor(doctor);

            doctor.IsActive = true;

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

            _doctorRepository.Update(doctor);

            return doctor;
        }

        private static void ValidateDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentException("Doctor details are required.");

            if (string.IsNullOrWhiteSpace(doctor.FullName))
                throw new ArgumentException("Doctor full name is required.");

            if (doctor.YearsOfExperience < 0)
                throw new ArgumentException("Years of experience cannot be negative.");

            if (doctor.ConsultationFee < 0)
                throw new ArgumentException("Consultation fee cannot be negative.");

            if (doctor.OffDays == null || doctor.OffDays.Count != 2)
                throw new ArgumentException("Doctor must have exactly two off-duty days.");

            if (doctor.OffDays.Distinct().Count() != 2)
                throw new ArgumentException("Doctor off-duty days must be different.");
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
                throw new ArgumentException("Valid Doctor ID is required.");
        }
    }
}