namespace HealthcareApp.Exceptions
{
    public class PastDateException : Exception
    {
        public PastDateException(string message)
            : base(message)
        {
        }
    }
}