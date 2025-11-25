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
            /// <summary>
            /// AFTER(?I1,?I2) ^ EQUALS(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            AfterEqualsEntailment = 1,
            /// <summary>
            /// AFTER(?I1,?I2) ^ FINISHES(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            AfterFinishesEntailment = 2,
            /// <summary>
            /// AFTER(?I1,?I2) ^ METBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            AfterMetByEntailment = 3,
            /// <summary>
            /// AFTER(?I1,?I2) ^ AFTER(?I3,?I1) -> AFTER(?I3,?I2)
            /// </summary>
            AfterTransitiveEntailment = 4,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ EQUALS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            BeforeEqualsEntailment = 5,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ MEETS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            BeforeMeetsEntailment = 6,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ STARTS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            BeforeStartsEntailment = 7,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ BEFORE(?I3,?I1) -> BEFORE(?I3,?I2)
            /// </summary>
            BeforeTransitiveEntailment = 8,
            /// <summary>
            /// CONTAINS(?I1,?I2) ^ EQUALS(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            ContainsEqualsEntailment = 9,
            /// <summary>
            /// CONTAINS(?I1,?I2) ^ CONTAINS(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            ContainsTransitiveEntailment = 10,
            /// <summary>
            /// DURING(?I1,?I2) ^ EQUALS(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            DuringEqualsEntailment = 11,
            /// <summary>
            /// DURING(?I1,?I2) ^ DURING(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            DuringTransitiveEntailment = 12,
            /// <summary>
            /// STARTS(?I1,?I2) ^ FINISHES(?I1,?I2) -> EQUALS(?I1,?I2)
            /// </summary>
            EqualsEntailment = 13,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ FINISHEDBY(?I1,?I2) -> EQUALS(?I1,?I2)
            /// </summary>
            EqualsInverseEntailment = 14,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ EQUALS(?I2,?I3) -> EQUALS(?I1,?I3)
            /// </summary>
            EqualsTransitiveEntailment = 15,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ EQUALS(?I2,?I3) -> FINISHEDBY(?I1,?I3)
            /// </summary>
            FinishedByEqualsEntailment = 16,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ EQUALS(?I2,?I3) -> FINISHES(?I1,?I3)
            /// </summary>
            FinishesEqualsEntailment = 17,
            /// <summary>
            /// MEETS(?I1,?I2) ^ EQUALS(?I2,?I3) -> MEETS(?I1,?I3)
            /// </summary>
            MeetsEqualsEntailment = 18,
            /// <summary>
            /// MEETS(?I1,?I2) ^ STARTS(?I2,?I3) -> MEETS(?I1,?I3)
            /// </summary>
            MeetsStartsEntailment = 19,
            /// <summary>
            /// METBY(?I1,?I2) ^ EQUALS(?I2,?I3) -> METBY(?I1,?I3)
            /// </summary>
            MetByEqualsEntailment = 20,
            /// <summary>
            /// OVERLAPPEDBY(?I1,?I2) ^ EQUALS(?I2,?I3) -> OVERLAPPEDBY(?I1,?I3)
            /// </summary>
            OverlappedByEqualsEntailment = 21,
            /// <summary>
            /// OVERLAPS(?I1,?I2) ^ EQUALS(?I2,?I3) -> OVERLAPS(?I1,?I3)
            /// </summary>
            OverlapsEqualsEntailment = 22,
            /// <summary>
            /// STARTS(?I1,?I2) ^ EQUALS(?I2,?I3) -> STARTS(?I1,?I3)
            /// </summary>
            StartsEqualsEntailment = 23,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ EQUALS(?I2,?I3) -> STARTEDBY(?I1,?I3)
            /// </summary>
            StartedByEqualsEntailment = 24
        }

        /// <summary>
        /// TIMEValidatorRules represents an enumeration for supported OWL-TIME validator rules
        /// </summary>
        public enum TIMEValidatorRules
        {
            /// <summary>
            /// Instants should not clash in temporal relations (time:after, time:before)
            /// </summary>
            InstantAfterAnalysis = 1,
            /// <summary>
            /// Instants should not clash in temporal relations (time:before, time:after)
            /// </summary>
            InstantBeforeAnalysis = 2,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalAfter)
            /// </summary>
            IntervalAfterAnalysis = 3,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalBefore)
            /// </summary>
            IntervalBeforeAnalysis = 4,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalContains)
            /// </summary>
            IntervalContainsAnalysis = 5,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalDisjoint)
            /// </summary>
            IntervalDisjointAnalysis = 6,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalDuring)
            /// </summary>
            IntervalDuringAnalysis = 7,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalEquals)
            /// </summary>
            IntervalEqualsAnalysis = 8,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalFinishes)
            /// </summary>
            IntervalFinishesAnalysis = 9,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalFinishedBy)
            /// </summary>
            IntervalFinishedByAnalysis = 10,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:hasInside)
            /// </summary>
            IntervalHasInsideAnalysis = 11,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalIn)
            /// </summary>
            IntervalInAnalysis = 12,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalMeets)
            /// </summary>
            IntervalMeetsAnalysis = 13,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalMetBy)
            /// </summary>
            IntervalMetByAnalysis = 14,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:notDisjoint)
            /// </summary>
            IntervalNotDisjointAnalysis = 15,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalOverlaps)
            /// </summary>
            IntervalOverlapsAnalysis = 16,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalOverlappedBy)
            /// </summary>
            IntervalOverlappedByAnalysis = 17,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalStarts)
            /// </summary>
            IntervalStartsAnalysis = 18,
            /// <summary>
            /// Intervals should not clash in temporal relations (time:intervalStartedBy)
            /// </summary>
            IntervalStartedByAnalysis = 19
        }
    }
}