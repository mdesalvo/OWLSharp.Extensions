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
    /// Represents a description of temporal instant properties through clock/calendar coordinates
    /// without committing to a specific temporal reference system. It provides a flexible way
    /// to describe moments in time using decomposed temporal components (year, month, day, hour, minute, second)
    /// that can be interpreted within different TRS contexts.
    /// This abstraction enables temporal specifications that are independent of particular calendar systems or epochs,
    /// allowing the same instant description to be evaluated or converted across multiple temporal frameworks
    /// while maintaining semantic consistency.
    /// </summary>
    public sealed class TIMEInstantDescription : RDFResource, IEquatable<TIMEInstantDescription>, IComparable<TIMEInstantDescription>
    {
        #region Properties
        /// <summary>
        /// Temporal coordinate that describes this instant through decomposed calendar components
        /// (year, month, day, hour, minute, second), providing a structured representation of the
        /// temporal position that can be interpreted within different temporal reference systems
        /// </summary>
        public TIMECoordinate Coordinate { get; }
        #endregion

        #region Ctors
        internal TIMEInstantDescription(RDFResource timeInstantDescriptionUri) : base(timeInstantDescriptionUri?.ToString()) { }

        /// <summary>
        /// Builds an instant description with the given name and given typed literal representation
        /// </summary>
        public TIMEInstantDescription(RDFResource timeInstantDescriptionUri, DateTime dateTime) : this(timeInstantDescriptionUri)
            => Coordinate = new TIMECoordinate(dateTime.ToUniversalTime());

        /// <summary>
        /// Builds an instant description with the given name and given coordinate representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInstantDescription(RDFResource timeInstantDescriptionUri, TIMECoordinate timeInstantCoordinate) : this(timeInstantDescriptionUri)
            => Coordinate = timeInstantCoordinate ?? throw new OWLException($"Cannot create description of time instant because given '{nameof(timeInstantCoordinate)}' parameter is null");
        #endregion

        #region Interfaces
        /// <summary>
        /// Compares the temporal coordinate of this instant description to the given one's coordinate
        /// </summary>
        public int CompareTo(TIMEInstantDescription other)
            => Coordinate.CompareTo(other?.Coordinate);

        /// <summary>
        /// Compares the temporal coordinate of this instant description to the given one's coordinate for equality
        /// </summary>
        public bool Equals(TIMEInstantDescription other)
            => CompareTo(other) == 0;
        #endregion
    }
}