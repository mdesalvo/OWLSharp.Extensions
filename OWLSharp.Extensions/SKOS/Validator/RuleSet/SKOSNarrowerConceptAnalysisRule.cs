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
    internal static class SKOSNarrowerConceptAnalysisRule
    {
        internal static readonly string rulename = nameof(SKOSEnums.SKOSValidatorRules.NarrowerConceptAnalysis);
        internal const string rulesugg1A = "There should not be SKOS concepts having a clash in hierarchical relations (skos:narrower VS skos:broader)";
        internal const string rulesugg1B = "There should not be SKOS concepts having a clash in hierarchical relations (skos:narrowerTransitive VS skos:broaderTransitive)";
        internal const string rulesugg2A = "There should not be SKOS concepts having a clash in hierarchical VS associative relations (skos:narrower VS skos:related)";
        internal const string rulesugg2B = "There should not be SKOS concepts having a clash in hierarchical VS associative relations (skos:narrowerTransitive VS skos:related)";
        internal const string rulesugg3A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrower VS skos:narrowMatch)";
        internal const string rulesugg3B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrowerTransitive VS skos:narrowMatch)";
        internal const string rulesugg4A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrower VS skos:closeMatch)";
        internal const string rulesugg4B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrowerTransitive VS skos:closeMatch)";
        internal const string rulesugg5A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrower VS skos:exactMatch)";
        internal const string rulesugg5B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrowerTransitive VS skos:exactMatch)";
        internal const string rulesugg6A = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrower VS skos:relatedMatch)";
        internal const string rulesugg6B = "There should not be SKOS concepts having a clash in hierarchical VS mapping relations (skos:narrowerTransitive VS skos:relatedMatch)";

        internal static async Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();

            //skos:narrower VS skos:broader
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.BROADER, rulesugg1A,
                "hierarchical relations (skos:narrower VS skos:broader)", issues);

            //skos:narrowerTransitive VS skos:broaderTransitive
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.BROADER_TRANSITIVE, rulesugg1B,
                "hierarchical relations (skos:narrowerTransitive VS skos:broaderTransitive)", issues);

            //skos:narrower VS skos:related
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.RELATED, rulesugg2A,
                "hierarchical/associative relations (skos:narrower VS skos:related)", issues);

            //skos:narrowerTransitive VS skos:related
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.RELATED, rulesugg2B,
                "hierarchical/associative relations (skos:narrowerTransitive VS skos:related)", issues);

            //skos:narrower VS skos:narrowMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.NARROW_MATCH, rulesugg3A,
                "hierarchical/mapping relations (skos:narrower VS skos:narrowMatch)", issues);

            //skos:narrowerTransitive VS skos:narrowMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.NARROW_MATCH, rulesugg3B,
                "hierarchical/mapping relations (skos:narrowerTransitive VS skos:narrowMatch)", issues);

            //skos:narrower VS skos:closeMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.CLOSE_MATCH, rulesugg4A,
                "hierarchical/mapping relations (skos:narrower VS skos:closeMatch)", issues);

            //skos:narrowerTransitive VS skos:closeMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.CLOSE_MATCH, rulesugg4B,
                "hierarchical/mapping relations (skos:narrowerTransitive VS skos:closeMatch)", issues);

            //skos:narrower VS skos:exactMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.EXACT_MATCH, rulesugg5A,
                "hierarchical/mapping relations (skos:narrower VS skos:exactMatch)", issues);

            //skos:narrowerTransitive VS skos:exactMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.EXACT_MATCH, rulesugg5B,
                "hierarchical/mapping relations (skos:narrowerTransitive VS skos:exactMatch)", issues);

            //skos:narrower VS skos:relatedMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.RELATED_MATCH, rulesugg6A,
                "hierarchical/mapping relations (skos:narrower VS skos:relatedMatch)", issues);

            //skos:narrowerTransitive VS skos:relatedMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.RELATED_MATCH, rulesugg6B,
                "hierarchical/mapping relations (skos:narrowerTransitive VS skos:relatedMatch)", issues);

            return issues;
        }
    }
}
