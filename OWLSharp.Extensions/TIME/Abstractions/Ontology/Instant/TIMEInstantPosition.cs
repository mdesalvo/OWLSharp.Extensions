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
    /// Represents the specific positioning of a temporal instant within a defined temporal reference system,
    /// expressed as a numeric value relative to the system's origin. It provides a scalar measurement that
    /// locates a zero-dimensional point in time according to the metrics and units of its associated TRS.
    /// This numeric representation enables precise temporal calculations, comparisons, and conversions
    /// between different reference systems, supporting both fine-grained positioning (like Unix timestamps in seconds)
    /// and coarse-grained positioning (like geological time in millions of years)
    /// </summary>
    public sealed class TIMEInstantPosition : RDFResource
    {
        #region Properties
        /// <summary>
        /// Gets the temporal reference system (TRS) within which this instant position is defined,
        /// establishing the interpretive context for the numeric or nominal value
        /// </summary>
        public RDFResource TRS { get; }

        /// <summary>
        /// Gets the scalar numeric value representing the instant's position relative to the TRS origin,
        /// expressed in the units defined by the reference system
        /// </summary>
        public double NumericValue { get; }

        /// <summary>
        /// Gets the named reference (IRI) representing the instant's position ordinally within a reference system,
        /// used for ordinal TRS where positions are identified by names rather than numbers (e.g., geological eras, historical periods)
        /// </summary>
        public RDFResource NominalValue { get; }

        /// <summary>
        /// Gets or sets the temporal duration representing the margin of uncertainty or imprecision
        /// associated with this instant position, acknowledging that temporal measurements may have
        /// inherent inaccuracy or granularity limits
        /// </summary>
        public TIMEIntervalDuration PositionalUncertainty { get; set; }

        /// <summary>
        /// Internal flag indicating whether this instant position uses nominal (ordinal, name-based) representation
        /// rather than numeric (scalar) representation, determining which value property is meaningful
        /// </summary>
        internal bool IsNominal => NominalValue != null;
        #endregion

        #region Ctors
        internal TIMEInstantPosition(RDFResource timeInstantPositionUri) : base(timeInstantPositionUri?.ToString()) { }

        /// <summary>
        /// Builds an instant position with the given name and given numeric representation (for positional TRS)
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInstantPosition(RDFResource timeInstantPositionUri, RDFResource trsUri, double numericValue)
            : this(timeInstantPositionUri)
        {
            TRS = trsUri ?? throw new OWLException($"Cannot create position of time instant because given '{nameof(trsUri)}' parameter is null");
            NumericValue = numericValue;
        }

        /// <summary>
        /// Builds an instant position with the given name and given nominal representation (for ordinal TRS)
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInstantPosition(RDFResource timeInstantPositionUri, RDFResource trsUri, RDFResource nominalValue)
            : this(timeInstantPositionUri)
        {
            TRS = trsUri ?? throw new OWLException($"Cannot create position of time instant because given '{nameof(trsUri)}' parameter is null");
            NominalValue = nominalValue ?? throw new OWLException($"Cannot create position of time instant because given '{nameof(nominalValue)}' parameter is null");
        }
        #endregion
    }
}