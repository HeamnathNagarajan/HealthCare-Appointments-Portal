using HealthCare_Appointment_Portal.Models;
using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Data
{

    [ExcludeFromCodeCoverage]
    public class DataStore
    {

        // Patient Collection
        public List<Patient> Patients { get; set; }= new();

        // Doctor Collection
        public List<Doctor> Doctors { get; set; }= new();

        // Appointment Collection
        public List<Appointment> Appointments { get; set; } = new();

        // Health Record Collection
        public List<HealthRecord> HealthRecords { get; set; } = new();
    }
}
