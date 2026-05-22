namespace HealthCare_Appointment_Portal.Exceptions
{

    public class AppointmentConflictException : Exception
    {

        public AppointmentConflictException()
            : base("Appointment slot already taken.")
        {
        }
    }
}