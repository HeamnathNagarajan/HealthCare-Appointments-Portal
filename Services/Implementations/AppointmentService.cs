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

        // ✅ UPDATED: No time slots
        public Appointment BookAppointment(int patientId, int doctorId, DateOnly date)
        {
            var now = SystemTime.Now;

            if (date < now)
                throw new PastDateException("Cannot book an appointment in the past.");

            var patient = _patientRepository.GetById(patientId);
            var doctor = _doctorRepository.GetById(doctorId);

            // ✅ Get doctor's appointments
            var doctorAppointments = _appointmentRepository.GetByDoctorId(doctorId);

            // ✅ Use updated availability logic
            if (!doctor.IsAvailable(date, doctorAppointments))
            {
                throw new DoctorUnavailableException(
                    "Doctor is not available on the selected date.");
            }

            // ✅ Patient cannot double-book same doctor same day
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
            var appointment = _appointmentRepository.GetById(appointmentId);

            appointment.Confirm();

            _appointmentRepository.Update(appointment);

            return appointment;
        }

        public Appointment CancelAppointment(int appointmentId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Cancellation reason is required.");

            var appointment = _appointmentRepository.GetById(appointmentId);

            appointment.Cancel(reason);

            _appointmentRepository.Update(appointment);

            return appointment;
        }

        public Appointment CompleteAppointment(int appointmentId)
        {
            var appointment = _appointmentRepository.GetById(appointmentId);

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
            var now = SystemTime.Now;

            return _appointmentRepository
                .GetAll()
                .Where(a =>
                    a.Status == AppointmentStatus.Confirmed &&
                    a.ScheduledDate >= now)
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