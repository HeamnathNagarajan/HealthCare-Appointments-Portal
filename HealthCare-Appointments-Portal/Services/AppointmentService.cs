using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Exceptions;

namespace HealthCare_Appointments_Portal.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        // Dependency Injection
        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        // Book Appointment
        public Appointment BookAppointment(
            Patient patient,
            Doctor doctor,
            DateOnly date,
            TimeOnly slot)
        {
            if (date < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new PastDateException();
            }

            if (!doctor.IsAvailable(date))
            {
                throw new DoctorUnavailableException();
            }

            bool slotExists = _repository
                .GetAllAppointments()
                .Any(a =>
                    a.Doctor.DoctorId == doctor.DoctorId &&
                    a.ScheduledDate == date &&
                    a.TimeSlot == slot &&
                    a.Status != AppointmentStatus.Cancelled);

            if (slotExists)
            {
                throw new AppointmentConflictException();
            }

            Appointment appointment = new()
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                TimeSlot = slot,
                Status = AppointmentStatus.Pending
            };

            _repository.AddAppointment(appointment);
            doctor.Appointments.Add(appointment);

            return appointment;
        }

        // Cancel Appointment
        public void CancelAppointment(Guid appointmentId, string reason)
        {
            Appointment? appointment = _repository.GetAppointmentById(appointmentId);

            if (appointment != null)
            {
                appointment.Cancel(reason);
            }
        }

        // Get Appointments By Doctor
        public List<Appointment> GetAppointmentsByDoctor(Guid doctorId)
        {
            return _repository
                .GetAllAppointments()
                .Where(a => a.Doctor.DoctorId == doctorId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        // Get Upcoming Confirmed Appointments
        public List<Appointment> GetUpcomingAppointments()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            return _repository
                .GetAllAppointments()
                .Where(a =>
                    (a.Status == AppointmentStatus.Pending ||
                     a.Status == AppointmentStatus.Confirmed) &&
                    a.ScheduledDate >= today)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        // Get Appointments By Patient
        public List<Appointment> GetAppointmentsByPatient(Guid patientId)
        {
            return _repository
                .GetAllAppointments()
                .Where(a =>
                    a.Patient.PatientId == patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        // Get Completed Appointments
        public List<Appointment> GetCompletedAppointments()
        {
            return _repository
                .GetAllAppointments()
                .Where(a => a.Status == AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

    }
}
    