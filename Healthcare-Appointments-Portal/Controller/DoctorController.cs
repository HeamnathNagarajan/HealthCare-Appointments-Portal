using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System.Diagnostics.CodeAnalysis;
using HealthCare_Appointment_Portal.Exceptions;

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
                .ReadValidInt(
                    ConsoleConstants.EnterYearsOfExperience);

            doctor.ConsultationFee =
                UtilityHelper
                .ReadValidDecimal(
                    ConsoleConstants.EnterConsultationFee);

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

            Console.WriteLine(
                doctor
                .GetDoctorSummary());
        }

        // Search Doctors
        public void SearchDoctors()
        {
            try
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

                Console.WriteLine(
                    ConsoleConstants
                    .AvailableDoctors);

                foreach (Doctor doctor in doctors)
                {
                    Console.WriteLine(
                        doctor
                        .GetDoctorSummary());
                }
            }
            catch (DoctorNotFoundException ex)
            {
                Console.WriteLine(
                    $"Exception Handled: {ex.Message}");
            }
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

            Console.WriteLine(
                doctor
                .GetDoctorSummary());
        }

        // View All Doctors
        public void GetAllDoctors()
        {
            List<Doctor> doctors =
                _doctorService
                .GetAllDoctors();

            foreach (Doctor doctor
                in doctors)
            {
                Console.WriteLine(
                    doctor
                    .GetDoctorSummary());
            }
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

            foreach (Doctor doctor
                in doctors)
            {
                Console.WriteLine(
                    doctor
                    .GetDoctorSummary());
            }
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

            Console.WriteLine(
                existingDoctor
                .GetDoctorSummary());

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

            Console.WriteLine(
                _doctorService
                .GetDoctorById(
                    doctorId)!
                .GetDoctorSummary());
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