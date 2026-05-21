using HealthCare_Appointments_portal.Models;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Data
{
    public class DataStore
    {
        public List<Patient> Patients { get; set; }
            = new List<Patient>();

        public List<Doctor> Doctors { get; set; }
            = new List<Doctor>();

        public List<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public List<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();
    }
}