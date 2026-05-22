using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using System;

namespace HealthcareApp.Models
{
    public class Appointment : BaseEntity
    {
        public int AppointmentId
        {
            get => Id;
            set => Id = value;
        }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime => SlotStartTime.Add(TimeSpan.FromHours(1));

        public string TimeSlot => $"{SlotStartTime:hh\\:mm}-{SlotEndTime:hh\\:mm}";

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
                $"Appointment ID: {AppointmentId} | Patient ID: {PatientId} | Doctor ID: {DoctorId} | " +
                $"Date: {ScheduledDate:yyyy-MM-dd} | Slot: {TimeSlot} | Status: {Status}";

            if (Status == AppointmentStatus.Cancelled)
            {
                details += $" | Reason: {CancellationReason}";
            }

            return details;
        }
    }
}