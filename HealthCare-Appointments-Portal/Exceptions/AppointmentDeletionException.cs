namespace HealthCare_Appointments_Portal.Exceptions
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