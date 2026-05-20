namespace HealthCare_Appointment_Portal.Exceptions
{

    public class PatientNotFoundException : Exception
    {

        public PatientNotFoundException()
            : base("Patient not found.") { }
    }
}
