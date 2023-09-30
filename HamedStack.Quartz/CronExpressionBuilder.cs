// ReSharper disable All 

using System;
using System.Linq;

namespace Quartz
{
    public enum CronDayOfWeek
    {
        None = -1,
        Any = 0,
        SUN = 1,
        MON,
        TUE,
        WED,
        THU,
        FRI,
        SAT
    }

    public enum CronMonth
    {
        Any = 0,
        JAN = 1,
        FEB,
        MAR,
        APR,
        MAY,
        JUN,
        JUL,
        AUG,
        SEP,
        OCT,
        NOV,
        DEC
    }

    public class CronExpressionBuilder
    {
        private string _daysOfMonth = "*";
        private bool _daysOfMonthSet;
        private string _daysOfWeek = "?";
        private bool _daysOfWeekSet;
        private string _hours = "0";
        private bool _hoursSet;
        private string _minutes = "0";
        private bool _minutesSet;
        private string _months = "*";
        private bool _monthsSet;
        private string _seconds = "0";
        private bool _secondsSet;
        private string _years = "*";
        private bool _yearsSet;

        public string Build()
        {
            return $"{_seconds} {_minutes} {_hours} {_daysOfMonth} {_months} {_daysOfWeek} {_years}";
        }

        public override string ToString()
        {
            return $"CronExpressionBuilder: Seconds={_seconds}, Minutes={_minutes}, Hours={_hours}, DaysOfMonth={_daysOfMonth}, Months={_months}, DaysOfWeek={_daysOfWeek}, Years={_years}";
        }

        public CronExpressionBuilder WithDaysOfMonth(params int[] daysOfMonth)
        {
            if (_daysOfMonthSet)
            {
                throw new InvalidOperationException("WithDaysOfMonth method should not be called more than once.");
            }
            _daysOfMonthSet = true;

            if (daysOfMonth.All(day => day >= 1 && day <= 31))
            {
                _daysOfMonth = daysOfMonth.Length == 0 ? "*" : string.Join(",", daysOfMonth);
                _daysOfWeek = "?";
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(daysOfMonth), "Days of month must be between 1 and 31.");
            }
            return this;
        }

        public CronExpressionBuilder WithDaysOfMonthEvery(int daysOfMonthIncrement)
        {
            if (_daysOfMonthSet)
            {
                throw new InvalidOperationException("WithDaysOfMonth or WithDaysOfMonthEvery method should not be called more than once.");
            }
            _daysOfMonthSet = true;

            if (daysOfMonthIncrement > 0 && daysOfMonthIncrement <= 31)
            {
                _daysOfMonth = $"1/{daysOfMonthIncrement}";
                _daysOfWeek = "?";
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(daysOfMonthIncrement), "Days of month increment must be between 1 and 31.");
            }
            return this;
        }

        public CronExpressionBuilder WithDaysOfWeek(params CronDayOfWeek[] daysOfWeek)
        {
            if (_daysOfWeekSet)
            {
                throw new InvalidOperationException("WithDaysOfWeek method should not be called more than once.");
            }
            _daysOfWeekSet = true;

            if (daysOfWeek.Length == 0 || daysOfWeek.Contains(CronDayOfWeek.Any))
            {
                _daysOfWeek = "?";
                _daysOfMonth = "*";
            }
            else
            {
                _daysOfWeek = string.Join(",", daysOfWeek.Select(d => (int)d));
                _daysOfMonth = "?";
            }
            return this;
        }

        public CronExpressionBuilder WithDaysOfWeekList(params CronDayOfWeek[] daysOfWeek)
        {
            if (_daysOfWeekSet)
            {
                throw new InvalidOperationException("DaysOfWeek methods should not be called more than once.");
            }
            _daysOfWeekSet = true;

            if (daysOfWeek.All(day => day >= CronDayOfWeek.SUN && day <= CronDayOfWeek.SAT))
            {
                _daysOfWeek = string.Join(",", daysOfWeek.Select(d => (int)d));
                _daysOfMonth = "?";
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid days of week list.");
            }
            return this;
        }

        public CronExpressionBuilder WithDaysOfWeekRange(CronDayOfWeek startDay, CronDayOfWeek endDay)
        {
            if (_daysOfWeekSet)
            {
                throw new InvalidOperationException("DaysOfWeek methods should not be called more than once.");
            }
            _daysOfWeekSet = true;

            if (startDay >= CronDayOfWeek.SUN && startDay <= CronDayOfWeek.SAT && endDay >= CronDayOfWeek.SUN && endDay <= CronDayOfWeek.SAT && startDay < endDay)
            {
                _daysOfWeek = $"{(int)startDay}-{(int)endDay}";
                _daysOfMonth = "?";
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid days of week range. Start day must be less than end day.");
            }
            return this;
        }

        public CronExpressionBuilder WithHours(int hours)
        {
            if (_hoursSet)
            {
                throw new InvalidOperationException("WithHours method should not be called more than once.");
            }
            _hoursSet = true;

            if (hours >= 0 && hours <= 23)
            {
                _hours = Convert.ToString(hours);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(hours), "Hours must be between 0 and 23.");
            }
            return this;
        }

        public CronExpressionBuilder WithHoursEvery(int hoursIncrement)
        {
            if (_hoursSet)
            {
                throw new InvalidOperationException("WithHours or WithHoursEvery method should not be called more than once.");
            }
            _hoursSet = true;

            if (hoursIncrement > 0 && hoursIncrement <= 23)
            {
                _hours = $"0/{hoursIncrement}";
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(hoursIncrement), "Hours increment must be between 1 and 23.");
            }
            return this;
        }

        public CronExpressionBuilder WithHoursList(params int[] hours)
        {
            if (_hoursSet)
            {
                throw new InvalidOperationException("Hours methods should not be called more than once.");
            }
            _hoursSet = true;

            if (hours.All(hour => hour >= 0 && hour <= 23))
            {
                _hours = string.Join(",", hours);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid hours list. Hours must be between 0 and 23.");
            }
            return this;
        }

        public CronExpressionBuilder WithHoursRange(int startHour, int endHour)
        {
            if (_hoursSet)
            {
                throw new InvalidOperationException("Hours methods should not be called more than once.");
            }
            _hoursSet = true;

            if (startHour >= 0 && startHour <= 23 && endHour >= 0 && endHour <= 23 && startHour < endHour)
            {
                _hours = $"{startHour}-{endHour}";
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid hours range. Hours must be between 0 and 23, and startHour must be less than endHour.");
            }
            return this;
        }

        public CronExpressionBuilder WithMinutes(int minutes)
        {
            if (_minutesSet)
            {
                throw new InvalidOperationException("WithMinutes method should not be called more than once.");
            }
            _minutesSet = true;

            if (minutes >= 0 && minutes <= 59)
            {
                _minutes = Convert.ToString(minutes);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(minutes), "Minutes must be between 0 and 59.");
            }
            return this;
        }

        public CronExpressionBuilder WithMinutesEvery(int minutesIncrement)
        {
            if (_minutesSet)
            {
                throw new InvalidOperationException("WithMinutes or WithMinutesEvery method should not be called more than once.");
            }
            _minutesSet = true;

            if (minutesIncrement > 0 && minutesIncrement <= 59)
            {
                _minutes = $"0/{minutesIncrement}";
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(minutesIncrement), "Minutes increment must be between 1 and 59.");
            }
            return this;
        }

        public CronExpressionBuilder WithMonths(params CronMonth[] months)
        {
            if (_monthsSet)
            {
                throw new InvalidOperationException("WithMonths method should not be called more than once.");
            }
            _monthsSet = true;

            if (months.Length == 0 || months.Contains(CronMonth.Any))
            {
                _months = "*";
            }
            else
            {
                _months = string.Join(",", months.Select(m => (int)m));
            }
            return this;
        }

        public CronExpressionBuilder WithSeconds(int seconds)
        {
            if (_secondsSet)
            {
                throw new InvalidOperationException("WithSeconds method should not be called more than once.");
            }
            _secondsSet = true;

            if (seconds >= 0 && seconds <= 59)
            {
                _seconds = Convert.ToString(seconds);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), "Seconds must be between 0 and 59.");
            }
            return this;
        }

        public CronExpressionBuilder WithSecondsEvery(int secondsIncrement)
        {
            if (_secondsSet)
            {
                throw new InvalidOperationException("WithSeconds or WithSecondsEvery method should not be called more than once.");
            }
            _secondsSet = true;

            if (secondsIncrement > 0 && secondsIncrement <= 59)
            {
                _seconds = $"0/{secondsIncrement}";
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(secondsIncrement), "Seconds increment must be between 1 and 59.");
            }
            return this;
        }
        public CronExpressionBuilder WithYears(params int[] years)
        {
            if (_yearsSet)
            {
                throw new InvalidOperationException("WithYears method should not be called more than once.");
            }
            _yearsSet = true;

            if (years.All(year => year >= 1970 && year.ToString().Length == 4))
            {
                _years = years.Length == 0 ? "*" : string.Join(",", years);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(years), "Years must be 4-digit numbers and greater than or equal to 1970.");
            }
            return this;
        }

        public CronExpressionBuilder WithYearsEvery(int yearsIncrement, int startYear = 1970)
        {
            if (_yearsSet)
            {
                throw new InvalidOperationException("Years methods should not be called more than once.");
            }
            _yearsSet = true;

            if (yearsIncrement > 0 && startYear >= 1970 && startYear.ToString().Length == 4)
            {
                _years = $"{startYear}/{yearsIncrement}";
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid years increment or start year.");
            }
            return this;
        }
        public CronExpressionBuilder WithYearsEvery(int yearsIncrement)
        {
            if (_yearsSet)
            {
                throw new InvalidOperationException("WithYears or WithYearsEvery method should not be called more than once.");
            }
            _yearsSet = true;

            if (yearsIncrement > 0)
            {
                _years = $"1970/{yearsIncrement}";
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(yearsIncrement), "Years increment must be greater than 0.");
            }
            return this;
        }
    }
}