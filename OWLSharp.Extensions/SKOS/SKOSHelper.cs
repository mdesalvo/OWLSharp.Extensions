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
using OWLSharp.Reasoner;
using OWLSharp.Validator;
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
        public static OWLOntology DeclareConceptScheme(this OWLOntology ontology, OWLNamedIndividual conceptScheme, OWLNamedIndividual[] concepts=null)
        {
            #region Guards
            if (conceptScheme == null)
                throw new OWLException($"Cannot declare concept scheme because given '{nameof(conceptScheme)}' parameter is null");
            #endregion

            ontology.DeclareEntity(conceptScheme);
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
                conceptScheme));

            if (concepts?.Length > 0)
                foreach (OWLNamedIndividual concept in concepts)
                {
                    ontology.DeclareEntity(concept);
                    ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                        RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                        concept));
                    ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                        new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                        concept,
                        conceptScheme));
                }

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a SKOS concept having the given name.<br/>
        /// It can also be given a set of preferred language labels and the concept scheme to which this concept belongs.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareConcept(this OWLOntology ontology, OWLNamedIndividual concept, OWLLiteral[] labels=null, OWLNamedIndividual conceptScheme=null)
        {
            #region Guards
            if (concept == null)
                throw new OWLException($"Cannot declare concept because given '{nameof(concept)}' parameter is null");
            #endregion

            ontology.DeclareEntity(concept);
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                concept));

            if (labels?.Length > 0)
            {
                HashSet<string> langtagLookup = new HashSet<string>();
                foreach (OWLLiteral preferredLabel in labels)
                {
                    //(S14) skos:prefLabel annotation requires uniqueness of language tags foreach rdfs:Resource
                    if (!langtagLookup.Add(preferredLabel.Language))
                        throw new OWLException($"Cannot setup preferred label of concept '{concept.GetIRI()}' because having more than one occurrence of the same language tag is not allowed!");

                    ontology.DeclareAnnotationAxiom(new OWLAnnotationAssertion(
                        new OWLAnnotationProperty(RDFVocabulary.SKOS.PREF_LABEL),
                        concept.GetIRI(),
                        preferredLabel));
                }
            }

            if (conceptScheme != null)
            {
                ontology.DeclareConceptScheme(conceptScheme);
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    concept,
                    conceptScheme));
            }

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a SKOS collection having the given name and the given set of concepts.<br/>
        /// It can also be given a set of preferred language labels, the concept scheme to which this collection belongs, and whether it is ordered.<br/>
        /// (When "ordered" is true, only the skos:OrderedCollection typing is captured: the skos:memberList sequence is not representable at the OWL axiomatic level).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareCollection(this OWLOntology ontology, OWLNamedIndividual collection, OWLNamedIndividual[] concepts, OWLLiteral[] labels=null, OWLNamedIndividual conceptScheme=null, bool ordered=false)
        {
            #region Guards
            if (collection == null)
                throw new OWLException($"Cannot declare collection because given '{nameof(collection)}' parameter is null");
            if (concepts == null)
                throw new OWLException($"Cannot declare collection because given '{nameof(concepts)}' parameter is null");
            if (concepts.Length == 0)
                throw new OWLException($"Cannot declare collection because given '{nameof(concepts)}' parameter must contain at least 1 element");
            #endregion

            ontology.DeclareEntity(collection);
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                (ordered ? RDFVocabulary.SKOS.ORDERED_COLLECTION : RDFVocabulary.SKOS.COLLECTION).ToEntity<OWLClass>(),
                collection));

            foreach (OWLNamedIndividual concept in concepts)
            {
                ontology.DeclareConcept(concept);
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.MEMBER),
                    collection,
                    concept));
            }

            if (labels?.Length > 0)
            {
                HashSet<string> langtagLookup = new HashSet<string>();
                foreach (OWLLiteral preferredLabel in labels)
                {
                    //(S14) skos:prefLabel annotation requires uniqueness of language tags foreach rdfs:Resource
                    if (!langtagLookup.Add(preferredLabel.Language))
                        throw new OWLException($"Cannot setup preferred label of collection '{collection.GetIRI()}' because having more than one occurrence of the same language tag is not allowed!");

                    ontology.DeclareAnnotationAxiom(new OWLAnnotationAssertion(
                        new OWLAnnotationProperty(RDFVocabulary.SKOS.PREF_LABEL),
                        collection.GetIRI(),
                        preferredLabel));
                }
            }

            if (conceptScheme != null)
            {
                ontology.DeclareConceptScheme(conceptScheme);
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    collection,
                    conceptScheme));
            }

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring a "skos:broader"/"skos:broaderTransitive" hierarchy relation (childConcept -> parentConcept).<br/>
        /// The inverse "skos:narrower"/"skos:narrowerTransitive" relation is automatically asserted as an inference.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareBroaderConcept(this OWLOntology ontology, OWLNamedIndividual childConcept, OWLNamedIndividual parentConcept, bool transitive=false)
        {
            #region Guards
            if (childConcept == null)
                throw new OWLException($"Cannot declare broader concept relation because given '{nameof(childConcept)}' parameter is null");
            if (parentConcept == null)
                throw new OWLException($"Cannot declare broader concept relation because given '{nameof(parentConcept)}' parameter is null");
            if (childConcept.GetIRI().Equals(parentConcept.GetIRI()))
                throw new OWLException($"Cannot declare broader concept relation because given '{nameof(childConcept)}' and '{nameof(parentConcept)}' parameters are the same concept");
            #endregion

            ontology.DeclareConcept(childConcept);
            ontology.DeclareConcept(parentConcept);

            OWLObjectProperty broaderProp = new OWLObjectProperty(transitive ? RDFVocabulary.SKOS.BROADER_TRANSITIVE : RDFVocabulary.SKOS.BROADER);
            OWLObjectProperty narrowerProp = new OWLObjectProperty(transitive ? RDFVocabulary.SKOS.NARROWER_TRANSITIVE : RDFVocabulary.SKOS.NARROWER);

            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(broaderProp, childConcept, parentConcept));
            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(narrowerProp, parentConcept, childConcept) { IsInference = true });

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring a "skos:narrower"/"skos:narrowerTransitive" hierarchy relation (parentConcept -> childConcept).<br/>
        /// The inverse "skos:broader"/"skos:broaderTransitive" relation is automatically asserted as an inference.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareNarrowerConcept(this OWLOntology ontology, OWLNamedIndividual parentConcept, OWLNamedIndividual childConcept, bool transitive=false)
        {
            #region Guards
            if (parentConcept == null)
                throw new OWLException($"Cannot declare narrower concept relation because given '{nameof(parentConcept)}' parameter is null");
            if (childConcept == null)
                throw new OWLException($"Cannot declare narrower concept relation because given '{nameof(childConcept)}' parameter is null");
            if (parentConcept.GetIRI().Equals(childConcept.GetIRI()))
                throw new OWLException($"Cannot declare narrower concept relation because given '{nameof(parentConcept)}' and '{nameof(childConcept)}' parameters are the same concept");
            #endregion

            ontology.DeclareConcept(parentConcept);
            ontology.DeclareConcept(childConcept);

            OWLObjectProperty narrowerProp = new OWLObjectProperty(transitive ? RDFVocabulary.SKOS.NARROWER_TRANSITIVE : RDFVocabulary.SKOS.NARROWER);
            OWLObjectProperty broaderProp = new OWLObjectProperty(transitive ? RDFVocabulary.SKOS.BROADER_TRANSITIVE : RDFVocabulary.SKOS.BROADER);

            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(narrowerProp, parentConcept, childConcept));
            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(broaderProp, childConcept, parentConcept) { IsInference = true });

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring a "skos:related" associative relation between the two given concepts.<br/>
        /// Being skos:related symmetric, the inverse direction is automatically asserted as an inference.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareRelatedConcept(this OWLOntology ontology, OWLNamedIndividual conceptA, OWLNamedIndividual conceptB)
        {
            #region Guards
            if (conceptA == null)
                throw new OWLException($"Cannot declare related concept relation because given '{nameof(conceptA)}' parameter is null");
            if (conceptB == null)
                throw new OWLException($"Cannot declare related concept relation because given '{nameof(conceptB)}' parameter is null");
            if (conceptA.GetIRI().Equals(conceptB.GetIRI()))
                throw new OWLException($"Cannot declare related concept relation because given '{nameof(conceptA)}' and '{nameof(conceptB)}' parameters are the same concept");
            #endregion

            ontology.DeclareConcept(conceptA);
            ontology.DeclareConcept(conceptB);

            OWLObjectProperty relatedProp = RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>();

            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(relatedProp, conceptA, conceptB));
            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(relatedProp, conceptB, conceptA) { IsInference = true });

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the given mapping relation (skos:exactMatch, skos:closeMatch, skos:broadMatch,
        /// skos:narrowMatch or skos:relatedMatch) between the two given concepts, typically belonging to different schemes.<br/>
        /// Symmetric relations (exactMatch, closeMatch, relatedMatch) and inverse relations (broadMatch/narrowMatch) automatically
        /// assert their counterpart direction as an inference.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareMappingConcept(this OWLOntology ontology, OWLNamedIndividual leftConcept, OWLNamedIndividual rightConcept, SKOSEnums.SKOSMappingRelation relation)
        {
            #region Guards
            if (leftConcept == null)
                throw new OWLException($"Cannot declare mapping concept relation because given '{nameof(leftConcept)}' parameter is null");
            if (rightConcept == null)
                throw new OWLException($"Cannot declare mapping concept relation because given '{nameof(rightConcept)}' parameter is null");
            if (leftConcept.GetIRI().Equals(rightConcept.GetIRI()))
                throw new OWLException($"Cannot declare mapping concept relation because given '{nameof(leftConcept)}' and '{nameof(rightConcept)}' parameters are the same concept");
            #endregion

            ontology.DeclareConcept(leftConcept);
            ontology.DeclareConcept(rightConcept);

            switch (relation)
            {
                case SKOSEnums.SKOSMappingRelation.ExactMatch:
                    DeclareSymmetricMapping(RDFVocabulary.SKOS.EXACT_MATCH);
                    break;
                case SKOSEnums.SKOSMappingRelation.CloseMatch:
                    DeclareSymmetricMapping(RDFVocabulary.SKOS.CLOSE_MATCH);
                    break;
                case SKOSEnums.SKOSMappingRelation.RelatedMatch:
                    DeclareSymmetricMapping(RDFVocabulary.SKOS.RELATED_MATCH);
                    break;
                case SKOSEnums.SKOSMappingRelation.BroadMatch:
                    ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                        RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>(), leftConcept, rightConcept));
                    ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                        RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>(), rightConcept, leftConcept) { IsInference = true });
                    break;
                case SKOSEnums.SKOSMappingRelation.NarrowMatch:
                    ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                        RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>(), leftConcept, rightConcept));
                    ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                        RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>(), rightConcept, leftConcept) { IsInference = true });
                    break;
            }

            return ontology;

            void DeclareSymmetricMapping(RDFResource mappingProperty)
            {
                OWLObjectProperty mappingProp = mappingProperty.ToEntity<OWLObjectProperty>();
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(mappingProp, leftConcept, rightConcept));
                ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(mappingProp, rightConcept, leftConcept) { IsInference = true });
            }
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring a "skos:notation" value for the given concept
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareNotation(this OWLOntology ontology, OWLNamedIndividual concept, OWLLiteral notation)
        {
            #region Guards
            if (concept == null)
                throw new OWLException($"Cannot declare notation because given '{nameof(concept)}' parameter is null");
            if (notation == null)
                throw new OWLException($"Cannot declare notation because given '{nameof(notation)}' parameter is null");
            #endregion

            ontology.DeclareConcept(concept);
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.SKOS.NOTATION.ToEntity<OWLDataProperty>(),
                concept,
                notation));

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a SKOS-XL label having the given literal form
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareLabel(this OWLOntology ontology, OWLNamedIndividual label, OWLLiteral literalForm)
        {
            #region Guards
            if (label == null)
                throw new OWLException($"Cannot declare label because given '{nameof(label)}' parameter is null");
            if (literalForm == null)
                throw new OWLException($"Cannot declare label because given '{nameof(literalForm)}' parameter is null");
            #endregion

            ontology.DeclareEntity(label);
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.SKOS.SKOSXL.LABEL.ToEntity<OWLClass>(),
                label));
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM.ToEntity<OWLDataProperty>(),
                label,
                literalForm));

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring a "skosxl:prefLabel" relation between the given concept and the given label
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclarePreferredLabel(this OWLOntology ontology, OWLNamedIndividual concept, OWLNamedIndividual label)
            => DeclareXLLabelRelation(ontology, concept, label, RDFVocabulary.SKOS.SKOSXL.PREF_LABEL);

        /// <summary>
        /// Injects the A-BOX axioms for declaring a "skosxl:altLabel" relation between the given concept and the given label
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareAlternativeLabel(this OWLOntology ontology, OWLNamedIndividual concept, OWLNamedIndividual label)
            => DeclareXLLabelRelation(ontology, concept, label, RDFVocabulary.SKOS.SKOSXL.ALT_LABEL);

        /// <summary>
        /// Injects the A-BOX axioms for declaring a "skosxl:hiddenLabel" relation between the given concept and the given label
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareHiddenLabel(this OWLOntology ontology, OWLNamedIndividual concept, OWLNamedIndividual label)
            => DeclareXLLabelRelation(ontology, concept, label, RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL);

        internal static OWLOntology DeclareXLLabelRelation(OWLOntology ontology, OWLNamedIndividual concept, OWLNamedIndividual label, RDFResource labelRelation)
        {
            #region Guards
            if (concept == null)
                throw new OWLException($"Cannot declare SKOS-XL label relation because given '{nameof(concept)}' parameter is null");
            if (label == null)
                throw new OWLException($"Cannot declare SKOS-XL label relation because given '{nameof(label)}' parameter is null");
            #endregion

            ontology.DeclareConcept(concept);
            ontology.DeclareEntity(label);
            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                labelRelation.ToEntity<OWLObjectProperty>(),
                concept,
                label));

            return ontology;
        }
        #endregion

        #region Analyzer
        /// <summary>
        /// Checks if the given SKOS concept scheme is found in the working ontology
        /// </summary>
        public static bool CheckHasConceptScheme(this OWLOntology ontology, OWLNamedIndividual conceptScheme)
            => conceptScheme != null && ontology?.GetIndividualsOf(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>()).Any(cs => cs.GetIRI().Equals(conceptScheme.GetIRI())) == true;

        /// <summary>
        /// Checks if the given SKOS concept is found in the working ontology under the given scheme
        /// </summary>
        public static bool CheckHasConcept(this OWLOntology ontology, OWLNamedIndividual conceptScheme, OWLNamedIndividual concept)
            => conceptScheme != null && concept != null && ontology?.GetConceptsInScheme(conceptScheme).Any(c => c.GetIRI().Equals(concept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found in the working ontology under the given scheme
        /// </summary>
        public static List<OWLNamedIndividual> GetConceptsInScheme(this OWLOntology ontology, OWLNamedIndividual skosConceptScheme)
        {
            List<RDFResource> conceptsInScheme = new List<RDFResource>();

            if (skosConceptScheme != null && ontology != null)
            {
                RDFResource skosConceptSchemeIRI = skosConceptScheme.GetIRI();
                OWLClass skosConcept = RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosInSchemeAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME));
                List<OWLObjectPropertyAssertion> skosHasTopConceptAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.HAS_TOP_CONCEPT));
                List<OWLObjectPropertyAssertion> skosTopConceptOfAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.TOP_CONCEPT_OF));

                //skos:inScheme
                conceptsInScheme.AddRange(
                    skosInSchemeAsns.Where(asn => ontology.CheckIsIndividualOf(skosConcept, asn.SourceIndividualExpression)
                                                    && asn.TargetIndividualExpression.GetIRI().Equals(skosConceptSchemeIRI))
                                    .Select(skosInSchemeAsn => skosInSchemeAsn.SourceIndividualExpression.GetIRI()));

                //skos:hasTopConcept
                conceptsInScheme.AddRange(
                    skosHasTopConceptAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptSchemeIRI))
                                         .Select(skosHasTopConceptAsn => skosHasTopConceptAsn.TargetIndividualExpression.GetIRI()));

                //skos:topConceptOf
                conceptsInScheme.AddRange(
                    skosTopConceptOfAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConceptSchemeIRI))
                                        .Select(skosTopConceptOfAsn => skosTopConceptOfAsn.SourceIndividualExpression.GetIRI()));
            }

            return RDFQueryUtilities.RemoveDuplicates(conceptsInScheme).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Checks if the given SKOS collection is found in the working ontology under the given scheme
        /// </summary>
        public static bool CheckHasCollection(this OWLOntology ontology, OWLNamedIndividual conceptScheme, OWLNamedIndividual collection)
            => conceptScheme != null && collection != null && ontology?.GetCollectionsInScheme(conceptScheme).Any(cl => cl.GetIRI().Equals(collection.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS collections found in the working ontology under the given scheme
        /// </summary>
        public static List<OWLNamedIndividual> GetCollectionsInScheme(this OWLOntology ontology, OWLNamedIndividual skosConceptScheme)
        {
            List<RDFResource> collectionsInScheme = new List<RDFResource>();

            if (skosConceptScheme != null && ontology != null)
            {
                RDFResource skosConceptSchemeIRI = skosConceptScheme.GetIRI();
                OWLClass skosCollection = RDFVocabulary.SKOS.COLLECTION.ToEntity<OWLClass>();
                OWLClass skosOrderedCollection = RDFVocabulary.SKOS.ORDERED_COLLECTION.ToEntity<OWLClass>();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosInSchemeAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME));

                //skos:inScheme
                collectionsInScheme.AddRange(
                    skosInSchemeAsns.Where(asn => (ontology.CheckIsIndividualOf(skosCollection, asn.SourceIndividualExpression)
                                                   || ontology.CheckIsIndividualOf(skosOrderedCollection, asn.SourceIndividualExpression))
                                                  && asn.TargetIndividualExpression.GetIRI().Equals(skosConceptSchemeIRI))
                                    .Select(skosInSchemeAsn => skosInSchemeAsn.SourceIndividualExpression.GetIRI()));
            }

            return RDFQueryUtilities.RemoveDuplicates(collectionsInScheme).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:broader" or a "skos:broaderTransitive" hierarchy relation (child->parent)
        /// </summary>
        public static bool CheckHasBroaderConcept(this OWLOntology ontology, OWLNamedIndividual childConcept, OWLNamedIndividual parentConcept)
            => childConcept != null && parentConcept != null && ontology?.GetBroaderConcepts(childConcept).Any(concept => concept.GetIRI().Equals(parentConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be hierarchically broader than the given one in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetBroaderConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> broaderConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosBroaderAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.BROADER));
                List<OWLObjectPropertyAssertion> skosBroaderTransitiveAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE));

                //skos:broader
                broaderConcepts.AddRange(
                    skosBroaderAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                   .Select(skosBroaderAsn => skosBroaderAsn.TargetIndividualExpression.GetIRI()));

                //skos:broaderTransitive
                broaderConcepts.AddRange(ontology.SubsumeBroaderTransitivity(skosConceptIRI, skosBroaderTransitiveAsns, new Dictionary<long, RDFResource>()));
            }

            broaderConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return broaderConcepts.Select(r => new OWLNamedIndividual(r)).ToList();
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
        public static bool CheckHasNarrowerConcept(this OWLOntology ontology, OWLNamedIndividual parentConcept, OWLNamedIndividual childConcept)
            => parentConcept != null && childConcept != null && ontology?.GetNarrowerConcepts(parentConcept).Any(concept => concept.GetIRI().Equals(childConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be hierarchically narrower than the given one in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetNarrowerConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> narrowerConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosNarrowerAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER));
                List<OWLObjectPropertyAssertion> skosNarrowerTransitiveAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER_TRANSITIVE));

                //skos:narrower
                narrowerConcepts.AddRange(
                    skosNarrowerAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                    .Select(skosNarrowerAsn => skosNarrowerAsn.TargetIndividualExpression.GetIRI()));

                //skos:narrowerTransitive
                narrowerConcepts.AddRange(ontology.SubsumeNarrowerTransitivity(skosConceptIRI, skosNarrowerTransitiveAsns, new Dictionary<long, RDFResource>()));
            }

            narrowerConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return narrowerConcepts.Select(r => new OWLNamedIndividual(r)).ToList();
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
        public static bool CheckHasRelatedConcept(this OWLOntology ontology, OWLNamedIndividual leftConcept, OWLNamedIndividual rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetRelatedConcepts(leftConcept).Any(concept => concept.GetIRI().Equals(rightConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:related" relation in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetRelatedConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> relatedConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosRelatedAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>());

                //skos:related
                relatedConcepts.AddRange(
                    skosRelatedAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                   .Select(skosRelatedAsn => skosRelatedAsn.TargetIndividualExpression.GetIRI()));
                relatedConcepts.AddRange(
                    skosRelatedAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                   .Select(skosRelatedAsn => skosRelatedAsn.SourceIndividualExpression.GetIRI()));
            }

            relatedConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return RDFQueryUtilities.RemoveDuplicates(relatedConcepts).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:broadMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasBroadMatchConcept(this OWLOntology ontology, OWLNamedIndividual leftConcept, OWLNamedIndividual rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetBroadMatchConcepts(leftConcept).Any(concept => concept.GetIRI().Equals(rightConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:broadMatch" relation in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetBroadMatchConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> broadMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosBroadMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>());
                List<OWLObjectPropertyAssertion> skosNarrowMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>());

                //skos:broadMatch
                broadMatchConcepts.AddRange(
                    skosBroadMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                      .Select(skosBroadMatchAsn => skosBroadMatchAsn.TargetIndividualExpression.GetIRI()));

                //skos:narrowMatch
                broadMatchConcepts.AddRange(
                    skosNarrowMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                       .Select(skosNarrowMatchAsn => skosNarrowMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            broadMatchConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return RDFQueryUtilities.RemoveDuplicates(broadMatchConcepts).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:narrowMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasNarrowMatchConcept(this OWLOntology ontology, OWLNamedIndividual leftConcept, OWLNamedIndividual rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetNarrowMatchConcepts(leftConcept).Any(concept => concept.GetIRI().Equals(rightConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:narrowMatch" relation in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetNarrowMatchConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> narrowMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosNarrowMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>());
                List<OWLObjectPropertyAssertion> skosBroadMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>());

                //skos:narrowMatch
                narrowMatchConcepts.AddRange(
                    skosNarrowMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                       .Select(skosNarrowMatchAsn => skosNarrowMatchAsn.TargetIndividualExpression.GetIRI()));

                //skos:broadMatch
                narrowMatchConcepts.AddRange(
                    skosBroadMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                      .Select(skosBroadMatchAsn => skosBroadMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            narrowMatchConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return RDFQueryUtilities.RemoveDuplicates(narrowMatchConcepts).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:closeMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasCloseMatchConcept(this OWLOntology ontology, OWLNamedIndividual leftConcept, OWLNamedIndividual rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetCloseMatchConcepts(leftConcept).Any(concept => concept.GetIRI().Equals(rightConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:closeMatch" relation in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetCloseMatchConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> closeMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosCloseMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.CLOSE_MATCH.ToEntity<OWLObjectProperty>());

                //skos:closeMatch
                closeMatchConcepts.AddRange(
                    skosCloseMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                      .Select(skosCloseMatchAsn => skosCloseMatchAsn.TargetIndividualExpression.GetIRI()));
                closeMatchConcepts.AddRange(
                    skosCloseMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                      .Select(skosCloseMatchAsn => skosCloseMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            closeMatchConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return RDFQueryUtilities.RemoveDuplicates(closeMatchConcepts).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Checks if the given SKOS concepts are linked with a "skos:exactMatch" mapping relation (left->right)
        /// </summary>
        public static bool CheckHasExactMatchConcept(this OWLOntology ontology, OWLNamedIndividual leftConcept, OWLNamedIndividual rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetExactMatchConcepts(leftConcept).Any(concept => concept.GetIRI().Equals(rightConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:exactMatch" relation in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetExactMatchConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> exactMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosExactMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.SKOS.EXACT_MATCH.ToEntity<OWLObjectProperty>());

                //skos:exactMatch
                exactMatchConcepts.AddRange(ontology.SubsumeExactMatchTransitivity(skosConceptIRI, skosExactMatchAsns, new Dictionary<long, RDFResource>()));
            }

            exactMatchConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return RDFQueryUtilities.RemoveDuplicates(exactMatchConcepts).Select(r => new OWLNamedIndividual(r)).ToList();
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
        public static bool CheckHasRelatedMatchConcept(this OWLOntology ontology, OWLNamedIndividual leftConcept, OWLNamedIndividual rightConcept)
            => leftConcept != null && rightConcept != null && ontology?.GetRelatedMatchConcepts(leftConcept).Any(concept => concept.GetIRI().Equals(rightConcept.GetIRI())) == true;

        /// <summary>
        /// Enlists the SKOS concepts found to be mapped to the given one with a "skos:relatedMatch" relation in the working ontology
        /// </summary>
        public static List<OWLNamedIndividual> GetRelatedMatchConcepts(this OWLOntology ontology, OWLNamedIndividual skosConcept)
        {
            List<RDFResource> relatedMatchConcepts = new List<RDFResource>();

            if (skosConcept != null && ontology != null)
            {
                RDFResource skosConceptIRI = skosConcept.GetIRI();
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
                List<OWLObjectPropertyAssertion> skosRelatedMatchAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, new OWLObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH));

                //skos:relatedMatch
                relatedMatchConcepts.AddRange(
                    skosRelatedMatchAsns.Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                        .Select(skosRelatedMatchAsn => skosRelatedMatchAsn.TargetIndividualExpression.GetIRI()));
                relatedMatchConcepts.AddRange(
                    skosRelatedMatchAsns.Where(asn => asn.TargetIndividualExpression.GetIRI().Equals(skosConceptIRI))
                                        .Select(skosRelatedMatchAsn => skosRelatedMatchAsn.SourceIndividualExpression.GetIRI()));
            }

            relatedMatchConcepts.RemoveAll(c => c.Equals(skosConcept.GetIRI()));
            return RDFQueryUtilities.RemoveDuplicates(relatedMatchConcepts).Select(r => new OWLNamedIndividual(r)).ToList();
        }
        #endregion

        #region Validator
        /// <summary>
        /// Checks for SKOS concepts clashing on two given object properties and collects any violations as issues
        /// </summary>
        internal static async Task CheckConceptRelationClashAsync(
            OWLOntology ontology,
            string ruleName,
            RDFResource propertyA,
            RDFResource propertyB,
            string description,
            string suggestionSuffix,
            List<OWLIssue> issues,
            bool invertSecondProperty = false)
        {
            SWRLRule clashRule = new SWRLRule(
                new RDFPlainLiteral(ruleName),
                new RDFPlainLiteral($"This rule checks for skos:Concept instances clashing on their relations ({suggestionSuffix})"),
                new SWRLAntecedent
                {
                    Atoms = new List<SWRLAtom>
                    {
                        new SWRLClassAtom(
                            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                            new SWRLVariableArgument(new RDFVariable("?C1"))),
                        new SWRLClassAtom(
                            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                            new SWRLVariableArgument(new RDFVariable("?C2"))),
                        new SWRLObjectPropertyAtom(
                            new OWLObjectProperty(propertyA),
                            new SWRLVariableArgument(new RDFVariable("?C1")),
                            new SWRLVariableArgument(new RDFVariable("?C2"))),
                        new SWRLObjectPropertyAtom(
                            new OWLObjectProperty(propertyB),
                            new SWRLVariableArgument(new RDFVariable(invertSecondProperty ? "?C2" : "?C1")),
                            new SWRLVariableArgument(new RDFVariable(invertSecondProperty ? "?C1" : "?C2")))
                    },
                    BuiltIns = new List<SWRLBuiltIn>
                    {
                        SWRLBuiltIn.NotEqual(
                            new SWRLVariableArgument(new RDFVariable("?C1")),
                            new SWRLVariableArgument(new RDFVariable("?C2")))
                    }
                },
                new SWRLConsequent
                {
                    Atoms = new List<SWRLAtom>
                    {
                        new SWRLObjectPropertyAtom(
                            new OWLObjectProperty(SKOSValidator.ViolationIRI),
                            new SWRLVariableArgument(new RDFVariable("?C1")),
                            new SWRLVariableArgument(new RDFVariable("?C2")))
                    }
                });

            List<OWLInference> violations = await clashRule.ApplyToOntologyAsync(ontology);
            violations.ForEach(violation => issues.Add(
                new OWLIssue(
                    OWLEnums.OWLIssueSeverity.Error,
                    ruleName,
                    description,
                    $"SKOS concepts '{((OWLObjectPropertyAssertion)violation.Axiom).SourceIndividualExpression.GetIRI()}' and '{((OWLObjectPropertyAssertion)violation.Axiom).TargetIndividualExpression.GetIRI()}' should be adjusted to not clash on {suggestionSuffix}"
                )));
        }
        #endregion
    }
}
