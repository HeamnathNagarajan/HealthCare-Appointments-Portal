namespace HealthCare_Appointment_Portal.Exceptions
{

    public class DuplicateDoctorException : Exception
    {

        public DuplicateDoctorException()
            : base("Doctor already exists.") { }
    }
}