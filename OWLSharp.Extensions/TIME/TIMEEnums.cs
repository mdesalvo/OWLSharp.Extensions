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

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// TIMEEnums represents a collector for all the enumerations used by the "OWLSharp.Extensions.TIME" namespace
    /// </summary>
    public static class TIMEEnums
    {
        /// <summary>
        /// Indicates the reference OWL-TIME unit types
        /// </summary>
        public enum TIMEUnitType
        {
            /// <summary>
            /// Indicates a unit type expressing years according to the metrics of the calendar TRS in use
            /// </summary>
            Year = 1,
            /// <summary>
            /// Indicates a unit type expressing months according to the metrics of the calendar TRS in use
            /// </summary>
            Month = 2,
            /// <summary>
            /// Indicates a unit type expressing days according to the metrics of the calendar TRS in use
            /// </summary>
            Day = 3,
            /// <summary>
            /// Indicates a unit type expressing hours according to the metrics of the calendar TRS in use
            /// </summary>
            Hour = 4,
            /// <summary>
            /// Indicates a unit type expressing minutes according to the metrics of the calendar TRS in use
            /// </summary>
            Minute = 5,
            /// <summary>
            /// Indicates a unit type expressing seconds according to the metrics of the calendar TRS in use
            /// </summary>
            Second = 6
        }

        /// <summary>
        /// TIMEReasonerRules represents an enumeration for supported OWL-TIME reasoner rules
        /// </summary>
        public enum TIMEReasonerRules
        {
            AfterEqualsEntailment = 1,
            AfterFinishesEntailment = 2,
            AfterMetByEntailment = 3,
            AfterTransitiveEntailment = 4,
            BeforeEqualsEntailment = 5,
            BeforeMeetsEntailment = 6,
            BeforeStartsEntailment = 7,
            BeforeTransitiveEntailment = 8,
            ContainsEqualsEntailment = 9,
            ContainsTransitiveEntailment = 10,
            DuringEqualsEntailment = 11,
            DuringTransitiveEntailment = 12,
            EqualsEntailment = 13,
            EqualsInverseEntailment = 14,
            EqualsTransitiveEntailment = 15,
            FinishedByEqualsEntailment = 16,
            FinishesEqualsEntailment = 17,
            MeetsEqualsEntailment = 18,
            MeetsStartsEntailment = 19,
            MetByEqualsEntailment = 20,
            OverlappedByEqualsEntailment = 21,
            OverlapsEqualsEntailment = 22,
            StartsEqualsEntailment = 23,
            StartedByEqualsEntailment = 24
        }

        /// <summary>
        /// TIMEValidatorRules represents an enumeration for supported OWL-TIME validator rules
        /// </summary>
        public enum TIMEValidatorRules
        {
            InstantAfterAnalysis = 1,
            InstantBeforeAnalysis = 2,
            IntervalAfterAnalysis = 3,
            IntervalBeforeAnalysis = 4,
            IntervalContainsAnalysis = 5,
            IntervalDisjointAnalysis = 6,
            IntervalDuringAnalysis = 7,
            IntervalEqualsAnalysis = 8,
            IntervalFinishesAnalysis = 9,
            IntervalFinishedByAnalysis = 10,
            IntervalHasInsideAnalysis = 11,
            IntervalInAnalysis = 12,
            IntervalMeetsAnalysis = 13,
            IntervalMetByAnalysis = 14,
            IntervalNotDisjointAnalysis = 15,
            IntervalOverlapsAnalysis = 16,
            IntervalOverlappedByAnalysis = 17,
            IntervalStartsAnalysis = 18,
            IntervalStartedByAnalysis = 19
        }
    }
}