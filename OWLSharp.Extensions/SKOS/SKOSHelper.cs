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

using OWLSharp.Ontology;
using RDFSharp.Model;
using RDFSharp.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace OWLSharp.Extensions.SKOS
{
    /// <summary>
    /// SKOS engine of OWLSharp: it can declare and correlate SKOS concepts
    /// </summary>
    public static class SKOSHelper
    {
        #region Helper (Initializer, Declarer)
        /// <summary>
        /// Imports SKOS-related ontologies into the working ontology, enriching it with T-BOX required for SKOS modeling and validation
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static async Task InitializeSKOSAsync(this OWLOntology ontology, int timeoutMilliseconds=20000, int cacheMilliseconds=3600000)
        {
            if (ontology != null)
            {
                await ontology.ImportAsync(new Uri(RDFVocabulary.SKOS.DEREFERENCE_URI), timeoutMilliseconds, cacheMilliseconds);
                await ontology.ImportAsync(new Uri(RDFVocabulary.SKOS.SKOSXL.DEREFERENCE_URI), timeoutMilliseconds, cacheMilliseconds);
            }
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a SKOS concept scheme having the given name.<br/>
        /// It can also be given the set of concepts which all together form this concept scheme.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareConceptScheme(this OWLOntology ontology, RDFResource conceptScheme, RDFResource[] concepts=null)
        {
            #region Guards
            if (conceptScheme == null)
                throw new OWLException($"Cannot declare concept scheme because given '{nameof(conceptScheme)}' parameter is null");
            #endregion

            ontology.DeclareEntity(new OWLNamedIndividual(conceptScheme));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
                new OWLNamedIndividual(conceptScheme)));

            if (concepts?.Length > 0)
                foreach (RDFResource concept in concepts)
                {
                    ontology.DeclareEntity(new OWLNamedIndividual(concept));
                    ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                        RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                        new OWLNamedIndividual(concept)));
                    ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                        new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                        new OWLNamedIndividual(concept),
                        new OWLNamedIndividual(conceptScheme)));
                }

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a SKOS concept having the given name.<br/>
        /// It can also be given a set of preferred language labels and the concept scheme to which this concept belongs.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareConcept(this OWLOntology ontology, RDFResource concept, RDFPlainLiteral[] labels=null, RDFResource conceptScheme=null)
        {
            #region Guards
            if (concept == null)
                throw new OWLException($"Cannot declare concept because given '{nameof(concept)}' parameter is null");
            #endregion

            ontology.DeclareEntity(new OWLNamedIndividual(concept));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                new OWLNamedIndividual(concept)));

            if (labels?.Length > 0)
            {
                HashSet<string> langtagLookup = new HashSet<string>();
                foreach (RDFPlainLiteral preferredLabel in labels)
                {
                    //(S14) skos:prefLabel annotation requires uniqueness of language tags foreach rdfs:Resource
                    if (!langtagLookup.Add(preferredLabel.Language))
                        throw new OWLException($"Cannot setup preferred label of concept '{concept}' because having more than one occurrence of the same language tag is not allowed!");

                    ontology.DeclareAnnotationAxiom(new OWLAnnotationAssertion(
                        new OWLAnnotationProperty(RDFVocabulary.SKOS.PREF_LABEL),
                        concept,
                        new OWLLiteral(preferredLabel)));
                }
            }

            if (conceptScheme != null)
            {
                ontology.DeclareConceptScheme(conceptScheme);
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(concept),
                    new OWLNamedIndividual(conceptScheme)));
            }

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a SKOS collection having the given name and the given set of concepts.<br/>
        /// It can also be given a set of preferred language labels and the concept scheme to which this collection belongs.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareCollection(this OWLOntology ontology, RDFResource collection, RDFResource[] concepts, RDFPlainLiteral[] labels=null, RDFResource conceptScheme=null)
        {
            #region Guards
            if (collection == null)
                throw new OWLException($"Cannot declare collection because given '{nameof(collection)}' parameter is null");
            if (concepts == null)
                throw new OWLException($"Cannot declare collection because given '{nameof(concepts)}' parameter is null");
            if (concepts.Length == 0)
                throw new OWLException($"Cannot declare collection because given '{nameof(concepts)}' parameter must contain at least 1 element");
            #endregion

            ontology.DeclareEntity(new OWLNamedIndividual(collection));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.SKOS.COLLECTION.ToEntity<OWLClass>(),
                new OWLNamedIndividual(collection)));

            foreach (RDFResource concept in concepts)
            {
                ontology.DeclareConcept(concept);
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.MEMBER),
                    new OWLNamedIndividual(collection),
                    new OWLNamedIndividual(concept)));
            }

            if (labels?.Length > 0)
            {
                HashSet<string> langtagLookup = new HashSet<string>();
                foreach (RDFPlainLiteral preferredLabel in labels)
                {
                    //(S14) skos:prefLabel annotation requires uniqueness of language tags foreach rdfs:Resource
                    if (!langtagLookup.Add(preferredLabel.Language))
                        throw new OWLException($"Cannot setup preferred label of collection '{collection}' because having more than one occurrence of the same language tag is not allowed!");

                    ontology.DeclareAnnotationAxiom(new OWLAnnotationAssertion(
                        new OWLAnnotationProperty(RDFVocabulary.SKOS.PREF_LABEL),
                        collection,
                        new OWLLiteral(preferredLabel)));
                }
            }

            if (conceptScheme != null)
            {
                ontology.DeclareConceptScheme(conceptScheme);
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(collection),
                    new OWLNamedIndividual(conceptScheme)));
            }

            return ontology;
        }
        #endregion

        #region Analyzer
        /// <summary>
        /// Checks if the given SKOS concept scheme is found in the working ontology
        /// </summary>
        public static bool CheckHasConceptScheme(this OWLOntology ontology, RDFResource conceptScheme)
            => conceptScheme != null && ontology?.GetIndividualsOf(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>()).Any(cs => cs.GetIRI().Equals(conceptScheme)) == true;

        /// <summary>
        /// Checks if the given SKOS concept is found in the working ontology under the given scheme
        /// </summary>
        public static bool CheckHasConcept(this OWLOntology ontology, RDFResource conceptScheme, RDFResource concept)
            => conceptScheme != null && concept != null && ontology?.GetConceptsInScheme(conceptScheme).Any(c => c.Equals(concept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found in the working ontology under the given scheme
        /// </summary>
        public static List<RDFResource> GetConceptsInScheme(this OWLOntology ontology, RDFResource skosConceptScheme)
        {
            List<RDFResource> conceptsInScheme = new List<RDFResource>();

            if (skosConceptScheme != null && ontology != null)
            {
                OWLClass skosConcept = RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosInSchemeAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME));
                List<OWLObjectPropertyAssertion> skosHasTopConceptAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.HAS_TOP_CONCEPT));
                List<OWLObjectPropertyAssertion> skosTopConceptOfAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.TOP_CONCEPT_OF));

                //skos:inScheme
                conceptsInScheme.AddRange(
                    skosInSchemeAsns.Where(asn => ontology.CheckIsIndividualOf(skosConcept, asn.SourceIndividualExpression)
                                                    && asn.TargetIndividualExpression.GetIRI().Equals(skosConceptScheme))
                                    .Select(skosInSchemeAsn => skosInSchemeAsn.SourceIndividualExpression.GetIRI()));

                //skos:hasTopConcept
                conceptsInScheme.AddRange(
                    skosHasTopConceptAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptScheme))
                                         .Select(skosHasTopConceptAsn => skosHasTopConceptAsn.TargetIndividualExpression.GetIRI()));

                //skos:topConceptOf
                conceptsInScheme.AddRange(
                    skosTopConceptOfAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConceptScheme))
                                        .Select(skosTopConceptOfAsn => skosTopConceptOfAsn.SourceIndividualExpression.GetIRI()));
            }

            return RDFQueryUtilities.RemoveDuplicates(conceptsInScheme);
        }

        /// <summary>
        /// Checks if the given SKOS collection is found in the working ontology under the given scheme
        /// </summary>
        public static bool CheckHasCollection(this OWLOntology ontology, RDFResource conceptScheme, RDFResource collection)
            => conceptScheme != null && collection != null && ontology?.GetCollectionsInScheme(conceptScheme).Any(cl => cl.Equals(collection)) == true;

        /// <summary>
        /// Enlists the SKOS collections found in the working ontology under the given scheme
        /// </summary>
        public static List<RDFResource> GetCollectionsInScheme(this OWLOntology ontology, RDFResource skosConceptScheme)
        {
            List<RDFResource> collectionsInScheme = new List<RDFResource>();

            if (skosConceptScheme != null && ontology != null)
            {
                OWLClass skosCollection = RDFVocabulary.SKOS.COLLECTION.ToEntity<OWLClass>();
                OWLClass skosOrderedCollection = RDFVocabulary.SKOS.ORDERED_COLLECTION.ToEntity<OWLClass>();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosInSchemeAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME));

                //skos:inScheme
                collectionsInScheme.AddRange(
                    skosInSchemeAsns.Where(asn => (ontology.CheckIsIndividualOf(skosCollection, asn.SourceIndividualExpression)
                                                   || ontology.CheckIsIndividualOf(skosOrderedCollection, asn.SourceIndividualExpression))
                                                  && asn.TargetIndividualExpression.GetIRI().Equals(skosConceptScheme))
                                    .Select(skosInSchemeAsn => skosInSchemeAsn.SourceIndividualExpression.GetIRI()));
            }

            return RDFQueryUtilities.RemoveDuplicates(collectionsInScheme);
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:broader" or a "skos:broaderTransitive" hierarchy relation (child->parent)
        /// </summary>
        public static bool CheckHasBroaderConcept(this OWLOntology ontology, RDFResource childConcept, RDFResource parentConcept)
            => childConcept != null && parentConcept != null && ontology?.GetBroaderConcepts(childConcept).Any(concept => concept.Equals(parentConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be hierarchically broader than the given one in the working ontology
        /// </summary>
        public static List<RDFResource> GetBroaderConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> broaderConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosBroaderAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.BROADER));
                List<OWLObjectPropertyAssertion> skosBroaderTransitiveAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE));

                //skos:broader
                broaderConcepts.AddRange(
                    skosBroaderAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                   .Select(skosBroaderAsn => skosBroaderAsn.TargetIndividualExpression.GetIRI()));

                //skos:broaderTransitive
                broaderConcepts.AddRange(ontology.SubsumeBroaderTransitivity(skosConcept, skosBroaderTransitiveAsns, new Dictionary<long, RDFResource>()));
            }

            broaderConcepts.RemoveAll(c => c.Equals(skosConcept));
            return broaderConcepts;
        }
        internal static List<RDFResource> SubsumeBroaderTransitivity(this OWLOntology ontology, RDFResource skosConcept, List<OWLObjectPropertyAssertion> skosBroaderTransitiveAsns, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> broaderTransitiveConcepts = new List<RDFResource>();

            #region visitContext
            if (!visitContext.ContainsKey(skosConcept.PatternMemberID))
                visitContext.Add(skosConcept.PatternMemberID, skosConcept);
            else
                return broaderTransitiveConcepts;
            #endregion

            //skos:broaderTransitive
            broaderTransitiveConcepts.AddRange(
                skosBroaderTransitiveAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                         .Select(skosBroaderTransitiveAsn => skosBroaderTransitiveAsn.TargetIndividualExpression.GetIRI()));

            foreach (RDFResource broaderTransitiveConcept in broaderTransitiveConcepts.ToList())
                broaderTransitiveConcepts.AddRange(ontology.SubsumeBroaderTransitivity(broaderTransitiveConcept, skosBroaderTransitiveAsns, visitContext));

            return broaderTransitiveConcepts;
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:narrower" or a "skos:narrowerTransitive" hierarchy relation (parent->child)
        /// </summary>
        public static bool CheckHasNarrowerConcept(this OWLOntology ontology, RDFResource parentConcept, RDFResource childConcept)
            => parentConcept != null && childConcept != null && ontology?.GetNarrowerConcepts(parentConcept).Any(concept => concept.Equals(childConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be hierarchically narrower than the given one in the working ontology
        /// </summary>
        public static List<RDFResource> GetNarrowerConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> narrowerConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosNarrowerAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER));
                List<OWLObjectPropertyAssertion> skosNarrowerTransitiveAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER_TRANSITIVE));

                //skos:narrower
                narrowerConcepts.AddRange(
                    skosNarrowerAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                    .Select(skosNarrowerAsn => skosNarrowerAsn.TargetIndividualExpression.GetIRI()));

                //skos:narrowerTransitive
                narrowerConcepts.AddRange(ontology.SubsumeNarrowerTransitivity(skosConcept, skosNarrowerTransitiveAsns, new Dictionary<long, RDFResource>()));
            }

            narrowerConcepts.RemoveAll(c => c.Equals(skosConcept));
            return narrowerConcepts;
        }
        internal static List<RDFResource> SubsumeNarrowerTransitivity(this OWLOntology ontology, RDFResource skosConcept, List<OWLObjectPropertyAssertion> skosNarrowerTransitiveAsns, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> narrowerTransitiveConcepts = new List<RDFResource>();

            #region visitContext
            if (!visitContext.ContainsKey(skosConcept.PatternMemberID))
                visitContext.Add(skosConcept.PatternMemberID, skosConcept);
            else
                return narrowerTransitiveConcepts;
            #endregion

            //skos:narrowerTransitive
            narrowerTransitiveConcepts.AddRange(
                skosNarrowerTransitiveAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                          .Select(skosNarrowerTransitiveAsn => skosNarrowerTransitiveAsn.TargetIndividualExpression.GetIRI()));

            foreach (RDFResource narrowerTransitiveConcept in narrowerTransitiveConcepts.ToList())
                narrowerTransitiveConcepts.AddRange(ontology.SubsumeNarrowerTransitivity(narrowerTransitiveConcept, skosNarrowerTransitiveAsns, visitContext));

            return narrowerTransitiveConcepts;
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:related" mapping relation
        /// </summary>
        public static bool CheckHasRelatedConcept(this OWLOntology ontology, RDFResource leftConcept, RDFResource rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetRelatedConcepts(leftConcept).Any(concept => concept.Equals(rightConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:related" relation in the working ontology
        /// </summary>
        public static List<RDFResource> GetRelatedConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> relatedConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosRelatedAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>());

                //skos:related
                relatedConcepts.AddRange(
                    skosRelatedAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                   .Select(skosRelatedAsn => skosRelatedAsn.TargetIndividualExpression.GetIRI()));
                relatedConcepts.AddRange(
                    skosRelatedAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConcept))
                                   .Select(skosRelatedAsn => skosRelatedAsn.SourceIndividualExpression.GetIRI()));
            }

            relatedConcepts.RemoveAll(c => c.Equals(skosConcept));
            return RDFQueryUtilities.RemoveDuplicates(relatedConcepts);
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:broadMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasBroadMatchConcept(this OWLOntology ontology, RDFResource leftConcept, RDFResource rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetBroadMatchConcepts(leftConcept).Any(concept => concept.Equals(rightConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:broadMatch" relation in the working ontology
        /// </summary>
        public static List<RDFResource> GetBroadMatchConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> broadMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosBroadMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>());
                List<OWLObjectPropertyAssertion> skosNarrowMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>());

                //skos:broadMatch
                broadMatchConcepts.AddRange(
                    skosBroadMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                      .Select(skosBroadMatchAsn => skosBroadMatchAsn.TargetIndividualExpression.GetIRI()));

                //skos:narrowMatch
                broadMatchConcepts.AddRange(
                    skosNarrowMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConcept))
                                       .Select(skosNarrowMatchAsn => skosNarrowMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            broadMatchConcepts.RemoveAll(c => c.Equals(skosConcept));
            return RDFQueryUtilities.RemoveDuplicates(broadMatchConcepts);
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:narrowMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasNarrowMatchConcept(this OWLOntology ontology, RDFResource leftConcept, RDFResource rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetNarrowMatchConcepts(leftConcept).Any(concept => concept.Equals(rightConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:narrowMatch" relation in the working ontology
        /// </summary>
        public static List<RDFResource> GetNarrowMatchConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> narrowMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosNarrowMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>());
                List<OWLObjectPropertyAssertion> skosBroadMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>());

                //skos:narrowMatch
                narrowMatchConcepts.AddRange(
                    skosNarrowMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                       .Select(skosNarrowMatchAsn => skosNarrowMatchAsn.TargetIndividualExpression.GetIRI()));

                //skos:broadMatch
                narrowMatchConcepts.AddRange(
                    skosBroadMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConcept))
                                      .Select(skosBroadMatchAsn => skosBroadMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            narrowMatchConcepts.RemoveAll(c => c.Equals(skosConcept));
            return RDFQueryUtilities.RemoveDuplicates(narrowMatchConcepts);
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:closeMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasCloseMatchConcept(this OWLOntology ontology, RDFResource leftConcept, RDFResource rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetCloseMatchConcepts(leftConcept).Any(concept => concept.Equals(rightConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:closeMatch" relation in the working ontology
        /// </summary>
        public static List<RDFResource> GetCloseMatchConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> closeMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosCloseMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.CLOSE_MATCH.ToEntity<OWLObjectProperty>());

                //skos:closeMatch
                closeMatchConcepts.AddRange(
                    skosCloseMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                      .Select(skosCloseMatchAsn => skosCloseMatchAsn.TargetIndividualExpression.GetIRI()));
                closeMatchConcepts.AddRange(
                    skosCloseMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConcept))
                                      .Select(skosCloseMatchAsn => skosCloseMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            closeMatchConcepts.RemoveAll(c => c.Equals(skosConcept));
            return RDFQueryUtilities.RemoveDuplicates(closeMatchConcepts);
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:exactMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasExactMatchConcept(this OWLOntology ontology, RDFResource leftConcept, RDFResource rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetExactMatchConcepts(leftConcept).Any(concept => concept.Equals(rightConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:exactMatch" relation in the working ontology
        /// </summary>
        public static List<RDFResource> GetExactMatchConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> exactMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosExactMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.EXACT_MATCH.ToEntity<OWLObjectProperty>());

                //skos:exactMatch
                exactMatchConcepts.AddRange(ontology.SubsumeExactMatchTransitivity(skosConcept, skosExactMatchAsns, new Dictionary<long, RDFResource>()));
            }

            exactMatchConcepts.RemoveAll(c => c.Equals(skosConcept));
            return RDFQueryUtilities.RemoveDuplicates(exactMatchConcepts);
        }
        internal static List<RDFResource> SubsumeExactMatchTransitivity(this OWLOntology ontology, RDFResource skosConcept, List<OWLObjectPropertyAssertion> skosExactMatchAsns, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> exactMatchConcepts = new List<RDFResource>();

            #region visitContext
            if (!visitContext.ContainsKey(skosConcept.PatternMemberID))
                visitContext.Add(skosConcept.PatternMemberID, skosConcept);
            else
                return exactMatchConcepts;
            #endregion

            //skos:exactMatch
            exactMatchConcepts.AddRange(
                skosExactMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                  .Select(skosExactMatchAsn => skosExactMatchAsn.TargetIndividualExpression.GetIRI()));
            exactMatchConcepts.AddRange(
                skosExactMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConcept))
                                  .Select(skosExactMatchAsn => skosExactMatchAsn.SourceIndividualExpression.GetIRI()));

            foreach (RDFResource exactMatchConcept in exactMatchConcepts.ToList())
                exactMatchConcepts.AddRange(ontology.SubsumeExactMatchTransitivity(exactMatchConcept, skosExactMatchAsns, visitContext));

            return exactMatchConcepts;
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:relatedMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasRelatedMatchConcept(this OWLOntology ontology, RDFResource leftConcept, RDFResource rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetRelatedMatchConcepts(leftConcept).Any(concept => concept.Equals(rightConcept)) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:relatedMatch" relation in the working ontology
        /// </summary>
        public static List<RDFResource> GetRelatedMatchConcepts(this OWLOntology ontology, RDFResource skosConcept)
        {
            List<RDFResource> relatedMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosRelatedMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH));

                //skos:relatedMatch
                relatedMatchConcepts.AddRange(
                    skosRelatedMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConcept))
                                        .Select(skosRelatedMatchAsn => skosRelatedMatchAsn.TargetIndividualExpression.GetIRI()));
                relatedMatchConcepts.AddRange(
                    skosRelatedMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConcept))
                                        .Select(skosRelatedMatchAsn => skosRelatedMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            relatedMatchConcepts.RemoveAll(c => c.Equals(skosConcept));
            return RDFQueryUtilities.RemoveDuplicates(relatedMatchConcepts);
        }
        #endregion
    }
}