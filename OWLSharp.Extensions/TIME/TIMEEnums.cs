/*
   Copyright 2014-2026 Marco De Salvo

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
            StartedByEqualsEntailment = 24,
            /// <summary>
            /// AFTER(?I1,?I2) ^ CONTAINS(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            AfterContainsEntailment = 25,
            /// <summary>
            /// AFTER(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            AfterFinishedByEntailment = 26,
            /// <summary>
            /// AFTER(?I1,?I2) ^ OVERLAPPEDBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            AfterOverlappedByEntailment = 27,
            /// <summary>
            /// AFTER(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            AfterStartedByEntailment = 28,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ CONTAINS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            BeforeContainsEntailment = 29,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            BeforeFinishedByEntailment = 30,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ OVERLAPS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            BeforeOverlapsEntailment = 31,
            /// <summary>
            /// BEFORE(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            BeforeStartedByEntailment = 32,
            /// <summary>
            /// CONTAINS(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            ContainsFinishedByEntailment = 33,
            /// <summary>
            /// CONTAINS(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            ContainsStartedByEntailment = 34,
            /// <summary>
            /// DURING(?I1,?I2) ^ AFTER(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            DuringAfterEntailment = 35,
            /// <summary>
            /// DURING(?I1,?I2) ^ BEFORE(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            DuringBeforeEntailment = 36,
            /// <summary>
            /// DURING(?I1,?I2) ^ FINISHES(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            DuringFinishesEntailment = 37,
            /// <summary>
            /// DURING(?I1,?I2) ^ MEETS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            DuringMeetsEntailment = 38,
            /// <summary>
            /// DURING(?I1,?I2) ^ METBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            DuringMetByEntailment = 39,
            /// <summary>
            /// DURING(?I1,?I2) ^ STARTS(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            DuringStartsEntailment = 40,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ AFTER(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            EqualsAfterEntailment = 41,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ BEFORE(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            EqualsBeforeEntailment = 42,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ CONTAINS(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            EqualsContainsEntailment = 43,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ DURING(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            EqualsDuringEntailment = 44,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> FINISHEDBY(?I1,?I3)
            /// </summary>
            EqualsFinishedByEntailment = 45,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ FINISHES(?I2,?I3) -> FINISHES(?I1,?I3)
            /// </summary>
            EqualsFinishesEntailment = 46,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ MEETS(?I2,?I3) -> MEETS(?I1,?I3)
            /// </summary>
            EqualsMeetsEntailment = 47,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ METBY(?I2,?I3) -> METBY(?I1,?I3)
            /// </summary>
            EqualsMetByEntailment = 48,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ OVERLAPPEDBY(?I2,?I3) -> OVERLAPPEDBY(?I1,?I3)
            /// </summary>
            EqualsOverlappedByEntailment = 49,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ OVERLAPS(?I2,?I3) -> OVERLAPS(?I1,?I3)
            /// </summary>
            EqualsOverlapsEntailment = 50,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> STARTEDBY(?I1,?I3)
            /// </summary>
            EqualsStartedByEntailment = 51,
            /// <summary>
            /// EQUALS(?I1,?I2) ^ STARTS(?I2,?I3) -> STARTS(?I1,?I3)
            /// </summary>
            EqualsStartsEntailment = 52,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ BEFORE(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            FinishedByBeforeEntailment = 53,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ CONTAINS(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            FinishedByContainsEntailment = 54,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> FINISHEDBY(?I1,?I3)
            /// </summary>
            FinishedByFinishedByEntailment = 55,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ MEETS(?I2,?I3) -> MEETS(?I1,?I3)
            /// </summary>
            FinishedByMeetsEntailment = 56,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ OVERLAPS(?I2,?I3) -> OVERLAPS(?I1,?I3)
            /// </summary>
            FinishedByOverlapsEntailment = 57,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            FinishedByStartedByEntailment = 58,
            /// <summary>
            /// FINISHEDBY(?I1,?I2) ^ STARTS(?I2,?I3) -> OVERLAPS(?I1,?I3)
            /// </summary>
            FinishedByStartsEntailment = 59,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ AFTER(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            FinishesAfterEntailment = 60,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ BEFORE(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            FinishesBeforeEntailment = 61,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ DURING(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            FinishesDuringEntailment = 62,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ FINISHES(?I2,?I3) -> FINISHES(?I1,?I3)
            /// </summary>
            FinishesFinishesEntailment = 63,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ MEETS(?I2,?I3) -> MEETS(?I1,?I3)
            /// </summary>
            FinishesMeetsEntailment = 64,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ METBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            FinishesMetByEntailment = 65,
            /// <summary>
            /// FINISHES(?I1,?I2) ^ STARTS(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            FinishesStartsEntailment = 66,
            /// <summary>
            /// MEETS(?I1,?I2) ^ BEFORE(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            MeetsBeforeEntailment = 67,
            /// <summary>
            /// MEETS(?I1,?I2) ^ CONTAINS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            MeetsContainsEntailment = 68,
            /// <summary>
            /// MEETS(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            MeetsFinishedByEntailment = 69,
            /// <summary>
            /// MEETS(?I1,?I2) ^ MEETS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            MeetsMeetsEntailment = 70,
            /// <summary>
            /// MEETS(?I1,?I2) ^ OVERLAPS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            MeetsOverlapsEntailment = 71,
            /// <summary>
            /// MEETS(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> MEETS(?I1,?I3)
            /// </summary>
            MeetsStartedByEntailment = 72,
            /// <summary>
            /// METBY(?I1,?I2) ^ AFTER(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            MetByAfterEntailment = 73,
            /// <summary>
            /// METBY(?I1,?I2) ^ CONTAINS(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            MetByContainsEntailment = 74,
            /// <summary>
            /// METBY(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> METBY(?I1,?I3)
            /// </summary>
            MetByFinishedByEntailment = 75,
            /// <summary>
            /// METBY(?I1,?I2) ^ FINISHES(?I2,?I3) -> METBY(?I1,?I3)
            /// </summary>
            MetByFinishesEntailment = 76,
            /// <summary>
            /// METBY(?I1,?I2) ^ METBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            MetByMetByEntailment = 77,
            /// <summary>
            /// METBY(?I1,?I2) ^ OVERLAPPEDBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            MetByOverlappedByEntailment = 78,
            /// <summary>
            /// METBY(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            MetByStartedByEntailment = 79,
            /// <summary>
            /// OVERLAPPEDBY(?I1,?I2) ^ AFTER(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            OverlappedByAfterEntailment = 80,
            /// <summary>
            /// OVERLAPPEDBY(?I1,?I2) ^ FINISHES(?I2,?I3) -> OVERLAPPEDBY(?I1,?I3)
            /// </summary>
            OverlappedByFinishesEntailment = 81,
            /// <summary>
            /// OVERLAPPEDBY(?I1,?I2) ^ METBY(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            OverlappedByMetByEntailment = 82,
            /// <summary>
            /// OVERLAPS(?I1,?I2) ^ BEFORE(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            OverlapsBeforeEntailment = 83,
            /// <summary>
            /// OVERLAPS(?I1,?I2) ^ MEETS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            OverlapsMeetsEntailment = 84,
            /// <summary>
            /// OVERLAPS(?I1,?I2) ^ STARTS(?I2,?I3) -> OVERLAPS(?I1,?I3)
            /// </summary>
            OverlapsStartsEntailment = 85,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ AFTER(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            StartedByAfterEntailment = 86,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ CONTAINS(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            StartedByContainsEntailment = 87,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ FINISHEDBY(?I2,?I3) -> CONTAINS(?I1,?I3)
            /// </summary>
            StartedByFinishedByEntailment = 88,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ FINISHES(?I2,?I3) -> OVERLAPPEDBY(?I1,?I3)
            /// </summary>
            StartedByFinishesEntailment = 89,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ METBY(?I2,?I3) -> METBY(?I1,?I3)
            /// </summary>
            StartedByMetByEntailment = 90,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ OVERLAPPEDBY(?I2,?I3) -> OVERLAPPEDBY(?I1,?I3)
            /// </summary>
            StartedByOverlappedByEntailment = 91,
            /// <summary>
            /// STARTEDBY(?I1,?I2) ^ STARTEDBY(?I2,?I3) -> STARTEDBY(?I1,?I3)
            /// </summary>
            StartedByStartedByEntailment = 92,
            /// <summary>
            /// STARTS(?I1,?I2) ^ AFTER(?I2,?I3) -> AFTER(?I1,?I3)
            /// </summary>
            StartsAfterEntailment = 93,
            /// <summary>
            /// STARTS(?I1,?I2) ^ BEFORE(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            StartsBeforeEntailment = 94,
            /// <summary>
            /// STARTS(?I1,?I2) ^ DURING(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            StartsDuringEntailment = 95,
            /// <summary>
            /// STARTS(?I1,?I2) ^ FINISHES(?I2,?I3) -> DURING(?I1,?I3)
            /// </summary>
            StartsFinishesEntailment = 96,
            /// <summary>
            /// STARTS(?I1,?I2) ^ MEETS(?I2,?I3) -> BEFORE(?I1,?I3)
            /// </summary>
            StartsMeetsEntailment = 97,
            /// <summary>
            /// STARTS(?I1,?I2) ^ METBY(?I2,?I3) -> METBY(?I1,?I3)
            /// </summary>
            StartsMetByEntailment = 98,
            /// <summary>
            /// STARTS(?I1,?I2) ^ STARTS(?I2,?I3) -> STARTS(?I1,?I3)
            /// </summary>
            StartsStartsEntailment = 99,
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