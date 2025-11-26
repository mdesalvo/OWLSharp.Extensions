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

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// In the OWL-TIME context, TIMECoordinate represents a temporal position expressed through six fundamental dimensions:
    /// year, month, day, hour, minute, and second. It serves as a decomposed representation of a specific point in time
    /// within a given Temporal Reference System (TRS). Each dimension is nullable, allowing for partial or granular
    /// temporal specifications where not all components need to be defined. The class includes predefined epoch coordinates
    /// like Unix time (1970-01-01) and geologic time (1950-01-01), and carries metadata that describes the associated TRS,
    /// temporal units, and contextual calendar information. It implements comparison and equality operations,
    /// enabling temporal reasoning and ordering of temporal entities in accordance with OWL-TIME semantics.
    /// </summary>
    public sealed class TIMECoordinate : IComparable<TIMECoordinate>, IEquatable<TIMECoordinate>
    {
        #region Built-Ins
        /// <summary>
        /// Represents the origin point (year 0) in a temporal coordinate system, serving as the absolute baseline for temporal calculations
        /// </summary>
        public static readonly TIMECoordinate Zero = new TIMECoordinate(0, 0, 0, 0, 0, 0);

        /// <summary>
        /// Represents the Unix epoch (January 1, 1970, 00:00:00 UTC), the standard reference point for Unix timestamp systems
        /// </summary>
        public static readonly TIMECoordinate UnixTime = new TIMECoordinate(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Represents the geologic time epoch (January 1, 1950, 00:00:00 UTC), commonly used as the reference point for
        /// radiocarbon dating and geological measurements
        /// </summary>
        public static readonly TIMECoordinate GeologicTime = new TIMECoordinate(1950, 1, 1, 0, 0, 0);
        #endregion

        #region Properties
        /// <summary>
        /// Indicates the component of the temporal coordinate giving the year
        /// </summary>
        public double? Year { get; internal set; }

        /// <summary>
        /// Indicates the component of the temporal coordinate giving the month
        /// </summary>
        public double? Month { get; internal set; }

        /// <summary>
        /// Indicates the component of the temporal coordinate giving the day
        /// </summary>
        public double? Day { get; internal set; }

        /// <summary>
        /// Indicates the component of the temporal coordinate giving the hour
        /// </summary>
        public double? Hour { get; internal set; }

        /// <summary>
        /// Indicates the component of the temporal coordinate giving the minute
        /// </summary>
        public double? Minute { get; internal set; }

        /// <summary>
        /// Indicates the component of the temporal coordinate giving the second
        /// </summary>
        public double? Second { get; internal set; }

        /// <summary>
        /// Contains contextual information about the coordinate including its associated TRS, temporal units,
        /// and calendar-specific details like day of week and day of year
        /// </summary>
        public TIMECoordinateMetadata Metadata { get; internal set; }
        #endregion

        #region Ctors
        internal TIMECoordinate() { }

        /// <summary>
        /// Builds a temporal coordinate from the given components and metadata
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMECoordinate(double? year, double? month, double? day,
            double? hour, double? minute, double? second, TIMECoordinateMetadata metadata=null)
        {
            #region Guards
            if (month < 0)
                throw new OWLException($"Cannot create temporal coordinate because given '{nameof(month)}' parameter is negative");
            if (day < 0)
                throw new OWLException($"Cannot create temporal coordinate because given '{nameof(day)}' parameter is negative");
            if (hour < 0)
                throw new OWLException($"Cannot create temporal coordinate because given '{nameof(hour)}' parameter is negative");
            if (minute < 0)
                throw new OWLException($"Cannot create temporal coordinate because given '{nameof(minute)}' parameter is negative");
            if (second < 0)
                throw new OWLException($"Cannot create temporal coordinate because given '{nameof(second)}' parameter is negative");
            #endregion

            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Metadata = metadata ?? new TIMECoordinateMetadata();
        }

        /// <summary>
        /// Builds a temporal coordinate from the given DateTime (so that it implicitly uses Gregorian TRS)
        /// </summary>
        public TIMECoordinate(DateTime dateTime)
        {
            DateTime utcDateTime = dateTime.ToUniversalTime();

            Year = utcDateTime.Year;
            Month = utcDateTime.Month;
            Day = utcDateTime.Day;
            Hour = utcDateTime.Hour;
            Minute = utcDateTime.Minute;
            Second = utcDateTime.Second;
            Metadata = new TIMECoordinateMetadata(
                TIMECalendarReferenceSystem.Gregorian,
                RDFVocabulary.TIME.UNIT_SECOND,
                TIMEHelper.GetMonthOfYear(utcDateTime.Month),
                TIMEHelper.GetDayOfWeek(utcDateTime.DayOfWeek),
                Convert.ToUInt32(utcDateTime.DayOfYear));
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Compares this temporal coordinate to the given one
        /// </summary>
        public int CompareTo(TIMECoordinate other)
        {
            if (other == null)
                return 1;

            //Compare year
            double thisYear = Year ?? Zero.Year.Value;
            double otherYear = other.Year ?? Zero.Year.Value;
            if (thisYear != otherYear)
                return thisYear.CompareTo(otherYear);

            //Compare month
            double thisMonth = Month ?? Zero.Month.Value;
            double otherMonth = other.Month ?? Zero.Month.Value;
            if (thisMonth != otherMonth)
                return thisMonth.CompareTo(otherMonth);

            //Compare day
            double thisDay = Day ?? Zero.Day.Value;
            double otherDay = other.Day ?? Zero.Day.Value;
            if (thisDay != otherDay)
                return thisDay.CompareTo(otherDay);

            //Compare hour
            double thisHour = Hour ?? Zero.Hour.Value;
            double otherHour = other.Hour ?? Zero.Hour.Value;
            if (thisHour != otherHour)
                return thisHour.CompareTo(otherHour);

            //Compare minute
            double thisMinute = Minute ?? Zero.Minute.Value;
            double otherMinute = other.Minute ?? Zero.Minute.Value;
            if (thisMinute != otherMinute)
                return thisMinute.CompareTo(otherMinute);

            //Compare second
            double thisSecond = Second ?? Zero.Second.Value;
            double otherSecond = other.Second ?? Zero.Second.Value;
            if (thisSecond != otherSecond)
                return thisSecond.CompareTo(otherSecond);

            return 0;
        }

        /// <summary>
        /// Compares this temporal coordinate to the given one for equality
        /// </summary>
        public bool Equals(TIMECoordinate other)
            => CompareTo(other) == 0;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the string representation of this temporal coordinate
        /// </summary>
        public override string ToString()
            => $"{Year ?? 0}_{Month ?? 0}_{Day ?? 0}_{Hour ?? 0}_{Minute ?? 0}_{Second ?? 0}";
        #endregion
    }

    /// <summary>
    /// TIMECoordinateMetadata provides contextual semantic information about a TIMECoordinate,
    /// describing the temporal reference system, measurement units, and calendar-specific attributes
    /// that give meaning to the coordinate's numeric values within the OWL-TIME ontology framework
    /// </summary>
    public sealed class TIMECoordinateMetadata
    {
        #region Properties
        /// <summary>
        /// IRI of the Temporal Reference System that defines how the coordinate's values
        /// should be interpreted and positioned in time
        /// </summary>
        public RDFResource TRS { get; internal set; }

        /// <summary>
        /// IRI of the temporal unit used for measuring durations and expressing the
        /// coordinate's precision, such as seconds, minutes, or hours
        /// </summary>
        public RDFResource UnitType { get; internal set; }

        /// <summary>
        /// An optional reference to the semantic representation of a month subdivision
        /// within the coordinate's TRS, if applicable to that system
        /// </summary>
        public RDFResource MonthOfYear { get; internal set; }

        /// <summary>
        /// An optional reference to the semantic representation of a cyclic week subdivision
        /// within the coordinate's TRS, if such a concept exists in that system
        /// </summary>
        public RDFResource DayOfWeek { get; internal set; }

        /// <summary>
        /// An optional numeric value representing the ordinal position within a year cycle
        /// of the coordinate's TRS, if the system defines such a yearly subdivision
        /// </summary>
        public uint? DayOfYear { get; internal set; }
        #endregion

        #region Ctors
        internal TIMECoordinateMetadata() { }

        /// <summary>
        /// Builds a temporal coordinate metadata with the given IRI for the TRS,
        /// the given IRI for the temporal unit and other descriptive attributes
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMECoordinateMetadata(RDFResource trsUri, RDFResource unitTypeUri,
            RDFResource monthOfYear=null, RDFResource dayOfWeek=null, uint? dayOfYear=null)
        {
            TRS = trsUri ?? throw new OWLException($"Cannot create coordinate metadata because given '{nameof(trsUri)}' parameter is null");
            UnitType = unitTypeUri ?? throw new OWLException($"Cannot create coordinate metadata because given '{nameof(unitTypeUri)}' parameter is null");
            MonthOfYear = monthOfYear;
            DayOfWeek = dayOfWeek;
            DayOfYear = dayOfYear;
        }
        #endregion
    }
}