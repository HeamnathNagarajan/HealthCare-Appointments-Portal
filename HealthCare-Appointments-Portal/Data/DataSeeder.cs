using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Data
{
    [ExcludeFromCodeCoverage]
    public static class DataSeeder
    {
        public static void Seed(DataStore dataStore)
        {
            // PATIENTS
            Patient patient1 = new()
            {
                PatientId = 1,
                FullName = "Arun Kumar",
                DateOfBirth = new DateOnly(1998, 5, 12),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "arun@gmail.com",
                InsuranceId = "INS1001"
            };

            Patient patient2 = new()
            {
                PatientId = 2,
                FullName = "Priya Sharma",
                DateOfBirth = new DateOnly(1995, 8, 22),
                Gender = Gender.Female,
                PhoneNumber = "9876543222",
                Email = "priya@gmail.com",
                InsuranceId = "INS1002"
            };

            Patient patient3 = new()
            {
                PatientId = 3,
                FullName = "Rahul Verma",
                DateOfBirth = new DateOnly(2000, 1, 15),
                Gender = Gender.Male,
                PhoneNumber = "9876543333",
                Email = "rahul@gmail.com",
                InsuranceId = "INS1003"
            };

            Patient patient4 = new()
            {
                PatientId = 4,
                FullName = "Sneha Reddy",
                DateOfBirth = new DateOnly(1997, 4, 18),
                Gender = Gender.Female,
                PhoneNumber = "9876544444",
                Email = "sneha@gmail.com",
                InsuranceId = "INS1004"
            };

            Patient patient5 = new()
            {
                PatientId = 5,
                FullName = "Vikram Singh",
                DateOfBirth = new DateOnly(1992, 9, 9),
                Gender = Gender.Male,
                PhoneNumber = "9876545555",
                Email = "vikram@gmail.com",
                InsuranceId = "INS1005"
            };

            Patient patient6 = new()
            {
                PatientId = 6,
                FullName = "Anjali Mehta",
                DateOfBirth = new DateOnly(1999, 7, 30),
                Gender = Gender.Female,
                PhoneNumber = "9876546666",
                Email = "anjali@gmail.com",
                InsuranceId = "INS1006"
            };

            Patient patient7 = new()
            {
                PatientId = 7,
                FullName = "Karan Patel",
                DateOfBirth = new DateOnly(1994, 11, 11),
                Gender = Gender.Male,
                PhoneNumber = "9876547777",
                Email = "karan@gmail.com",
                InsuranceId = "INS1007"
            };

            dataStore.Patients.AddRange(
            [
                patient1,
                patient2,
                patient3,
                patient4,
                patient5,
                patient6,
                patient7
            ]);

            // DOCTORS
            Doctor doctor1 = new()
            {
                DoctorId = 1,
                FullName = "Dr. Rajesh",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 800,
                IsActive = true
            };

            Doctor doctor2 = new()
            {
                DoctorId = 2,
                FullName = "Dr. Meena",
                Specialisation = Specialisation.Dermatology,
                YearsOfExperience = 7,
                ConsultationFee = 600,
                IsActive = true
            };

            Doctor doctor3 = new()
            {
                DoctorId = 3,
                FullName = "Dr. Ashok",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 12,
                ConsultationFee = 900,
                IsActive = true
            };

            Doctor doctor4 = new()
            {
                DoctorId = 4,
                FullName = "Dr. Kavya",
                Specialisation = Specialisation.Neurology,
                YearsOfExperience = 5,
                ConsultationFee = 1000,
                IsActive = true
            };

            Doctor doctor5 = new()
            {
                DoctorId = 5,
                FullName = "Dr. Sanjay",
                Specialisation = Specialisation.Orthopedics,
                YearsOfExperience = 9,
                ConsultationFee = 750,
                IsActive = false
            };

            Doctor doctor6 = new()
            {
                DoctorId = 6,
                FullName = "Dr. Divya",
                Specialisation = Specialisation.Dermatology,
                YearsOfExperience = 4,
                ConsultationFee = 550,
                IsActive = true
            };

            Doctor doctor7 = new()
            {
                DoctorId = 7,
                FullName = "Dr. Ravi",
                Specialisation = Specialisation.Pediatrics,
                YearsOfExperience = 8,
                ConsultationFee = 650,
                IsActive = true
            };

            dataStore.Doctors.AddRange(
            [
                doctor1,
                doctor2,
                doctor3,
                doctor4,
                doctor5,
                doctor6,
                doctor7
            ]);

            // APPOINTMENTS
            Appointment appointment1 =
                CreateAppointment(
                    1,
                    patient1,
                    doctor1,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(2)),
                    new TimeOnly(10, 30),
                    AppointmentStatus.Confirmed);

            Appointment appointment2 =
                CreateAppointment(
                    2,
                    patient2,
                    doctor2,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(-1)),
                    new TimeOnly(11, 0),
                    AppointmentStatus.Completed);

            Appointment appointment3 =
                CreateAppointment(
                    3,
                    patient3,
                    doctor1,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(3)),
                    new TimeOnly(12, 15),
                    AppointmentStatus.Pending);

            Appointment appointment4 =
                CreateAppointment(
                    4,
                    patient4,
                    doctor4,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(1)),
                    new TimeOnly(2, 0),
                    AppointmentStatus.Confirmed);

            Appointment appointment5 =
                CreateAppointment(
                    5,
                    patient5,
                    doctor3,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(-2)),
                    new TimeOnly(4, 30),
                    AppointmentStatus.Completed);

            Appointment appointment6 =
                CreateAppointment(
                    6,
                    patient6,
                    doctor6,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(5)),
                    new TimeOnly(9, 45),
                    AppointmentStatus.Cancelled);

            appointment6.CancellationReason =
                "Patient Sick";

            Appointment appointment7 =
                CreateAppointment(
                    7,
                    patient7,
                    doctor7,
                    DateOnly.FromDateTime(
                        DateTime.Now.AddDays(4)),
                    new TimeOnly(5, 15),
                    AppointmentStatus.Pending);

            dataStore.Appointments.AddRange(
            [
                appointment1,
                appointment2,
                appointment3,
                appointment4,
                appointment5,
                appointment6,
                appointment7
            ]);

            // HEALTH RECORDS   
            dataStore.HealthRecords.AddRange(
            [
                new HealthRecord
                {
                    RecordId = 1,
                    AppointmentId = 2,
                    Patient = patient2,
                    Doctor = doctor2,
                    Diagnosis = "Skin Allergy",
                    Prescription = "Cetirizine",
                    Notes = "Avoid Dust",
                    VisitDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(-1))
                },

                new HealthRecord
                {
                    RecordId = 2,
                    AppointmentId = 5,
                    Patient = patient5,
                    Doctor = doctor3,
                    Diagnosis = "Heart Pain",
                    Prescription = "ECG Test",
                    Notes = "Weekly Review",
                    VisitDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(-2))
                },

                new HealthRecord
                {
                    RecordId = 3,
                    AppointmentId = 1,
                    Patient = patient1,
                    Doctor = doctor1,
                    Diagnosis = "High BP",
                    Prescription = "BP Tablets",
                    Notes = "Reduce Salt",
                    VisitDate =
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays(-5))
                }
            ]);
        }

        // Helper Method
        private static Appointment CreateAppointment(
            int id,
            Patient patient,
            Doctor doctor,
            DateOnly date,
            TimeOnly slot,
            AppointmentStatus status)
        {
            Appointment appointment =
                new()
                {
                    AppointmentId = id,
                    Patient = patient,
                    Doctor = doctor,
                    ScheduledDate = date,
                    TimeSlot = slot,
                    Status = status
                };

            doctor.Appointments
                .Add(appointment);

            return appointment;
        }
    }
}
