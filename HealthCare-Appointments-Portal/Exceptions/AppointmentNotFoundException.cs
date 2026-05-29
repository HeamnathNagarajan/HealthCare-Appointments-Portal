namespace HealthCare_Appointments_Portal.Exceptions
{

    public class AppointmentNotFoundException : Exception
    {

        public AppointmentNotFoundException()
            : base("Appointment not found.") { }
    }
}