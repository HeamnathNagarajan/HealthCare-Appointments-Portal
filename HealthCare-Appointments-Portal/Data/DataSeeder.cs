using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Enums;

namespace HealthCare_Appointments_Portal.Data
{
    public static class DataSeeder
    {
        public static void Seed(
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IHealthRecordService healthService)
        {
            SeedPatients(patientService);
            SeedDoctors(doctorService);
            SeedAppointments(patientService, doctorService, appointmentService, healthService);
        }

        private static void SeedPatients(IPatientService patientService)
        {
            var patient1 = new Patient
            {
                PatientId = Guid.NewGuid(),
                FullName = "Aniket Singh",
                DateOfBirth = new DateOnly(2000, 5, 10),
                PhoneNumber = "9876543210",
                Email = "aniket@gmail.com",
                Gender = "Male"
            };

            var patient2 = new Patient
            {
                PatientId = Guid.NewGuid(),
                FullName = "Abhishek Kumar",
                DateOfBirth = new DateOnly(1998, 8, 15),
                PhoneNumber = "9123456780",
                Email = "abhishek@gmail.com",
                Gender = "Male"
            };

            SafeExecute(() => patientService.AddPatient(patient1), "Failed to add patient1");
            SafeExecute(() => patientService.AddPatient(patient2), "Failed to add patient2");
        }

        private static void SeedDoctors(IDoctorService doctorService)
        {
            var doctor1 = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FullName = "Dr. Hemnath",
                Specialisation = Specialisation.Cardiology, 
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            var doctor2 = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FullName = "Dr. Meera",
                Specialisation = Specialisation.Dermatology,
                YearsOfExperience = 7,
                ConsultationFee = 400,
                IsActive = true
            };

            SafeExecute(() => doctorService.AddDoctor(doctor1), "Failed to add doctor1");
            SafeExecute(() => doctorService.AddDoctor(doctor2), "Failed to add doctor2");
        }

        private static void SeedAppointments(
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IHealthRecordService healthService)
        {
            SafeExecute(() =>
            {
                var patient = patientService.GetAllPatients()[0];
                var doctor = doctorService.GetAllDoctors()[0];
               
                var appointment = appointmentService.BookAppointment(
                    patient,
                    doctor,
                    DateOnly.FromDateTime(DateTime.Now).AddDays(1),
                    new TimeOnly(11, 0)
                );

                appointment.Confirm();
                appointment.Status = AppointmentStatus.Completed;

                var record = new HealthRecord
                {
                    Patient = patient,
                    Doctor = doctor,
                    Appointment = appointment,
                    VisitDate = appointment.ScheduledDate,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol 500mg",
                    Notes = "Take rest and drink fluids",
                    CreatedOn = DateTime.Now
                };

                healthService.AddRecord(record);

            }, "Failed to seed appointment/health record");
        }

        private static void SafeExecute(Action action, string errorMessage)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seeder Warning] {errorMessage}: {ex.Message}");
            }
        }
    }
}
