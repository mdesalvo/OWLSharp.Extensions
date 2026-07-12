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
                        case TIMEEnums.TIMEReasonerRules.AfterContainsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterContainsEntailment)] = await TIMEAfterContainsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.AfterFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterFinishedByEntailment)] = await TIMEAfterFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.AfterOverlappedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterOverlappedByEntailment)] = await TIMEAfterOverlappedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.AfterStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.AfterStartedByEntailment)] = await TIMEAfterStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeContainsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeContainsEntailment)] = await TIMEBeforeContainsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeFinishedByEntailment)] = await TIMEBeforeFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeOverlapsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeOverlapsEntailment)] = await TIMEBeforeOverlapsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.BeforeStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.BeforeStartedByEntailment)] = await TIMEBeforeStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.ContainsFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.ContainsFinishedByEntailment)] = await TIMEContainsFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.ContainsStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.ContainsStartedByEntailment)] = await TIMEContainsStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringAfterEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringAfterEntailment)] = await TIMEDuringAfterEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringBeforeEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringBeforeEntailment)] = await TIMEDuringBeforeEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringFinishesEntailment)] = await TIMEDuringFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringMeetsEntailment)] = await TIMEDuringMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringMetByEntailment)] = await TIMEDuringMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.DuringStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.DuringStartsEntailment)] = await TIMEDuringStartsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsAfterEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsAfterEntailment)] = await TIMEEqualsAfterEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsBeforeEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsBeforeEntailment)] = await TIMEEqualsBeforeEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsContainsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsContainsEntailment)] = await TIMEEqualsContainsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsDuringEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsDuringEntailment)] = await TIMEEqualsDuringEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsFinishedByEntailment)] = await TIMEEqualsFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsFinishesEntailment)] = await TIMEEqualsFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsMeetsEntailment)] = await TIMEEqualsMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsMetByEntailment)] = await TIMEEqualsMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsOverlappedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsOverlappedByEntailment)] = await TIMEEqualsOverlappedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsOverlapsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsOverlapsEntailment)] = await TIMEEqualsOverlapsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsStartedByEntailment)] = await TIMEEqualsStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.EqualsStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.EqualsStartsEntailment)] = await TIMEEqualsStartsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByBeforeEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByBeforeEntailment)] = await TIMEFinishedByBeforeEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByContainsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByContainsEntailment)] = await TIMEFinishedByContainsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByFinishedByEntailment)] = await TIMEFinishedByFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByMeetsEntailment)] = await TIMEFinishedByMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByOverlapsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByOverlapsEntailment)] = await TIMEFinishedByOverlapsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByStartedByEntailment)] = await TIMEFinishedByStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishedByStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishedByStartsEntailment)] = await TIMEFinishedByStartsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesAfterEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesAfterEntailment)] = await TIMEFinishesAfterEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesBeforeEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesBeforeEntailment)] = await TIMEFinishesBeforeEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesDuringEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesDuringEntailment)] = await TIMEFinishesDuringEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesFinishesEntailment)] = await TIMEFinishesFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesMeetsEntailment)] = await TIMEFinishesMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesMetByEntailment)] = await TIMEFinishesMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.FinishesStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.FinishesStartsEntailment)] = await TIMEFinishesStartsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsBeforeEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsBeforeEntailment)] = await TIMEMeetsBeforeEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsContainsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsContainsEntailment)] = await TIMEMeetsContainsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsFinishedByEntailment)] = await TIMEMeetsFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsMeetsEntailment)] = await TIMEMeetsMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsOverlapsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsOverlapsEntailment)] = await TIMEMeetsOverlapsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MeetsStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MeetsStartedByEntailment)] = await TIMEMeetsStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByAfterEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByAfterEntailment)] = await TIMEMetByAfterEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByContainsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByContainsEntailment)] = await TIMEMetByContainsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByFinishedByEntailment)] = await TIMEMetByFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByFinishesEntailment)] = await TIMEMetByFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByMetByEntailment)] = await TIMEMetByMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByOverlappedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByOverlappedByEntailment)] = await TIMEMetByOverlappedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.MetByStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.MetByStartedByEntailment)] = await TIMEMetByStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlappedByAfterEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlappedByAfterEntailment)] = await TIMEOverlappedByAfterEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlappedByFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlappedByFinishesEntailment)] = await TIMEOverlappedByFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlappedByMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlappedByMetByEntailment)] = await TIMEOverlappedByMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlapsBeforeEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlapsBeforeEntailment)] = await TIMEOverlapsBeforeEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlapsMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlapsMeetsEntailment)] = await TIMEOverlapsMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.OverlapsStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.OverlapsStartsEntailment)] = await TIMEOverlapsStartsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByAfterEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByAfterEntailment)] = await TIMEStartedByAfterEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByContainsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByContainsEntailment)] = await TIMEStartedByContainsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByFinishedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByFinishedByEntailment)] = await TIMEStartedByFinishedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByFinishesEntailment)] = await TIMEStartedByFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByMetByEntailment)] = await TIMEStartedByMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByOverlappedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByOverlappedByEntailment)] = await TIMEStartedByOverlappedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartedByStartedByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartedByStartedByEntailment)] = await TIMEStartedByStartedByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsAfterEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsAfterEntailment)] = await TIMEStartsAfterEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsBeforeEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsBeforeEntailment)] = await TIMEStartsBeforeEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsDuringEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsDuringEntailment)] = await TIMEStartsDuringEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsFinishesEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsFinishesEntailment)] = await TIMEStartsFinishesEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsMeetsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsMeetsEntailment)] = await TIMEStartsMeetsEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsMetByEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsMetByEntailment)] = await TIMEStartsMetByEntailmentRule.ExecuteRuleAsync(ontology);
                            break;
                        case TIMEEnums.TIMEReasonerRules.StartsStartsEntailment:
                            inferenceRegistry[nameof(TIMEEnums.TIMEReasonerRules.StartsStartsEntailment)] = await TIMEStartsStartsEntailmentRule.ExecuteRuleAsync(ontology);
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