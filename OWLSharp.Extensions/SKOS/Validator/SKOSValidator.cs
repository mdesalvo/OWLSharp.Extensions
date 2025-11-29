/*
   Copyright 2014-2025 Marco De Salvo
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

#if !NET8_0_OR_GREATER
using Dasync.Collections;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWLSharp.Ontology;
using OWLSharp.Validator;
using RDFSharp.Model;

namespace OWLSharp.Extensions.SKOS
{
    /// <summary>
    /// SKOSValidator is an analysis engine that checks that a SKOS vocabulary
    /// conforms to the SKOS data model specification. It verifies structural integrity by
    /// ensuring that concepts have required properties (like preferred labels),
    /// validates semantic consistency of hierarchical relationships (broader/narrower),
    /// detects logical issues like concept scheme violations or orphaned concepts,
    /// and checks for SKOS-specific constraints such as label conflicts or invalid
    /// documentation properties. It helps at maintaining quality and interoperability
    /// of controlled vocabularies by identifying violations of SKOS integrity rules defined
    /// in the W3C recommendation.
    /// </summary>
    public sealed class SKOSValidator
    {
        #region Properties
        internal static readonly RDFResource ViolationIRI = new RDFResource("urn:owlsharp:swrl:hasViolations");

        /// <summary>
        /// A predefined validator including all available SKOS/SKOS-XL validator rules
        /// </summary>
        public static readonly SKOSValidator Default = new SKOSValidator {
            Rules = Enum.GetValues(typeof(SKOSEnums.SKOSValidatorRules)).Cast<SKOSEnums.SKOSValidatorRules>().ToList() };

        /// <summary>
        /// The set of rules to be applied by the validator
        /// </summary>
        public List<SKOSEnums.SKOSValidatorRules> Rules { get; internal set; } = new List<SKOSEnums.SKOSValidatorRules>();
        #endregion

        #region Methods
        /// <summary>
        /// Adds the given rule to the validator
        /// </summary>
        /// <returns>The validator itself</returns>
        public SKOSValidator AddRule(SKOSEnums.SKOSValidatorRules rule)
        {
            Rules.Add(rule);
            return this;
        }

        /// <summary>
        /// Applies the validator on the given ontology
        /// </summary>
        /// <returns>The list of detected issues</returns>
        public async Task<List<OWLIssue>> ApplyToOntologyAsync(OWLOntology ontology)
        {
            List<OWLIssue> issues = new List<OWLIssue>();

            if (ontology != null)
            {
                OWLEvents.RaiseInfo($"Launching SKOS validator on ontology '{ontology.IRI}'...");

                //Initialize issue registry
                Dictionary<string, List<OWLIssue>> issueRegistry = new Dictionary<string, List<OWLIssue>>(Rules.Count);
                Rules = Rules.Distinct().ToList();
                Rules.ForEach(rule => issueRegistry.Add(rule.ToString(), null));

                //Execute validator rules
#if !NET8_0_OR_GREATER
                await Rules.ParallelForEachAsync(async (rule, _) =>
#else
                await Parallel.ForEachAsync(Rules, async (rule, _) =>
#endif
                {
                    OWLEvents.RaiseInfo($"Launching SKOS rule {rule}...");

                    switch (rule)
                    {
                        case SKOSEnums.SKOSValidatorRules.AlternativeLabelAnalysis:
                            issueRegistry[SKOSAlternativeLabelAnalysisRule.rulename] = await SKOSAlternativeLabelAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.HiddenLabelAnalysis:
                            issueRegistry[SKOSHiddenLabelAnalysisRule.rulename] = await SKOSHiddenLabelAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.PreferredLabelAnalysis:
                            issueRegistry[SKOSPreferredLabelAnalysisRule.rulename] = await SKOSPreferredLabelAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.NotationAnalysis:
                            issueRegistry[SKOSNotationAnalysisRule.rulename] = await SKOSNotationAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.BroaderConceptAnalysis:
                            issueRegistry[SKOSBroaderConceptAnalysisRule.rulename] = await SKOSBroaderConceptAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.NarrowerConceptAnalysis:
                            issueRegistry[SKOSNarrowerConceptAnalysisRule.rulename] = await SKOSNarrowerConceptAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.CloseOrExactMatchConceptAnalysis:
                            issueRegistry[SKOSCloseOrExactMatchConceptAnalysisRule.rulename] = await SKOSCloseOrExactMatchConceptAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.RelatedConceptAnalysis:
                            issueRegistry[SKOSRelatedConceptAnalysisRule.rulename] = await SKOSRelatedConceptAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                        case SKOSEnums.SKOSValidatorRules.LiteralFormAnalysis:
                            issueRegistry[SKOSXLLiteralFormAnalysisRule.rulename] = await SKOSXLLiteralFormAnalysisRule.ExecuteRuleAsync(ontology);
                            break;
                    }

                    OWLEvents.RaiseInfo($"Completed SKOS rule {rule} => {issueRegistry[rule.ToString()].Count} issues");
                });

                //Process issues
                issues.AddRange(issueRegistry.SelectMany(ir => ir.Value ?? Enumerable.Empty<OWLIssue>()));

                OWLEvents.RaiseInfo($"Completed SKOS validator on ontology {ontology.IRI} => {issues.Count} issues");
            }

            return issues;
        }
        #endregion
    }
}