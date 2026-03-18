namespace ThursdayConsole;

public static class ThursdayCalculator
{
    // Known full moon reference: 2000-01-06 18:14 UTC
    private static readonly DateTime FullMoonReference = new(2000, 1, 6, 18, 14, 0, DateTimeKind.Utc);
    private const double SynodicMonth = 29.530588853;

    public static List<DateOnly> GetThursdays(int year, int month)
    {
        int days = DateTime.DaysInMonth(year, month);
        var result = new List<DateOnly>();

        for (int day = 1; day <= days; day++)
        {
            var date = new DateOnly(year, month, day);
            if (date.DayOfWeek == DayOfWeek.Thursday)
                result.Add(date);
        }

        return result;
    }

    public static HashSet<DateOnly> GetFullMoons(int year)
    {
        var fullMoons = new HashSet<DateOnly>();

        var startSearch = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(-SynodicMonth);
        double cyclesSinceRef = (startSearch - FullMoonReference).TotalDays / SynodicMonth;
        long firstCycle = (long)Math.Floor(cyclesSinceRef);

        for (long n = firstCycle; ; n++)
        {
            DateTime fullMoon = FullMoonReference.AddDays(n * SynodicMonth);
            if (fullMoon.Year > year) break;

            if (fullMoon.Year == year)
                fullMoons.Add(DateOnly.FromDateTime(fullMoon));
        }

        return fullMoons;
    }

    public static bool IsFullMoonThursday(DateOnly date, HashSet<DateOnly> fullMoons)
        => date.DayOfWeek == DayOfWeek.Thursday && fullMoons.Contains(date);
}
