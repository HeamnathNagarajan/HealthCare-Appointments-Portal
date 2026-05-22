using HealthcareApp.Enums;
using System;

namespace HealthcareApp.Dtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; }

        public AppointmentStatus Status { get; set; }

        public string CancellationReason { get; set; }

        public string GetDetails()
        {
            string details =
                $"Appointment ID: {AppointmentId} | Patient: {PatientName} | Doctor: Dr. {DoctorName} | " +
                $"Date: {ScheduledDate:yyyy-MM-dd} | Slot: {TimeSlot} | Status: {Status}";

            if (Status == AppointmentStatus.Cancelled)
            {
                details += $" | Reason: {CancellationReason}";
            }

            return details;
        }
    }
}