using Esport.Backend.Dtos;
using System.Globalization;

namespace Esport.Backend.Extensions
{
  public static class Date
  {
    public static string ToMonthName(this DateTime dateTime)
    {
      return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
    }

    public static string ToMonthNameShort(this DateTime dateTime)
    {
      return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
    }
    public static DateTime DateFromDateAndWeekday(this DateTime dt, DayOfWeek startOfWeek)
    {
      int diff = dt.DayOfWeek - startOfWeek;
      if (diff < 0)
      {
        diff += 7;
      }

      return dt.AddDays(-1 * diff).Date;
    }

    public static List<HolidayWeekendWorkdayDto> GetMonthYear(int month, int year)
    {
      var firstDayOfMonth = new DateTime(year, month, 1);
      var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
      var dates = GetDateRange(firstDayOfMonth, lastDayOfMonth).ToList();
      return GetHolidaysAndWorkdays(dates, "da-DK");
    }

    public static List<HolidayWeekendWorkdayDto> GetPeriodYear(DateTime start, DateTime end)
    {
      var dates = GetDateRange(start, end).ToList();
      return GetHolidaysAndWorkdays(dates, "da-DK");
    }

    public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
    {
      var dates = new List<DateTime>();

      for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
      {
        dates.Add(dt);
      }
      return dates;
    }

    public static List<HolidayWeekendWorkdayDto> GetHolidaysAndWorkdays(List<DateTime> dates, string culture)
    {
      List<int> years = dates.Select(x => x.Year).ToList();
      List<HolidayWeekendWorkdayDto> holidays = new List<HolidayWeekendWorkdayDto>();
      foreach (var year in years)
      {
        DateTime date = GetAscensionDay(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "Kristi himmelfartsdag", //"AscensionDay",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetAshWednesday(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "Askeonsdag", //"AshWednesday",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetEasterMonday(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "2. påskedag", //"EasterMonday",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetGoodFriday(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "Langfredag", //"GoodFriday",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetGreatPrayerDay(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "Store bededag", //"GreatPrayerDay",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetMaundyThursday(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "Skærtorsdag", //"MaundyThursday",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetPalmSunday(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "Palmesøndag", //"PalmSunday",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetWhitMonday(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "2. Pinsedag", //"WhitMonday",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });
        date = GetWhitSunday(year);
        holidays.Add(new HolidayWeekendWorkdayDto
        {
          Date = date,
          IsHoliday = true,
          IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
          IsWorkDay = false,
          Title = "Pinsedag", //"WhitSunday",
          ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
        });

        //Danish holidays
        if (culture == "da-DK")
        {
          date = new DateTime(year, 1, 1);
          holidays.Add(new HolidayWeekendWorkdayDto
          {
            Date = date,
            IsHoliday = true,
            IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
            IsWorkDay = false,
            Title = "Nytårsdag", //"NewYearsDay",
            ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
          });
          date = new DateTime(year, 6, 5);
          holidays.Add(new HolidayWeekendWorkdayDto
          {
            Date = date,
            IsHoliday = true,
            IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
            IsWorkDay = false,
            Title = "Grundlovsdag", //"ConstitutionDay",
            ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
          });
          date = new DateTime(year, 12, 24);
          holidays.Add(new HolidayWeekendWorkdayDto
          {
            Date = date,
            IsHoliday = true,
            IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
            IsWorkDay = false,
            Title = "Juleaften", //"ChristmasEve",
            ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
          });
          date = new DateTime(year, 12, 25);
          holidays.Add(new HolidayWeekendWorkdayDto
          {
            Date = date,
            IsHoliday = true,
            IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
            IsWorkDay = false,
            Title = "1. Juledag", //"FirstChristmasDay",
            ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
          });
          date = new DateTime(year, 12, 26);
          holidays.Add(new HolidayWeekendWorkdayDto
          {
            Date = date,
            IsHoliday = true,
            IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
            IsWorkDay = false,
            Title = "2. Juledag", //"SecondChristmasDay",
            ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
          });
        }

      }
      List<HolidayWeekendWorkdayDto> result = new List<HolidayWeekendWorkdayDto>();
      foreach (var date in dates)
      {
        HolidayWeekendWorkdayDto holiday = holidays.FirstOrDefault(x => x.Date == date);
        if (holiday != null)
        {
          result.Add(holiday);
        }
        else
        {
          bool isWeekend = isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
          result.Add(new HolidayWeekendWorkdayDto
          {
            Date = date,
            IsHoliday = false,
            IsWeekend = isWeekend,
            IsWorkDay = isWeekend == true ? false : true,
            Title = string.Empty,
            ShortWeekDayName = date.ToString("ddd", new CultureInfo(culture))
          });
        }

      }

      return result;
    }



    #region -- Moving holydays --
    /// <summary>
    /// Gets the palm sunday of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks>Sunday before Easter.</remarks>
    public static DateTime GetPalmSunday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Palm sunday for", "year");
      }
      return CalculateEasterSunday(year).AddDays(-7);
    }

    /// <summary>
    /// Gets the maundy thursday of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks>The Thursday before Easter; commemorates the Last Supper.</remarks>
    public static DateTime GetMaundyThursday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Maundy Thursday for", "year");
      }
      return CalculateEasterSunday(year).AddDays(-3);
    }
    /// <summary>
    /// Gets the good friday of the specified <paramref name="year"/>. 
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks>Friday before Easter.</remarks>
    public static DateTime GetGoodFriday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate good friday for", "year");
      }
      return GetMaundyThursday(year).AddDays(1);
    }

    /// <summary>
    /// Gets the easter monday of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static DateTime GetEasterMonday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Easter Monday date for", "year");
      }
      return CalculateEasterSunday(year).AddDays(1);
    }

    /// <summary>
    /// Gets the great prayer day of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks>This is a specific danish holyday.</remarks>
    public static DateTime GetGreatPrayerDay(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Great Prayer Day date for", "year");
      }
      //fourth friday after easter.
      return CalculateEasterSunday(year).AddDays(5 + 3 * 7);
    }

    /// <summary>
    /// Gets the ascension day of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks>Celebration of the Ascension of Christ into heaven; observed on the 40th day after Easter</remarks>
    public static DateTime GetAscensionDay(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to Ascension Day date for", "year");
      }
      //sixth thursday after easter.
      return CalculateEasterSunday(year).AddDays(39);
    }

    /// <summary>
    /// Gets the whit sunday of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks>Seventh Sunday after Easter; commemorates the emanation of the Holy Spirit to the Apostles; a quarter day in Scotland.</remarks>
    public static DateTime GetWhitSunday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Whit Sunday date for", "year");
      }
      return CalculateEasterSunday(year).AddDays(7 * 7);
    }

    /// <summary>
    /// Gets the whit monday of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    /// <remarks>The day after Whitsunday; a legal holiday in England and Wales and Ireland.</remarks>
    public static DateTime GetWhitMonday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Whit Monday date for", "year");
      }
      return GetWhitSunday(year).AddDays(1);
    }


    /// <summary>
    /// Gets ash wednesday.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    public static DateTime GetAshWednesday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Ash Wednesday date for", "year");
      }
      return CalculateEasterSunday(year).AddDays(-46);
    }

    /// <summary>
    /// Calculates easter sunday for the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>The <see cref="DateTime">date</see> of easter sunday.</returns>
    /// <remarks>This method uses the algorithm specified in the wikipedia article: <a href="http://en.wikipedia.org/wiki/Computus">Computus</a>.</remarks>
    public static DateTime CalculateEasterSunday(int year)
    {
      if (year > DateTime.MaxValue.Year)
      {
        throw new ArgumentException("The Year is not possible to calculate Easter Sunday date for", "year");
      }
      int a = year % 19;
      int b = year / 100;
      int c = year % 100;
      int d = b / 4;
      int e = b % 4;
      int f = (b + 8) / 25;
      int g = (b - f + 1) / 3;
      int h = (19 * a + b - d - g + 15) % 30;
      int i = c / 4;
      int k = c % 4;
      int l = (32 + 2 * e + 2 * i - h - k) % 7;
      int m = (a + 11 * h + 22 * l) / 451;
      int month = (h + l - 7 * m + 114) / 31;
      int day = ((h + l - 7 * m + 114) % 31) + 1;
      return new DateTime(year, month, day).Date;
    }
    #endregion
  }
}