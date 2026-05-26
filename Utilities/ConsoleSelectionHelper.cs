using HealthcareApp.Models;

namespace HealthcareApp.Utilities
{
    public static class ConsoleSelectionHelper
    {
        public static void DisplayAppointments(string heading, List<Appointment> appointments)
        {
            Console.WriteLine(heading);

            ConsoleTableHelper.DisplayAppointments(appointments);
        }

        public static int ReadAppointmentIdFromDisplayedList(
            List<Appointment> appointments,
            string invalidSelectionMessage)
        {
            Console.WriteLine("\nEnter 0 to cancel and return to the main menu.");

            int appointmentId = Validations.ReadIntInRange(
                "Enter Appointment ID: ",
                "Appointment ID",
                0,
                int.MaxValue
            );

            if (appointmentId == 0)
            {
                Console.WriteLine("Returning to main menu...");
                return 0;
            }

            bool appointmentExistsInList =
                appointments.Exists(a => a.AppointmentId == appointmentId);

            while (!appointmentExistsInList)
            {
                Console.WriteLine(invalidSelectionMessage);
                Console.WriteLine("Enter 0 to cancel and return to the main menu.");

                appointmentId = Validations.ReadIntInRange(
                    "Enter Appointment ID: ",
                    "Appointment ID",
                    0,
                    int.MaxValue
                );

                if (appointmentId == 0)
                {
                    Console.WriteLine("Returning to main menu...");
                    return 0;
                }

                appointmentExistsInList =
                    appointments.Exists(a => a.AppointmentId == appointmentId);
            }

            return appointmentId;
        }
    }
}