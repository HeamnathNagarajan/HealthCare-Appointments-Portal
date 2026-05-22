using System.ComponentModel.DataAnnotations;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Models
{
    public class Patient
    {
        // Auto Increment Patient Id
        private static int _patientCounter = 1;

        // Unique Patient Identifier
        public int PatientId { get; set; } = _patientCounter++;

        // Patient Full Name
        [Required(
            ErrorMessage = Constants.FullNameRequired)]
        [RegularExpression(
            @"^[a-zA-Z\s]+$",
            ErrorMessage = Constants.InvalidFullNameFormat)]
        public string FullName { get; set; }
            = string.Empty;

        // Patient Date Of Birth
        [Required(
            ErrorMessage = Constants.DateOfBirthRequired)]

        [CustomValidation(
            typeof(Patient),
            nameof(ValidateDateOfBirth))]
        public DateOnly DateOfBirth { get; set; }

        // Patient Gender
        [Required(
            ErrorMessage = Constants.GenderRequired)]
        public Gender Gender { get; set; }

        // Patient Phone Number
        [Required(
            ErrorMessage = Constants.PhoneNumberRequired)]
        [RegularExpression(
            @"^\d{10}$",
            ErrorMessage = Constants.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; }
            = string.Empty;

        // Patient Email Address
        [Required(
            ErrorMessage = Constants.EmailRequired)]
        [EmailAddress(
            ErrorMessage = Constants.InvalidEmailFormat)]
        public string Email { get; set; }
            = string.Empty;

        // Patient Insurance Identifier
        [Required(
            ErrorMessage = Constants.InsuranceIdRequired)]
        public string InsuranceId { get; set; }
            = string.Empty;

        // Record Creation Date
        public DateTime CreatedDate { get; private set; }
            = DateTime.Now;

        // Calculate Patient Age
        public int GetAge()
        {
            DateOnly today =
                DateOnly.FromDateTime(DateTime.Now);

            int age =
                today.Year - DateOfBirth.Year;

            if (today <
                DateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }

        // Return Patient Profile Summary
        public string GetProfileSummary()
        {
            return string.Format(
                Constants.PatientProfileSummaryFormat,
                PatientId,
                FullName,
                GetAge(),
                PhoneNumber);
        }

        // Validate Date Of Birth
        public static ValidationResult?
            ValidateDateOfBirth(
                DateOnly date,
                ValidationContext context)
        {
            if (date >
                DateOnly.FromDateTime(
                    DateTime.Now))
            {
                return new ValidationResult(
                    Constants.DateOfBirthCannotBeFuture);
            }

            return ValidationResult.Success;
        }
    }
}