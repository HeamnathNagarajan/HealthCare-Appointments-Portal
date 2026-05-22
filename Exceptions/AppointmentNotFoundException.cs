namespace HealthcareApp.Exceptions
{
    public class AppointmentNotFoundException : Exception
    {
        public AppointmentNotFoundException(int appointmentId)
            : base($"Invalid Appointment ID: {appointmentId}")
        {
        }
    }
}

