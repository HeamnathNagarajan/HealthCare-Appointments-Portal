namespace HealthCare_Appointments_Portal.Exceptions
{
    public class InvalidAppointmentStatusException
        : Exception
    {
        public InvalidAppointmentStatusException(string message) : base(message) {}
    }
}