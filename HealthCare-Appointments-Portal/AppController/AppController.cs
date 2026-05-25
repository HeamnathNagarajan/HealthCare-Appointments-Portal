using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Utilities;
using System.Linq;

namespace HealthCare_Appointments_Portal;

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
            Console.WriteLine($"{(int)g} - {g}");

        while (true)
        {
            int choice = InputHelper.ReadValidInt(ChoicePrompt);
            if (Enum.IsDefined(typeof(Gender), choice))
            {
                patient.Gender = ((Gender)choice).ToString();
                break;
            }
        }

        _patientService.AddPatient(patient);
        Console.WriteLine("Patient Added!");
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
            Console.WriteLine($"{(int)s} - {s}");

        doctor.Specialisation = (Specialisation)InputHelper.ReadValidInt(ChoicePrompt);
        doctor.YearsOfExperience = InputHelper.ReadValidInt("Experience: ");

        doctor.ConsultationFee = decimal.Parse(InputHelper.ReadNonEmpty("Fee: "));
        doctor.IsActive = true;

        _doctorService.AddDoctor(doctor);
        Console.WriteLine("Doctor Added!");
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

        doctors.ForEach(d => Console.WriteLine($"Dr. {d.FullName}"));
    }

    private void BookAppointment()
    {
        var patients = _patientService.GetAllPatients();
        var doctors = _doctorService.GetAllDoctors();

        if (patients.Count == 0 || doctors.Count == 0)
        {
            Console.WriteLine("Patients or Doctors not available.");
            return;
        }

        int pIndex = InputHelper.ReadValidIndex(patients.Count);
        int dIndex = InputHelper.ReadValidIndex(doctors.Count);

        var appt = _appointmentService.BookAppointment(
            patients[pIndex],
            doctors[dIndex],
            InputHelper.ReadFutureDate(),
            InputHelper.ReadValidTime(DateOnly.FromDateTime(DateTime.Now))
        );

        Console.WriteLine(appt.GetDetails());
    }

    private void ViewAppointments()
    {
        var patients = _patientService.GetAllPatients();
        if (patients.Count == 0) return;

        int index = InputHelper.ReadValidIndex(patients.Count);

        var list = _appointmentService.GetAppointmentsByPatient(patients[index].PatientId);

        if (list.Count == 0)
        {
            Console.WriteLine("No appointments");
            return;
        }

        list.ForEach(a => Console.WriteLine(a.GetDetails()));
    }

    private void ManageAppointment()
    {
        var list = _appointmentService.GetUpcomingAppointments();
        if (list.Count == 0) return;

        int index = InputHelper.ReadValidIndex(list.Count);
        var appt = list[index];

        Console.WriteLine("1. Confirm 2. Cancel");
        int action = InputHelper.ReadValidInt(ChoicePrompt);

        if (action == 1) appt.Confirm();
        else _appointmentService.CancelAppointment(appt.AppointmentId, "Cancelled");
    }

    private void AddHealthRecord()
    {
        var list = _appointmentService.GetCompletedAppointments();
        if (list.Count == 0) return;

        int index = InputHelper.ReadValidIndex(list.Count);
        var appt = list[index];

        _healthService.AddRecord(new HealthRecord
        {
            Patient = appt.Patient,
            Doctor = appt.Doctor,
            Appointment = appt,
            VisitDate = appt.ScheduledDate,
            Diagnosis = InputHelper.ReadCleanText("Diagnosis: "),
            Prescription = InputHelper.ReadCleanText("Prescription: "),
            Notes = "",
            CreatedOn = DateTime.Now
        });

        Console.WriteLine("Record Added");
    }

    private void ViewHealthHistory()
    {
        var patients = _patientService.GetAllPatients();
        if (patients.Count == 0) return;

        int index = InputHelper.ReadValidIndex(patients.Count);

        var records = _healthService.GetRecordsByPatient(patients[index].PatientId);

        if (records.Count == 0)
        {
            Console.WriteLine("No records");
            return;
        }

        records.ForEach(r => Console.WriteLine(r.Diagnosis));
    }
}
