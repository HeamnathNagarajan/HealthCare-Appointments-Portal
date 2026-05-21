namespace HealthCare_Appointments_Portal.Utilities
{
    public class Constants
    {


        public const string FullNameRequired =
            "Full Name is required.";

        public const string InvalidFullNameFormat =
            "Full Name must contain only alphabets and spaces.";

        public const string DateOfBirthRequired =
            "Date Of Birth is required.";

        public const string GenderRequired =
            "Gender is required.";

        public const string PhoneNumberRequired =
            "Phone Number is required.";

        public const string InvalidPhoneNumberFormat =
            "Phone Number must contain exactly 10 digits.";

        public const string EmailRequired =
            "Email is required.";

        public const string InvalidEmailFormat =
            "Please enter a valid email address.";

        public const string InsuranceIdRequired =
            "Insurance Id is required.";

        public const string PatientProfileSummaryFormat =
            "Patient ID: {0} | Name: {1} | Age: {2} | Phone: {3}";

        public const string DoctorScheduleSummaryFormat =
            "Dr. {0} has {1} upcoming appointment(s).";

        public const string AppointmentDetailsFormat =
            "Appointment ID: {0} | Patient: {1} | Doctor: {2} | Date: {3:dd-MM-yyyy} | Time: {4} | Status: {5}";

        public const string HealthRecordSummaryFormat =
            "Visit Date: {0:dd-MM-yyyy} | Patient: {1} | Doctor: {2} | Diagnosis: {3} | Prescription: {4} | Notes: {5}";
    }
}