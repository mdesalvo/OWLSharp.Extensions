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
    internal static class TIMEInstantAfterAnalysisRule
    {
        internal static readonly string rulename = nameof(TIMEEnums.TIMEValidatorRules.InstantAfterAnalysis);
        internal const string rulesugg1 = "There should not be OWL-TIME instants having a clash in temporal relations (time:after VS time:after)";
        internal const string rulesugg2 = "There should not be OWL-TIME instants having a clash in temporal relations (time:after VS time:before)";

        internal static Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename, rulesugg1,
                RDFVocabulary.TIME.AFTER, RDFVocabulary.TIME.AFTER, true,
                "instants", "time:after VS time:after"));

            issues.AddRange(TIMEValidatorHelper.CheckRelationClash(ontology, rulename, rulesugg2,
                RDFVocabulary.TIME.AFTER, RDFVocabulary.TIME.BEFORE, false,
                "instants", "time:after VS time:before"));

            return Task.FromResult(issues);
        }
    }
}
