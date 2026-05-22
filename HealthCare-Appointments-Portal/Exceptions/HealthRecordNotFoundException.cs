namespace HealthCare_Appointments_Portal.Exceptions
{

    public class HealthRecordNotFoundException : Exception
    {

        public HealthRecordNotFoundException()
            : base("Health record not found.") { }
    }
}