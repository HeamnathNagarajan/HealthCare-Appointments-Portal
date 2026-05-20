namespace HealthCare_Appointment_Portal.Exceptions
{

    public class PastDateException : Exception
    {

        public PastDateException()
            : base("Appointment date cannot be in the past.")
        {
        }
    }
}
