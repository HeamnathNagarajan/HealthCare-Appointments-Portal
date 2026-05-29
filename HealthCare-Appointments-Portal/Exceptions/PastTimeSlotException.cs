namespace HealthCare_Appointments_Portal.Exceptions
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