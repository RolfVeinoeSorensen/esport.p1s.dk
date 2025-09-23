namespace Esport.Backend.Dtos
{
  public class HolidayWeekendWorkdayDto
  {
    public DateTime Date { get; set; }
    public bool IsHoliday { get; set; }
    public bool IsWeekend { get; set; }
    public bool IsWorkDay { get; set; }
    public string? Title { get; set; }
    public string? ShortWeekDayName { get; set; }

    public HolidayWeekendWorkdayDto() { }
    public HolidayWeekendWorkdayDto(
        DateTime Date,
        bool IsHoliday,
        bool IsWeekend,
        bool IsWorkDay,
        string Title,
        string ShortWeekDayName
        )
    {
      this.Date = Date;
      this.IsHoliday = IsHoliday;
      this.IsWeekend = IsWeekend;
      this.IsWorkDay = IsWorkDay;
      this.Title = Title;
      this.ShortWeekDayName = ShortWeekDayName;
    }
  }
}