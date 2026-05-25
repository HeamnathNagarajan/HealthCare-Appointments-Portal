namespace HealthCare_Appointment_Portal.Utilities
{
    public static class ConsoleConstants
    {
        // Application Title
        public const string
            ApplicationTitle =
            "\n===== HEALTHCARE APPOINTMENT PORTAL =====";

        // Menu Options
        public const string
            RegisterPatient =
            "1. Register Patient";

        public const string
            AddDoctor =
            "2. Add Doctor";

        public const string
            SearchDoctors =
            "3. Search Doctors By Specialisation";

        public const string
            BookAppointment =
            "4. Book Appointment";

        public const string
            ViewAppointments =
            "5. View Patient Appointments";

        public const string
            ManageAppointments =
            "6. Confirm / Cancel / Complete Appointment";

        public const string
            AddHealthRecord =
            "7. Add Health Record";

        public const string
            ViewHealthRecords =
            "8. View Patient Health Records";

        public const string
            Exit =
            "10. Exit";

        // Input Messages
        public const string
            EnterChoice =
            "\nEnter Choice: ";

        public const string
            EnterPatientEmail =
            "Enter Patient Email: ";

        public const string
            EnterDoctorName =
            "Enter Doctor Name: ";

        public const string
            EnterAppointmentId =
            "\nEnter Appointment Id: ";

        public const string
            EnterCancellationReason =
            "Enter Cancellation Reason: ";

        public const string
            EnterNotes =
            "Enter Notes: ";

        // Section Titles
        public const string
            AvailableDoctors =
            "\nAvailable Doctors:";

        public const string
            Appointments =
            "\nAppointments:";

        public const string
            CompletedAppointments =
            "\nCompleted Appointments:";

        // Appointment Menu
        public const string
            ConfirmAppointment =
            "\n1. Confirm Appointment";

        public const string
            CancelAppointment =
            "2. Cancel Appointment";

        public const string
            CompleteAppointment =
            "3. Complete Appointment";

        // Success Messages
        public const string
            PatientRegisteredSuccessfully =
            "\nPatient Registered Successfully.";

        public const string
            DoctorAddedSuccessfully =
            "\nDoctor Added Successfully.";

        public const string
            AppointmentBookedSuccessfully =
            "\nAppointment Booked Successfully.";

        public const string
            AppointmentConfirmedSuccessfully =
            "Appointment Confirmed.";

        public const string
            AppointmentCancelledSuccessfully =
            "Appointment Cancelled.";

        public const string
            AppointmentCompletedSuccessfully =
            "Appointment Completed.";

        public const string
            HealthRecordAddedSuccessfully =
            "\nHealth Record Added Successfully.";

        public const string
            ApplicationClosed =
            "Application Closed.";

        // General Messages
        public const string
            InvalidChoice =
            "Invalid Choice.";

        public const string
            NoDoctorsFound =
            "No Doctors Found.";

        public const string
            NoDoctorsAvailable =
            "No Doctors Available.";

        public const string
            NoAppointmentsFound =
            "No Appointments Found.";

        public const string
            NoAppointmentsAvailable =
            "No Appointments Available.";

        public const string
            NoCompletedAppointmentsFound =
            "No Completed Appointments Found.";

        public const string
            NoHealthRecordsFound =
            "No Health Records Found.";

        // Validation Messages
        public const string
            InvalidInputFormat =
            "\nInvalid Input Format. Please enter valid data.";

        public const string
            InvalidRange =
            "\nInput value is too large or too small.";

        public const string
            InputCannotBeEmpty =
            "\nInput cannot be empty.";

        // Exception Messages
        public const string
            OperationFailed =
            "\nOperation Failed: ";

        public const string
            RecordNotFound =
            "\nRecord Not Found: ";

        public const string
            DuplicatePatient =
            "\nDuplicate Patient: ";

        public const string
            DuplicateDoctor =
            "\nDuplicate Doctor: ";

        public const string
            PatientError =
            "\nPatient Error: ";

        public const string
            DoctorError =
            "\nDoctor Error: ";

        public const string
            AppointmentError =
            "\nAppointment Error: ";

        public const string
            AppointmentConflict =
            "\nAppointment Conflict: ";

        public const string
            DoctorUnavailable =
            "\nDoctor Unavailable: ";

        public const string
            PastDateError =
            "\nPast Date Error: ";

        public const string
            HealthRecordError =
            "\nHealth Record Error: ";

        public const string
            UnexpectedError =
            "\nUnexpected Error: ";

        // MANAGEMENT MODULES
        public const string
            ManagementModules =
                "9. Management Modules";

        public const string
            ManagementModuleTitle =
            "\n===== MANAGEMENT MODULES =====";

        public const string
            PatientManagement =
            "1. Patient Management";

        public const string
            DoctorManagement =
            "2. Doctor Management";

        public const string
            AppointmentManagement =
            "3. Appointment Management";

        public const string
            HealthRecordManagement =
            "4. Health Record Management";

        public const string
            Back =
            "5. Back";

        // PATIENT MANAGEMENT
        public const string
            PatientManagementTitle =
            "\n===== PATIENT MANAGEMENT =====";

        public const string
            GetPatientById =
            "1. Get Patient By Id";

        public const string
            ViewAllPatients =
            "2. View All Patients";

        public const string
            GetPatientByEmail =
            "3. Get Patient By Email";

        public const string
            UpdatePatient =
            "4. Update Patient";

        public const string
            DeletePatient =
            "5. Delete Patient";

        public const string
            EnterPatientId =
            "Enter Patient Id: ";

        public const string
            PatientUpdatedSuccessfully =
            "Patient Updated Successfully.";

        public const string
            PatientDeletedSuccessfully =
            "Patient Deleted Successfully.";

        public const string
            CurrentPatientDetails =
            "\nCurrent Patient Details:";

        public const string
            UpdatedPatientDetails =
            "\nUpdated Patient Details:";

        // DOCTOR MANAGEMENT

        public const string
            DoctorManagementTitle =
            "\n===== DOCTOR MANAGEMENT =====";

        public const string
            GetDoctorById =
            "1. Get Doctor By Id";

        public const string
            ViewAllDoctors =
            "2. View All Doctors";

        public const string
            GetAvailableDoctors =
            "3. Get Available Doctors";

        public const string
            UpdateDoctor =
            "4. Update Doctor";

        public const string
            DeleteDoctor =
            "5. Delete Doctor";

        public const string
            EnterDoctorId =
            "Enter Doctor Id: ";

        public const string
            DoctorUpdatedSuccessfully =
            "Doctor Updated Successfully.";

        public const string
            DoctorDeletedSuccessfully =
            "Doctor Deleted Successfully.";

        public const string
            CurrentDoctorDetails =
            "\nCurrent Doctor Details:";

        public const string
            UpdatedDoctorDetails =
            "\nUpdated Doctor Details:";

        // APPOINTMENT MANAGEMENT
        public const string
            AppointmentManagementTitle =
            "\n===== APPOINTMENT MANAGEMENT =====";

        public const string
            GetAppointmentById =
            "1. Get Appointment By Id";

        public const string
            ViewAllAppointments =
            "2. View All Appointments";

        public const string
            GetAppointmentsByPatient =
            "3. Get Appointments By Patient";

        public const string
            GetAppointmentsByDoctor =
            "4. Get Appointments By Doctor";

        public const string
            GetUpcomingAppointments =
            "5. Get Upcoming Appointments";

        public const string
            GetCompletedAppointments =
            "6. Get Completed Appointments";

        public const string
            UpdateAppointment =
            "7. Update Appointment";

        public const string
            DeleteAppointment =
            "8. Delete Appointment";

        public const string
            AppointmentUpdatedSuccessfully =
            "Appointment Updated Successfully.";

        public const string
            AppointmentDeletedSuccessfully =
            "Appointment Deleted Successfully.";

        public const string
            CurrentAppointmentDetails =
            "\nCurrent Appointment Details:";

        public const string
            UpdatedAppointmentDetails =
            "\nUpdated Appointment Details:";

        // HEALTH RECORD MANAGEMENT
        public const string
            HealthRecordManagementTitle =
            "\n===== HEALTH RECORD MANAGEMENT =====";

        public const string
            GetHealthRecordById =
            "1. Get Health Record By Id";

        public const string
            ViewAllHealthRecords =
            "2. View All Health Records";

        public const string
            GetRecordsByDoctor =
            "3. Get Records By Doctor";

        public const string
            UpdateHealthRecord =
            "4. Update Health Record";

        public const string
            DeleteHealthRecord =
            "5. Delete Health Record";

        public const string
            EnterRecordId =
            "Enter Record Id: ";

        public const string
            HealthRecordUpdatedSuccessfully =
            "Health Record Updated Successfully.";

        public const string
            HealthRecordDeletedSuccessfully =
            "Health Record Deleted Successfully.";

        public const string
            CurrentHealthRecordDetails =
            "\nCurrent Health Record Details:";

        public const string
            UpdatedHealthRecordDetails =
            "\nUpdated Health Record Details:";

        // INPUT LABELS

        public const string
            EnterFullName =
            "Enter Full Name: ";

        public const string
            EnterDob =
            "Enter DOB (yyyy-MM-dd): ";

        public const string
            EnterGenderChoice =
            "Enter Gender Choice: ";

        public const string
            EnterPhoneNumber =
            "Enter Phone Number: ";

        public const string
            EnterEmail =
            "Enter Email: ";

        public const string
            EnterInsuranceId =
            "Enter Insurance Id: ";

        public const string
            EnterSpecialisationChoice =
            "Enter Specialisation Choice: ";

        public const string
            EnterYearsOfExperience =
            "Enter Years Of Experience: ";

        public const string
            EnterConsultationFee =
            "Enter Consultation Fee: ";

        public const string
            EnterAppointmentDate =
            "Enter Appointment Date (yyyy-MM-dd): ";

        public const string
            EnterTimeSlot =
            "Enter Time Slot (HH:mm): ";

        public const string
            EnterDiagnosis =
            "Enter Diagnosis: ";

        public const string
            EnterPrescription =
            "Enter Prescription: ";

        // UPDATE FIELD LABELS

        // Patient Update
        public const string
            FullNameLabel =
            "Full Name";

        public const string
            DateOfBirthLabel =
            "Date Of Birth";

        public const string
            GenderLabel =
            "Gender";

        public const string
            PhoneNumberLabel =
            "Phone Number";

        public const string
            EmailLabel =
            "Email";

        public const string
            InsuranceIdLabel =
            "Insurance Id";

        // Doctor Update
        public const string
            DoctorNameLabel =
            "Doctor Name";

        public const string
            SpecialisationLabel =
            "Specialisation";

        public const string
            YearsOfExperienceLabel =
            "Years Of Experience";

        public const string
            ConsultationFeeLabel =
            "Consultation Fee";

        public const string
            IsActiveLabel =
            "Is Active";

        // Appointment Update
        public const string
            ScheduledDateLabel =
            "Scheduled Date";

        public const string
            TimeSlotLabel =
            "Time Slot";

        public const string
            AppointmentStatusLabel =
            "Appointment Status";

        public const string
            CancellationReasonLabel =
            "Cancellation Reason";

        // Health Record Update
        public const string
            DiagnosisLabel =
            "Diagnosis";

        public const string
            PrescriptionLabel =
            "Prescription";

        public const string
            NotesLabel =
            "Notes";

        public const string
            VisitDateLabel =
            "Visit Date";


        public const string
            PatientManagementBack =
            "6. Back";

        public const string
            DoctorManagementBack =
            "6. Back";

        public const string
            AppointmentManagementBack =
            "9. Back";

        public const string
            HealthRecordManagementBack =
            "6. Back";

        // EXCEPTION MESSAGES

        public const string
            OperationFailedFormat =
            "Operation Failed: {0}";

        public const string
            RecordNotFoundFormat =
            "Record Not Found: {0}";

        public const string
            DuplicatePatientFormat =
            "Duplicate Patient: {0}";

        public const string
            DuplicateDoctorFormat =
            "Duplicate Doctor: {0}";

        public const string
            PatientErrorFormat =
            "Patient Error: {0}";

        public const string
            DoctorErrorFormat =
            "Doctor Error: {0}";

        public const string
            AppointmentErrorFormat =
            "Appointment Error: {0}";

        public const string
            AppointmentConflictFormat =
            "Appointment Conflict: {0}";

        public const string
            DoctorUnavailableFormat =
            "Doctor Unavailable: {0}";

        public const string
            PastDateErrorFormat =
            "Past Date Error: {0}";

        public const string
            HealthRecordErrorFormat =
            "Health Record Error: {0}";

        public const string
            UnexpectedErrorFormat =
            "Unexpected Error: {0}";

        public const string
            AppointmentDeletionErrorFormat =
            "Appointment Deletion Error: {0}";

        public const string
            DoctorDeletionErrorFormat =
            "Doctor Deletion Error: {0}";

        public const string
            PastTimeSlotErrorFormat =
            "Past Time Slot Error: {0}";

        public const string
            PatientDeletionErrorFormat =
            "Patient Deletion Error: {0}";

    }
}
