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
            DateTime seedNow = SystemTime.Now;
            DateTime today = seedNow.Date;

            SeedPatients(today);
            SeedDoctors(today);
            SeedAppointments(seedNow, today);
            SeedHealthRecords(today);
        }

        private void SeedPatients(DateTime today)
        {
            Patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "Arun Kumar",
                    DateOfBirth = today.AddYears(-34).AddMonths(-2),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543210",
                    Email = "arun.kumar@example.com",
                    InsuranceId = "INS1001",
                    CreatedDate = today.AddDays(-30)
                },
                new Patient
                {
                    PatientId = 2,
                    FullName = "Meera Nair",
                    DateOfBirth = today.AddYears(-38).AddMonths(-1),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543211",
                    Email = "meera.nair@example.com",
                    InsuranceId = "INS1002",
                    CreatedDate = today.AddDays(-25)
                },
                new Patient
                {
                    PatientId = 3,
                    FullName = "Rohan Mathew",
                    DateOfBirth = today.AddYears(-25).AddMonths(-4),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543212",
                    Email = "rohan.mathew@example.com",
                    InsuranceId = "INS1003",
                    CreatedDate = today.AddDays(-20)
                },
                new Patient
                {
                    PatientId = 4,
                    FullName = "Anjali Menon",
                    DateOfBirth = today.AddYears(-31).AddMonths(-3),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543213",
                    Email = "anjali.menon@example.com",
                    InsuranceId = "INS1004",
                    CreatedDate = today.AddDays(-15)
                },
                new Patient
                {
                    PatientId = 5,
                    FullName = "Kiran Joseph",
                    DateOfBirth = today.AddYears(-46).AddMonths(-5),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543214",
                    Email = "kiran.joseph@example.com",
                    InsuranceId = "INS1005",
                    CreatedDate = today.AddDays(-10)
                },
                new Patient
                {
                    PatientId = 6,
                    FullName = "Sara Thomas",
                    DateOfBirth = today.AddYears(-12).AddMonths(-1),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543215",
                    Email = "sara.thomas@example.com",
                    InsuranceId = "INS1006",
                    CreatedDate = today.AddDays(-5)
                }
            };
        }

        private void SeedDoctors(DateTime today)
        {
            Doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Anitha Varghese",
                    Specialisation = Specialisation.GeneralMedicine,
                    YearsOfExperience = 12,
                    ConsultationFee = 500,
                    IsActive = true,
                    OffDays = GetTwoOffDaysExcluding(today.DayOfWeek, 1)
                },
                new Doctor
                {
                    DoctorId = 2,
                    FullName = "Rahul Menon",
                    Specialisation = Specialisation.Cardiology,
                    YearsOfExperience = 15,
                    ConsultationFee = 900,
                    IsActive = true,
                    OffDays = GetTwoOffDaysExcluding(today.DayOfWeek, 2)
                },
                new Doctor
                {
                    DoctorId = 3,
                    FullName = "Priya Nair",
                    Specialisation = Specialisation.Dermatology,
                    YearsOfExperience = 8,
                    ConsultationFee = 650,
                    IsActive = true,
                    OffDays = GetTwoOffDaysExcluding(today.DayOfWeek, 3)
                },
                new Doctor
                {
                    DoctorId = 4,
                    FullName = "Vikram Iyer",
                    Specialisation = Specialisation.Neurology,
                    YearsOfExperience = 20,
                    ConsultationFee = 1200,
                    IsActive = true,
                    OffDays = GetTwoOffDaysExcluding(today.DayOfWeek, 4)
                },
                new Doctor
                {
                    DoctorId = 5,
                    FullName = "Lakshmi Pillai",
                    Specialisation = Specialisation.Paediatrics,
                    YearsOfExperience = 10,
                    ConsultationFee = 550,
                    IsActive = true,
                    OffDays = GetTwoOffDaysExcluding(today.DayOfWeek, 5)
                },
                new Doctor
                {
                    DoctorId = 6,
                    FullName = "Sameer Khan",
                    Specialisation = Specialisation.ENT,
                    YearsOfExperience = 7,
                    ConsultationFee = 600,
                    IsActive = true,
                    OffDays = GetTwoOffDaysExcluding(today.DayOfWeek, 6)
                },

                // Inactive doctor for active-doctor filtering demo
                new Doctor
                {
                    DoctorId = 7,
                    FullName = "Divya Krishnan",
                    Specialisation = Specialisation.Orthopaedics,
                    YearsOfExperience = 14,
                    ConsultationFee = 850,
                    IsActive = false,
                    OffDays = GetTwoOffDaysExcluding(today.DayOfWeek, 2)
                },

                // Active doctor who is off-duty today for availability demo
                new Doctor
                {
                    DoctorId = 8,
                    FullName = "Naveen Raj",
                    Specialisation = Specialisation.Ophthalmology,
                    YearsOfExperience = 9,
                    ConsultationFee = 700,
                    IsActive = true,
                    OffDays = GetTwoOffDaysIncluding(today.DayOfWeek)
                }
            };
        }

        private void SeedAppointments(DateTime seedNow, DateTime today)
        {
            List<TimeSpan> laterSlotsToday = GetLaterSlotsToday(seedNow);

            TimeSpan todaySlotOne = laterSlotsToday.Count > 0
                ? laterSlotsToday[0]
                : new TimeSpan(15, 0, 0);

            TimeSpan todaySlotTwo = laterSlotsToday.Count > 1
                ? laterSlotsToday[1]
                : new TimeSpan(16, 0, 0);

            Appointments = new List<Appointment>
            {
                // Pending appointments for confirm/cancel demo
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = today.AddDays(1),
                    SlotStartTime = new TimeSpan(9, 0, 0),
                    Status = AppointmentStatus.Pending
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 2,
                    ScheduledDate = today.AddDays(1),
                    SlotStartTime = new TimeSpan(10, 0, 0),
                    Status = AppointmentStatus.Pending
                },

                // Confirmed appointments for today's completion demo
                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 3,
                    DoctorId = 1,
                    ScheduledDate = today,
                    SlotStartTime = todaySlotOne,
                    Status = AppointmentStatus.Confirmed
                },
                new Appointment
                {
                    AppointmentId = 4,
                    PatientId = 4,
                    DoctorId = 2,
                    ScheduledDate = today,
                    SlotStartTime = todaySlotTwo,
                    Status = AppointmentStatus.Confirmed
                },

                // Confirmed future appointments for later-date demo
                new Appointment
                {
                    AppointmentId = 5,
                    PatientId = 5,
                    DoctorId = 3,
                    ScheduledDate = today.AddDays(2),
                    SlotStartTime = new TimeSpan(11, 0, 0),
                    Status = AppointmentStatus.Confirmed
                },
                new Appointment
                {
                    AppointmentId = 6,
                    PatientId = 6,
                    DoctorId = 5,
                    ScheduledDate = today.AddDays(3),
                    SlotStartTime = new TimeSpan(15, 0, 0),
                    Status = AppointmentStatus.Confirmed
                },

                // Completed appointments with matching health records
                new Appointment
                {
                    AppointmentId = 7,
                    PatientId = 1,
                    DoctorId = 2,
                    ScheduledDate = today.AddDays(-5),
                    SlotStartTime = new TimeSpan(9, 0, 0),
                    Status = AppointmentStatus.Completed
                },
                new Appointment
                {
                    AppointmentId = 8,
                    PatientId = 2,
                    DoctorId = 3,
                    ScheduledDate = today.AddDays(-3),
                    SlotStartTime = new TimeSpan(10, 0, 0),
                    Status = AppointmentStatus.Completed
                },
                new Appointment
                {
                    AppointmentId = 9,
                    PatientId = 5,
                    DoctorId = 6,
                    ScheduledDate = today.AddDays(-2),
                    SlotStartTime = new TimeSpan(16, 0, 0),
                    Status = AppointmentStatus.Completed
                },

                // Cancelled appointment
                new Appointment
                {
                    AppointmentId = 10,
                    PatientId = 4,
                    DoctorId = 4,
                    ScheduledDate = today.AddDays(4),
                    SlotStartTime = new TimeSpan(14, 0, 0),
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient requested rescheduling."
                },

                // Additional confirmed later appointment
                new Appointment
                {
                    AppointmentId = 11,
                    PatientId = 2,
                    DoctorId = 6,
                    ScheduledDate = today.AddDays(1),
                    SlotStartTime = new TimeSpan(13, 0, 0),
                    Status = AppointmentStatus.Confirmed
                }
            };
        }

        private void SeedHealthRecords(DateTime today)
        {
            HealthRecords = new List<HealthRecord>
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    AppointmentId = 7,
                    VisitDate = today.AddDays(-5),
                    Diagnosis = "Mild hypertension",
                    Prescription = "Amlodipine 5mg once daily",
                    Notes = "Advised low-salt diet and follow-up after one month."
                },
                new HealthRecord
                {
                    RecordId = 2,
                    PatientId = 2,
                    DoctorId = 3,
                    AppointmentId = 8,
                    VisitDate = today.AddDays(-3),
                    Diagnosis = "Skin allergy",
                    Prescription = "Cetirizine 10mg once daily for five days",
                    Notes = "Avoid suspected allergen and monitor symptoms."
                },
                new HealthRecord
                {
                    RecordId = 3,
                    PatientId = 5,
                    DoctorId = 6,
                    AppointmentId = 9,
                    VisitDate = today.AddDays(-2),
                    Diagnosis = "Ear infection",
                    Prescription = "Antibiotic ear drops twice daily for seven days",
                    Notes = "Keep ear dry and return if pain increases."
                }
            };
        }

        private static List<TimeSpan> GetLaterSlotsToday(DateTime seedNow)
        {
            return TimeSlots.DailySlotStartTimes
                .Where(slot => seedNow.Date.Add(slot) > seedNow)
                .ToList();
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