namespace HealthCare_Appointments_Portal.Exceptions
{
    public class DuplicatePatientException : Exception
    {
        public DuplicatePatientException()
            : base("Patient already exists.")
        {
        }

        public DuplicatePatientException(string message)
            : base(message)
        {
        }
    }
}
