using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointments_Portal.Models

{
    public class Patient
    {

        public Guid PatientId { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        
        public DateOnly DateOfBirth { get; set; }
        
        public string Gender { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        
        public string InsuranceId { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; private set; }


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
            return $"ID: {PatientId} | Name: {FullName} | Age: {GetAge()} | Phone: {PhoneNumber}";

        }

    }
}
