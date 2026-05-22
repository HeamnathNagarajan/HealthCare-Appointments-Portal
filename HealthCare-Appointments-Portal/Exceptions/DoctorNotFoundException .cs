namespace HealthCare_Appointments_Portal.Exceptions
{

    public class DoctorNotFoundException : Exception
    {

        public DoctorNotFoundException()
            : base("Doctor not found.") { }
    }
}