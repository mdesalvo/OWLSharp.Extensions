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

namespace OWLSharp.Extensions.SKOS
{
    /// <summary>
    /// Detects cyclic hierarchical relations between SKOS concepts (e.g., A broader B and B broader A)
    /// </summary>
    internal static class SKOSHierarchyCycleAnalysisRule
    {
        internal static readonly string rulename = nameof(SKOSEnums.SKOSValidatorRules.HierarchyCycleAnalysis);
        internal const string rulesugg1A = "There should not be SKOS concepts having a cycle in hierarchical relations (skos:broader)";
        internal const string rulesugg1B = "There should not be SKOS concepts having a cycle in hierarchical relations (skos:broaderTransitive)";
        internal const string rulesugg2A = "There should not be SKOS concepts having a cycle in hierarchical relations (skos:narrower)";
        internal const string rulesugg2B = "There should not be SKOS concepts having a cycle in hierarchical relations (skos:narrowerTransitive)";

        internal static async Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();

            //skos:broader cycle (A broader B AND B broader A)
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.BROADER, rulesugg1A,
                "hierarchical cycle (skos:broader)", issues, invertSecondProperty: true);

            //skos:broaderTransitive cycle
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.BROADER_TRANSITIVE, rulesugg1B,
                "hierarchical cycle (skos:broaderTransitive)", issues, invertSecondProperty: true);

            //skos:narrower cycle (A narrower B AND B narrower A)
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.NARROWER, rulesugg2A,
                "hierarchical cycle (skos:narrower)", issues, invertSecondProperty: true);

            //skos:narrowerTransitive cycle
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.NARROWER_TRANSITIVE, rulesugg2B,
                "hierarchical cycle (skos:narrowerTransitive)", issues, invertSecondProperty: true);

            return issues;
        }
    }
}
