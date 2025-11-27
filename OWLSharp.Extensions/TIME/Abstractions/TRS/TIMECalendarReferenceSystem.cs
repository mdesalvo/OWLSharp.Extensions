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
    /// 
    /// </summary>
    public sealed class TIMECalendarReferenceSystem : TIMEReferenceSystem
    {
        #region Built-Ins
        /// <summary>
        /// 
        /// </summary>
        public static readonly TIMECalendarReferenceSystem Gregorian = new TIMECalendarReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/Gregorian_calendar"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 24, new uint[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 })
                .SetLeapYearRule(year => {
                    return year >= 1582 && ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                        ? new uint[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }
                        : new uint[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                }));
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public TIMECalendarReferenceSystemMetrics Metrics { get; }
        #endregion

        #region Ctors
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMECalendarReferenceSystem(RDFResource trsUri, TIMECalendarReferenceSystemMetrics trsMetrics) : base(trsUri)
            => Metrics = trsMetrics ?? throw new OWLException($"Cannot create calendar-based TRS because given '{nameof(trsMetrics)}' parameter is null");
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class TIMECalendarReferenceSystemMetrics
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public uint SecondsInMinute { get; }

        /// <summary>
        /// 
        /// </summary>
        public uint MinutesInHour { get; }

        /// <summary>
        /// 
        /// </summary>
        public uint HoursInDay { get; }

        /// <summary>
        /// 
        /// </summary>
        public uint[] Months { get; }

        //Derived

        /// <summary>
        /// 
        /// </summary>
        public uint DaysInYear { get; }

        /// <summary>
        /// 
        /// </summary>
        public uint MonthsInYear { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasExactMetric { get; }

        /// <summary>
        /// 
        /// </summary>
        public Func<double,uint[]> LeapYearRule { get; internal set; }
        #endregion

        #region Ctors
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public TIMECalendarReferenceSystemMetrics SetLeapYearRule(Func<double,uint[]> leapYearRule)
        {
            LeapYearRule = leapYearRule;
            return this;
        }
        #endregion
    }
}