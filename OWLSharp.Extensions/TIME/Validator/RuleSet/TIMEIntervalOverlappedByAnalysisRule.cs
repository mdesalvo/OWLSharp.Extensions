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

using OWLSharp.Ontology;
using OWLSharp.Validator;
using RDFSharp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OWLSharp.Extensions.TIME
{
    internal static class TIMEIntervalOverlappedByAnalysisRule
    {
        internal static readonly string rulename = nameof(TIMEEnums.TIMEValidatorRules.IntervalOverlappedByAnalysis);
        internal const string rulesugg = "There should not be OWL-TIME intervals having a clash in temporal relations (time:intervalOverlappedBy VS {0})";

        internal static Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();
            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalBefore"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_BEFORE, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalBefore"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalAfter"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_AFTER, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalAfter"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalContains"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_CONTAINS, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalContains"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalDisjoint"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_DISJOINT, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalDisjoint"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalDuring"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_DURING, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalDuring"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalEquals"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_EQUALS, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalEquals"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalFinishedBy"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_FINISHED_BY, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalFinishedBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalFinishes"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_FINISHES, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalFinishes"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:hasInside"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.HAS_INSIDE, false,
                "intervals", $"time:intervalOverlappedBy VS time:hasInside"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalIn"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_IN, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalIn"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalMeets"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_MEETS, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalMeets"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalMetBy"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_MET_BY, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalMetBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalOverlaps"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_OVERLAPS, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalOverlaps"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalStartedBy"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_STARTED_BY, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalStartedBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalStarts"),
                RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, RDFVocabulary.TIME.INTERVAL_STARTS, false,
                "intervals", $"time:intervalOverlappedBy VS time:intervalStarts"));

            return Task.FromResult(issues);
        }
    }
}
