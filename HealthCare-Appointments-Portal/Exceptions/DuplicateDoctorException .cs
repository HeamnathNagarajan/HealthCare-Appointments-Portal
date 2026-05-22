namespace HealthCare_Appointments_Portal.Exceptions
{

    public class DuplicateDoctorException : Exception
    {

        public DuplicateDoctorException()
            : base("Doctor already exists.") { }
    }
}