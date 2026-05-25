using HealthcareApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareApp.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; }

        public Specialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public List<DayOfWeek> OffDays { get; set; } = new List<DayOfWeek>();

        public bool IsAvailable(DateOnly date, List<Appointment> appointments)
        {
            // 1. Active check
            if (!IsActive)
                return false;

            // 2. Off-day check
            if (OffDays.Contains(date.DayOfWeek))
                return false;

            // 3. Daily limit check (max 10)
            int appointmentCount = appointments.Count(a =>
                a.ScheduledDate == date &&
                a.Status != AppointmentStatus.Cancelled);

            return appointmentCount < 10;
        }

        public string GetDoctorSummary()
        {
            return $"Doctor ID: {DoctorId} | Dr. {FullName} | {Specialisation} | " +
                   $"Experience: {YearsOfExperience} years | Fee: {ConsultationFee}";
        }
    }
}