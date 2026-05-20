namespace HealthCare_Appointment_Portal.Exceptions
{

    public class InvalidAppointmentStatusException : Exception
    {

        public InvalidAppointmentStatusException()
            : base("Invalid appointment status.") { }
    }
}
