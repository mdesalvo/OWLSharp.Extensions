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
    internal static class TIMEIntervalDisjointAnalysisRule
    {
        internal static readonly string rulename = nameof(TIMEEnums.TIMEValidatorRules.IntervalDisjointAnalysis);
        internal const string rulesugg = "There should not be OWL-TIME intervals having a clash in temporal relations (time:intervalDisjoint VS {0})";

        internal static Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();
            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalContains"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_CONTAINS, false,
                "intervals", $"time:intervalDisjoint VS time:intervalContains"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalDuring"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_DURING, false,
                "intervals", $"time:intervalDisjoint VS time:intervalDuring"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalEquals"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_EQUALS, false,
                "intervals", $"time:intervalDisjoint VS time:intervalEquals"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalFinishedBy"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_FINISHED_BY, false,
                "intervals", $"time:intervalDisjoint VS time:intervalFinishedBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalFinishes"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_FINISHES, false,
                "intervals", $"time:intervalDisjoint VS time:intervalFinishes"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:hasInside"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.HAS_INSIDE, true,
                "intervals", $"time:intervalDisjoint VS time:hasInside"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalIn"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_IN, false,
                "intervals", $"time:intervalDisjoint VS time:intervalIn"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalMeets"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_MEETS, false,
                "intervals", $"time:intervalDisjoint VS time:intervalMeets"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalMetBy"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_MET_BY, false,
                "intervals", $"time:intervalDisjoint VS time:intervalMetBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:notDisjoint"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.NOT_DISJOINT, false,
                "intervals", $"time:intervalDisjoint VS time:notDisjoint"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalOverlappedBy"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, false,
                "intervals", $"time:intervalDisjoint VS time:intervalOverlappedBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalOverlaps"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_OVERLAPS, false,
                "intervals", $"time:intervalDisjoint VS time:intervalOverlaps"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalStartedBy"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_STARTED_BY, false,
                "intervals", $"time:intervalDisjoint VS time:intervalStartedBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalStarts"),
                RDFVocabulary.TIME.INTERVAL_DISJOINT, RDFVocabulary.TIME.INTERVAL_STARTS, false,
                "intervals", $"time:intervalDisjoint VS time:intervalStarts"));

            return Task.FromResult(issues);
        }
    }
}
