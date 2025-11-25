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

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// In OWL-TIME, a temporal unit represents a standard unit for measuring temporal duration or extent.
    /// It serves as a reference measure for quantifying temporal durations, such as seconds, minutes, hours,
    /// days, weeks, months, or years. Temporal units are essential for expressing the magnitude of durations
    /// in a standardized, interoperable way. They are typically drawn from standardized systems like the
    /// International System of Units (SI) or conventional calendar units, enabling consistent temporal measurements
    /// across different applications and domains.
    /// </summary>
    public sealed class TIMEUnit : RDFResource
    {
        #region Built-ins
        /// <summary>
        /// A temporal unit of 1000 years
        /// </summary>
        public static readonly TIMEUnit Millennium = new TIMEUnit(RDFVocabulary.TIME.UNIT_MILLENIUM, TIMEEnums.TIMEUnitType.Year, 1000);
        /// <summary>
        /// A temporal unit of 100 years
        /// </summary>
        public static readonly TIMEUnit Century = new TIMEUnit(RDFVocabulary.TIME.UNIT_CENTURY, TIMEEnums.TIMEUnitType.Year, 100);
        /// <summary>
        /// A temporal unit of 10 years
        /// </summary>
        public static readonly TIMEUnit Decade = new TIMEUnit(RDFVocabulary.TIME.UNIT_DECADE, TIMEEnums.TIMEUnitType.Year, 10);
        /// <summary>
        /// A temporal unit of 1 year
        /// </summary>
        public static readonly TIMEUnit Year = new TIMEUnit(RDFVocabulary.TIME.UNIT_YEAR, TIMEEnums.TIMEUnitType.Year, 1);
        /// <summary>
        /// A temporal unit of 1 month
        /// </summary>
        public static readonly TIMEUnit Month = new TIMEUnit(RDFVocabulary.TIME.UNIT_MONTH, TIMEEnums.TIMEUnitType.Month, 1);
        /// <summary>
        /// A temporal unit of 7 days
        /// </summary>
        public static readonly TIMEUnit Week = new TIMEUnit(RDFVocabulary.TIME.UNIT_WEEK, TIMEEnums.TIMEUnitType.Day, 7);
        /// <summary>
        /// A temporal unit of 1 day
        /// </summary>
        public static readonly TIMEUnit Day = new TIMEUnit(RDFVocabulary.TIME.UNIT_DAY, TIMEEnums.TIMEUnitType.Day, 1);
        /// <summary>
        /// A temporal unit of 1 hour
        /// </summary>
        public static readonly TIMEUnit Hour = new TIMEUnit(RDFVocabulary.TIME.UNIT_HOUR, TIMEEnums.TIMEUnitType.Hour, 1);
        /// <summary>
        /// A temporal unit of 1 minute
        /// </summary>
        public static readonly TIMEUnit Minute = new TIMEUnit(RDFVocabulary.TIME.UNIT_MINUTE, TIMEEnums.TIMEUnitType.Minute, 1);
        /// <summary>
        /// A temporal unit of 1 second
        /// </summary>
        public static readonly TIMEUnit Second = new TIMEUnit(RDFVocabulary.TIME.UNIT_SECOND, TIMEEnums.TIMEUnitType.Second, 1);

        //Derived

        /// <summary>
        /// A temporal unit of negative 1B years
        /// </summary>
        public static readonly TIMEUnit BillionYearsAgo = new TIMEUnit(new RDFResource("https://en.wikipedia.org/wiki/Bya"), TIMEEnums.TIMEUnitType.Year, -1_000_000_000);

        /// <summary>
        /// A temporal unit of negative 1M years
        /// </summary>
        public static readonly TIMEUnit MillionYearsAgo = new TIMEUnit(new RDFResource("https://en.wikipedia.org/wiki/Million_years_ago"), TIMEEnums.TIMEUnitType.Year, -1_000_000);

        /// <summary>
        /// A temporal unit of 1.02749125 days (which is the length of a "Sol" on Mars)
        /// </summary>
        public static readonly TIMEUnit MarsSol = new TIMEUnit(new RDFResource("https://en.wikipedia.org/wiki/Mars_sol"), TIMEEnums.TIMEUnitType.Day, 1.02749125);
        #endregion

        #region Properties
        /// <summary>
        /// Indicates the reference unit type on which this temporal unit is built
        /// </summary>
        public TIMEEnums.TIMEUnitType UnitType { get; }

        /// <summary>
        /// Indicates the scale factor to be applied on the reference unit type for computating this temporal unit
        /// </summary>
        public double ScaleFactor { get; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a temporal unit with the given name, the given reference unit type and scale factor
        /// </summary>
        public TIMEUnit(RDFResource timeUnitUri, TIMEEnums.TIMEUnitType unitType, double scaleFactor) : base(timeUnitUri?.ToString())
        {
            UnitType = unitType;
            ScaleFactor = scaleFactor;
        }
        #endregion
    }
}