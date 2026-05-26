using HealthcareApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareApp.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public required string FullName { get; set; }

        public Specialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public List<DateOnly> OffDays { get; set; } = new List<DateOnly>();

        public bool IsAvailable(DateOnly date)
        {
            if (!IsActive)
            {
                return false;
            }

            return !OffDays.Contains(date);
        }

        public string GetDoctorSummary()
        {
            return $"Doctor ID: {DoctorId} | Dr. {FullName} | {Specialisation} | " +
                   $"Experience: {YearsOfExperience} years | Fee: {ConsultationFee}";
        }
    }
}