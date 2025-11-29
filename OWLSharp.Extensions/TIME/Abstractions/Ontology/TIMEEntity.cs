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
    /// In OWL-TIME, a temporal entity represents any abstract or concrete element that exists in time or describes temporal phenomena.
    /// It serves as the foundational concept from which all temporal constructs derive, encompassing instants (zero-dimensional temporal points),
    /// intervals (one-dimensional temporal extents with duration), and their combinations.
    /// Temporal entities can represent specific moments in history, durations of events, temporal relationships between occurrences,
    /// or any other time-bounded phenomenon. They are characterized by their position within one or more temporal reference systems
    /// and can be related to domain entities (events, processes, states) to provide temporal context and enable temporal reasoning
    /// within knowledge graphs and semantic applications
    /// </summary>
    public class TIMEEntity : RDFResource
    {
        #region Ctors
        /// <summary>
        /// Builds a temporal entity with the given name
        /// </summary>
        internal TIMEEntity(RDFResource timeEntityUri)
            : base(timeEntityUri?.ToString()) { }
        #endregion
    }
}