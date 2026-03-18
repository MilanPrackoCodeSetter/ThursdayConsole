using ThursdayConsole;

if (args.Length != 1 || !int.TryParse(args[0], out int year) || year < 1 || year > 9999)
{
    Console.WriteLine("Usage: ThursdayConsole <year>");
    Console.WriteLine("Example: ThursdayConsole 2025");
    return 1;
}

Console.WriteLine($"Thursdays in {year}:");
Console.WriteLine();

var fullMoons = ThursdayCalculator.GetFullMoons(year);
int totalThursdays = 0;
int totalFullMoonThursdays = 0;

for (int month = 1; month <= 12; month++)
{
    var thursdays = ThursdayCalculator.GetThursdays(year, month);
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
