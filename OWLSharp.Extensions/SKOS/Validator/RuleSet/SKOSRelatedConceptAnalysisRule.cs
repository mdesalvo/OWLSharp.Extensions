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
    internal static class SKOSRelatedConceptAnalysisRule
    {
        internal static readonly string rulename = nameof(SKOSEnums.SKOSValidatorRules.RelatedConceptAnalysis);
        internal const string rulesugg1A = "There should not be SKOS concepts having a clash in associative VS hierarchical relations (skos:related VS skos:broader)";
        internal const string rulesugg1B = "There should not be SKOS concepts having a clash in associative VS hierarchical relations (skos:relatedMatch VS skos:broader)";
        internal const string rulesugg2A = "There should not be SKOS concepts having a clash in associative VS hierarchical relations (skos:related VS skos:narrower)";
        internal const string rulesugg2B = "There should not be SKOS concepts having a clash in associative VS hierarchical relations (skos:relatedMatch VS skos:narrower)";
        internal const string rulesugg3A = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:related VS skos:broadMatch)";
        internal const string rulesugg3B = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:relatedMatch VS skos:broadMatch)";
        internal const string rulesugg4A = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:related VS skos:narrowMatch)";
        internal const string rulesugg4B = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:relatedMatch VS skos:narrowMatch)";
        internal const string rulesugg5A = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:related VS skos:closeMatch)";
        internal const string rulesugg5B = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:relatedMatch VS skos:closeMatch)";
        internal const string rulesugg6A = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:related VS skos:exactMatch)";
        internal const string rulesugg6B = "There should not be SKOS concepts having a clash in associative VS mapping relations (skos:relatedMatch VS skos:exactMatch)";

        internal static async Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();

            //skos:related VS skos:broader
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.BROADER, rulesugg1A,
                "associative VS hierarchical relations (skos:related VS skos:broader)", issues);

            //skos:relatedMatch VS skos:broader
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.BROADER, rulesugg1B,
                "associative VS hierarchical relations (skos:relatedMatch VS skos:broader)", issues);

            //skos:related VS skos:narrower
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.NARROWER, rulesugg2A,
                "associative VS hierarchical relations (skos:related VS skos:narrower)", issues);

            //skos:relatedMatch VS skos:narrower
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.NARROWER, rulesugg2B,
                "associative VS mapping relations (skos:relatedMatch VS skos:narrower)", issues);

            //skos:related VS skos:broadMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.BROAD_MATCH, rulesugg3A,
                "associative VS mapping relations (skos:related VS skos:broadMatch)", issues);

            //skos:relatedMatch VS skos:broadMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.BROAD_MATCH, rulesugg3B,
                "associative VS mapping relations (skos:relatedMatch VS skos:broadMatch)", issues);

            //skos:related VS skos:narrowMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.NARROW_MATCH, rulesugg4A,
                "associative VS mapping relations (skos:related VS skos:narrowMatch)", issues);

            //skos:relatedMatch VS skos:narrowMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.NARROW_MATCH, rulesugg4B,
                "associative VS mapping relations (skos:relatedMatch VS skos:narrowMatch)", issues);

            //skos:related VS skos:closeMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.CLOSE_MATCH, rulesugg5A,
                "associative VS mapping relations (skos:related VS skos:closeMatch)", issues);

            //skos:relatedMatch VS skos:closeMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.CLOSE_MATCH, rulesugg5B,
                "associative VS mapping relations (skos:relatedMatch VS skos:closeMatch)", issues);

            //skos:related VS skos:exactMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.EXACT_MATCH, rulesugg6A,
                "associative VS mapping relations (skos:related VS skos:exactMatch)", issues);

            //skos:relatedMatch VS skos:exactMatch
            await SKOSHelper.CheckConceptRelationClashAsync(ontology, rulename,
                RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.EXACT_MATCH, rulesugg6B,
                "associative VS mapping relations (skos:relatedMatch VS skos:exactMatch)", issues);

            return issues;
        }
    }
}
