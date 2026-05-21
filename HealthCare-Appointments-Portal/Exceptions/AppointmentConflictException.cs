namespace HealthCare_Appointments_Portal.Exceptions
{
    public class AppointmentConflictException : Exception
    {
        public AppointmentConflictException()
            : base("Appointment slot already taken.")
        {
        }
    }
}
