namespace HealthCare_Appointments_Portal.Exceptions
{
    public class DoctorDeletionException : Exception
    {
        public DoctorDeletionException()
            : base(
                "Doctor has confirmed appointments and cannot be deleted.")
        {
        }
    }
}