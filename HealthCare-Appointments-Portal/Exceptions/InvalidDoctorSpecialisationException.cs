namespace HealthCare_Appointment_Portal.Exceptions
{
    public class InvalidDoctorSpecialisationException
        : Exception
    {
        public InvalidDoctorSpecialisationException()
            : base(
                "Selected doctor does not belong to the chosen specialisation.")
        {
        }
    }
}
