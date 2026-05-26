using HealthcareApp.Models;
using HealthcareApp.Services;
using HealthcareApp.Utilities;

namespace HealthcareApp.Controllers
{
    public class HealthRecordController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordController(
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        public void CompleteAppointmentAndAddRecord()
        {
            Console.WriteLine("\n========== Complete Appointment And Add Health Record ==========");

            int doctorId = Validations.ReadPositiveInt("Enter Doctor ID: ", "Doctor ID");

            List<Appointment> todayConfirmedAppointments =
                _appointmentService.GetTodayConfirmedAppointmentsByDoctor(doctorId);

            if (todayConfirmedAppointments.Count == 0)
            {
                Console.WriteLine("No confirmed appointments found for this doctor today.");
                return;
            }

            ConsoleSelectionHelper.DisplayAppointments(
                $"\nConfirmed Appointments For Today ({SystemTime.Now:yyyy-MM-dd}):",
                todayConfirmedAppointments
            );

            int appointmentId = ConsoleSelectionHelper.ReadAppointmentIdFromDisplayedList(
                todayConfirmedAppointments,
                "Please choose an Appointment ID from the displayed confirmed appointments."
            );

            if (appointmentId == 0)
            {
                return;
            }

            Console.WriteLine("\nEnter Health Record Details");

            string diagnosis = Validations.ReadRequiredString("Diagnosis: ", "Diagnosis", 1);

            string prescription = Validations.ReadRequiredString("Prescription: ", "Prescription", 1);

            Console.Write("Notes: ");
            string? notes = Console.ReadLine();

            Appointment completedAppointment =
                _appointmentService.CompleteAppointment(appointmentId);

            Console.WriteLine("\nAppointment marked as completed.");
            Console.WriteLine(completedAppointment.GetDetails());

            HealthRecord record = _healthRecordService.AddRecord(
                appointmentId,
                diagnosis,
                prescription,
                notes
            );

            Console.WriteLine("\nHealth record added successfully.");
            Console.WriteLine(record.GetSummary());
        }

        public void ViewHealthHistory()
        {
            Console.WriteLine("\n========== View Health History ==========");

            int patientId = Validations.ReadPositiveInt("Enter Patient ID: ", "Patient ID");

            List<HealthRecord> records =
                _healthRecordService.GetRecordsByPatient(patientId);

            Console.WriteLine("\nHealth History:");

            foreach (HealthRecord record in records)
            {
                ConsoleTableHelper.DisplayHealthRecords(records);

            }
        }
    }
}