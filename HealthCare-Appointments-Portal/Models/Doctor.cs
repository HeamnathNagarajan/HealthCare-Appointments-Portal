namespace HealthCare_Appointments_Portal.Models
{
    public class Doctor
    {
        public Guid DoctorId { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;

        public Specialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public bool IsAvailable(DateOnly date)
        {
            int appointmentCount = Appointments.Count(a =>
            a.ScheduledDate == date &&
            a.Status != AppointmentStatus.Cancelled);

            return appointmentCount < 10;
        }

        public string GetScheduleSummary()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int upcomingAppointments = Appointments.Count(a =>
                a.Status == AppointmentStatus.Confirmed &&
                a.ScheduledDate >= today);

            return string.Format(
                Constants.DoctorScheduleSummaryFormat,
                FullName,
                upcomingAppointments);
        }
    }
}
