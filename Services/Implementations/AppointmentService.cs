using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;
using HealthcareApp.Repositories;
using HealthcareApp.Utilities;

namespace HealthcareApp.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public Appointment BookAppointment(int patientId, int doctorId, DateOnly date)
        {
            DateOnly today = SystemTime.Now;

            if (date < today)
            {
                throw new PastDateException("Cannot book an appointment in the past.");
            }

            Patient patient = _patientRepository.GetById(patientId);
            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (!doctor.IsAvailable(date))
            {
                throw new DoctorUnavailableException(
                    "Doctor is unavailable on the selected date.");
            }

            List<Appointment> doctorAppointments =
                _appointmentRepository.GetByDoctorId(doctorId) ?? new List<Appointment>();

            int appointmentCount = doctorAppointments.Count(a =>
                a.ScheduledDate == date &&
                a.Status != AppointmentStatus.Cancelled);

            if (appointmentCount >= 10)
            {
                throw new DoctorUnavailableException(
                    "Doctor is fully booked on the selected date.");
            }

            bool alreadyBooked = doctorAppointments.Any(a =>
                a.Patient.PatientId == patientId &&
                a.ScheduledDate == date &&
                a.Status != AppointmentStatus.Cancelled);

            if (alreadyBooked)
            {
                throw new AppointmentConflictException(
                    "Patient already has an appointment with this doctor on the selected date.");
            }

            var appointment = new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                Status = AppointmentStatus.Pending
            };

            _appointmentRepository.Add(appointment);

            return appointment;
        }

        public Appointment ConfirmAppointment(int appointmentId)
        {
            Appointment appointment = _appointmentRepository.GetById(appointmentId);

            appointment.Confirm();

            _appointmentRepository.Update(appointment);

            return appointment;
        }

        public Appointment CancelAppointment(int appointmentId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Cancellation reason is required.");
            }

            Appointment appointment = _appointmentRepository.GetById(appointmentId);

            appointment.Cancel(reason);

            _appointmentRepository.Update(appointment);

            return appointment;
        }

        public Appointment CompleteAppointment(int appointmentId)
        {
            Appointment appointment = _appointmentRepository.GetById(appointmentId);

            appointment.Complete();

            _appointmentRepository.Update(appointment);

            return appointment;
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            _patientRepository.GetById(patientId);

            return _appointmentRepository
                .GetByPatientId(patientId)
                .OrderBy(a => a.ScheduledDate)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            _doctorRepository.GetById(doctorId);

            return _appointmentRepository
                .GetByDoctorId(doctorId)
                .OrderBy(a => a.ScheduledDate)
                .ToList();
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            DateOnly today = SystemTime.Now;

            return _appointmentRepository
                .GetAll()
                .Where(a =>
                    a.Status == AppointmentStatus.Confirmed &&
                    a.ScheduledDate >= today)
                .OrderBy(a => a.ScheduledDate)
                .ToList();
        }

        public List<Appointment> GetPendingAppointmentsByPatient(int patientId)
        {
            _patientRepository.GetById(patientId);

            return _appointmentRepository
                .GetByPatientId(patientId)
                .Where(a => a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ToList();
        }

        public List<Appointment> GetTodayConfirmedAppointmentsByDoctor(int doctorId)
        {
            _doctorRepository.GetById(doctorId);

            DateOnly today = SystemTime.Now;

            return _appointmentRepository
                .GetByDoctorId(doctorId)
                .Where(a =>
                    a.Status == AppointmentStatus.Confirmed &&
                    a.ScheduledDate == today)
                .OrderBy(a => a.ScheduledDate)
                .ToList();
        }
    }
}
