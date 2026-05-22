namespace HealthcareApp.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(int doctorId)
            : base($"Invalid Doctor ID: {doctorId}")
        {
        }
    }
}