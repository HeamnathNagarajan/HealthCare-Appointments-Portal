namespace HealthcareApp.Utilities
{
    public static class TimeSlots
    {
        public static readonly List<TimeSpan> DailySlotStartTimes = new List<TimeSpan>
        {
            new TimeSpan(9, 0, 0),
            new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0),

            // Lunch break: 12:00-13:00 skipped

            new TimeSpan(13, 0, 0),
            new TimeSpan(14, 0, 0),
            new TimeSpan(15, 0, 0),
            new TimeSpan(16, 0, 0)
        };

        public static string FormatSlot(TimeSpan startTime)
        {
            TimeSpan endTime = startTime.Add(TimeSpan.FromHours(1));
            return $"{startTime:hh\\:mm}-{endTime:hh\\:mm}";
        }
    }
}