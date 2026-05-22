using HealthcareApp.Enums;
using HealthcareApp.Utilities;
using HealthcareApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Models
{
    public class Patient : BaseEntity
    {
        public int PatientId
        {
            get => Id;
            set => Id = value;
        }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string InsuranceId { get; set; }

        public DateTime CreatedDate { get; set; }

        // -------- Methods --------

        public int GetAge()
        {
            var currentDate = SystemTime.Now;

            int age = currentDate.Year - DateOfBirth.Year;

            if (DateOfBirth > currentDate.AddYears(-age))
                age--;

            return age;
        }

        public string GetProfileSummary()
        {
            return $"Patient ID: {PatientId} | Name: {FullName} | Age: {GetAge()} | Gender: {Gender} | Phone: {PhoneNumber}";
        }
    }

}
