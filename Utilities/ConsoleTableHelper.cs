using HealthcareApp.Models;

namespace HealthcareApp.Utilities
{
    public static class ConsoleTableHelper
    {
        public static void DisplayAppointments(List<Appointment> appointments)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"{"ID",-5} {"Patient",-20} {"Doctor",-20} {"Date",-12} {"Status",-12}"
            );

            Console.WriteLine(new string('-', 105));

            foreach (Appointment appointment in appointments)
            {
                Console.WriteLine(
                    $"{appointment.AppointmentId,-5} " +
                    $"{Truncate(appointment.Patient.FullName, 20),-20} " +
                    $"{Truncate(appointment.Doctor.FullName, 20),-20} " +
                    $"{appointment.ScheduledDate,-12:yyyy-MM-dd} " +
                    $"{appointment.Status,-12} "                
                );
            }
        }

        public static void DisplayDoctors(List<Doctor> doctors)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"{"ID",-5} {"Name",-20} {"Specialisation",-20} {"Experience",-12} {"Fee",-10} {"Status",-12}"
            );

            Console.WriteLine(new string('-', 90));

            foreach (Doctor doctor in doctors)
            {
                string status = doctor.IsActive ? "Active" : "Inactive";

                Console.WriteLine(
                    $"{doctor.DoctorId,-5} " +
                    $"{Truncate(doctor.FullName, 20),-20} " +
                    $"{doctor.Specialisation,-20} " +
                    $"{doctor.YearsOfExperience,-12} " +
                    $"{doctor.ConsultationFee,-10} " +
                    $"{status,-12} "
                );
            }
        }

        public static void DisplayHealthRecords(List<HealthRecord> records)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"{"ID",-5} {"Patient",-20} {"Doctor",-20} {"Appt ID",-8} {"Visit Date",-12} {"Diagnosis",-20} {"Prescription",-25}"
            );

            Console.WriteLine(new string('-', 120));

            foreach (HealthRecord record in records)
            {
                Console.WriteLine(
                    $"{record.RecordId,-5} " +
                    $"{Truncate(record.Patient.FullName, 20),-20} " +
                    $"{Truncate(record.Doctor.FullName, 20),-20} " +
                    $"{record.AppointmentId,-8} " +
                    $"{record.VisitDate,-12:yyyy-MM-dd} " +
                    $"{Truncate(record.Diagnosis, 20),-20} " +
                    $"{Truncate(record.Prescription, 25),-25} "
                );
            }
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "-";
            }

            if (value.Length <= maxLength)
            {
                return value;
            }

            if (maxLength <= 3)
            {
                return value.AsSpan(0, maxLength).ToString();
            }

            return string.Concat(value.AsSpan(0, maxLength - 3), "...");
        }
    }
}