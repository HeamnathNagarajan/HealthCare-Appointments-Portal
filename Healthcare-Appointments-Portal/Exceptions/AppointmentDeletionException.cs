namespace HealthCare_Appointment_Portal.Exceptions
{
    public class AppointmentDeletionException : Exception
    {
        public AppointmentDeletionException()
            : base(
                "Pending or confirmed appointments cannot be deleted.")
        {
        }
    }
}