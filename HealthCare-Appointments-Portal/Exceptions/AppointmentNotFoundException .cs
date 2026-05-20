namespace HealthCare_Appointment_Portal.Exceptions
{

    public class AppointmentNotFoundException : Exception
    {

        public AppointmentNotFoundException()
            : base("Appointment not found.") { }
    }
}
