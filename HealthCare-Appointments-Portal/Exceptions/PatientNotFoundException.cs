namespace HealthCare_Appointments_Portal.Exceptions
{

    public class PatientNotFoundException : Exception
    {

        public PatientNotFoundException()
            : base("Patient not found.") { }
    }
}