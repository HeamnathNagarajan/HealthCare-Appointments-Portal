namespace HealthCare_Appointment_Portal.Exceptions
{
    public class PastTimeSlotException : Exception
    {
        public PastTimeSlotException()
            : base(
                "Time slot cannot be in the past.")
        {
        }
    }
}
