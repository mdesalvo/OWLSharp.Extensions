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
    /// Register for custom temporal unit types
    /// </summary>
    public sealed class TIMEUnitTypeRegistry : IEnumerable<TIMEUnit>
    {
        #region Properties
        /// <summary>
        /// Singleton instance of the register
        /// </summary>
        public static TIMEUnitTypeRegistry Instance { get; }

        /// <summary>
        /// Count of the registered temporal unit types
        /// </summary>
        public static int UnitTypeCount
            => Instance.UnitTypes.Count;

        /// <summary>
        /// Enumerator to iterate the registered temporal unit types
        /// </summary>
        public static IEnumerator<TIMEUnit> UnitTypeEnumerator
            => Instance.UnitTypes.Values.GetEnumerator();

        /// <summary>
        /// Internal dictionary of registered temporal unit types
        /// </summary>
        internal Dictionary<string,TIMEUnit> UnitTypes { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Initializes the singleton register instance with standard OWL-TIME temporal unit types (and a couple of custom ones)
        /// </summary>
        static TIMEUnitTypeRegistry()
        {
            Instance = new TIMEUnitTypeRegistry
            {
                UnitTypes = new Dictionary<string, TIMEUnit>
                {
                    //Built-Ins
                    { TIMEUnit.Millennium.ToString(), TIMEUnit.Millennium },
                    { TIMEUnit.Century.ToString(), TIMEUnit.Century },
                    { TIMEUnit.Decade.ToString(), TIMEUnit.Decade },
                    { TIMEUnit.Year.ToString(), TIMEUnit.Year },
                    { TIMEUnit.Month.ToString(), TIMEUnit.Month },
                    { TIMEUnit.Week.ToString(), TIMEUnit.Week },
                    { TIMEUnit.Day.ToString(), TIMEUnit.Day },
                    { TIMEUnit.Hour.ToString(), TIMEUnit.Hour },
                    { TIMEUnit.Minute.ToString(), TIMEUnit.Minute },
                    { TIMEUnit.Second.ToString(), TIMEUnit.Second },
                    //Derived
                    { TIMEUnit.BillionYearsAgo.ToString(), TIMEUnit.BillionYearsAgo },
                    { TIMEUnit.MillionYearsAgo.ToString(), TIMEUnit.MillionYearsAgo },
                    { TIMEUnit.MarsSol.ToString(), TIMEUnit.MarsSol }
                }
            };
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Gets the enumerator on the registered temporal unit types
        /// </summary>
        IEnumerator<TIMEUnit> IEnumerable<TIMEUnit>.GetEnumerator()
            => UnitTypeEnumerator;

        /// <summary>
        /// Gets the enumerator on the registered temporal unit types
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
            => UnitTypeEnumerator;
        #endregion

        #region Methods
        /// <summary>
        /// Registers the given temporal unit type
        /// </summary>
        public static void AddUnitType(TIMEUnit unitType)
        {
            if (unitType != null && !Instance.UnitTypes.ContainsKey(unitType.ToString()))
                Instance.UnitTypes.Add(unitType.ToString(), unitType);
        }

        /// <summary>
        /// Checks if the given temporal unit type is registered or not
        /// </summary>
        public static bool ContainsUnitType(RDFResource unitTypeURI)
            => unitTypeURI != null && Instance.UnitTypes.ContainsKey(unitTypeURI.ToString());
        #endregion
    }
}