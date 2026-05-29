namespace HealthCare_Appointments_Portal.Exceptions
{
    public class PatientDeletionException : Exception
    {
        public PatientDeletionException()
            : base(
                "Patient has confirmed appointments and cannot be deleted.")
        {
        }
    }
}