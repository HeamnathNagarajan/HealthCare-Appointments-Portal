using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;
using HealthcareApp.Repositories;
using HealthcareApp.Utilities;
using HealthcareApp.Dtos;

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

        public Appointment BookAppointment(int patientId, int doctorId, DateTime date, TimeSpan slotStartTime)
        {
            var now = SystemTime.Now;

            if (date.Date < now.Date)
                throw new PastDateException("Cannot book an appointment in the past.");

            if (!TimeSlots.DailySlotStartTimes.Contains(slotStartTime))
                throw new ArgumentException("Invalid time slot selected.");

            DateTime appointmentStartDateTime = GetSlotStartDateTime(date, slotStartTime);

            if (appointmentStartDateTime <= now)
                throw new PastDateException("Cannot book an appointment for a time that has already passed.");

            var patient = _patientRepository.GetById(patientId);
            var doctor = _doctorRepository.GetById(doctorId);

            if (!doctor.IsAvailable(date))
                throw new DoctorUnavailableException("Doctor is not available on the selected date.");

            bool patientAlreadyHasAppointmentWithDoctorThatDay = _appointmentRepository.GetAll().Any(a =>
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate.Date == date.Date &&
                a.Status != AppointmentStatus.Cancelled);

            if (patientAlreadyHasAppointmentWithDoctorThatDay)
            {
                throw new AppointmentConflictException(
                    "Patient already has an appointment with this doctor on the selected date.");
            }

            bool slotTaken = _appointmentRepository.GetAll().Any(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate.Date == date.Date &&
                a.SlotStartTime == slotStartTime &&
                a.Status != AppointmentStatus.Cancelled);

            if (slotTaken)
                throw new AppointmentConflictException("Selected time slot is already booked for this doctor.");

            var appointment = new Appointment
            {
                PatientId = patient.PatientId,
                DoctorId = doctor.DoctorId,
                ScheduledDate = date.Date,
                SlotStartTime = slotStartTime,
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
                .ThenBy(a => a.SlotStartTime)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            _doctorRepository.GetById(doctorId);

            return _appointmentRepository
                .GetByDoctorId(doctorId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotStartTime)
                .ToList();
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            var now = SystemTime.Now;

            return _appointmentRepository
                .GetAll()
                .Where(a =>
                    a.Status == AppointmentStatus.Confirmed &&
                    GetSlotStartDateTime(a.ScheduledDate, a.SlotStartTime) > now)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotStartTime)
                .ToList();
        }

        public List<TimeSpan> GetAvailableSlotsForDoctor(int doctorId, DateTime date)
        {
            var now = SystemTime.Now;

            if (date.Date < now.Date)
                throw new PastDateException("Cannot view available slots for a past date.");

            var doctor = _doctorRepository.GetById(doctorId);

            if (!doctor.IsAvailable(date))
                return new List<TimeSpan>();

            var bookedSlots = _appointmentRepository
                .GetByDoctorId(doctorId)
                .Where(a =>
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.SlotStartTime)
                .ToList();

            var availableSlots = TimeSlots.DailySlotStartTimes
                .Except(bookedSlots)
                .ToList();

            if (date.Date == now.Date)
            {
                availableSlots = availableSlots
                    .Where(slotStartTime => GetSlotStartDateTime(date, slotStartTime) > now)
                    .ToList();
            }

            return availableSlots;
        }

        public List<Appointment> GetPendingAppointmentsByPatient(int patientId)
        {
            _patientRepository.GetById(patientId);

            return _appointmentRepository
                .GetByPatientId(patientId)
                .Where(a => a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotStartTime)
                .ToList();
        }

        public List<Appointment> GetTodayConfirmedAppointmentsByDoctor(int doctorId)
        {
            _doctorRepository.GetById(doctorId);

            DateTime today = SystemTime.Now.Date;

            return _appointmentRepository
                .GetByDoctorId(doctorId)
                .Where(a =>
                    a.Status == AppointmentStatus.Confirmed &&
                    a.ScheduledDate.Date == today)
                .OrderBy(a => a.SlotStartTime)
                .ToList();
        }

        private DateTime GetSlotStartDateTime(DateTime date, TimeSpan slotStartTime)
        {
            return date.Date.Add(slotStartTime);
        }
        private AppointmentDto BuildAppointmentDto(Appointment appointment)
        {
            var patient = _patientRepository.GetById(appointment.PatientId);
            var doctor = _doctorRepository.GetById(appointment.DoctorId);

            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientName = patient.FullName,
                DoctorName = doctor.FullName,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status,
                CancellationReason = appointment.CancellationReason
            };
        }
        public List<AppointmentDto> GetAppointmentSummariesByPatient(int patientId)
        {
            _patientRepository.GetById(patientId);

            return _appointmentRepository
                .GetByPatientId(patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotStartTime)
                .Select(BuildAppointmentDto)
                .ToList();
        }
        public List<AppointmentDto> GetPendingAppointmentSummariesByPatient(int patientId)
        {
            _patientRepository.GetById(patientId);

            return _appointmentRepository
                .GetByPatientId(patientId)
                .Where(a => a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotStartTime)
                .Select(BuildAppointmentDto)
                .ToList();
        }
        public List<AppointmentDto> GetTodayConfirmedAppointmentSummariesByDoctor(int doctorId)
        {
            _doctorRepository.GetById(doctorId);

            DateTime today = SystemTime.Now.Date;

            return _appointmentRepository
                .GetByDoctorId(doctorId)
                .Where(a =>
                    a.Status == AppointmentStatus.Confirmed &&
                    a.ScheduledDate.Date == today)
                .OrderBy(a => a.SlotStartTime)
                .Select(BuildAppointmentDto)
                .ToList();
        }
        public AppointmentDto GetAppointmentSummaryById(int appointmentId)
        {
            var appointment = _appointmentRepository.GetById(appointmentId);

            return BuildAppointmentDto(appointment);
        }


    }
}