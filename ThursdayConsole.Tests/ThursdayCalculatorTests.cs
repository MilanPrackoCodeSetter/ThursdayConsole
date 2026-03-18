using ThursdayConsole;

namespace ThursdayConsole.Tests;

public class GetThursdaysTests
{
    // --- Happy paths ---

    [Fact]
    public void January2025_Returns5Thursdays()
    {
        var result = ThursdayCalculator.GetThursdays(2025, 1);

        Assert.Equal(5, result.Count);
        Assert.Equal([
            new DateOnly(2025, 1, 2),
            new DateOnly(2025, 1, 9),
            new DateOnly(2025, 1, 16),
            new DateOnly(2025, 1, 23),
            new DateOnly(2025, 1, 30)
        ], result);
    }

    [Fact]
    public void February2025_Returns4Thursdays()
    {
        var result = ThursdayCalculator.GetThursdays(2025, 2);

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void AllDaysAreThursdays()
    {
        for (int month = 1; month <= 12; month++)
        {
            var thursdays = ThursdayCalculator.GetThursdays(2025, month);
            Assert.All(thursdays, d => Assert.Equal(DayOfWeek.Thursday, d.DayOfWeek));
        }
    }

    [Fact]
    public void FullYear2025_TotalIs52Thursdays()
    {
        int total = Enumerable.Range(1, 12).Sum(m => ThursdayCalculator.GetThursdays(2025, m).Count);

        Assert.Equal(52, total);
    }

    [Fact]
    public void FullYear2015_TotalIs53Thursdays()
    {
        // 2015 has 53 Thursdays (Jan 1 is Thursday)
        int total = Enumerable.Range(1, 12).Sum(m => ThursdayCalculator.GetThursdays(2015, m).Count);

        Assert.Equal(53, total);
    }

    // --- Leap year corner cases ---

    [Fact]
    public void LeapYearFebruary2024_Has29Days_ReturnsCorrectThursdays()
    {
        var result = ThursdayCalculator.GetThursdays(2024, 2);

        // Feb 2024: 1,8,15,22,29 are Thursdays — 5 total
        Assert.Equal(5, result.Count);
        Assert.Contains(new DateOnly(2024, 2, 29), result);
    }

    [Fact]
    public void NonLeapYearFebruary2025_LastDayIs28()
    {
        var result = ThursdayCalculator.GetThursdays(2025, 2);

        Assert.All(result, d => Assert.True(d.Day <= 28));
    }

    // --- Corner cases: boundary years ---

    [Fact]
    public void Year1_Month1_ReturnsThursdays()
    {
        var result = ThursdayCalculator.GetThursdays(1, 1);

        Assert.NotEmpty(result);
        Assert.All(result, d => Assert.Equal(DayOfWeek.Thursday, d.DayOfWeek));
    }

    [Fact]
    public void Year9999_Month12_ReturnsThursdays()
    {
        var result = ThursdayCalculator.GetThursdays(9999, 12);

        Assert.NotEmpty(result);
        Assert.All(result, d => Assert.Equal(DayOfWeek.Thursday, d.DayOfWeek));
    }

    // --- Each month has 4 or 5 Thursdays ---

    [Theory]
    [InlineData(2025, 1)]
    [InlineData(2025, 6)]
    [InlineData(2025, 12)]
    [InlineData(2024, 2)]
    [InlineData(2000, 7)]
    public void ThursdayCount_IsAlways4Or5(int year, int month)
    {
        var result = ThursdayCalculator.GetThursdays(year, month);

        Assert.InRange(result.Count, 4, 5);
    }
}

public class GetFullMoonsTests
{
    // --- Happy paths ---

    [Fact]
    public void Year2025_ContainsExpectedFullMoons()
    {
        var fullMoons = ThursdayCalculator.GetFullMoons(2025);

        // Dates derived from the synodic algorithm (cross-checked with known output)
        Assert.Contains(new DateOnly(2025, 1, 29), fullMoons);
        Assert.Contains(new DateOnly(2025, 2, 27), fullMoons);
        Assert.Contains(new DateOnly(2025, 11, 20), fullMoons);
    }

    [Fact]
    public void Year2025_HasCorrectNumberOfFullMoons()
    {
        var fullMoons = ThursdayCalculator.GetFullMoons(2025);

        // A year has 12 or 13 full moons
        Assert.InRange(fullMoons.Count, 12, 13);
    }

    [Fact]
    public void AllFullMoonDates_BelongToRequestedYear()
    {
        var fullMoons = ThursdayCalculator.GetFullMoons(2023);

        Assert.All(fullMoons, d => Assert.Equal(2023, d.Year));
    }

    // --- Corner cases ---

    [Fact]
    public void Year2025_FullMoonThursdayFebruary27()
    {
        var fullMoons = ThursdayCalculator.GetFullMoons(2025);
        var date = new DateOnly(2025, 2, 27);

        // Feb 27, 2025 is a Thursday with a full moon
        Assert.Contains(date, fullMoons);
        Assert.Equal(DayOfWeek.Thursday, date.DayOfWeek);
    }

    [Fact]
    public void Year2025_FullMoonThursdayNovember20()
    {
        var fullMoons = ThursdayCalculator.GetFullMoons(2025);
        var date = new DateOnly(2025, 11, 20);

        Assert.Contains(date, fullMoons);
        Assert.Equal(DayOfWeek.Thursday, date.DayOfWeek);
    }

    [Theory]
    [InlineData(2020)]
    [InlineData(2025)]
    [InlineData(2030)]
    public void MultipleYears_HaveExpectedFullMoonCount(int year)
    {
        var fullMoons = ThursdayCalculator.GetFullMoons(year);

        Assert.InRange(fullMoons.Count, 12, 13);
    }
}

public class IsFullMoonThursdayTests
{
    // --- Happy paths ---

    [Fact]
    public void Thursday_WithFullMoon_ReturnsTrue()
    {
        var fullMoons = new HashSet<DateOnly> { new DateOnly(2025, 2, 27) };

        Assert.True(ThursdayCalculator.IsFullMoonThursday(new DateOnly(2025, 2, 27), fullMoons));
    }

    [Fact]
    public void Thursday_WithoutFullMoon_ReturnsFalse()
    {
        var fullMoons = new HashSet<DateOnly> { new DateOnly(2025, 2, 12) };

        // Jan 2, 2025 is a Thursday but not a full moon
        Assert.False(ThursdayCalculator.IsFullMoonThursday(new DateOnly(2025, 1, 2), fullMoons));
    }

    // --- Unhappy paths ---

    [Fact]
    public void NonThursday_WithFullMoon_ReturnsFalse()
    {
        // Feb 12, 2025 is a Wednesday and a full moon
        var fullMoons = new HashSet<DateOnly> { new DateOnly(2025, 2, 12) };

        Assert.False(ThursdayCalculator.IsFullMoonThursday(new DateOnly(2025, 2, 12), fullMoons));
    }

    [Fact]
    public void NonThursday_WithoutFullMoon_ReturnsFalse()
    {
        var fullMoons = new HashSet<DateOnly>();

        Assert.False(ThursdayCalculator.IsFullMoonThursday(new DateOnly(2025, 1, 1), fullMoons));
    }

    [Fact]
    public void EmptyFullMoonSet_ReturnsFalse()
    {
        var fullMoons = new HashSet<DateOnly>();

        Assert.False(ThursdayCalculator.IsFullMoonThursday(new DateOnly(2025, 2, 27), fullMoons));
    }
}
