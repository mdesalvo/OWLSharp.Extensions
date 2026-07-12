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
    internal static class TIMEIntervalHasInsideAnalysisRule
    {
        internal static readonly string rulename = nameof(TIMEEnums.TIMEValidatorRules.IntervalHasInsideAnalysis);
        internal const string rulesugg = "There should not be OWL-TIME intervals having a clash in temporal relations (time:hasInside VS {0})";

        internal static Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();
            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalBefore"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_BEFORE, false,
                "intervals", $"time:hasInside VS time:intervalBefore"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalAfter"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_AFTER, false,
                "intervals", $"time:hasInside VS time:intervalAfter"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalDisjoint"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_DISJOINT, false,
                "intervals", $"time:hasInside VS time:intervalDisjoint"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalDuring"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_DURING, false,
                "intervals", $"time:hasInside VS time:intervalDuring"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalEquals"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_EQUALS, false,
                "intervals", $"time:hasInside VS time:intervalEquals"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalFinishes"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_FINISHES, false,
                "intervals", $"time:hasInside VS time:intervalFinishes"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalIn"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_IN, false,
                "intervals", $"time:hasInside VS time:intervalIn"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalMeets"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_MEETS, false,
                "intervals", $"time:hasInside VS time:intervalMeets"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalMetBy"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_MET_BY, false,
                "intervals", $"time:hasInside VS time:intervalMetBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalOverlappedBy"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_OVERLAPPED_BY, false,
                "intervals", $"time:hasInside VS time:intervalOverlappedBy"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalOverlaps"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_OVERLAPS, false,
                "intervals", $"time:hasInside VS time:intervalOverlaps"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalStarts"),
                RDFVocabulary.TIME.HAS_INSIDE, RDFVocabulary.TIME.INTERVAL_STARTS, false,
                "intervals", $"time:hasInside VS time:intervalStarts"));

            return Task.FromResult(issues);
        }
    }
}
