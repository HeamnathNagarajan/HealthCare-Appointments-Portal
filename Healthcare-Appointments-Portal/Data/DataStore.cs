using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Data
{

    public class DataStore
    {

        // Patient Collection
        public List<Patient> Patients { get; set; } = new();

        // Doctor Collection
        public List<Doctor> Doctors { get; set; } = new();

        // Appointment Collection
        public List<Appointment> Appointments { get; set; } = new();

        // Health Record Collection
        public List<HealthRecord> HealthRecords { get; set; } = new();
    }
}