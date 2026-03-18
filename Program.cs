if (args.Length != 1 || !int.TryParse(args[0], out int year) || year < 1 || year > 9999)
{
    Console.WriteLine("Usage: ThursdayConsole <year>");
    Console.WriteLine("Example: ThursdayConsole 2025");
    return 1;
}

Console.WriteLine($"Thursdays in {year}:");
Console.WriteLine();

var fullMoons = GetFullMoons(year);
int totalThursdays = 0;
int totalFullMoonThursdays = 0;

for (int month = 1; month <= 12; month++)
{
    var thursdays = GetThursdays(year, month);
    var fullMoonThursdays = thursdays.Where(d => fullMoons.Contains(d)).ToList();

    totalThursdays += thursdays.Count;
    totalFullMoonThursdays += fullMoonThursdays.Count;

    string monthName = new DateTime(year, month, 1).ToString("MMMM");
    string fullMoonMark = fullMoonThursdays.Count > 0
        ? $"  ** Full moon: {string.Join(", ", fullMoonThursdays.Select(d => d.ToString("dd.MM.")))}"
        : string.Empty;

    Console.WriteLine($"  {monthName,-12}: {thursdays.Count}{fullMoonMark}");
}

Console.WriteLine();
Console.WriteLine($"  Total       : {totalThursdays}  (full moon Thursdays: {totalFullMoonThursdays})");

return 0;

static List<DateOnly> GetThursdays(int year, int month)
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

// Uses a known full moon reference and the synodic month period to compute full moon dates.
// Reference: full moon on 2000-01-06 18:14 UTC (J2000 epoch proximity).
// Synodic month: 29.530588853 days.
static HashSet<DateOnly> GetFullMoons(int year)
{
    var reference = new DateTime(2000, 1, 6, 18, 14, 0, DateTimeKind.Utc);
    const double synodicMonth = 29.530588853;

    var fullMoons = new HashSet<DateOnly>();

    // Start searching a full lunar cycle before Jan 1 of the target year
    var startSearch = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(-synodicMonth);
    double cyclesSinceRef = (startSearch - reference).TotalDays / synodicMonth;
    long firstCycle = (long)Math.Floor(cyclesSinceRef);

    for (long n = firstCycle; ; n++)
    {
        DateTime fullMoon = reference.AddDays(n * synodicMonth);
        if (fullMoon.Year > year) break;

        if (fullMoon.Year == year)
            fullMoons.Add(DateOnly.FromDateTime(fullMoon));
    }

    return fullMoons;
}
