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
    /// Represents a zero-dimensional temporal entity corresponding to a single, indivisible point in time with no duration.
    /// An instant marks a specific moment that can be positioned within a temporal reference system through coordinates
    /// or numeric positions, serving as a temporal boundary or timestamp for events.
    /// Instants can represent precise moments like "midnight on January 1, 2000" or boundaries between temporal intervals,
    /// enabling temporal relationships such as "before", "after" or "simultaneous"
    /// </summary>
    public sealed class TIMEInstant : TIMEEntity
    {
        #region Properties
        /// <summary>
        /// When a time:Instant individual can be directly represented as a typed literal (time:inXSDDateTimeStamp)
        /// </summary>
        public DateTime? DateTime { get; internal set; }

        /// <summary>
        /// When a time:Instant individual can be represented as a time:GeneralDateTimeDescription individual (time:inDateTime)
        /// </summary>
        public TIMEInstantDescription Description { get; internal set; }

        /// <summary>
        /// When a time:Instant individual can be represented as a time:TimePosition individual (time:inTimePosition)
        /// </summary>
        public TIMEInstantPosition Position { get; internal set; }
        #endregion

        #region Ctors
        internal TIMEInstant(RDFResource timeInstantUri)
            : base(timeInstantUri) { }

        /// <summary>
        /// Builds a time:Instant individual with the given name and given typed literal representation
        /// </summary>
        public TIMEInstant(RDFResource timeInstantUri, DateTime dateTime) : base(timeInstantUri)
            => DateTime = dateTime.ToUniversalTime();

        /// <summary>
        /// Builds a time:Instant individual with the given name and given general description representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInstant(RDFResource timeInstantUri, TIMEInstantDescription timeInstantDescription) : base(timeInstantUri)
            => Description = timeInstantDescription ?? throw new OWLException($"Cannot create time instant because given '{nameof(timeInstantDescription)}' parameter is null");

        /// <summary>
        /// Builds a time:Instant individual with the given name and given positional representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInstant(RDFResource timeInstantUri, TIMEInstantPosition timeInstantPosition) : base(timeInstantUri)
            => Position = timeInstantPosition ?? throw new OWLException($"Cannot create time instant because given '{nameof(timeInstantPosition)}' parameter is null");
        #endregion
    }
}