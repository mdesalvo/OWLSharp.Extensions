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
using OWLSharp.Ontology;
using OWLSharp.Reasoner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// Provides reasoning and inference capabilities for OWL-TIME ontologies, enabling automatic derivation of
    /// temporal relationships (before, after, during, overlaps), calculation of temporal distances,
    /// interval intersections, temporal constraint propagation, and consistency checking of temporal assertions
    /// across different temporal reference systems
    /// </summary>
    public sealed class TIMEReasoner
    {
        #region Properties
        /// <summary>
        /// A predefined reasoner including all available OWL-TIME inference rules
        /// </summary>
        public static readonly TIMEReasoner Default = new TIMEReasoner {
            Rules = Enum.GetValues(typeof(TIMEEnums.TIMEReasonerRules)).Cast<TIMEEnums.TIMEReasonerRules>().ToList() };

        /// <summary>
        /// The set of rules to be applied by the reasoner
        /// </summary>
        public List<TIMEEnums.TIMEReasonerRules> Rules { get; internal set; } = new List<TIMEEnums.TIMEReasonerRules>();
        #endregion

        #region Methods
        /// <summary>
        /// Adds the given rule to the reasoner
        /// </summary>
        /// <returns>The reasoner itself</returns>
        public TIMEReasoner AddRule(TIMEEnums.TIMEReasonerRules rule)
        {
            Rules.Add(rule);
            return this;
        }

        /// <summary>
        /// Applies the reasoner on the given ontology, using the eventually specified options
        /// </summary>
        /// <returns>The list of discovered inferences</returns>
        public async Task<List<OWLInference>> ApplyToOntologyAsync(OWLOntology ontology, OWLReasonerOptions reasonerOptions=null)
        {
            List<OWLInference> inferences = new List<OWLInference>();

            if (ontology != null)
            {
                if (reasonerOptions == null)
                    reasonerOptions = new OWLReasonerOptions();
                Rules = Rules.Distinct().ToList();

                #region Execute
                OWLEvents.RaiseInfo($"Launching OWL-TIME reasoner on ontology '{ontology.IRI}'...");

                //Initialize inference registry
                Dictionary<string, List<OWLInference>> inferenceRegistry = new Dictionary<string, List<OWLInference>>(Rules.Count);
                Rules.ForEach(timeRule => inferenceRegistry.Add(timeRule.ToString(), null));

                //Initialize axioms XML
                Task<HashSet<string>> dtPropAsnAxiomsTask = Task.Run(() => new HashSet<string>(ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>().Select(asn => asn.GetXML())));
                Task<HashSet<string>> opPropAsnAxiomsTask = Task.Run(() => new HashSet<string>(ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>().Select(asn => asn.GetXML())));

                //Execute OWL-TIME reasoner rules
#if !NET8_0_OR_GREATER
                await Rules.ParallelForEachAsync(async (rule, _) =>
#else
                await Parallel.ForEachAsync(Rules, async (rule, _) =>
#endif
                {
                    OWLEvents.RaiseInfo($"Launching OWL-TIME rule {rule}...");

                    switch (rule)
                    {
                        case TIMEEnums.TIMEReasonerRules.AfterEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterEqualsEntailment)] = await TIMEAfterEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.AfterFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterFinishesEntailment)] = await TIMEAfterFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.AfterMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterMetByEntailment)] = await TIMEAfterMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.AfterTransitiveEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterTransitiveEntailment)] = await TIMEAfterTransitiveEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeEqualsEntailment)] = await TIMEBeforeEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeMeetsEntailment)] = await TIMEBeforeMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeStartsEntailment)] = await TIMEBeforeStartsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeTransitiveEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeTransitiveEntailment)] = await TIMEBeforeTransitiveEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.ContainsEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.ContainsEqualsEntailment)] = await TIMEContainsEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.ContainsTransitiveEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.ContainsTransitiveEntailment)] = await TIMEContainsTransitiveEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringEqualsEntailment)] = await TIMEDuringEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringTransitiveEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringTransitiveEntailment)] = await TIMEDuringTransitiveEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsEntailment)] = await TIMEEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsInverseEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsInverseEntailment)] = await TIMEEqualsInverseEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsTransitiveEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsTransitiveEntailment)] = await TIMEEqualsTransitiveEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByEqualsEntailment)] = await TIMEFinishedByEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesEqualsEntailment)] = await TIMEFinishesEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsEqualsEntailment)] = await TIMEMeetsEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsStartsEntailment)] = await TIMEMeetsStartsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByEqualsEntailment)] = await TIMEMetByEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlappedByEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlappedByEqualsEntailment)] = await TIMEOverlappedByEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlapsEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlapsEqualsEntailment)] = await TIMEOverlapsEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsEqualsEntailment)] = await TIMEStartsEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByEqualsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByEqualsEntailment)] = await TIMEStartedByEqualsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                    }

                    OWLEvents.RaiseInfo($"Completed OWL-TIME rule {rule} => {inferenceRegistry[rule.ToString()].Count} candidate inferences");
                });

                //Deduplicate inferences by analyzing explicit knowledge
                await Task.WhenAll(dtPropAsnAxiomsTask, opPropAsnAxiomsTask);
                foreach (KeyValuePair<string, List<OWLInference>> inferenceRegistryEntry in inferenceRegistry.Where(ir => ir.Value?.Count > 0))
                {
                    inferenceRegistryEntry.Value.RemoveAll(inf =>
                    {
                        string infXML = inf.Axiom.GetXML();
                        return dtPropAsnAxiomsTask.Result.Contains(infXML)
                               || opPropAsnAxiomsTask.Result.Contains(infXML);
                    });
                }

                //Collect inferences
                IEnumerable<OWLInference> emptyInferenceSet = Enumerable.Empty<OWLInference>();
                inferences.AddRange(inferenceRegistry.SelectMany(ir => ir.Value ?? emptyInferenceSet).Distinct());

                OWLEvents.RaiseInfo($"Completed OWL-TIME reasoner on ontology {ontology.IRI} => {inferences.Count} unique inferences");
                #endregion

                #region Iterate
                if (reasonerOptions.EnableIterativeReasoning
                     && inferences.Count > 0)
                {
                    OWLEvents.RaiseInfo("Merging OWL-TIME inferences...");
                    foreach (IGrouping<Type, OWLInference> inferenceGroupType in inferences.GroupBy(inf => inf.Axiom.GetType()))
                    {
                        switch (inferenceGroupType.Key.BaseType?.Name)
                        {
                            case nameof(OWLAssertionAxiom):
                                ontology.AssertionAxioms.AddRange(inferenceGroupType.Select(g => (OWLAssertionAxiom)g.Axiom));
                                ontology.AssertionAxioms = OWLAxiomHelper.RemoveDuplicates(ontology.AssertionAxioms);
                                break;
                        }
                    }
                    reasonerOptions.CurrentIteration = 1;
                    inferences.AddRange(await ApplyToOntologyAsync(ontology, reasonerOptions));
                }
                #endregion
            }

            return inferences;
        }
        #endregion
    }
}