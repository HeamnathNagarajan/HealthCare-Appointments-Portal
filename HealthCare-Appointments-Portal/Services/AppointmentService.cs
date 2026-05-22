using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Exceptions;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Services
{

    public class AppointmentService : IAppointmentService
    {

        private readonly IAppointmentRepository
            _appointmentRepository;

        // Dependency Injection
        public AppointmentService(IAppointmentRepository
            appointmentRepository)
        {

            _appointmentRepository =
                appointmentRepository;
        }

        // Book New Appointment
        public Appointment BookAppointment(
            Patient patient,
            Doctor doctor,
            DateOnly date,
            TimeOnly slot)
        {

            if (date <
                DateOnly.FromDateTime(
                    DateTime.Now))
            {

                throw new PastDateException();
            }

            if (!doctor.IsAvailable(date))
            {

                throw new DoctorUnavailableException();
            }

            Appointment? existingAppointment =
                _appointmentRepository
                .GetAllAppointments()
                .FirstOrDefault(a =>
                    a.Doctor.DoctorId ==
                    doctor.DoctorId &&
                    a.ScheduledDate == date &&
                    a.TimeSlot == slot &&
                    a.Status !=
                    AppointmentStatus.Cancelled);

            if (existingAppointment != null)
            {

                throw new AppointmentConflictException();
            }

            Appointment appointment =
                new Appointment
                {

                    Patient = patient,
                    Doctor = doctor,
                    ScheduledDate = date,
                    TimeSlot = slot,
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepository
                .AddAppointment(appointment);

            return appointment;
        }

        // Get Appointment By Id
        public Appointment? GetAppointmentById(
            int appointmentId)
        {

            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    appointmentId);

            if (appointment == null)
            {

                throw new AppointmentNotFoundException();
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
                .GetAllAppointments()
                .Where(a =>
                    a.Patient.PatientId ==
                    patientId)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToList();
        }

        // Get Appointments By Doctor
        public List<Appointment>
            GetAppointmentsByDoctor(
                int doctorId)
        {

            return _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                    a.Doctor.DoctorId ==
                    doctorId)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToList();
        }

        // Get Upcoming Appointments
        public List<Appointment>
            GetUpcomingAppointments()
        {

            DateOnly today =
                DateOnly.FromDateTime(
                    DateTime.Now);

            return _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                    a.ScheduledDate >= today &&
                    a.Status ==
                    AppointmentStatus.Confirmed)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToList();
        }

        // Get Completed Appointments
        public List<Appointment> GetCompletedAppointments()
        {

            return _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                    a.Status ==
                    AppointmentStatus.Completed)
                .ToList();
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

                throw new AppointmentNotFoundException();
            }

            appointment.Confirm();

            _appointmentRepository
                .UpdateAppointment(
                    appointment);
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

                throw new AppointmentNotFoundException();
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

                throw new AppointmentNotFoundException();
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

                throw new AppointmentNotFoundException();
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

                throw new AppointmentNotFoundException();
            }

            _appointmentRepository
                .DeleteAppointmentById(
                    appointmentId);
        }
    }
}