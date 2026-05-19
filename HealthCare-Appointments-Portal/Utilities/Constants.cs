namespace HealthCare_Appointment_Portals.Utilities
{
    public class Constants
    {

        public const string InvalidPhoneNumberFormat = "Invalid phone number format";

        public const string InvalidEmailFormat = "Invalid email format";

        public const string PatientProfileSummaryFormat = "ID: {0} | Name: {1} | Age: {2} | Phone: {3}";

        public const string DoctorScheduleSummaryFormat = "{0} has {1} upcoming appointments.";

        public const string AppointmentDetailsFormat = "AppointmentId: {0} | Patient: {1} | Doctor: {2} | Date: {3:dd-MM-yyyy} | Time: {4} | Status: {5}";

        public const string HealthRecordSummaryFormat = "VisitDate: {0:dd-MM-yyyy} | Patient: {1} | Doctor: {2} | Diagnosis: {3} | Prescription: {4} | Notes: {5} ";
    }
}
