using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Controllers
{
    [ExcludeFromCodeCoverage]
    public class DoctorController
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(
            IDoctorService doctorService)
        {
            _doctorService =
                doctorService;
        }

        // Add Doctor
        public void AddDoctor()
        {
            Doctor doctor = new();

            doctor.FullName =
                UtilityHelper
                .ReadValidatedProperty(
                    ConsoleConstants.EnterFullName,
                    nameof(Doctor.FullName),
                    doctor);

            doctor.Specialisation =
                UtilityHelper
                .ReadValidEnum<Specialisation>(
                    ConsoleConstants.EnterSpecialisationChoice);

            doctor.YearsOfExperience =
            UtilityHelper
            .ReadValidatedIntProperty(
                ConsoleConstants.EnterYearsOfExperience,
                nameof(Doctor.YearsOfExperience),
                doctor);

            doctor.ConsultationFee =
              UtilityHelper
              .ReadValidatedDecimalProperty(
               ConsoleConstants.EnterConsultationFee,
               nameof(Doctor.ConsultationFee),
               doctor);

            doctor.IsActive = true;

            if (!UtilityHelper
                .ValidateModel(doctor))
            {
                return;
            }

            _doctorService
                .AddDoctor(doctor);

            Console.WriteLine(
                ConsoleConstants
                .DoctorAddedSuccessfully);

            UtilityHelper.DisplayDoctorTable(
                new List<Doctor> { doctor });
        }

        // Search Doctors
        public void SearchDoctors()
        {
            Specialisation specialisation =
                UtilityHelper
                .ReadValidEnum<Specialisation>(
                    ConsoleConstants
                    .EnterSpecialisationChoice);

            List<Doctor> doctors =
                _doctorService
                .GetDoctorsBySpecialisation(
                    specialisation);

            if (doctors.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoDoctorsAvailable);

                return;
            }

            Console.WriteLine(
                ConsoleConstants
                .AvailableDoctors);

            UtilityHelper.DisplayDoctorTable(
                doctors);
        }

        // Get Doctor By Id
        public void GetDoctorById()
        {
            int doctorId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterDoctorId);

            Doctor doctor =
                _doctorService
                .GetDoctorById(
                    doctorId)!;

            UtilityHelper.DisplayDoctorTable(
                new List<Doctor> { doctor });
        }

        // View All Doctors
        public void GetAllDoctors()
        {
            List<Doctor> doctors =
                _doctorService
                .GetAllDoctors();

            if (doctors.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoDoctorsAvailable);

                return;
            }

            UtilityHelper.DisplayDoctorTable(
                doctors);
        }

        // Get Available Doctors
        public void GetAvailableDoctors()
        {
            Specialisation specialisation =
                UtilityHelper
                .ReadValidEnum<Specialisation>(
                    ConsoleConstants
                    .EnterSpecialisationChoice);

            List<Doctor> doctors =
                _doctorService
                .GetAvailableDoctorsBySpecialisation(
                    specialisation);

            if (doctors.Count == 0)
            {
                Console.WriteLine(
                    ConsoleConstants
                    .NoDoctorsAvailable);

                return;
            }

            UtilityHelper.DisplayDoctorTable(
                doctors);
        }

        // Update Doctor
        public void UpdateDoctor()
        {
            int doctorId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterDoctorId);

            Doctor existingDoctor =
                _doctorService
                .GetDoctorById(
                    doctorId)!;

            Console.WriteLine(
                ConsoleConstants
                .CurrentDoctorDetails);

            UtilityHelper.DisplayDoctorTable(
                new List<Doctor>
                {
                    existingDoctor
                });

            Doctor updatedDoctor = new()
            {
                DoctorId =
                    existingDoctor.DoctorId,

                FullName =
                    UtilityHelper
                    .ReadOptionalString(
                        ConsoleConstants.DoctorNameLabel,
                        existingDoctor.FullName),

                Specialisation =
                    UtilityHelper
                    .ReadOptionalEnum<Specialisation>(
                        ConsoleConstants.SpecialisationLabel,
                        existingDoctor.Specialisation),

                YearsOfExperience =
                    UtilityHelper
                    .ReadOptionalInt(
                        ConsoleConstants.YearsOfExperienceLabel,
                        existingDoctor.YearsOfExperience),

                ConsultationFee =
                    UtilityHelper
                    .ReadOptionalDecimal(
                        ConsoleConstants.ConsultationFeeLabel,
                        existingDoctor.ConsultationFee),

                IsActive =
                    UtilityHelper
                    .ReadOptionalBool(
                        ConsoleConstants.IsActiveLabel,
                        existingDoctor.IsActive)
            };

            _doctorService
                .UpdateDoctor(
                    updatedDoctor);

            Console.WriteLine(
                ConsoleConstants
                .DoctorUpdatedSuccessfully);

            Console.WriteLine(
                ConsoleConstants
                .UpdatedDoctorDetails);

            Doctor updatedDoctorDetails =
                _doctorService
                .GetDoctorById(
                    doctorId)!;

            UtilityHelper.DisplayDoctorTable(
                new List<Doctor>
                {
                    updatedDoctorDetails
                });
        }

        // Delete Doctor
        public void DeleteDoctor()
        {
            int doctorId =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants
                    .EnterDoctorId);

            _doctorService
                .DeleteDoctorById(
                    doctorId);

            Console.WriteLine(
                ConsoleConstants
                .DoctorDeletedSuccessfully);
        }
    }
}
