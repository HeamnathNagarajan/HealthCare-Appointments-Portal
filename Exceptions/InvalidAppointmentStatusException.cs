namespace HealthcareApp.Exceptions
{
    public class InvalidAppointmentStatusException : Exception
    {
        public InvalidAppointmentStatusException(string message)
            : base(message)
        {
        }
    }
}