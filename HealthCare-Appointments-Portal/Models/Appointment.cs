using HealthCare_Appointments_Portals.Utilities;
using HealthCare_Appointments_Portal.Enum;
namespace HealthCare_Appointments_Portal.Models;

public class Appointment
{

    public Guid AppointmentId { get; set; } = Guid.NewGuid();

    public int DisplayId { get; set; }

    public required Patient Patient { get; set; } 

    public required Doctor Doctor { get; set; }

    public DateOnly ScheduledDate { get; set; }

    public TimeOnly TimeSlot { get; set; }

    public AppointmentStatus Status { get; set; }

    public string CancellationReason { get; set; } = string.Empty;

    public void Confirm()
    {

        Status = AppointmentStatus.Confirmed;
    }

    public void Cancel(string reason)
    {

        Status = AppointmentStatus.Cancelled;
        CancellationReason = reason;
    }

    public void Complete()
    {

        Status = AppointmentStatus.Completed;
    }

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