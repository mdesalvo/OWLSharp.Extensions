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

namespace OWLSharp.Extensions.GEO
{
    /// <summary>
    /// GEOEnums represents a collector for all the enumerations used by the "OWLSharp.Extensions.GEO" namespace
    /// </summary>
    public static class GEOEnums
    {
        /// <summary>
        /// Indicates the direction requested for the proximity analysis of a spatial feature
        /// </summary>
        public enum GeoDirections
        {
            /// <summary>
            /// Will search for features located north of the starting feature
            /// </summary>
            North = 1,
            /// <summary>
            /// Will search for features located east of the starting feature
            /// </summary>
            East = 2,
            /// <summary>
            /// Will search for features located south of the starting feature
            /// </summary>
            South = 3,
            /// <summary>
            /// Will search for features located west of the starting feature
            /// </summary>
            West = 4,
            /// <summary>
            /// Will search for features located north-east of the starting feature
            /// </summary>
            NorthEast = 5,
            /// <summary>
            /// Will search for features located north-west of the starting feature
            /// </summary>
            NorthWest = 6,
            /// <summary>
            /// Will search for features located south-east of the starting feature
            /// </summary>
            SouthEast = 7,
            /// <summary>
            /// Will search for features located south-west of the starting feature
            /// </summary>
            SouthWest = 8
        }

        /// <summary>
        /// Indicates the type of analysis requested for a spatial feature
        /// </summary>
        internal enum GeoAnalysis
        {
            Boundary = 1,
            Buffer = 2,
            Centroid = 3,
            ConvexHull = 4,
            Envelope = 5
        }
    }
}