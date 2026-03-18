# ThursdayConsole

## Project overview
.NET 9 console application that calculates the number of Thursdays per month for a given year, including detection of Thursdays that fall on a full moon.

## Repository
https://github.com/MilanPrackoCodeSetter/ThursdayConsole

## Structure
```
ThursdayConsole/
├── Program.cs                         # Entry point, argument parsing, output formatting
├── ThursdayCalculator.cs              # Core logic (GetThursdays, GetFullMoons, IsFullMoonThursday)
├── ThursdayConsole.csproj             # Main project (net9.0, Exe)
├── ThursdayConsole.sln                # Solution file including both projects
└── ThursdayConsole.Tests/
    ├── ThursdayCalculatorTests.cs     # 27 xUnit tests
    └── ThursdayConsole.Tests.csproj   # Test project (net9.0, xUnit)
```

## Commands

### Run
```powershell
& "C:\Users\milan\.dotnet\dotnet.exe" run --project "C:\git\ThursdayConsole\ThursdayConsole.csproj" -- 2025
```

### Test
```powershell
& "C:\Users\milan\.dotnet\dotnet.exe" test "C:\git\ThursdayConsole\ThursdayConsole.sln"
```

## Key decisions
- **Full moon algorithm**: uses a known reference date (2000-01-06 18:14 UTC) and synodic month period (29.530588853 days) to compute full moon dates
- **Test project location**: `ThursdayConsole.Tests/` lives inside the main repo folder; the main `.csproj` explicitly excludes it via `<Compile Remove="ThursdayConsole.Tests\**" />`
- **dotnet location**: `C:\Users\milan\.dotnet\dotnet.exe` (not on PATH in Git Bash — use absolute path)
- **gh CLI location**: `C:\Program Files\GitHub CLI\gh.exe`

## Workflow
- Feature work goes on `feature/` branches
- PRs are created via `gh pr create` and merged into `master`
