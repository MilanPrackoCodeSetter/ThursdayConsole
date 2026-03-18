if (args.Length != 1 || !int.TryParse(args[0], out int year) || year < 1 || year > 9999)
{
    Console.WriteLine("Usage: ThursdayConsole <year>");
    Console.WriteLine("Example: ThursdayConsole 2025");
    return 1;
}

Console.WriteLine($"Thursdays in {year}:");
Console.WriteLine();

int totalThursdays = 0;

for (int month = 1; month <= 12; month++)
{
    int thursdayCount = CountThursdays(year, month);
    totalThursdays += thursdayCount;

    string monthName = new DateTime(year, month, 1).ToString("MMMM");
    Console.WriteLine($"  {monthName,-12}: {thursdayCount}");
}

Console.WriteLine();
Console.WriteLine($"  Total       : {totalThursdays}");

return 0;

static int CountThursdays(int year, int month)
{
    int days = DateTime.DaysInMonth(year, month);
    int count = 0;

    for (int day = 1; day <= days; day++)
    {
        if (new DateTime(year, month, day).DayOfWeek == DayOfWeek.Thursday)
            count++;
    }

    return count;
}
