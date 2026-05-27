using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HealthCare_Appointments_Portal;

[ExcludeFromCodeCoverage]
public class AppController
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly IHealthRecordService _healthService;

    private const string ChoicePrompt = "Choice: ";

    public AppController(
        IAppointmentService appointmentService,
        IDoctorService doctorService,
        IPatientService patientService,
        IHealthRecordService healthService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _patientService = patientService;
        _healthService = healthService;
    }

    public void Run()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n==== HealthCare Appointment Portal ====");
            Console.WriteLine("1. Register Patient");
            Console.WriteLine("2. Add Doctor");
            Console.WriteLine("3. Search Doctors");
            Console.WriteLine("4. Book Appointment");
            Console.WriteLine("5. View Appointments");
            Console.WriteLine("6. Confirm/Cancel Appointment");
            Console.WriteLine("7. Add Health Record");
            Console.WriteLine("8. View Health History");
            Console.WriteLine("9. Exit");

            int choice = InputHelper.ReadValidInt("Enter choice: ");

            try
            {
                switch (choice)
                {
                    case 1: AddPatient(); break;
                    case 2: AddDoctor(); break;
                    case 3: SearchDoctor(); break;
                    case 4: BookAppointment(); break;
                    case 5: ViewAppointments(); break;
                    case 6: ManageAppointment(); break;
                    case 7: AddHealthRecord(); break;
                    case 8: ViewHealthHistory(); break;
                    case 9: exit = true; break;
                    default: Console.WriteLine("Invalid option"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private void AddPatient()
    {
        var patient = new Patient
        {
            PatientId = Guid.NewGuid(),
            FullName = InputHelper.ReadName("Enter Name: "),
            DateOfBirth = InputHelper.ReadDOB(),
            PhoneNumber = InputHelper.ReadPhone(),
            Email = InputHelper.ReadEmail(),
            InsuranceId = InputHelper.ReadNonEmpty("Enter Insurance ID: ")
        };

        Console.WriteLine("Select Gender:");

        foreach (var g in Enum.GetValues<Gender>())
        {
            Console.WriteLine($"{(int)g} - {g}");
        }

        while (true)
        {
            int choice = InputHelper.ReadValidInt(ChoicePrompt);

            if (Enum.IsDefined(typeof(Gender), choice))
            {
                patient.Gender = ((Gender)choice).ToString();
                break;
            }

            Console.WriteLine("Invalid gender selection");
        }

        _patientService.AddPatient(patient);

        Console.WriteLine("\nPatient Added Successfully!\n");

        // Dynamic Column Widths
        int nameWidth =
            Math.Max("Patient Name".Length, patient.FullName.Length) + 2;

        int genderWidth =
            Math.Max("Gender".Length, patient.Gender.Length) + 2;

        string dob = patient.DateOfBirth.ToString("dd-MM-yyyy");

        int dobWidth =
            Math.Max("DOB".Length, dob.Length) + 2;

        int phoneWidth =
            Math.Max("Phone".Length, patient.PhoneNumber.Length) + 2;

        int insuranceWidth =
            Math.Max("Insurance ID".Length, patient.InsuranceId.Length) + 2;

        // Total Border
        string border =
            "+" + new string('-', nameWidth + 2) +
            "+" + new string('-', genderWidth + 2) +
            "+" + new string('-', dobWidth + 2) +
            "+" + new string('-', phoneWidth + 2) +
            "+" + new string('-', insuranceWidth + 2) + "+";

        Console.WriteLine(border);

        // Header
        Console.WriteLine(
            $"| {"Patient Name".PadRight(nameWidth)} " +
            $"| {"Gender".PadRight(genderWidth)} " +
            $"| {"DOB".PadRight(dobWidth)} " +
            $"| {"Phone".PadRight(phoneWidth)} " +
            $"| {"Insurance ID".PadRight(insuranceWidth)} |"); ;

        Console.WriteLine(border);

        // Data Row
        Console.WriteLine(
            $"| {patient.FullName.PadRight(nameWidth)} " +
            $"| {patient.Gender.PadRight(genderWidth)} " +
            $"| {dob.PadRight(dobWidth)} " +
            $"| {patient.PhoneNumber.PadRight(phoneWidth)} " +
            $"| {patient.InsuranceId.PadRight(insuranceWidth)} |");

        Console.WriteLine(border);
    }

    private void AddDoctor()
    {
        var doctor = new Doctor
        {
            DoctorId = Guid.NewGuid(),
            FullName = InputHelper.ReadName("Name: ")
        };

        Console.WriteLine("Select Specialisation:");

        foreach (var s in Enum.GetValues<Specialisation>())
        {
            Console.WriteLine($"{(int)s} - {s}");
        }

        while (true)
        {
            int choice = InputHelper.ReadValidInt(ChoicePrompt);

            if (Enum.IsDefined(typeof(Specialisation), choice))
            {
                doctor.Specialisation = (Specialisation)choice;
                break;
            }

            Console.WriteLine("Invalid specialisation");
        }

        doctor.YearsOfExperience =
            InputHelper.ReadExperience("Experience: ");

        doctor.ConsultationFee =
            InputHelper.ReadConsultationFee("Fee: ");

        doctor.IsActive = true;

        _doctorService.AddDoctor(doctor);

        Console.WriteLine("\nDoctor Added Successfully!\n");

        // Dynamic Column Widths
        int nameWidth =
            Math.Max("Doctor Name".Length, doctor.FullName.Length) + 2;

        int specializationWidth =
            Math.Max(
                "Specialisation".Length,
                doctor.Specialisation.ToString().Length) + 2;

        int experienceWidth =
            Math.Max(
                "Experience".Length,
                doctor.YearsOfExperience.ToString().Length) + 2;

        int feeWidth =
            Math.Max(
                "Fee".Length,
                doctor.ConsultationFee.ToString().Length) + 2;

        // Dynamic Border
        string border =
            "+" + new string('-', nameWidth + 2) +
            "+" + new string('-', specializationWidth + 2) +
            "+" + new string('-', experienceWidth + 2) +
            "+" + new string('-', feeWidth + 2) + "+";

        Console.WriteLine(border);

        // Header
        Console.WriteLine(
            $"| {"Doctor Name".PadRight(nameWidth)} " +
            $"| {"Specialisation".PadRight(specializationWidth)} " +
            $"| {"Experience".PadRight(experienceWidth)} " +
            $"| {"Fee".PadRight(feeWidth)} |");

        Console.WriteLine(border);

        // Data Row
        Console.WriteLine(
            $"| {doctor.FullName.PadRight(nameWidth)} " +
            $"| {doctor.Specialisation.ToString().PadRight(specializationWidth)} " +
            $"| {doctor.YearsOfExperience.ToString().PadRight(experienceWidth)} " +
            $"| {doctor.ConsultationFee.ToString().PadRight(feeWidth)} |");

        Console.WriteLine(border);
    }

    private void SearchDoctor()
    {
        Console.WriteLine("Select Specialisation:");
        foreach (var s in Enum.GetValues<Specialisation>())
            Console.WriteLine($"{(int)s} - {s}");

        var spec = (Specialisation)InputHelper.ReadValidInt(ChoicePrompt);

        var doctors = _doctorService.GetAllDoctors()
            .Where(d => d.Specialisation == spec)
            .ToList();

        if (doctors.Count == 0)
        {
            Console.WriteLine("No doctors found");
            return;
        }

        doctors.ForEach(d =>
            Console.WriteLine($"{d.FullName} - {d.Specialisation}"));
    }

    private void BookAppointment()
    {
        var patients = _patientService.GetAllPatients();
        var doctors = _doctorService
            .GetAllDoctors()
            .Where(d => d.IsActive)
            .ToList();
        if (patients.Count == 0 || doctors.Count == 0)
        {
            Console.WriteLine("Patients or Doctors not available.");
            return;
        }

        Console.WriteLine("\nAvailable Patients:");
        for (int i = 0; i < patients.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {patients[i].FullName}");
        }

        int pIndex = InputHelper.ReadValidIndex(patients.Count);

        Console.WriteLine("\nAvailable Doctors:");
        for (int i = 0; i < doctors.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {doctors[i].FullName} ({doctors[i].Specialisation})");
        }

        int dIndex = InputHelper.ReadValidIndex(doctors.Count);

        var appointmentDate = InputHelper.ReadFutureDate();

        var appt = _appointmentService.BookAppointment(
            patients[pIndex],
            doctors[dIndex],
            appointmentDate,
            InputHelper.ReadValidTime(appointmentDate)
        );

        Console.WriteLine("\nAppointment Booked Successfully!");
        Console.WriteLine(appt.GetDetails());
    }

    private void ViewAppointments()
    {
        var patients = _patientService.GetAllPatients();

        if (patients.Count == 0)
        {
            Console.WriteLine("No patients available.");
            return;
        }

        Console.WriteLine("\nAvailable Patients:");

        for (int i = 0; i < patients.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {patients[i].FullName}");
        }

        int index = InputHelper.ReadValidIndex(patients.Count);

        var list = _appointmentService
            .GetAppointmentsByPatient(patients[index].PatientId);

        if (list.Count == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        Console.WriteLine("\nAppointments:");

        foreach (var appointment in list)
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(appointment.GetDetails());
        }

        Console.WriteLine("-----------------------------------");
    }

    private void ManageAppointment()
    {
        var list = _appointmentService.GetUpcomingAppointments();

        if (list.Count == 0)
        {
            Console.WriteLine("No upcoming appointments.");
            return;
        }

        Console.WriteLine("\nUpcoming Appointments:");

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1} - {list[i].Patient.FullName} with " +
                $"{list[i].Doctor.FullName} on " +
                $"{list[i].ScheduledDate}");
        }

        int index = InputHelper.ReadValidIndex(list.Count);

        var appt = list[index];

        Console.WriteLine("\n1. Confirm");
        Console.WriteLine("2. Cancel");

        int action;

        while (true)
        {
            action = InputHelper.ReadValidInt(ChoicePrompt);

            if (action == 1 || action == 2)
                break;

            Console.WriteLine("Invalid option");
        }

        if (action == 1)
        {
            appt.Confirm();
            Console.WriteLine("Appointment confirmed successfully!");
        }
        else
        {
            _appointmentService.CancelAppointment(
                appt.AppointmentId,
                "Cancelled by user");

            Console.WriteLine("Appointment cancelled successfully!");
        }
    }

    private void AddHealthRecord()
    {
        var list = _appointmentService.GetCompletedAppointments();

        if (list.Count == 0)
        {
            Console.WriteLine("No completed appointments available.");
            return;
        }

        Console.WriteLine("\nCompleted Appointments:");

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1} - {list[i].Patient.FullName} with " +
                $"{list[i].Doctor.FullName} on " +
                $"{list[i].ScheduledDate}");
        }

        int index = InputHelper.ReadValidIndex(list.Count);

        var appt = list[index];

        var record = new HealthRecord
        {
            Patient = appt.Patient,
            Doctor = appt.Doctor,
            Appointment = appt,
            VisitDate = appt.ScheduledDate,
            Diagnosis = InputHelper.ReadCleanText("Diagnosis: "),
            Prescription = InputHelper.ReadCleanText("Prescription: "),
            Notes = InputHelper.ReadOptionalText("Notes (optional): "),
            CreatedOn = DateTime.Now
        };

        _healthService.AddRecord(record);

        Console.WriteLine("Health record added successfully!");
    }

    private void ViewHealthHistory()
    {
        var patients = _patientService.GetAllPatients();

        if (patients.Count == 0)
        {
            Console.WriteLine("No patients available.");
            return;
        }

        Console.WriteLine("\nAvailable Patients:");

        for (int i = 0; i < patients.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {patients[i].FullName}");
        }

        int index = InputHelper.ReadValidIndex(patients.Count);

        var records = _healthService
            .GetRecordsByPatient(patients[index].PatientId);

        if (records.Count == 0)
        {
            Console.WriteLine("No health records found.");
            return;
        }

        Console.WriteLine("\nHealth History:");

        foreach (var record in records)
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"Visit Date   : {record.VisitDate}");
            Console.WriteLine($"Doctor       : {record.Doctor.FullName}");
            Console.WriteLine($"Diagnosis    : {record.Diagnosis}");
            Console.WriteLine($"Prescription : {record.Prescription}");
            Console.WriteLine($"Notes        : {record.Notes}");
        }

        Console.WriteLine("-----------------------------------");
    }
}
