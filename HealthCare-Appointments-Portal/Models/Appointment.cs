using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using HealthCare_Appointments_Portal.Utilities;

namespace HealthCare_Appointments_Portal.Models;

public class Appointment
{
    // Auto Increment Appointment Id
    private static int _appointmentCounter = 1;

    // Unique Appointment Identifier
    public int AppointmentId { get; set; } = _appointmentCounter++;

    // Patient Information
    [Required(
        ErrorMessage = Constants.PatientRequired)]
    public required Patient Patient { get; set; }

    // Doctor Information
    [Required(
        ErrorMessage = Constants.DoctorRequired)]
    public required Doctor Doctor { get; set; }

    // Appointment Scheduled Date
    [Required(
        ErrorMessage = Constants.ScheduledDateRequired)]
    public DateOnly ScheduledDate { get; set; }

    // Appointment Time Slot
    [Required(
        ErrorMessage = Constants.TimeSlotRequired)]
    public TimeOnly TimeSlot { get; set; }

    // Appointment Status
    [Required(
        ErrorMessage = Constants.AppointmentStatusRequired)]
    public AppointmentStatus Status { get; set; }

    // Cancellation Reason
    public string CancellationReason { get; set; }
        = string.Empty;

    // Confirm Appointment
    public void Confirm()
    {
        Status =
            AppointmentStatus.Confirmed;
    }

    // Cancel Appointment
    public void Cancel(string reason)
    {
        Status =
            AppointmentStatus.Cancelled;

        CancellationReason =
            reason;
    }

    // Complete Appointment
    public void Complete()
    {
        Status =
            AppointmentStatus.Completed;
    }

    // Return Appointment Details
    public string GetDetails()
    {
        return string.Format(
            Constants.AppointmentDetailsFormat,
            AppointmentId,
            Patient.FullName,
            Doctor.FullName,
            ScheduledDate,
            TimeSlot,
            Status);
    }
}