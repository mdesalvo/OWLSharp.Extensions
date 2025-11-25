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
    /// In OWL-TIME, a temporal extent represents the duration or span of time associated with a temporal entity.
    /// It quantifies how long an interval lasts or the temporal distance between two instants.
    /// A temporal extent is typically expressed as a numerical value combined with a temporal unit
    /// (such as 5 days, 3 hours, or 2.5 years). It provides a measurable magnitude for temporal phenomena,
    /// enabling comparisons and calculations involving durations.
    /// Temporal extents are fundamental for describing the length of events, processes, or any time-bounded entities in the domain.
    /// </summary>
    public sealed class TIMEExtent : IComparable<TIMEExtent>, IEquatable<TIMEExtent>
    {
        #region Built-Ins
        /// <summary>
        /// An empty temporal extent
        /// </summary>
        public static readonly TIMEExtent Zero = new TIMEExtent(0, 0, 0, 0, 0, 0, 0);
        #endregion

        #region Properties
        /// <summary>
        /// Indicates the component of the temporal extent giving the years
        /// </summary>
        public double? Years { get; internal set; }
        /// <summary>
        /// Indicates the component of the temporal extent giving the months
        /// </summary>
        public double? Months { get; internal set; }
        /// <summary>
        /// Indicates the component of the temporal extent giving the weeks
        /// </summary>
        public double? Weeks { get; internal set; }
        /// <summary>
        /// Indicates the component of the temporal extent giving the days
        /// </summary>
        public double? Days { get; internal set; }
        /// <summary>
        /// Indicates the component of the temporal extent giving the hours
        /// </summary>
        public double? Hours { get; internal set; }
        /// <summary>
        /// Indicates the component of the temporal extent giving the minutes
        /// </summary>
        public double? Minutes { get; internal set; }
        /// <summary>
        /// Indicates the component of the temporal extent giving the seconds
        /// </summary>
        public double? Seconds { get; internal set; }
        /// <summary>
        /// Indicates the metadata describing the metrics of the temporal extent
        /// </summary>
        public TIMEExtentMetadata Metadata { get; internal set; }
        #endregion

        #region Ctors
        internal TIMEExtent() { }

        /// <summary>
        /// Builds a temporal extent from the given components and metadata
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEExtent(double? years, double? months, double? weeks, double? days,
            double? hours, double? minutes, double? seconds, TIMEExtentMetadata metadata=null)
        {
            #region Guards
            if (years < 0)
                throw new OWLException($"Cannot create temporal extent because given '{nameof(years)}' parameter is negative");
            if (months < 0)
                throw new OWLException($"Cannot create temporal extent because given '{nameof(months)}' parameter is negative");
            if (weeks < 0)
                throw new OWLException($"Cannot create temporal extent because given '{nameof(weeks)}' parameter is negative");
            if (days < 0)
                throw new OWLException($"Cannot create temporal extent because given '{nameof(days)}' parameter is negative");
            if (hours < 0)
                throw new OWLException($"Cannot create temporal extent because given '{nameof(hours)}' parameter is negative");
            if (minutes < 0)
                throw new OWLException($"Cannot create temporal extent because given '{nameof(minutes)}' parameter is negative");
            if (seconds < 0)
                throw new OWLException($"Cannot create temporal extent because given '{nameof(seconds)}' parameter is negative");
            #endregion

            Years = years;
            Months = months;
            Weeks = weeks;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Metadata = metadata ?? new TIMEExtentMetadata();
        }

        /// <summary>
        /// Builds a temporal extent from the given TimeSpan (so that it implicitly uses Gregorian TRS)
        /// </summary>
        public TIMEExtent(TimeSpan timeSpan)
        {
            Days = timeSpan.Days;
            Hours = timeSpan.Hours;
            Minutes = timeSpan.Minutes;
            Seconds = timeSpan.Seconds;
            Metadata = new TIMEExtentMetadata(TIMECalendarReferenceSystem.Gregorian);
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Compares this temporal extent to the given one
        /// </summary>
        public int CompareTo(TIMEExtent other)
        {
            if (other == null)
                return 1;

            //Compare years
            double thisYears = Years ?? Zero.Years.Value;
            double otherYears = other.Years ?? Zero.Years.Value;
            if (thisYears != otherYears)
                return thisYears.CompareTo(otherYears);

            //Compare months
            double thisMonths = Months ?? Zero.Months.Value;
            double otherMonths = other.Months ?? Zero.Months.Value;
            if (thisMonths != otherMonths)
                return thisMonths.CompareTo(otherMonths);

            //Compare weeks
            double thisWeeks = Weeks ?? Zero.Weeks.Value;
            double otherWeeks = other.Weeks ?? Zero.Weeks.Value;
            if (thisWeeks != otherWeeks)
                return thisWeeks.CompareTo(otherWeeks);

            //Compare days
            double thisDays = Days ?? Zero.Days.Value;
            double otherDays = other.Days ?? Zero.Days.Value;
            if (thisDays != otherDays)
                return thisDays.CompareTo(otherDays);

            //Compare hours
            double thisHours = Hours ?? Zero.Hours.Value;
            double otherHours = other.Hours ?? Zero.Hours.Value;
            if (thisHours != otherHours)
                return thisHours.CompareTo(otherHours);

            //Compare minutes
            double thisMinutes = Minutes ?? Zero.Minutes.Value;
            double otherMinutes = other.Minutes ?? Zero.Minutes.Value;
            if (thisMinutes != otherMinutes)
                return thisMinutes.CompareTo(otherMinutes);

            //Compare seconds
            double thisSeconds = Seconds ?? Zero.Seconds.Value;
            double otherSeconds = other.Seconds ?? Zero.Seconds.Value;
            if (thisSeconds != otherSeconds)
                return thisSeconds.CompareTo(otherSeconds);

            return 0;
        }

        /// <summary>
        /// Compares this temporal extent to the given one for equality
        /// </summary>
        public bool Equals(TIMEExtent other)
            => CompareTo(other) == 0;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the string representation of this temporal extent
        /// </summary>
        public override string ToString()
            => $"{Years ?? 0}_{Months ?? 0}_{Weeks ?? 0}_{Days ?? 0}_{Hours ?? 0}_{Minutes ?? 0}_{Seconds ?? 0}";
        #endregion
    }

    /// <summary>
    /// Metadata describing the metrics of a temporal extent
    /// </summary>
    public sealed class TIMEExtentMetadata
    {
        #region Properties
        /// <summary>
        /// IRI of the TRS used by a temporal extent
        /// </summary>
        public RDFResource TRS { get; }
        #endregion

        #region Ctors
        internal TIMEExtentMetadata() { }

        /// <summary>
        /// Builds a temporal extent metadata with the given IRI for the TRS
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEExtentMetadata(RDFResource trsUri)
            => TRS = trsUri ?? throw new OWLException($"Cannot create extent metadata because given '{nameof(trsUri)}' parameter is null");
        #endregion
    }
}