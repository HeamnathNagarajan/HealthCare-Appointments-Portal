using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Models
{
    [ExcludeFromCodeCoverage]
    public class Doctor
    {
        // Auto Increment Doctor Id
        private static int _doctorCounter = 1;

        // Unique Doctor Identifier
        public int DoctorId { get; set; } = _doctorCounter++;

        // Doctor Full Name
        [Required(
            ErrorMessage = Constants.FullNameRequired)]
        [RegularExpression(
            @"^[a-zA-Z.\s]+$",
            ErrorMessage = Constants.InvalidFullNameFormat)]
        public string FullName { get; set; }
            = string.Empty;

        // Doctor Specialisation
        [Required(
            ErrorMessage = Constants.SpecialisationRequired)]
        public Specialisation Specialisation { get; set; }

        // Years Of Experience
        [Range(
            0,
            50,
            ErrorMessage = Constants.InvalidExperienceRange)]
        public int YearsOfExperience { get; set; }

        // Consultation Fee
        [Range(
            0,
            10000,
            ErrorMessage = Constants.InvalidConsultationFee)]
        public decimal ConsultationFee { get; set; }

        // Doctor Availability Status
        public bool IsActive { get; set; }

        // Doctor Appointments
        public List<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        // Check Doctor Availability
        public bool IsAvailable(DateOnly date)
        {
            int appointmentCount =
                Appointments.Count(a =>
                    a.ScheduledDate == date &&
                    a.Status != AppointmentStatus.Cancelled);

            return appointmentCount < 10;
        }

        // Return Doctor Schedule Summary
        public string GetScheduleSummary()
        {
            DateOnly today =
                DateOnly.FromDateTime(DateTime.Now);

            int upcomingAppointments =
                Appointments.Count(a =>
                    a.Status ==
                    AppointmentStatus.Confirmed &&
                    a.ScheduledDate >= today);

            return string.Format(
                Constants.DoctorScheduleSummaryFormat,
                FullName,
                upcomingAppointments);
        }
        // Return Doctor Profile Summary
        public string GetDoctorSummary()
        {

            return string.Format(
                Constants.DoctorProfileSummaryFormat,
                DoctorId,
                FullName,
                Specialisation,
                YearsOfExperience,
                ConsultationFee,
                IsActive
                    ? Constants.Available
                    : Constants.NotAvailable);
        }
    }
}
