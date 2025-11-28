/*
   Copyright 2014-2025 Marco De Salvo

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using RDFSharp.Model;
using System;
using System.Linq;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// Represents a calendar-based temporal reference system that defines how time is measured and organized
    /// through cyclic units (years, months, days, hours, minutes, seconds). It includes metrics specifying
    /// the structure of these units, rules for leap years, and the relationships between temporal divisions.
    /// Examples include the Gregorian calendar, Julian calendar, or any custom calendar system with defined periodicity.
    /// </summary>
    public sealed class TIMECalendarReferenceSystem : TIMEReferenceSystem
    {
        #region Built-Ins
        /// <summary>
        /// Built-in implementation of the Gregorian calendar TRS with standard temporal metrics
        /// (60 seconds per minute, 60 minutes per hour, 24 hours per day) and month lengths following
        /// the common calendar structure. Includes the leap year rule that adds an extra day to February
        /// (29 days instead of 28) for years divisible by 4, except century years unless divisible by 400,
        /// applicable from 1582 onwards when the Gregorian calendar was officially introduced by Pope Gregory XIII.
        /// </summary>
        public static readonly TIMECalendarReferenceSystem Gregorian = new TIMECalendarReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/Gregorian_calendar"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 24, new uint[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 })
                .SetLeapYearRule(year => {
                    return year >= 1582 && ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                        ? new uint[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }
                        : new uint[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                }));

        /// <summary>
        /// Built-in implementation of the Julian calendar TRS with the same temporal metrics as the Gregorian calendar
        /// (60 seconds per minute, 60 minutes per hour, 24 hours per day) and identical month structure.
        /// The key difference is the simpler leap year rule: every year divisible by 4 is a leap year,
        /// without the century exceptions present in the Gregorian calendar.
        /// This system was introduced by Julius Caesar in 45 BCE and remained the predominant calendar
        /// in the Western world until the Gregorian reform of 1582.
        /// </summary>
        public static readonly TIMECalendarReferenceSystem Julian = new TIMECalendarReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/Julian_calendar"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 24, new uint[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 })
                .SetLeapYearRule(year => {
                    return year % 4 == 0
                        ? new uint[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }
                        : new uint[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                }));
        #endregion

        #region Properties
        /// <summary>
        /// The metrics defining the mathematical structure and temporal organization of this calendar TRS,
        /// including unit relationships (seconds/minutes/hours/days), month configurations, and leap year rules
        /// used for all temporal calculations and conversions.
        /// </summary>
        public TIMECalendarReferenceSystemMetrics Metrics { get; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a calendar TRS with the given name and the given metrics
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMECalendarReferenceSystem(RDFResource trsUri, TIMECalendarReferenceSystemMetrics trsMetrics) : base(trsUri)
            => Metrics = trsMetrics ?? throw new OWLException($"Cannot create calendar-based TRS because given '{nameof(trsMetrics)}' parameter is null");
        #endregion
    }

    /// <summary>
    /// Encapsulates the mathematical structure and rules that define how time is measured and organized
    /// within a calendar-based temporal reference system. It specifies the fundamental temporal units
    /// (seconds per minute, minutes per hour, hours per day), the month structure with day counts for each month,
    /// and optionally a leap year rule for handling calendar variations.
    /// The class also provides derived properties like total days per year, number of months, and a flag indicating
    /// whether the calendar has exact (non-variable) metrics, enabling the framework to perform accurate temporal
    /// calculations while remaining agnostic to any specific calendar implementation.
    /// </summary>
    public sealed class TIMECalendarReferenceSystemMetrics
    {
        #region Properties
        /// <summary>
        /// Defines the number of seconds in one minute for this calendar system (e.g: 60 in most Earth-based calendars)
        /// </summary>
        public uint SecondsInMinute { get; }

        /// <summary>
        /// Defines the number of minutes in one hour for this calendar system (e.g: 60 in most Earth-based calendars)
        /// </summary>
        public uint MinutesInHour { get; }

        /// <summary>
        /// Defines the number of hours in one day for this calendar system (e.g: 24 in most Earth-based calendars)
        /// </summary>
        public uint HoursInDay { get; }

        /// <summary>
        /// An array defining the number of days in each month of a standard (non-leap) year,
        /// with array length determining the number of months in the calendar
        /// </summary>
        public uint[] Months { get; }

        //Derived

        /// <summary>
        /// Derived property that calculates the total number of days in a standard year by summing all month lengths
        /// </summary>
        public uint DaysInYear { get; }

        /// <summary>
        /// Derived property that returns the total number of months in the calendar year (the length of the Months array)
        /// </summary>
        public uint MonthsInYear { get; }

        /// <summary>
        /// Derived property indicating whether the calendar has fixed, non-variable metrics,
        /// determining if months/years can be precisely calculated or must be approximated as days
        /// </summary>
        public bool HasExactMetric { get; }

        /// <summary>
        /// Optional function that takes a year as input and returns the month structure for that specific year,
        /// enabling variable-length years (e.g., leap years with 29 days in February instead of 28)
        /// </summary>
        public Func<double,uint[]> LeapYearRule { get; internal set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds the metrics for a calendar TRS with the given specifications
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMECalendarReferenceSystemMetrics(uint secondsInMinute, uint minutesInHour, uint hoursInDay, uint[] months)
        {
            #region Guards
            if (secondsInMinute == 0)
                throw new OWLException($"Cannot build calendar metrics because given '{nameof(secondsInMinute)}' parameter must be greater than zero");
            if (minutesInHour == 0)
                throw new OWLException($"Cannot build calendar metrics because given '{nameof(minutesInHour)}' parameter must be greater than zero");
            if (hoursInDay == 0)
                throw new OWLException($"Cannot build calendar metrics because given '{nameof(hoursInDay)}' parameter must be greater than zero");
            if (months == null)
                throw new OWLException($"Cannot build calendar metrics because given '{nameof(months)}' parameter is null");
            if (months.Length == 0)
                throw new OWLException($"Cannot build calendar metrics because given '{nameof(months)}' parameter must contain at least one element");
            if (months.Contains<uint>(0))
                throw new OWLException($"Cannot build calendar metrics because given '{nameof(months)}' parameter must contain all elements greater than zerp");
            #endregion

            SecondsInMinute = secondsInMinute;
            MinutesInHour = minutesInHour;
            HoursInDay = hoursInDay;
            Months = months;

            //Derived
            DaysInYear = Convert.ToUInt32(months.Sum(m => m));
            MonthsInYear = Convert.ToUInt32(months.Length);
            HasExactMetric = months.Distinct().Count() == 1;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the function for determining if a given working year is leap or not,
        /// so that the Months array of the calendar TRS automatically reflects this
        /// </summary>
        public TIMECalendarReferenceSystemMetrics SetLeapYearRule(Func<double,uint[]> leapYearRule)
        {
            LeapYearRule = leapYearRule;
            return this;
        }
        #endregion
    }
}