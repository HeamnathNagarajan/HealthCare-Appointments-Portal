using HealthcareApp.Enums;
using HealthcareApp.Models;
using HealthcareApp.Services;
using HealthcareApp.Utilities;

namespace HealthcareApp.Controllers
{
    public class DoctorController
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public void AddDoctor()
        {
            Console.WriteLine("\n========== Add New Doctor ==========");

            string fullName = Validations.ReadRequiredString("Enter full name: ", "Full name", 2);

            Specialisation selectedSpecialisation = Validations.ReadEnumChoice<Specialisation>("specialisation");

            int yearsOfExperience = Validations.ReadIntInRange(
                "Enter years of experience: ",
                "Years of experience",
                0,
                70
            );

            decimal consultationFee = Validations.ReadDecimalInRange(
                "Enter consultation fee: ",
                "Consultation fee",
                0,
                100000
            );

            List<DayOfWeek> offDays = SelectTwoOffDays();

            var doctor = new Doctor
            {
                FullName = fullName,
                Specialisation = selectedSpecialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = true,
                OffDays = offDays
            };

            Doctor addedDoctor = _doctorService.AddDoctor(doctor);

            Console.WriteLine("\nDoctor added successfully.");
            Console.WriteLine($"{addedDoctor.GetDoctorSummary()} | Off Days: {string.Join(", ", addedDoctor.OffDays)}");
        }

        public void SearchDoctorsBySpecialisation()
        {
            Console.WriteLine("\n========== Search Doctors By Specialisation ==========");

            Specialisation selectedSpecialisation = Validations.ReadEnumChoice<Specialisation>("specialisation");

            List<Doctor> doctors = _doctorService.SearchDoctorsBySpecialisation(selectedSpecialisation);

            if (doctors.Count == 0)
            {
                Console.WriteLine($"\nNo active doctors available for specialisation: {selectedSpecialisation}");
                return;
            }

            Console.WriteLine($"\nActive doctors available for specialisation: {selectedSpecialisation}");

            foreach (Doctor doctor in doctors)
            {
                Console.WriteLine(doctor.GetDoctorSummary());
            }
        }

        private static List<DayOfWeek> SelectTwoOffDays()
        {
            var selectedOffDays = new List<DayOfWeek>();

            Console.WriteLine("\nSelect exactly two off-duty days:");

            var days = Enum.GetValues<DayOfWeek>();

            while (selectedOffDays.Count < 2)
            {
                Console.WriteLine();

                for (int i = 0; i < days.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {days[i]}");
                }

                int dayChoice = Validations.ReadIntInRange(
                    $"Choose off-duty day {selectedOffDays.Count + 1}: ",
                    "Off-duty day",
                    1,
                    days.Length
                );

                DayOfWeek selectedDay = days[dayChoice - 1];

                if (selectedOffDays.Contains(selectedDay))
                {
                    Console.WriteLine("This day has already been selected. Choose another day.");
                    continue;
                }

                selectedOffDays.Add(selectedDay);
            }

            return selectedOffDays;
        }
    }
}