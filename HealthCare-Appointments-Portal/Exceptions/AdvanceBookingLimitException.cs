namespace HealthCare_Appointment_Portal.Exceptions
{
    public class AdvanceBookingLimitException
        : Exception
    {
        public AdvanceBookingLimitException()
            : base(
                "Appointments can only be booked within 6 months from today.")
        {
        }
    }
}
