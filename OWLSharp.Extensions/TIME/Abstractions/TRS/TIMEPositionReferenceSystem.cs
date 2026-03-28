/*
   Copyright 2014-2026 Marco De Salvo

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
    /// Represents a positional temporal reference system that measures time as numeric values relative to a defined origin (epoch).
    /// It specifies a reference point in time, a measurement unit, and optionally a scale semantic
    /// (large-scale for coarse granularity like geological time, or little-scale for fine granularity like Unix timestamps).
    /// Time positions are expressed as scalar distances from the origin.
    /// An optional pair of correction functions enables non-linear mappings between positional seconds and calendar seconds
    /// (e.g., leap second tables for GPS, TAI, LORAN-C).
    /// </summary>
    public sealed class TIMEPositionReferenceSystem : TIMEReferenceSystem
    {
        #region Built-Ins
        /// <summary>
        /// Built-in implementation of Unix time (POSIX time), measuring seconds elapsed since the Unix epoch (January 1, 1970, 00:00:00 UTC).
        /// This is the standard time representation used in computing systems worldwide.
        /// </summary>
        public static readonly TIMEPositionReferenceSystem Unix = new TIMEPositionReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/Unix_time"),
            TIMECoordinate.Unix,
            TIMEUnit.Second);

        /// <summary>
        /// Built-in implementation of chronometric geologic time, measuring time in millions of years before present
        /// relative to the geologic epoch (January 1, 1950, 00:00:00 UTC). Uses large-scale semantics, appropriate for
        /// geological and paleontological applications spanning millions to billions of years.
        /// </summary>
        public static readonly TIMEPositionReferenceSystem Geologic = new TIMEPositionReferenceSystem(
            new RDFResource("http://www.opengis.net/def/crs/OGC/0/ChronometricGeologicTime"),
            TIMECoordinate.Geologic,
            TIMEUnit.MillionYearsAgo,
            true);

        /// <summary>
        /// Built-in implementation of GPS time, measuring SI seconds elapsed since the GPS epoch (January 6, 1980, 00:00:00 UTC).
        /// GPS time does not include leap seconds — it diverges from UTC by the accumulated leap second offset.
        /// The correction functions account for this non-linear relationship.
        /// </summary>
        public static readonly TIMEPositionReferenceSystem GPS = new TIMEPositionReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/Global_Positioning_System#Timekeeping"),
            TIMECoordinate.GPS,
            TIMEUnit.Second,
            false,
            correctionToCalendar: LeapSecondCorrections.GPSToCalendar,
            correctionFromCalendar: LeapSecondCorrections.CalendarToGPS);

        /// <summary>
        /// Built-in implementation of International Atomic Time (TAI), measuring SI seconds elapsed since the TAI epoch (January 1, 1972, 00:00:00 UTC).
        /// TAI is a continuous time scale maintained by averaging atomic clocks worldwide.
        /// It was ahead of UTC by exactly 10 seconds at its epoch (the initial offset from the 1958-1972 pre-leap-second era)
        /// and diverges further with each leap second inserted into UTC.
        /// </summary>
        public static readonly TIMEPositionReferenceSystem TAI = new TIMEPositionReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/International_Atomic_Time"),
            TIMECoordinate.TAI, TIMEUnit.Second,
            false,
            correctionToCalendar: LeapSecondCorrections.TAIToCalendar,
            correctionFromCalendar: LeapSecondCorrections.CalendarToTAI);

        /// <summary>
        /// Built-in implementation of LORAN-C time, measuring SI seconds elapsed since the LORAN-C epoch (January 1, 1958, 00:00:00 UTC).
        /// LORAN-C time is a continuous atomic time scale that does not include leap seconds.
        /// It was synchronized with UTC at its epoch and has since diverged by the accumulated leap second offset.
        /// </summary>
        public static readonly TIMEPositionReferenceSystem LORANC = new TIMEPositionReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/LORAN#LORAN-C"),
            TIMECoordinate.LORANC, TIMEUnit.Second,
            false,
            correctionToCalendar: LeapSecondCorrections.LORANCToCalendar,
            correctionFromCalendar: LeapSecondCorrections.CalendarToLORANC);
        #endregion

        #region Properties
        /// <summary>
        /// The reference point (epoch) from which all temporal positions are measured,
        /// expressed as a calendar coordinate that anchors the positional system to an absolute moment in time
        /// </summary>
        public TIMECoordinate Origin { get; }

        /// <summary>
        /// The temporal unit used to express positions in this reference system,
        /// defining both the measurement granularity (second, year, etc.)and any scale factor applied to values
        /// </summary>
        public TIMEUnit Unit { get; }

        /// <summary>
        /// Indicates whether this system uses large-scale semantics
        /// (true for geological/astronomical time scales working at year-level granularity)
        /// or little-scale semantics (false for precise time measurements working at second-level granularity)
        /// </summary>
        public bool HasLargeScale { get; }

        /// <summary>
        /// Optional function that corrects positional seconds (from epoch) to calendar-equivalent seconds.
        /// Used by time systems that diverge from calendar time (e.g., GPS/TAI/LORAN-C which lack leap seconds).
        /// When null, the identity mapping is assumed (positional seconds == calendar seconds).
        /// </summary>
        public Func<double, double> CorrectionToCalendar { get; }

        /// <summary>
        /// Optional function that corrects calendar-equivalent seconds to positional seconds (from epoch).
        /// This is the inverse of CorrectionToCalendar.
        /// When null, the identity mapping is assumed (calendar seconds == positional seconds).
        /// </summary>
        public Func<double, double> CorrectionFromCalendar { get; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a positional TRS with the given name, origin coordinate, temporal unit, scale behavior,
        /// and optional non-linear correction functions for systems that diverge from calendar time
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEPositionReferenceSystem(RDFResource trsUri, TIMECoordinate trsOrigin, TIMEUnit trsUnit,
            bool hasLargeScale=false,
            Func<double, double> correctionToCalendar=null,
            Func<double, double> correctionFromCalendar=null)
            : base(trsUri)
        {
            Origin = trsOrigin ?? throw new OWLException($"Cannot create PositionReferenceSystem because given '{nameof(trsOrigin)}' parameter is null");
            Unit = trsUnit ?? throw new OWLException($"Cannot create PositionReferenceSystem because given '{nameof(trsUnit)}' parameter is null");
            HasLargeScale = hasLargeScale;
            CorrectionToCalendar = correctionToCalendar;
            CorrectionFromCalendar = correctionFromCalendar;
        }
        #endregion
    }

    /// <summary>
    /// Provides leap second correction functions for positional time systems (GPS, TAI, LORAN-C)
    /// that maintain continuous second counts without UTC leap second insertions.
    /// The leap second table covers all insertions from the inception of the leap second system (1972)
    /// through the most recent insertion (January 1, 2017).
    /// Each entry represents a UTC date at which a leap second was inserted, expressed as
    /// Gregorian calendar seconds from year 0 (computed via the coordinate system origin).
    /// </summary>
    internal static class LeapSecondCorrections
    {
        #region Leap Second Table
        /// <summary>
        /// UTC leap second insertion points expressed as absolute Gregorian seconds from year 0.
        /// Each entry is the absolute-seconds value of the UTC date when a leap second was inserted.
        /// The table is ordered chronologically.
        /// Computed as: year * 365.2425 * 86400 + (month-1) * 30.436875 * 86400 + (day-1) * 86400
        /// using average Gregorian year/month lengths for a stable, leap-year-independent reference.
        /// </summary>
        private static readonly double[] LeapSecondInsertions = ComputeLeapSecondTable();

        private static double[] ComputeLeapSecondTable()
        {
            // UTC dates of all leap second insertions (year, month, day)
            int[][] dates =
            {
                new[] {1972, 7, 1},   // +1 (cumulative: 11, since TAI was already +10 at 1972-01-01)
                new[] {1973, 1, 1},   // +1 (12)
                new[] {1974, 1, 1},   // +1 (13)
                new[] {1975, 1, 1},   // +1 (14)
                new[] {1976, 1, 1},   // +1 (15)
                new[] {1977, 1, 1},   // +1 (16)
                new[] {1978, 1, 1},   // +1 (17)
                new[] {1979, 1, 1},   // +1 (18)
                new[] {1980, 1, 1},   // +1 (19)
                new[] {1981, 7, 1},   // +1 (20)
                new[] {1982, 7, 1},   // +1 (21)
                new[] {1983, 7, 1},   // +1 (22)
                new[] {1985, 7, 1},   // +1 (23)
                new[] {1988, 1, 1},   // +1 (24)
                new[] {1990, 1, 1},   // +1 (25)
                new[] {1991, 1, 1},   // +1 (26)
                new[] {1992, 7, 1},   // +1 (27)
                new[] {1993, 7, 1},   // +1 (28)
                new[] {1994, 7, 1},   // +1 (29)
                new[] {1996, 1, 1},   // +1 (30)
                new[] {1997, 7, 1},   // +1 (31)
                new[] {1999, 1, 1},   // +1 (32)
                new[] {2006, 1, 1},   // +1 (33)
                new[] {2009, 1, 1},   // +1 (34)
                new[] {2012, 7, 1},   // +1 (35)
                new[] {2015, 7, 1},   // +1 (36)
                new[] {2017, 1, 1},   // +1 (37)
            };

            double[] table = new double[dates.Length];
            for (int i = 0; i < dates.Length; i++)
            {
                TIMECoordinate coord = new TIMECoordinate(dates[i][0], dates[i][1], dates[i][2], 0, 0, 0);
                TIMECoordinate normalized = TIMEConverter.NormalizeCoordinate(coord, TIMECalendarReferenceSystem.Gregorian);
                table[i] = TIMEConverter.CoordinateToAbsoluteSeconds(normalized, TIMECalendarReferenceSystem.Gregorian);
            }
            return table;
        }

        /// <summary>
        /// Returns the number of UTC leap seconds accumulated at the given absolute Gregorian seconds from year 0.
        /// This counts how many leap second insertions have occurred up to (but not including) the given moment.
        /// </summary>
        private static double GetLeapSecondsAtAbsolute(double absoluteGregorianSeconds)
        {
            int count = 0;
            for (int i = 0; i < LeapSecondInsertions.Length; i++)
            {
                if (absoluteGregorianSeconds >= LeapSecondInsertions[i])
                    count++;
                else
                    break;
            }
            return count;
        }
        #endregion

        #region Epoch References (absolute Gregorian seconds)
        private static readonly double GPSEpochAbsolute;
        private static readonly double TAIEpochAbsolute;
        private static readonly double LORANCEpochAbsolute;

        static LeapSecondCorrections()
        {
            GPSEpochAbsolute = CoordinateToAbsolute(TIMECoordinate.GPS);
            TAIEpochAbsolute = CoordinateToAbsolute(TIMECoordinate.TAI);
            LORANCEpochAbsolute = CoordinateToAbsolute(TIMECoordinate.LORANC);
        }

        private static double CoordinateToAbsolute(TIMECoordinate coord)
        {
            TIMECoordinate normalized = TIMEConverter.NormalizeCoordinate(coord, TIMECalendarReferenceSystem.Gregorian);
            return TIMEConverter.CoordinateToAbsoluteSeconds(normalized, TIMECalendarReferenceSystem.Gregorian);
        }
        #endregion

        #region GPS Corrections
        /// <summary>
        /// Corrects GPS positional seconds to calendar-equivalent seconds.
        /// GPS does not include leap seconds, so GPS time runs ahead of UTC.
        /// To get the UTC calendar equivalent, subtract the accumulated leap seconds since GPS epoch.
        /// At GPS epoch (1980-01-06), UTC had accumulated 19 leap seconds since 1972.
        /// GPS was synchronized with UTC at that moment, so GPS drift = leapSeconds(now) - 19.
        /// </summary>
        internal static double GPSToCalendar(double gpsSeconds)
        {
            double absoluteSeconds = GPSEpochAbsolute + gpsSeconds;
            double leapSecondsNow = GetLeapSecondsAtAbsolute(absoluteSeconds);
            // At GPS epoch, 9 leap seconds had been inserted (entries 0-8 in the table, i.e., 1972-07-01 through 1980-01-01)
            double leapSecondsSinceGPSEpoch = leapSecondsNow - 9;
            return gpsSeconds - leapSecondsSinceGPSEpoch;
        }

        /// <summary>
        /// Corrects calendar-equivalent seconds to GPS positional seconds (inverse of GPSToCalendar).
        /// </summary>
        internal static double CalendarToGPS(double calendarSeconds)
        {
            double absoluteSeconds = GPSEpochAbsolute + calendarSeconds;
            double leapSecondsNow = GetLeapSecondsAtAbsolute(absoluteSeconds);
            double leapSecondsSinceGPSEpoch = leapSecondsNow - 9;
            return calendarSeconds + leapSecondsSinceGPSEpoch;
        }
        #endregion

        #region TAI Corrections
        /// <summary>
        /// Corrects TAI positional seconds to calendar-equivalent seconds.
        /// TAI epoch is 1972-01-01. At that moment, TAI was already ahead of UTC by 10 seconds
        /// (the initial offset accumulated during the pre-leap-second era 1958-1972).
        /// TAI seconds = UTC seconds + 10 + (leap seconds since 1972), so to get UTC:
        /// calendarSeconds = taiSeconds - 10 - leapSecondsSinceEpoch.
        /// </summary>
        internal static double TAIToCalendar(double taiSeconds)
        {
            double absoluteSeconds = TAIEpochAbsolute + taiSeconds;
            double leapSecondsSinceEpoch = GetLeapSecondsAtAbsolute(absoluteSeconds);
            return taiSeconds - 10 - leapSecondsSinceEpoch;
        }

        /// <summary>
        /// Corrects calendar-equivalent seconds to TAI positional seconds (inverse of TAIToCalendar).
        /// </summary>
        internal static double CalendarToTAI(double calendarSeconds)
        {
            double absoluteSeconds = TAIEpochAbsolute + calendarSeconds;
            double leapSecondsSinceEpoch = GetLeapSecondsAtAbsolute(absoluteSeconds);
            return calendarSeconds + 10 + leapSecondsSinceEpoch;
        }
        #endregion

        #region LORAN-C Corrections
        /// <summary>
        /// Corrects LORAN-C positional seconds to calendar-equivalent seconds.
        /// LORAN-C epoch is 1958-01-01 and was synchronized with UTC at that moment.
        /// LORAN-C runs continuously without leap seconds, so it diverges from UTC
        /// by the accumulated leap seconds since 1972 (no leap seconds existed before 1972).
        /// </summary>
        internal static double LORANCToCalendar(double lorancSeconds)
        {
            double absoluteSeconds = LORANCEpochAbsolute + lorancSeconds;
            double leapSeconds = GetLeapSecondsAtAbsolute(absoluteSeconds);
            return lorancSeconds - leapSeconds;
        }

        /// <summary>
        /// Corrects calendar-equivalent seconds to LORAN-C positional seconds (inverse of LORANCToCalendar).
        /// </summary>
        internal static double CalendarToLORANC(double calendarSeconds)
        {
            double absoluteSeconds = LORANCEpochAbsolute + calendarSeconds;
            double leapSeconds = GetLeapSecondsAtAbsolute(absoluteSeconds);
            return calendarSeconds + leapSeconds;
        }
        #endregion
    }
}