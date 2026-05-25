using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Controllers
{
    public class AppointmentController
    {
        private readonly IAppointmentService _appointmentService;

        private readonly IPatientService _patientService;

        private readonly IDoctorService _doctorService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _appointmentService =
                appointmentService;

            _patientService =
                patientService;

            _doctorService =
                doctorService;
        }

        // Book Appointment
        public void BookAppointment()
        {
            string patientEmail =
                UtilityHelper
                .ReadInput(
                    ConsoleConstants
                    .EnterPatientEmail);

            Patient existingPatient =
                _patientService
                .GetPatientByEmail(
                    patientEmail)!;

            Specialisation specialisation =
                UtilityHelper
                .ReadValidEnum<Specialisation>(
                    ConsoleConstants
                    .EnterSpecialisationChoice);

            List<Doctor> availableDoctors =
                _doctorService
                .GetDoctorsBySpecialisation(
                    specialisation);

            if (availableDoctors.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoDoctorsAvailable);

                return;
            }

            Console.WriteLine(
                ConsoleConstants
                .AvailableDoctors);

            foreach (Doctor doctor
                in availableDoctors)
            {
                Console.WriteLine(
                    doctor
                    .GetDoctorSummary());
            }

            int doctorId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterDoctorId);

            Doctor existingDoctor =
                _doctorService
                .GetDoctorById(
                    doctorId)!;

            Appointment appointmentModel = new()
            {
                Patient =
                    existingPatient,

                Doctor =
                    existingDoctor
            };

            DateOnly scheduledDate =
                UtilityHelper
                .ReadValidDate(
                    ConsoleConstants
                    .EnterAppointmentDate,
                    nameof(Appointment.ScheduledDate),
                    appointmentModel);

            TimeOnly timeSlot =
                UtilityHelper
                .ReadValidTime(
                    ConsoleConstants
                    .EnterTimeSlot);

            Appointment appointment =
                _appointmentService
                .BookAppointment(
                    existingPatient,
                    existingDoctor,
                    scheduledDate,
                    timeSlot);

            Console.WriteLine(
                ConsoleConstants
                .AppointmentBookedSuccessfully);

            Console.WriteLine(
                appointment
                .GetDetails());
        }

        // View All Appointments
        public void GetAllAppointments()
        {
            List<Appointment> appointments =
                _appointmentService
                .GetAllAppointments();

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoAppointmentsAvailable);

                return;
            }

            foreach (Appointment appointment
                in appointments)
            {
                Console.WriteLine(
                    appointment
                    .GetDetails());
            }
        }

        // Get Appointment By Id
        public void GetAppointmentById()
        {
            int appointmentId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterAppointmentId);

            Appointment appointment =
                _appointmentService
                .GetAppointmentById(
                    appointmentId)!;

            Console.WriteLine(
                appointment
                .GetDetails());
        }

        // Get Appointments By Patient
        public void GetAppointmentsByPatient()
        {
            int patientId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterPatientId);

            List<Appointment> appointments =
                _appointmentService
                .GetAppointmentsByPatient(
                    patientId);

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoAppointmentsFound);

                return;
            }

            foreach (Appointment appointment
                in appointments)
            {
                Console.WriteLine(
                    appointment
                    .GetDetails());
            }
        }

        // Get Appointments By Doctor
        public void GetAppointmentsByDoctor()
        {
            int doctorId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterDoctorId);

            List<Appointment> appointments =
                _appointmentService
                .GetAppointmentsByDoctor(
                    doctorId);

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoAppointmentsFound);

                return;
            }

            foreach (Appointment appointment
                in appointments)
            {
                Console.WriteLine(
                    appointment
                    .GetDetails());
            }
        }

        // Get Upcoming Appointments
        public void GetUpcomingAppointments()
        {
            List<Appointment> appointments =
                _appointmentService
                .GetUpcomingAppointments();

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoAppointmentsFound);

                return;
            }

            foreach (Appointment appointment
                in appointments)
            {
                Console.WriteLine(
                    appointment
                    .GetDetails());
            }
        }

        // Get Completed Appointments
        public void GetCompletedAppointments()
        {
            List<Appointment> appointments =
                _appointmentService
                .GetCompletedAppointments();

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoCompletedAppointmentsFound);

                return;
            }

            foreach (Appointment appointment
                in appointments)
            {
                Console.WriteLine(
                    appointment
                    .GetDetails());
            }
        }

        // Manage Appointment Status
        public void ManageAppointment()
        {
            List<Appointment> appointments =
                _appointmentService
                .GetAllAppointments();

            if (appointments.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoAppointmentsAvailable);

                return;
            }

            foreach (Appointment appointment
                in appointments)
            {
                Console.WriteLine(
                    appointment
                    .GetDetails());
            }

            int appointmentId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterAppointmentId);

            Console.WriteLine(
                ConsoleConstants
                .ConfirmAppointment);

            Console.WriteLine(
                ConsoleConstants
                .CancelAppointment);

            Console.WriteLine(
                ConsoleConstants
                .CompleteAppointment);

            int choice =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterChoice);

            switch (choice)
            {
                case 1:

                    _appointmentService
                        .ConfirmAppointment(
                            appointmentId);

                    Console.WriteLine(
                        ConsoleConstants
                        .AppointmentConfirmedSuccessfully);

                    break;

                case 2:

                    string reason =
                        UtilityHelper
                        .ReadInput(
                            ConsoleConstants
                            .EnterCancellationReason);

                    _appointmentService
                        .CancelAppointment(
                            appointmentId,
                            reason);

                    Console.WriteLine(
                        ConsoleConstants
                        .AppointmentCancelledSuccessfully);

                    break;

                case 3:

                    _appointmentService
                        .CompleteAppointment(
                            appointmentId);

                    Console.WriteLine(
                        ConsoleConstants
                        .AppointmentCompletedSuccessfully);

                    break;

                default:

                    Console.WriteLine(
                        ConsoleConstants
                        .InvalidChoice);

                    break;
            }
        }

        // Update Appointment
        public void UpdateAppointment()
        {
            int appointmentId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterAppointmentId);

            Appointment existingAppointment =
                _appointmentService
                .GetAppointmentById(
                    appointmentId)!;

            Console.WriteLine(
                ConsoleConstants
                .CurrentAppointmentDetails);

            Console.WriteLine(
                existingAppointment
                .GetDetails());

            Appointment updatedAppointment = new()
            {
                AppointmentId =
                    existingAppointment.AppointmentId,

                Patient =
                    existingAppointment.Patient,

                Doctor =
                    existingAppointment.Doctor,

                ScheduledDate =
                    UtilityHelper
                    .ReadOptionalDate(
                        ConsoleConstants.ScheduledDateLabel,
                        existingAppointment.ScheduledDate),

                TimeSlot =
                    UtilityHelper
                    .ReadOptionalTime(
                        ConsoleConstants.TimeSlotLabel,
                        existingAppointment.TimeSlot),

                Status =
                    UtilityHelper
                    .ReadOptionalEnum<AppointmentStatus>(
                        ConsoleConstants.AppointmentStatusLabel,
                        existingAppointment.Status),

                CancellationReason =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants.CancellationReasonLabel,
                        existingAppointment.CancellationReason
                        ?? string.Empty)
            };

            _appointmentService
                .UpdateAppointment(
                    updatedAppointment);

            Console.WriteLine(
                ConsoleConstants
                .AppointmentUpdatedSuccessfully);

            Console.WriteLine(
                ConsoleConstants
                .UpdatedAppointmentDetails);

            Console.WriteLine(
                _appointmentService
                .GetAppointmentById(
                    appointmentId)!
                .GetDetails());
        }

        // Delete Appointment
        public void DeleteAppointment()
        {
            int appointmentId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterAppointmentId);

            _appointmentService
                .DeleteAppointmentById(
                    appointmentId);

            Console.WriteLine(
                ConsoleConstants
                .AppointmentDeletedSuccessfully);
        }
    }
}
