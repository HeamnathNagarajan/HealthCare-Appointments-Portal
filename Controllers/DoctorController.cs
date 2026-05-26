using HealthcareApp.Enums;
using HealthcareApp.Models;
using HealthcareApp.Services;
using HealthcareApp.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            string fullName = Validations.ReadRequiredString(
                "Enter full name: ",
                "Full name",
                2
            );

            Specialisation selectedSpecialisation =
                Validations.ReadEnumChoice<Specialisation>("specialisation");

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

            var doctor = new Doctor
            {
                FullName = fullName,
                Specialisation = selectedSpecialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = true,
                OffDays = new List<DateOnly>()
            };

            Doctor addedDoctor = _doctorService.AddDoctor(doctor);

            Console.WriteLine("\nDoctor added successfully.");
            Console.WriteLine(addedDoctor.GetDoctorSummary());
        }

        public void SearchDoctorsBySpecialisation()
        {
            Console.WriteLine("\n========== Search Doctors By Specialisation ==========");

            Specialisation selectedSpecialisation =
                Validations.ReadEnumChoice<Specialisation>("specialisation");

            List<Doctor> doctors =
                _doctorService.SearchDoctorsBySpecialisation(selectedSpecialisation);

            if (doctors.Count == 0)
            {
                Console.WriteLine($"\nNo active doctors available for specialisation: {selectedSpecialisation}");
                return;
            }

            Console.WriteLine($"\nActive doctors available for specialisation: {selectedSpecialisation}");

            ConsoleTableHelper.DisplayDoctors(doctors);
        }

        public void ManageDoctorOffDays()
        {
            Console.WriteLine("\n========== Manage Doctor Off Days ==========");

            int doctorId = Validations.ReadPositiveInt(
                "Enter Doctor ID: ",
                "Doctor ID"
            );

            Doctor doctor = _doctorService.GetDoctorById(doctorId);

            bool goBack = false;

            while (!goBack)
            {
                Console.WriteLine($"\nDoctor: Dr. {doctor.FullName}");

                DisplayCurrentOffDays(doctorId);

                Console.WriteLine("\n1. Add off day");
                Console.WriteLine("2. Remove off day");
                Console.WriteLine("0. Go back");

                int choice = Validations.ReadIntInRange(
                    "Choose an option: ",
                    "Off day menu option",
                    0,
                    2
                );

                switch (choice)
                {
                    case 1:
                        AddDoctorOffDay(doctorId);
                        break;

                    case 2:
                        RemoveDoctorOffDay(doctorId);
                        break;

                    case 0:
                        goBack = true;
                        Console.WriteLine("Returning to main menu...");
                        break;
                }
            }
        }

        private void DisplayCurrentOffDays(int doctorId)
        {
            List<DateOnly> offDays = _doctorService.GetOffDays(doctorId);

            Console.WriteLine("\nCurrent Off Days:");

            if (offDays.Count == 0)
            {
                Console.WriteLine("No off days set.");
                return;
            }

            Console.WriteLine($"{"No.",-5} {"Date",-12}");
            Console.WriteLine(new string('-', 20));

            for (int i = 0; i < offDays.Count; i++)
            {
                Console.WriteLine($"{i + 1,-5} {offDays[i]:yyyy-MM-dd}");
            }
        }

        private void AddDoctorOffDay(int doctorId)
        {
            var offDay = new DateOnly();
            do
            { 
                offDay = Validations.ReadDate(
                    "Enter off day date (yyyy-MM-dd): "
                 );
                bool isPast = offDay < SystemTime.Now;
                if (isPast)
                {
                    Console.WriteLine("Selected date cannot be in the past.");
                    Console.Write("Enter again: ");
                }

             } while (offDay < SystemTime.Now);
    

            _doctorService.AddOffDay(doctorId, offDay);

            Console.WriteLine("Off day added successfully.");
        }

        private void RemoveDoctorOffDay(int doctorId)
        {
            DateOnly offDay = Validations.ReadDate(
                "Enter off day date to remove (yyyy-MM-dd): "
            );

            _doctorService.RemoveOffDay(doctorId, offDay);

            Console.WriteLine("Off day removed successfully.");
        }
    }
}