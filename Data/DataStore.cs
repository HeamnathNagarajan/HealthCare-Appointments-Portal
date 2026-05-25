using HealthcareApp.Enums;
using HealthcareApp.Models;
using HealthcareApp.Utilities;

namespace HealthcareApp.Data
{
    public class DataStore
    {
        public List<Patient> Patients { get; set; } = new();
        public List<Doctor> Doctors { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
        public List<HealthRecord> HealthRecords { get; set; } = new();

        public DataStore()
        {
            DateOnly today = SystemTime.Now;

            SeedPatients(today);
            SeedDoctors(today);
            SeedAppointments(today);
            SeedHealthRecords(today);
        }

        private void SeedPatients(DateOnly today)
        {
            Patients = new List<Patient>
            {
                CreatePatient(
                    patientId: 1,
                    fullName: "Arun Kumar",
                    dateOfBirth: today.AddYears(-34).AddMonths(-2),
                    gender: Gender.Male,
                    phoneNumber: "9876543210",
                    email: "arun.kumar@example.com",
                    insuranceId: "INS1001",
                    createdDate: today.AddDays(-30)
                ),
                CreatePatient(
                    patientId: 2,
                    fullName: "Meera Nair",
                    dateOfBirth: today.AddYears(-38).AddMonths(-1),
                    gender: Gender.Female,
                    phoneNumber: "9876543211",
                    email: "meera.nair@example.com",
                    insuranceId: "INS1002",
                    createdDate: today.AddDays(-25)
                ),
                CreatePatient(
                    patientId: 3,
                    fullName: "Rohan Mathew",
                    dateOfBirth: today.AddYears(-25).AddMonths(-4),
                    gender: Gender.Male,
                    phoneNumber: "9876543212",
                    email: "rohan.mathew@example.com",
                    insuranceId: "INS1003",
                    createdDate: today.AddDays(-20)
                ),
                CreatePatient(
                    patientId: 4,
                    fullName: "Anjali Menon",
                    dateOfBirth: today.AddYears(-31).AddMonths(-3),
                    gender: Gender.Female,
                    phoneNumber: "9876543213",
                    email: "anjali.menon@example.com",
                    insuranceId: "INS1004",
                    createdDate: today.AddDays(-15)
                ),
                CreatePatient(
                    patientId: 5,
                    fullName: "Kiran Joseph",
                    dateOfBirth: today.AddYears(-46).AddMonths(-5),
                    gender: Gender.Male,
                    phoneNumber: "9876543214",
                    email: "kiran.joseph@example.com",
                    insuranceId: "INS1005",
                    createdDate: today.AddDays(-10)
                ),
                CreatePatient(
                    patientId: 6,
                    fullName: "Sara Thomas",
                    dateOfBirth: today.AddYears(-12).AddMonths(-1),
                    gender: Gender.Female,
                    phoneNumber: "9876543215",
                    email: "sara.thomas@example.com",
                    insuranceId: "INS1006",
                    createdDate: today.AddDays(-5)
                )
            };
        }

        private void SeedDoctors(DateOnly today)
        {
            Doctors = new List<Doctor>
            {
                CreateDoctor(
                    doctorId: 1,
                    fullName: "Anitha Varghese",
                    specialisation: Specialisation.GeneralMedicine,
                    yearsOfExperience: 12,
                    consultationFee: 500,
                    isActive: true,
                    offDays: GetTwoOffDaysExcluding(today.DayOfWeek, 1)
                ),
                CreateDoctor(
                    doctorId: 2,
                    fullName: "Rahul Menon",
                    specialisation: Specialisation.Cardiology,
                    yearsOfExperience: 15,
                    consultationFee: 900,
                    isActive: true,
                    offDays: GetTwoOffDaysExcluding(today.DayOfWeek, 2)
                ),
                CreateDoctor(
                    doctorId: 3,
                    fullName: "Priya Nair",
                    specialisation: Specialisation.Dermatology,
                    yearsOfExperience: 8,
                    consultationFee: 650,
                    isActive: true,
                    offDays: GetTwoOffDaysExcluding(today.DayOfWeek, 3)
                ),
                CreateDoctor(
                    doctorId: 4,
                    fullName: "Vikram Iyer",
                    specialisation: Specialisation.Neurology,
                    yearsOfExperience: 20,
                    consultationFee: 1200,
                    isActive: true,
                    offDays: GetTwoOffDaysExcluding(today.DayOfWeek, 4)
                ),
                CreateDoctor(
                    doctorId: 5,
                    fullName: "Lakshmi Pillai",
                    specialisation: Specialisation.Paediatrics,
                    yearsOfExperience: 10,
                    consultationFee: 550,
                    isActive: true,
                    offDays: GetTwoOffDaysExcluding(today.DayOfWeek, 5)
                ),
                CreateDoctor(
                    doctorId: 6,
                    fullName: "Sameer Khan",
                    specialisation: Specialisation.ENT,
                    yearsOfExperience: 7,
                    consultationFee: 600,
                    isActive: true,
                    offDays: GetTwoOffDaysExcluding(today.DayOfWeek, 6)
                ),

                // Inactive doctor for active-doctor filtering demo.
                CreateDoctor(
                    doctorId: 7,
                    fullName: "Divya Krishnan",
                    specialisation: Specialisation.Orthopaedics,
                    yearsOfExperience: 14,
                    consultationFee: 850,
                    isActive: false,
                    offDays: GetTwoOffDaysExcluding(today.DayOfWeek, 2)
                ),

                // Active doctor who is off-duty today for availability demo.
                CreateDoctor(
                    doctorId: 8,
                    fullName: "Naveen Raj",
                    specialisation: Specialisation.Ophthalmology,
                    yearsOfExperience: 9,
                    consultationFee: 700,
                    isActive: true,
                    offDays: GetTwoOffDaysIncluding(today.DayOfWeek)
                )
            };
        }

        private void SeedAppointments(DateOnly today)
        {
            Appointments = new List<Appointment>
            {
                // Pending appointments for confirm/cancel demo.
                CreateAppointment(
                    appointmentId: 1,
                    patientId: 1,
                    doctorId: 1,
                    scheduledDate: today.AddDays(1),
                    status: AppointmentStatus.Pending
                ),
                CreateAppointment(
                    appointmentId: 2,
                    patientId: 2,
                    doctorId: 2,
                    scheduledDate: today.AddDays(1),
                    status: AppointmentStatus.Pending
                ),

                // Confirmed appointments for today's completion demo.
                CreateAppointment(
                    appointmentId: 3,
                    patientId: 3,
                    doctorId: 1,
                    scheduledDate: today,
                    status: AppointmentStatus.Confirmed
                ),
                CreateAppointment(
                    appointmentId: 4,
                    patientId: 4,
                    doctorId: 2,
                    scheduledDate: today,
                    status: AppointmentStatus.Confirmed
                ),

                // Future confirmed appointments.
                CreateAppointment(
                    appointmentId: 5,
                    patientId: 5,
                    doctorId: 3,
                    scheduledDate: today.AddDays(2),
                    status: AppointmentStatus.Confirmed
                ),
                CreateAppointment(
                    appointmentId: 6,
                    patientId: 6,
                    doctorId: 5,
                    scheduledDate: today.AddDays(3),
                    status: AppointmentStatus.Confirmed
                ),

                // Completed appointments with matching health records.
                CreateAppointment(
                    appointmentId: 7,
                    patientId: 1,
                    doctorId: 2,
                    scheduledDate: today.AddDays(-5),
                    status: AppointmentStatus.Completed
                ),
                CreateAppointment(
                    appointmentId: 8,
                    patientId: 2,
                    doctorId: 3,
                    scheduledDate: today.AddDays(-3),
                    status: AppointmentStatus.Completed
                ),
                CreateAppointment(
                    appointmentId: 9,
                    patientId: 5,
                    doctorId: 6,
                    scheduledDate: today.AddDays(-2),
                    status: AppointmentStatus.Completed
                ),

                // Cancelled appointment.
                CreateAppointment(
                    appointmentId: 10,
                    patientId: 4,
                    doctorId: 4,
                    scheduledDate: today.AddDays(4),
                    status: AppointmentStatus.Cancelled,
                    cancellationReason: "Patient requested rescheduling."
                ),

                // Additional confirmed future appointment.
                CreateAppointment(
                    appointmentId: 11,
                    patientId: 2,
                    doctorId: 6,
                    scheduledDate: today.AddDays(1),
                    status: AppointmentStatus.Confirmed
                )
            };
        }

        private void SeedHealthRecords(DateOnly today)
        {
            Appointment appointmentSeven = GetAppointment(7);
            Appointment appointmentEight = GetAppointment(8);
            Appointment appointmentNine = GetAppointment(9);

            HealthRecords = new List<HealthRecord>
            {
                CreateHealthRecord(
                    recordId: 1,
                    appointment: appointmentSeven,
                    visitDate: today.AddDays(-5),
                    diagnosis: "Mild hypertension",
                    prescription: "Amlodipine 5mg once daily",
                    notes: "Advised low-salt diet and follow-up after one month."
                ),
                CreateHealthRecord(
                    recordId: 2,
                    appointment: appointmentEight,
                    visitDate: today.AddDays(-3),
                    diagnosis: "Skin allergy",
                    prescription: "Cetirizine 10mg once daily for five days",
                    notes: "Avoid suspected allergen and monitor symptoms."
                ),
                CreateHealthRecord(
                    recordId: 3,
                    appointment: appointmentNine,
                    visitDate: today.AddDays(-2),
                    diagnosis: "Ear infection",
                    prescription: "Antibiotic ear drops twice daily for seven days",
                    notes: "Keep ear dry and return if pain increases."
                )
            };
        }

        private static Patient CreatePatient(
            int patientId,
            string fullName,
            DateOnly dateOfBirth,
            Gender gender,
            string phoneNumber,
            string email,
            string insuranceId,
            DateOnly createdDate)
        {
            return new Patient
            {
                PatientId = patientId,
                FullName = fullName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                PhoneNumber = phoneNumber,
                Email = email,
                InsuranceId = insuranceId,
                CreatedDate = createdDate
            };
        }

        private static Doctor CreateDoctor(
            int doctorId,
            string fullName,
            Specialisation specialisation,
            int yearsOfExperience,
            decimal consultationFee,
            bool isActive,
            List<DayOfWeek> offDays)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                FullName = fullName,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = isActive,
                OffDays = offDays
            };
        }

        private Appointment CreateAppointment(
            int appointmentId,
            int patientId,
            int doctorId,
            DateOnly scheduledDate,
            AppointmentStatus status,
            string cancellationReason = "")
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                Patient = GetPatient(patientId),
                Doctor = GetDoctor(doctorId),
                ScheduledDate = scheduledDate,
                Status = status,
                CancellationReason = cancellationReason
            };
        }

        private static HealthRecord CreateHealthRecord(
            int recordId,
            Appointment appointment,
            DateOnly visitDate,
            string diagnosis,
            string prescription,
            string notes)
        {
            return new HealthRecord
            {
                RecordId = recordId,
                Patient = appointment.Patient,
                Doctor = appointment.Doctor,
                AppointmentId = appointment.AppointmentId,
                VisitDate = visitDate,
                Diagnosis = diagnosis,
                Prescription = prescription,
                Notes = notes
            };
        }

        private Patient GetPatient(int patientId)
        {
            return Patients.First(p => p.PatientId == patientId);
        }

        private Doctor GetDoctor(int doctorId)
        {
            return Doctors.First(d => d.DoctorId == doctorId);
        }

        private Appointment GetAppointment(int appointmentId)
        {
            return Appointments.First(a => a.AppointmentId == appointmentId);
        }

        private static List<DayOfWeek> GetTwoOffDaysExcluding(DayOfWeek excludedDay, int offset)
        {
            var offDays = new List<DayOfWeek>();

            int dayIndex = ((int)excludedDay + offset) % 7;

            while (offDays.Count < 2)
            {
                DayOfWeek candidate = (DayOfWeek)dayIndex;

                if (candidate != excludedDay && !offDays.Contains(candidate))
                {
                    offDays.Add(candidate);
                }

                dayIndex = (dayIndex + 1) % 7;
            }

            return offDays;
        }

        private static List<DayOfWeek> GetTwoOffDaysIncluding(DayOfWeek includedDay)
        {
            var offDays = new List<DayOfWeek>
            {
                includedDay
            };

            DayOfWeek secondDay = (DayOfWeek)(((int)includedDay + 1) % 7);

            offDays.Add(secondDay);

            return offDays;
        }
    }
}