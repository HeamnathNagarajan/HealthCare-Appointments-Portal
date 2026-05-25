using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using System;

namespace HealthcareApp.Models
{
    public class Appointment 
    {
        public int AppointmentId { get; set; }
         
        public required Patient Patient { get; set; }

        public required Doctor Doctor { get; set; }

        public DateOnly ScheduledDate { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string CancellationReason { get; set; }

        public void Confirm()
        {
            if (Status != AppointmentStatus.Pending)
                throw new InvalidAppointmentStatusException(
                    "Only pending appointments can be confirmed.");

            Status = AppointmentStatus.Confirmed;
        }

        public void Cancel(string reason)
        {
            if (Status == AppointmentStatus.Completed)
                throw new InvalidAppointmentStatusException(
                    "Completed appointments cannot be cancelled.");

            Status = AppointmentStatus.Cancelled;
            CancellationReason = reason;
        }

        public void Complete()
        {
            if (Status != AppointmentStatus.Confirmed)
                throw new InvalidAppointmentStatusException(
                    "Only confirmed appointments can be completed.");

            Status = AppointmentStatus.Completed;
        }

        public string GetDetails()
        {
            string details =
                $"Appointment ID: {AppointmentId} | Patient Name: {Patient.FullName} | Doctor Name: {Doctor.FullName} | " +
                $"Date: {ScheduledDate:yyyy-MM-dd} | Status: {Status}";

            if (Status == AppointmentStatus.Cancelled)
            {
                details += $" | Reason: {CancellationReason}";
            }

            return details;
        }
    }
}