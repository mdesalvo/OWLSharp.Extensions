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

namespace OWLSharp.Extensions.SKOS
{
    /// <summary>
    /// SKOSEnums represents a collector for all the enumerations used by the "OWLSharp.Extensions.SKOS" namespace
    /// </summary>
    public static class SKOSEnums
    {
        /// <summary>
        /// SKOSValidatorRules represents an enumeration for supported SKOS/SKOS-XL validator rules
        /// </summary>
        public enum SKOSValidatorRules
        {
            /// <summary>
            /// Concepts should not assume the same value for alternative labels and preferred/hidden labels (SKOS, SKOS-XL)
            /// </summary>
            AlternativeLabelAnalysis = 1,
            /// <summary>
            /// Concepts should not assume the same value for hidden labels and preferred/alternative labels (SKOS, SKOS-XL)
            /// </summary>
            HiddenLabelAnalysis = 2,
            /// <summary>
            /// Concepts should not assume the same value for preferred labels and alternative/hidden labels (SKOS, SKOS-XL)
            /// </summary>
            PreferredLabelAnalysis = 3,
            /// <summary>
            /// Concepts should not assume the same value for notations under the same schema (SKOS)
            /// </summary>
            NotationAnalysis = 4,
            /// <summary>
            /// Concepts should not clash hierarchical relations with associative or mapping relations (SKOS)
            /// </summary>
            BroaderConceptAnalysis = 5,
            /// <summary>
            /// Concepts should not clash hierarchical relations with associative or mapping relations (SKOS)
            /// </summary>
            NarrowerConceptAnalysis = 6,
            /// <summary>
            /// Concepts should not clash mapping relations with associative relations (SKOS)
            /// </summary>
            CloseOrExactMatchConceptAnalysis = 7,
            /// <summary>
            /// Concepts should not clash associative relations with hierarchical or mapping relations (SKOS)
            /// </summary>
            RelatedConceptAnalysis = 8,
            /// <summary>
            /// Labels should not assume more than one literal form (SKOS-XL) 
            /// </summary>
            LiteralFormAnalysis = 9
        }
    }
}