using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Services
{
    public class AppointmentService
        : IAppointmentService
    {
        private readonly
            IAppointmentRepository
            _appointmentRepository;

        // Dependency Injection
        public AppointmentService(
            IAppointmentRepository
            appointmentRepository)
        {
            _appointmentRepository =
                appointmentRepository;
        }

        // Book New Appointment
        public Appointment
            BookAppointment(
                Patient patient,
                Doctor doctor,
                Specialisation specialisation,
                DateOnly date,
                TimeOnly slot)
        {
            if (doctor.Specialisation !=
                specialisation)
            {
                throw new
                    InvalidDoctorSpecialisationException();
            }

            DateOnly currentDate =
                DateOnly.FromDateTime(
                    DateTime.Now);

            TimeOnly currentTime =
                TimeOnly.FromDateTime(
                    DateTime.Now);

            // Past Date Check
            if (date < currentDate)
            {
                throw new
                    PastDateException();
            }

            // Maximum Advance Booking Date
            DateOnly maxBookingDate =
                currentDate.AddMonths(6);

            if (date > maxBookingDate)
            {
                throw new
                    AdvanceBookingLimitException();
            }

            // Same Day Past Time Check
            if (date == currentDate
                &&
                slot < currentTime)
            {
                throw new
                    PastTimeSlotException();
            }

            if (!doctor.IsAvailable(date))
            {
                throw new
                    DoctorUnavailableException();
            }

            Appointment?
                existingAppointment =
                _appointmentRepository
                .GetConflictingAppointment(
                    doctor.DoctorId,
                    date,
                    slot);

            if (existingAppointment != null)
            {
                throw new
                    AppointmentConflictException();
            }

            Appointment appointment =
                new Appointment
                {
                    Patient = patient,

                    Doctor = doctor,

                    ScheduledDate = date,

                    TimeSlot = slot,

                    Status =
                        AppointmentStatus
                            .Pending
                };

            doctor.Appointments
                .Add(appointment);

            _appointmentRepository
                .AddAppointment(
                    appointment);

            return appointment;
        }

        // Get Appointment By Id
        public Appointment?
            GetAppointmentById(
                int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    appointmentId);

            if (appointment == null)
            {
                throw new
                    AppointmentNotFoundException();
            }

            return appointment;
        }

        // Get All Appointments
        public List<Appointment>
            GetAllAppointments()
        {
            return _appointmentRepository
                .GetAllAppointments();
        }

        // Get Appointments By Patient
        public List<Appointment>
            GetAppointmentsByPatient(
                int patientId)
        {
            return _appointmentRepository
                .GetAppointmentsByPatient(
                    patientId);
        }

        // Get Appointments By Doctor
        public List<Appointment>
            GetAppointmentsByDoctor(
                int doctorId)
        {
            return _appointmentRepository
                .GetAppointmentsByDoctor(
                    doctorId);
        }

        // Get Upcoming Appointments
        public List<Appointment>
            GetUpcomingAppointments()
        {
            return _appointmentRepository
                .GetUpcomingAppointments();
        }

        // Get Completed Appointments
        public List<Appointment>
            GetCompletedAppointments()
        {
            return _appointmentRepository
                .GetCompletedAppointments();
        }

        // Confirm Appointment
        public void ConfirmAppointment(
            int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    appointmentId);

            if (appointment == null)
            {
                throw new
                    AppointmentNotFoundException();
            }

            // Allow only Pending appointments
            if (appointment.Status !=
                AppointmentStatus.Pending)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants
                        .ConfirmOnlyPending);
            }

            appointment.Confirm();

            _appointmentRepository
                .UpdateAppointment(
                    appointment);

            // Call Doctor Method
            string summary =
                appointment.Doctor
                .GetScheduleSummary();

            Console.WriteLine(summary);
        }

        // Cancel Appointment
        public void CancelAppointment(
            int appointmentId,
            string reason)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    appointmentId);

            if (appointment == null)
            {
                throw new
                    AppointmentNotFoundException();
            }

            // Allow only Pending
            // or Confirmed appointments
            if (appointment.Status !=
                AppointmentStatus.Pending
                &&
                appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants
                        .CancelOnlyPendingOrConfirmed);
            }

            appointment.Cancel(reason);

            _appointmentRepository
                .UpdateAppointment(
                    appointment);
        }

        // Complete Appointment
        public void CompleteAppointment(
            int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    appointmentId);

            if (appointment == null)
            {
                throw new
                    AppointmentNotFoundException();
            }

            // Allow only Confirmed appointments
            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants
                        .CompleteOnlyConfirmed);
            }

            appointment.Complete();

            _appointmentRepository
                .UpdateAppointment(
                    appointment);
        }

        // Update Existing Appointment
        public void UpdateAppointment(
            Appointment updatedAppointment)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    updatedAppointment
                    .AppointmentId);

            if (appointment == null)
            {
                throw new
                    AppointmentNotFoundException();
            }

            _appointmentRepository
                .UpdateAppointment(
                    updatedAppointment);
        }

        // Delete Appointment By Id
        public void DeleteAppointmentById(
            int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    appointmentId);

            if (appointment == null)
            {
                throw new
                    AppointmentNotFoundException();
            }

            // Prevent Delete
            if (appointment.Status ==
                AppointmentStatus.Pending
                ||
                appointment.Status ==
                AppointmentStatus.Confirmed)
            {
                throw new
                    AppointmentDeletionException();
            }

            _appointmentRepository
                .DeleteAppointmentById(
                    appointmentId);
        }
    }
}
