using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Controllers
{
    public class ManagementController
    {
        private readonly PatientController _patientController;

        private readonly DoctorController _doctorController;

        private readonly AppointmentController _appointmentController;

        private readonly HealthRecordController _healthRecordController;

        private readonly IAppointmentService _appointmentService;

        private readonly IHealthRecordService _healthRecordService;

        public ManagementController(
            PatientController patientController,
            DoctorController doctorController,
            AppointmentController appointmentController,
            HealthRecordController healthRecordController,
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            _patientController =
                patientController;

            _doctorController =
                doctorController;

            _appointmentController =
                appointmentController;

            _healthRecordController =
                healthRecordController;

            _appointmentService =
                appointmentService;

            _healthRecordService =
                healthRecordService;
        }

        // Main Application Menu
        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                DisplayMainMenu();

                int choice =
                    UtilityHelper
                    .ReadValidInt(
                        ConsoleConstants.EnterChoice);

                switch (choice)
                {
                    case 1:

                        _patientController
                            .RegisterPatient();

                        break;

                    case 2:

                        _doctorController
                            .AddDoctor();

                        break;

                    case 3:

                        _doctorController
                            .SearchDoctors();

                        break;

                    case 4:

                        _appointmentController
                            .BookAppointment();

                        break;

                    case 5:

                        _patientController
                            .ViewAppointments(
                                _appointmentService);

                        break;

                    case 6:

                        _appointmentController
                            .ManageAppointment();

                        break;

                    case 7:

                        _healthRecordController
                            .AddHealthRecord();

                        break;

                    case 8:

                        _patientController
                            .ViewHealthRecords(
                                _healthRecordService);

                        break;

                    case 9:

                        ShowManagementModules();

                        break;

                    case 10:

                        exit = true;

                        Console.WriteLine(
                            ConsoleConstants
                            .ApplicationClosed);

                        break;

                    default:

                        Console.WriteLine(
                            ConsoleConstants
                            .InvalidChoice);

                        break;
                }
            }
        }

        // Display Main Menu
        private static void DisplayMainMenu()
        {
            Console.WriteLine(
                ConsoleConstants.ApplicationTitle);

            Console.WriteLine(
                ConsoleConstants.RegisterPatient);

            Console.WriteLine(
                ConsoleConstants.AddDoctor);

            Console.WriteLine(
                ConsoleConstants.SearchDoctors);

            Console.WriteLine(
                ConsoleConstants.BookAppointment);

            Console.WriteLine(
                ConsoleConstants.ViewAppointments);

            Console.WriteLine(
                ConsoleConstants.ManageAppointments);

            Console.WriteLine(
                ConsoleConstants.AddHealthRecord);

            Console.WriteLine(
                ConsoleConstants.ViewHealthRecords);

            Console.WriteLine(
                ConsoleConstants.ManagementModules);

            Console.WriteLine(
                ConsoleConstants.Exit);
        }

        // Management Modules
        private void ShowManagementModules()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine(
                    ConsoleConstants.ManagementModuleTitle);

                Console.WriteLine(
                    ConsoleConstants.PatientManagement);

                Console.WriteLine(
                    ConsoleConstants.DoctorManagement);

                Console.WriteLine(
                    ConsoleConstants.AppointmentManagement);

                Console.WriteLine(
                    ConsoleConstants.HealthRecordManagement);

                Console.WriteLine(
                    ConsoleConstants.Back);

                int choice =
                    UtilityHelper
                    .ReadValidInt(
                        ConsoleConstants.EnterChoice);

                switch (choice)
                {
                    case 1:

                        ShowPatientManagement();

                        break;

                    case 2:

                        ShowDoctorManagement();

                        break;

                    case 3:

                        ShowAppointmentManagement();

                        break;

                    case 4:

                        ShowHealthRecordManagement();

                        break;

                    case 5:

                        exit = true;

                        break;

                    default:

                        Console.WriteLine(
                            ConsoleConstants.InvalidChoice);

                        break;
                }
            }
        }

        // Patient Management Menu
        private void ShowPatientManagement()
        {
            Console.WriteLine(
                ConsoleConstants.PatientManagementTitle);

            Console.WriteLine(
                ConsoleConstants.GetPatientById);

            Console.WriteLine(
                ConsoleConstants.ViewAllPatients);

            Console.WriteLine(
                ConsoleConstants.GetPatientByEmail);

            Console.WriteLine(
                ConsoleConstants.UpdatePatient);

            Console.WriteLine(
                ConsoleConstants.DeletePatient);

            Console.WriteLine(
                ConsoleConstants.PatientManagementBack);

            int choice =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants.EnterChoice);

            switch (choice)
            {
                case 1:

                    _patientController
                        .GetPatientById();

                    break;

                case 2:

                    _patientController
                        .GetAllPatients();

                    break;

                case 3:

                    _patientController
                        .GetPatientByEmail();

                    break;

                case 4:

                    _patientController
                        .UpdatePatient();

                    break;

                case 5:

                    _patientController
                        .DeletePatient();

                    break;

                default:

                    Console.WriteLine(
                        ConsoleConstants.InvalidChoice);

                    break;
            }
        }

        // Doctor Management Menu
        private void ShowDoctorManagement()
        {
            Console.WriteLine(
                ConsoleConstants.DoctorManagementTitle);

            Console.WriteLine(
                ConsoleConstants.GetDoctorById);

            Console.WriteLine(
                ConsoleConstants.ViewAllDoctors);

            Console.WriteLine(
                ConsoleConstants.GetAvailableDoctors);

            Console.WriteLine(
                ConsoleConstants.UpdateDoctor);

            Console.WriteLine(
                ConsoleConstants.DeleteDoctor);

            Console.WriteLine(
                ConsoleConstants.DoctorManagementBack);

            int choice =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants.EnterChoice);

            switch (choice)
            {
                case 1:

                    _doctorController
                        .GetDoctorById();

                    break;

                case 2:

                    _doctorController
                        .GetAllDoctors();

                    break;

                case 3:

                    _doctorController
                        .GetAvailableDoctors();

                    break;

                case 4:

                    _doctorController
                        .UpdateDoctor();

                    break;

                case 5:

                    _doctorController
                        .DeleteDoctor();

                    break;

                default:

                    Console.WriteLine(
                        ConsoleConstants.InvalidChoice);

                    break;
            }
        }

        // Appointment Management Menu
        private void ShowAppointmentManagement()
        {
            Console.WriteLine(
                ConsoleConstants.AppointmentManagementTitle);

            Console.WriteLine(
                ConsoleConstants.GetAppointmentById);

            Console.WriteLine(
                ConsoleConstants.ViewAllAppointments);

            Console.WriteLine(
                ConsoleConstants.GetAppointmentsByPatient);

            Console.WriteLine(
                ConsoleConstants.GetAppointmentsByDoctor);

            Console.WriteLine(
                ConsoleConstants.GetUpcomingAppointments);

            Console.WriteLine(
                ConsoleConstants.GetCompletedAppointments);

            Console.WriteLine(
                ConsoleConstants.UpdateAppointment);

            Console.WriteLine(
                ConsoleConstants.DeleteAppointment);

            Console.WriteLine(
                ConsoleConstants.AppointmentManagementBack);

            int choice =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants.EnterChoice);

            switch (choice)
            {
                case 1:

                    _appointmentController
                        .GetAppointmentById();

                    break;

                case 2:

                    _appointmentController
                        .GetAllAppointments();

                    break;

                case 3:

                    _appointmentController
                        .GetAppointmentsByPatient();

                    break;

                case 4:

                    _appointmentController
                        .GetAppointmentsByDoctor();

                    break;

                case 5:

                    _appointmentController
                        .GetUpcomingAppointments();

                    break;

                case 6:

                    _appointmentController
                        .GetCompletedAppointments();

                    break;

                case 7:

                    _appointmentController
                        .UpdateAppointment();

                    break;

                case 8:

                    _appointmentController
                        .DeleteAppointment();

                    break;

                default:

                    Console.WriteLine(
                        ConsoleConstants.InvalidChoice);

                    break;
            }
        }

        // Health Record Management Menu
        private void ShowHealthRecordManagement()
        {
            Console.WriteLine(
                ConsoleConstants.HealthRecordManagementTitle);

            Console.WriteLine(
                ConsoleConstants.GetHealthRecordById);

            Console.WriteLine(
                ConsoleConstants.ViewAllHealthRecords);

            Console.WriteLine(
                ConsoleConstants.GetRecordsByDoctor);

            Console.WriteLine(
                ConsoleConstants.UpdateHealthRecord);

            Console.WriteLine(
                ConsoleConstants.DeleteHealthRecord);

            Console.WriteLine(
                ConsoleConstants.HealthRecordManagementBack);

            int choice =
                UtilityHelper
                .ReadValidInt(
                    ConsoleConstants.EnterChoice);

            switch (choice)
            {
                case 1:

                    _healthRecordController
                        .GetHealthRecordById();

                    break;

                case 2:

                    _healthRecordController
                        .GetAllHealthRecords();

                    break;

                case 3:

                    _healthRecordController
                        .GetRecordsByDoctor();

                    break;

                case 4:

                    _healthRecordController
                        .UpdateHealthRecord();

                    break;

                case 5:

                    _healthRecordController
                        .DeleteHealthRecord();

                    break;

                default:

                    Console.WriteLine(
                        ConsoleConstants.InvalidChoice);

                    break;
            }
        }
    }
}