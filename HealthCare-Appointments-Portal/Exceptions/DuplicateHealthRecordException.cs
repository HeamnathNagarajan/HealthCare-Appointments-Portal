namespace HealthCare_Appointment_Portal.Exceptions
{
    public class DuplicateHealthRecordException
        : Exception
    {
        public DuplicateHealthRecordException()
            : base(
                "Health record already exists for this appointment.")
        {
        }
    }
}