using HealthcareApp.Data;
using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;
using HealthcareApp.Repositories;

namespace HealthcareApp.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments;
        private int _nextId;

        public AppointmentRepository(DataStore dataStore)
        {
            _appointments = dataStore.Appointments;

            _nextId = _appointments.Any()
                ? _appointments.Max(a => a.AppointmentId) + 1
                : 1;
        }

        public void Add(Appointment appointment)
        {
            appointment.AppointmentId = _nextId++;
            _appointments.Add(appointment);
        }

        public Appointment GetById(int appointmentId)
        {
            var appointment = _appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                throw new AppointmentNotFoundException(appointmentId);

            return appointment;
        }

        public List<Appointment> GetAll()
        {
            return _appointments.ToList();
        }

        public List<Appointment> GetByPatientId(int patientId)
        {
            return _appointments
                .Where(a => a.PatientId == patientId)
                .ToList();
        }

        public List<Appointment> GetByDoctorId(int doctorId)
        {
            return _appointments
                .Where(a => a.DoctorId == doctorId)
                .ToList();
        }

        public List<Appointment> GetByStatus(AppointmentStatus status)
        {
            return _appointments
                .Where(a => a.Status == status)
                .ToList();
        }

        public void Update(Appointment appointment)
        {
            var existingAppointment = GetById(appointment.AppointmentId);

            existingAppointment.PatientId = appointment.PatientId;
            existingAppointment.DoctorId = appointment.DoctorId;
            existingAppointment.ScheduledDate = appointment.ScheduledDate;
            existingAppointment.SlotStartTime = appointment.SlotStartTime;
            existingAppointment.Status = appointment.Status;
            existingAppointment.CancellationReason = appointment.CancellationReason;
        }
    }
}