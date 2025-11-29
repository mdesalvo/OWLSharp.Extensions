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
using System.Collections;
using System.Collections.Generic;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// Register for custom temporal reference systems (TRS)
    /// </summary>
    public sealed class TIMEReferenceSystemRegistry : IEnumerable<TIMEReferenceSystem>
    {
        #region Properties
        /// <summary>
        /// Singleton instance of the register
        /// </summary>
        public static TIMEReferenceSystemRegistry Instance { get; }

        /// <summary>
        /// Count of the registered temporal reference systems (TRS)
        /// </summary>
        public static int TRSCount
            => Instance.TRS.Count;

        /// <summary>
        /// Enumerator to iterate the registered temporal reference systems (TRS)
        /// </summary>
        public static IEnumerator<TIMEReferenceSystem> TRSEnumerator
            => Instance.TRS.Values.GetEnumerator();

        /// <summary>
        /// Internal dictionary of registered temporal reference systems (TRS)
        /// </summary>
        internal Dictionary<string,TIMEReferenceSystem> TRS { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Initializes the singleton register instance with standard calendar and positional temporal reference systems (TRS)
        /// </summary>
        static TIMEReferenceSystemRegistry()
        {
            Instance = new TIMEReferenceSystemRegistry
            {
                TRS = new Dictionary<string, TIMEReferenceSystem>
                {
                    //Calendar TRS
                    { TIMECalendarReferenceSystem.Gregorian.ToString(), TIMECalendarReferenceSystem.Gregorian },
                    { TIMECalendarReferenceSystem.Julian.ToString(), TIMECalendarReferenceSystem.Julian },
                    //Position TRS
                    { TIMEPositionReferenceSystem.UnixTime.ToString(), TIMEPositionReferenceSystem.UnixTime },
                    { TIMEPositionReferenceSystem.GeologicTime.ToString(), TIMEPositionReferenceSystem.GeologicTime }
                }
            };
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Gets the enumerator on the registered temporal reference systems (TRS)
        /// </summary>
        IEnumerator<TIMEReferenceSystem> IEnumerable<TIMEReferenceSystem>.GetEnumerator()
            => TRSEnumerator;

        /// <summary>
        /// Gets the enumerator on the registered temporal reference systems (TRS)
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
            => TRSEnumerator;
        #endregion

        #region Methods
        /// <summary>
        /// Registers the given temporal reference system (TRS)
        /// </summary>
        public static void AddTRS(TIMEReferenceSystem trs)
        {
            if (trs != null && !Instance.TRS.ContainsKey(trs.ToString()))
                Instance.TRS.Add(trs.ToString(), trs);
        }

        /// <summary>
        /// Checks if the given temporal reference system (TRS) is registered or not
        /// </summary>
        public static bool ContainsTRS(RDFResource trsURI)
            => trsURI != null && Instance.TRS.ContainsKey(trsURI.ToString());
        #endregion
    }
}