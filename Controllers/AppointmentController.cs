using HealthcareApp.Enums;
using HealthcareApp.Models;
using HealthcareApp.Services;
using HealthcareApp.Utilities;

namespace HealthcareApp.Controllers
{
    public class AppointmentController
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        public void BookAppointment()
        {
            Console.WriteLine("\n========== Book Appointment ==========");

            int patientId = Validations.ReadPositiveInt(
                "Enter Patient ID: ",
                "Patient ID"
            );

            _patientService.GetPatientById(patientId);

            List<Doctor> matchingDoctors;
            Specialisation selectedSpecialisation;

            while (true)
            {
                Console.WriteLine("\nSelect the department/specialisation you want to visit:");

                selectedSpecialisation =
                    Validations.ReadEnumChoice<Specialisation>("specialisation");

                matchingDoctors =
                    _doctorService.SearchDoctorsBySpecialisation(selectedSpecialisation);

                if (matchingDoctors.Count == 0)
                {
                    Console.WriteLine($"\nNo active doctors available for {selectedSpecialisation}.");
                    Console.WriteLine("Please select another specialisation.");
                    continue;
                }

                break;
            }

            Console.WriteLine($"\nAvailable doctors for {selectedSpecialisation}:");

            foreach (Doctor doctor in matchingDoctors)
            {
                Console.WriteLine(doctor.GetDoctorSummary());
            }

            int doctorId;

            while (true)
            {
                doctorId = Validations.ReadPositiveInt(
                    "Enter Doctor ID from the above list: ",
                    "Doctor ID"
                );

                bool doctorExistsInDisplayedList =
                    matchingDoctors.Exists(d => d.DoctorId == doctorId);

                if (!doctorExistsInDisplayedList)
                {
                    Console.WriteLine("Please choose a Doctor ID from the displayed list.");
                    continue;
                }

                break;
            }

            DateOnly appointmentDate =
                Validations.ReadDate("Enter appointment date (yyyy-MM-dd): ");

            while (appointmentDate < SystemTime.Now)
            {
                Console.WriteLine("Appointment date cannot be in the past.");

                appointmentDate =
                    Validations.ReadDate("Enter appointment date again (yyyy-MM-dd): ");
            }

            Appointment appointment = _appointmentService.BookAppointment(
                patientId,
                doctorId,
                appointmentDate
            );

            Console.WriteLine("\nAppointment booked successfully.");
            Console.WriteLine(appointment.GetDetails());
        }

        public void ViewAppointmentsForPatient()
        {
            Console.WriteLine("\n========== View Appointments For Patient ==========");

            int patientId = Validations.ReadPositiveInt(
                "Enter Patient ID: ",
                "Patient ID"
            );

            List<Appointment> appointments =
                _appointmentService.GetAppointmentsByPatient(patientId);

            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments found for this patient.");
                return;
            }

            ConsoleSelectionHelper.DisplayAppointments(
                "\nAppointments:",
                appointments
            );
        }

        public void ConfirmOrCancelAppointment()
        {
            Console.WriteLine("\n========== Confirm Or Cancel Appointment ==========");

            int patientId = Validations.ReadPositiveInt(
                "Enter Patient ID: ",
                "Patient ID"
            );

            List<Appointment> pendingAppointments =
                _appointmentService.GetPendingAppointmentsByPatient(patientId);

            if (pendingAppointments.Count == 0)
            {
                Console.WriteLine("No pending appointments found for this patient.");
                return;
            }

            ConsoleSelectionHelper.DisplayAppointments(
                "\nPending Appointments:",
                pendingAppointments
            );

            int appointmentId =
                ConsoleSelectionHelper.ReadAppointmentIdFromDisplayedList(
                    pendingAppointments,
                    "Please choose an Appointment ID from the displayed pending appointments."
                );

            if (appointmentId == 0)
            {
                return;
            }

            Console.WriteLine("\n1. Confirm appointment");
            Console.WriteLine("2. Cancel appointment");
            Console.WriteLine("0. Return to main menu");

            int actionChoice = Validations.ReadIntInRange(
                "Choose an option: ",
                "Action choice",
                0,
                2
            );

            if (actionChoice == 0)
            {
                Console.WriteLine("Returning to main menu...");
                return;
            }

            if (actionChoice == 1)
            {
                Appointment confirmedAppointment =
                    _appointmentService.ConfirmAppointment(appointmentId);

                Console.WriteLine("\nAppointment confirmed successfully.");
                Console.WriteLine(confirmedAppointment.GetDetails());
            }
            else
            {
                string reason = Validations.ReadRequiredString(
                    "Enter cancellation reason: ",
                    "Cancellation reason",
                    1
                );

                Appointment cancelledAppointment =
                    _appointmentService.CancelAppointment(appointmentId, reason);

                Console.WriteLine("\nAppointment cancelled successfully.");
                Console.WriteLine(cancelledAppointment.GetDetails());
            }
        }
    }
}