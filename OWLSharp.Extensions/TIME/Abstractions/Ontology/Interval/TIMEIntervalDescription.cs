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
    /// Represents a description of a temporal interval through its characteristic properties
    /// (duration, extent, beginning, end) without committing to specific temporal instants
    /// or reference systems. It provides a flexible way to describe periods of time using either
    /// their temporal extent (how long they last) or their boundaries (when they start and end),
    /// allowing interval specifications that can be interpreted, evaluated, or instantiated
    /// within different temporal frameworks while remaining independent of particular TRS implementations
    /// </summary>
    public sealed class TIMEIntervalDescription : RDFResource, IEquatable<TIMEIntervalDescription>, IComparable<TIMEIntervalDescription>
    {
        #region Properties
        /// <summary>
        /// Gets the temporal extent that describes the duration of this interval through decomposed components
        /// (years, months, weeks, days, hours, minutes, seconds), providing a structured representation of
        /// how long the interval lasts without specifying its absolute temporal position
        /// </summary>
        public TIMEExtent Extent { get; }
        #endregion

        #region Ctors
        internal TIMEIntervalDescription(RDFResource timeIntervalDescriptionUri) : base(timeIntervalDescriptionUri?.ToString()) { }

        /// <summary>
        /// Builds an interval description with the given name and given typed literal representation
        /// </summary>
        public TIMEIntervalDescription(RDFResource timeIntervalDescriptionUri, TimeSpan timeSpan) : this(timeIntervalDescriptionUri)
            => Extent = new TIMEExtent(timeSpan);

        /// <summary>
        /// Builds an interval description with the given name and given extent representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEIntervalDescription(RDFResource timeInstantDescriptionUri, TIMEExtent timeIntervalExtent) : this(timeInstantDescriptionUri)
            => Extent = timeIntervalExtent?? throw new OWLException($"Cannot create description of time interval because given '{nameof(timeIntervalExtent)}' parameter is null");
        #endregion

        #region Interfaces
        /// <summary>
        /// Compares the temporal extent of this interval description to the given one's coordinate
        /// </summary>
        public int CompareTo(TIMEIntervalDescription other)
            => Extent.CompareTo(other?.Extent);

        /// <summary>
        /// Compares the temporal extent of this interval description to the given one's coordinate for equality
        /// </summary>
        public bool Equals(TIMEIntervalDescription other)
            => CompareTo(other) == 0;
        #endregion
    }
}