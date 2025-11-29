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
    /// Represents the measurable length or extent of a temporal interval, expressed as a numeric duration value
    /// with an associated temporal unit. It quantifies how long an interval lasts without specifying when it occurs,
    /// providing a scalar measurement of temporal extent that can be expressed in various units
    /// (seconds, minutes, hours, days, years) and converted between different temporal reference systems.
    /// This duration-based representation enables temporal arithmetic, interval comparisons, and the calculation
    /// of elapsed time between temporal boundaries
    /// </summary>
    public sealed class TIMEIntervalDuration : RDFResource
    {
        #region Properties
        /// <summary>
        /// Gets or sets the temporal unit (IRI) used to express this interval's duration,
        /// defining the measurement granularity such as seconds, minutes, hours, days, months, or years
        /// </summary>
        public RDFResource UnitType { get; set; }

        /// <summary>
        /// Gets or sets the numeric scalar value representing the magnitude of this interval's duration,
        /// expressed in the units specified by the UnitType property
        /// </summary>
        public double Value { get; set; }
        #endregion

        #region Ctors
        internal TIMEIntervalDuration(RDFResource timeIntervalDurationUri) : base(timeIntervalDurationUri?.ToString()) { }

        /// <summary>
        /// Builds an interval duration with the given name, scalar value and associated unit type
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEIntervalDuration(RDFResource timeIntervalDurationUri, RDFResource unitTypeURI, double value) : this(timeIntervalDurationUri)
        {
            UnitType = unitTypeURI ?? throw new OWLException($"Cannot create duration of time interval because given '{nameof(unitTypeURI)}' parameter is null");
            Value = value;
        }
        #endregion
    }
}