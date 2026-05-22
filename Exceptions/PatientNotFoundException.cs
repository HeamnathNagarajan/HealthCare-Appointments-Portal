namespace HealthcareApp.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(int patientId)
            : base($"Invalid Patient ID: {patientId}")
        {
        }
    }
}