using HealthcareApp.Enums;
using System;
using System.Collections.Generic;

namespace HealthcareApp.Models
{
    public class Doctor : BaseEntity
    {
        public int DoctorId
        {
            get => Id;
            set => Id = value;
        }

        public string FullName { get; set; }

        public Specialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public List<DayOfWeek> OffDays { get; set; } = new List<DayOfWeek>();

        public bool IsAvailable(DateTime date)
        {
            if (!IsActive)
                return false;

            return !OffDays.Contains(date.DayOfWeek);
        }

        public string GetDoctorSummary()
        {
            return $"Doctor ID: {DoctorId} | Dr. {FullName} | {Specialisation} | " +
                   $"Experience: {YearsOfExperience} years | Fee: {ConsultationFee}";
        }
    }
}