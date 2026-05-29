namespace HealthCare_Appointments_Portal.Exceptions
{

    public class DoctorUnavailableException : Exception
    {

        public DoctorUnavailableException()
            : base("Doctor is unavailable.")
        {
        }
    }
}