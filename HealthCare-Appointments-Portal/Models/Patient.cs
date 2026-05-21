using HealthCare_Appointments_Portal.Utilities;
using HealthCare_Appointments_Portal.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HealthCare_Appointments_Portal.Models

{
    public class Patient
    {

        public Guid PatientId { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [Phone(ErrorMessage = Constants.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = Constants.InvalidEmailFormat)]
        public string Email { get; set; } = string.Empty;

        public string InsuranceId { get; set; } = string.Empty;

        public DateTime CreatedDate { get; private set; } = DateTime.Now;


        // Calculate Age
        public int GetAge()
        {

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            int age = today.Year - DateOfBirth.Year;

            if (today < DateOfBirth.AddYears(age))
            {

                age--;
            }

            return age;

        }

        // Return Profile Summary
        public string GetProfileSummary()
        {
            return string.Format(
                            Constants.PatientProfileSummaryFormat,
                            PatientId,
                            FullName,
                            GetAge(),
                            PhoneNumber);
        }

    }
}