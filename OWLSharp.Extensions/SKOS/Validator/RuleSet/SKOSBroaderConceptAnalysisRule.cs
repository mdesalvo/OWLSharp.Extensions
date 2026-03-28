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
    internal static class SKOSBroaderConceptAnalysisRule
    {
        internal static readonly string rulename = nameof(SKOSEnums.SKOSValidatorRules.BroaderConceptAnalysis);
        internal const string rulesugg1A = "There should not be SKOS concepts having a clash in hierarchical relations (skos:broader VS skos:narrower)";
        internal const string rulesugg1B = "There should not be SKOS concepts having a clash in hierarchical relations (skos:broaderTransitive VS skos:narrowerTransitive)";
        internal const string rulesugg2A = "There should not be SKOS concepts having a clash in hierarchical VS associative relations (skos:broader VS skos:related)";
        internal const string rulesugg2B = "There should not be SKOS concepts having a clash in hierarchical VS associative relations (skos:broaderTransitive VS skos:related)";
        internal const string rulesugg3A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broader VS skos:narrowMatch)";
        internal const string rulesugg3B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broaderTransitive VS skos:narrowMatch)";
        internal const string rulesugg4A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broader VS skos:closeMatch)";
        internal const string rulesugg4B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broaderTransitive VS skos:closeMatch)";
        internal const string rulesugg5A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broader VS skos:exactMatch)";
        internal const string rulesugg5B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broaderTransitive VS skos:exactMatch)";
        internal const string rulesugg6A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broader VS skos:relatedMatch)";
        internal const string rulesugg6B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:broaderTransitive VS skos:relatedMatch)";

        internal static async Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();

            //skos:broader VS skos:narrower
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.NARROWER, rulesugg1A,
                "hierarchical relations (skos:broader VS skos:narrower)", issues);

            //skos:broaderTransitive VS skos:narrowerTransitive
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.NARROWER_TRANSITIVE, rulesugg1B,
                "hierarchical relations (skos:broaderTransitive VS skos:narrowerTransitive)", issues);

            //skos:broader VS skos:related
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.RELATED, rulesugg2A,
                "hierarchical/associative relations (skos:broader VS skos:related)", issues);

            //skos:broaderTransitive VS skos:related
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.RELATED, rulesugg2B,
                "hierarchical/associative relations (skos:broaderTransitive VS skos:related)", issues);

            //skos:broader VS skos:narrowMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.NARROW_MATCH, rulesugg3A,
                "hierarchical/mapping relations (skos:broader VS skos:narrowMatch)", issues);

            //skos:broaderTransitive VS skos:narrowMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.NARROW_MATCH, rulesugg3B,
                "hierarchical/mapping relations (skos:broaderTransitive VS skos:narrowMatch)", issues);

            //skos:broader VS skos:closeMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.CLOSE_MATCH, rulesugg4A,
                "hierarchical/mapping relations (skos:broader VS skos:closeMatch)", issues);

            //skos:broaderTransitive VS skos:closeMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.CLOSE_MATCH, rulesugg4B,
                "hierarchical/mapping relations (skos:broaderTransitive VS skos:closeMatch)", issues);

            //skos:broader VS skos:exactMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.EXACT_MATCH, rulesugg5A,
                "hierarchical/mapping relations (skos:broader VS skos:exactMatch)", issues);

            //skos:broaderTransitive VS skos:exactMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.EXACT_MATCH, rulesugg5B,
                "hierarchical/mapping relations (skos:broaderTransitive VS skos:exactMatch)", issues);

            //skos:broader VS skos:relatedMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.RELATED_MATCH, rulesugg6A,
                "hierarchical/mapping relations (skos:broader VS skos:relatedMatch)", issues);

            //skos:broaderTransitive VS skos:relatedMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.RELATED_MATCH, rulesugg6B,
                "hierarchical/mapping relations (skos:broaderTransitive VS skos:relatedMatch)", issues);

            return issues;
        }
    }
}
