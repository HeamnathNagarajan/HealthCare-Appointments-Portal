using HealthcareApp.Enums;
using HealthcareApp.Models;
using HealthcareApp.Services;
using HealthcareApp.Utilities;

namespace HealthcareApp.Controllers
{
    public class PatientController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public void RegisterPatient()
        {
            Console.WriteLine("\n========== Register New Patient ==========");

            string fullName = Validations.ReadRequiredString("Enter full name: ", "Full name", 2);

            DateOnly dateOfBirth = Validations.ReadDateOfBirth();

            Gender selectedGender = Validations.ReadEnumChoice<Gender>("gender");

            string phoneNumber = Validations.ReadPhoneNumber();

            string email = Validations.ReadEmail();

            string insuranceId = Validations.ReadRequiredString("Enter insurance ID: ", "Insurance ID", 1);

            var patient = new Patient
            {
                FullName = fullName,
                DateOfBirth = dateOfBirth,
                Gender = selectedGender,
                PhoneNumber = phoneNumber,
                Email = email,
                InsuranceId = insuranceId
            };

            Patient registeredPatient = _patientService.RegisterPatient(patient);

            Console.WriteLine("\nPatient registered successfully.");
            Console.WriteLine(registeredPatient.GetProfileSummary());
        }
    }
}