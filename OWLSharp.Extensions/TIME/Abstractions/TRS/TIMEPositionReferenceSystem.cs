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
    /// Represents a positional temporal reference system that measures time as numeric values relative to a defined origin (epoch).
    /// It specifies a reference point in time, a measurement unit, and optionally a scale semantic
    /// (large-scale for coarse granularity like geological time, or little-scale for fine granularity like Unix timestamps).
    /// Time positions are expressed as scalar distances from the origin.
    /// </summary>
    public sealed class TIMEPositionReferenceSystem : TIMEReferenceSystem
    {
        #region Built-Ins
        /// <summary>
        /// Built-in implementation of Unix time (POSIX time), measuring seconds elapsed since the Unix epoch (January 1, 1970, 00:00:00 UTC).
        /// This is the standard time representation used in computing systems worldwide.
        /// </summary>
        public static readonly TIMEPositionReferenceSystem UnixTime = new TIMEPositionReferenceSystem(
            new RDFResource("https://en.wikipedia.org/wiki/Unix_time"), TIMECoordinate.UnixTime, TIMEUnit.Second);

        /// <summary>
        /// Built-in implementation of chronometric geologic time, measuring time in millions of years before present
        /// relative to the geologic epoch (January 1, 1950, 00:00:00 UTC). Uses large-scale semantics, appropriate for
        /// geological and paleontological applications spanning millions to billions of years.
        /// </summary>
        public static readonly TIMEPositionReferenceSystem GeologicTime = new TIMEPositionReferenceSystem(
            new RDFResource("http://www.opengis.net/def/crs/OGC/0/ChronometricGeologicTime"), TIMECoordinate.GeologicTime, TIMEUnit.MillionYearsAgo, true);
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
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a positional TRS with the given name, origin coordinate, temporal unit and scale behavior
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEPositionReferenceSystem(RDFResource trsUri, TIMECoordinate trsOrigin, TIMEUnit trsUnit, bool hasLargeScale=false)
            : base(trsUri)
        {
            Origin = trsOrigin ?? throw new OWLException($"Cannot create PositionReferenceSystem because given '{nameof(trsOrigin)}' parameter is null");
            Unit = trsUnit ?? throw new OWLException($"Cannot create PositionReferenceSystem because given '{nameof(trsUnit)}' parameter is null");
            HasLargeScale = hasLargeScale;
        }
        #endregion
    }
}