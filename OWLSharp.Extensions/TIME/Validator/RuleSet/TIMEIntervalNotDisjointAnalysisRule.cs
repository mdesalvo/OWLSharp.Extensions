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
    internal static class TIMEIntervalNotDisjointAnalysisRule
    {
        internal static readonly string rulename = nameof(TIMEEnums.TIMEValidatorRules.IntervalNotDisjointAnalysis);
        internal const string rulesugg = "There should not be OWL-TIME intervals having a clash in temporal relations (time:notDisjoint VS {0})";

        internal static Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();
            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalBefore"),
                RDFVocabulary.TIME.NOT_DISJOINT, RDFVocabulary.TIME.INTERVAL_BEFORE, false,
                "intervals", $"time:notDisjoint VS time:intervalBefore"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalAfter"),
                RDFVocabulary.TIME.NOT_DISJOINT, RDFVocabulary.TIME.INTERVAL_AFTER, false,
                "intervals", $"time:notDisjoint VS time:intervalAfter"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename,
                string.Format(rulesugg, "time:intervalDisjoint"),
                RDFVocabulary.TIME.NOT_DISJOINT, RDFVocabulary.TIME.INTERVAL_DISJOINT, false,
                "intervals", $"time:notDisjoint VS time:intervalDisjoint"));

            return Task.FromResult(issues);
        }
    }
}
