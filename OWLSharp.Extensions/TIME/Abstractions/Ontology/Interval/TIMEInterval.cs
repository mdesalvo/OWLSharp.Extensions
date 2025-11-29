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
    /// Represents a one-dimensional temporal entity corresponding to a period or span of time with measurable duration,
    /// bounded by two distinct temporal positions (beginning and end). An interval encompasses all instants between
    /// its boundaries and can represent the temporal extent of events, processes, or states.
    /// Intervals enable temporal relationships such as "overlaps", "contains", "meets", or "during"
    /// and can be characterized by their duration (extent) and position within temporal reference systems
    /// </summary>
    public sealed class TIMEInterval : TIMEEntity
    {
        #region Properties
        /// <summary>
        /// When a time:Interval individual can be directly represented as a typed literal (time:hasXSDDuration)
        /// </summary>
        public TimeSpan? TimeSpan { get; internal set; }

        /// <summary>
        /// When a time:Interval individual can be represented as a time:GeneralDurationDescription individual (time:hasDurationDescription)
        /// </summary>
        public TIMEIntervalDescription Description { get; internal set; }

        /// <summary>
        /// When a time:Interval individual can be represented as a time:Duration individual (time:hasDuration)
        /// </summary>
        public TIMEIntervalDuration Duration { get; internal set; }

        /// <summary>
        /// When a time:Interval individual can be given a beginning time:instant individual (time:hasBeginning)
        /// </summary>
        public TIMEInstant Beginning { get; internal set; }

        /// <summary>
        /// When a time:Interval individual can be given an ending time:instant individual (time:hasEnd)
        /// </summary>
        public TIMEInstant End { get; internal set; }
        #endregion

        #region Ctors
        internal TIMEInterval(RDFResource timeIntervalUri)
            : base(timeIntervalUri) { }

        /// <summary>
        /// Builds a time:Interval individual with the given name and given typed literal representation
        /// </summary>
        public TIMEInterval(RDFResource timeInstantUri, TimeSpan timeSpan) : base(timeInstantUri)
            => TimeSpan = timeSpan;

        /// <summary>
        /// Builds a time:Interval individual with the given name and given general description representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInterval(RDFResource timeInstantUri, TIMEIntervalDescription timeIntervalDescription) : base(timeInstantUri)
            => Description = timeIntervalDescription ?? throw new OWLException($"Cannot create time interval because given '{nameof(timeIntervalDescription)}' parameter is null");

        /// <summary>
        /// Builds a time:Interval individual with the given name and given duration representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInterval(RDFResource timeInstantUri, TIMEIntervalDuration timeIntervalDuration) : base(timeInstantUri)
            => Duration = timeIntervalDuration ?? throw new OWLException($"Cannot create time interval because given '{nameof(timeIntervalDuration)}' parameter is null");

        /// <summary>
        /// Builds a time:Interval with the given beginning/ending instant's representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEInterval(RDFResource timeInstantUri, TIMEInstant timeInstantBeginning, TIMEInstant timeInstantEnd) : base(timeInstantUri)
        {
            #region Guards
            if (timeInstantBeginning == null && timeInstantEnd == null)
                throw new OWLException($"Cannot create time interval because both '{nameof(timeInstantBeginning)}' and '{nameof(timeInstantEnd)}' parameters are null");
            #endregion

            Beginning = timeInstantBeginning;
            End = timeInstantEnd;
        }
        #endregion
    }
}