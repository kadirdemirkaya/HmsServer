namespace Hsm.Domain.Models.Constants
{
    public static class Constant
    {
        public static class ValidationErrors
        {
            public static string IsNullError(string prop) => $"{prop} property cannot be empty.";
        }

        public static class Table
        {
            public const string Appointments = "Appointment";
            public const string Cities = "Cities";
            public const string Clinical = "Clinicals";
            public const string Doctors = "Doctors";
            public const string Hospitals = "Hospitals";
            public const string Notifications = "Notifications";
            public const string WorkSchedules = "WorkSchedules";
        }
    }
}
