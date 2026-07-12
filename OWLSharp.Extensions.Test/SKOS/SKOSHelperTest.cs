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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OWLSharp.Ontology;
using OWLSharp.Extensions.SKOS;
using RDFSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OWLSharp.Extensions.Test.SKOS;

[TestClass]
public class SKOSHelperTest
{
    #region Tests
    [TestMethod]
    public void ShouldDeclareConceptScheme()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareConceptScheme(new OWLNamedIndividual(new RDFResource("ex:ConceptScheme")), [new OWLNamedIndividual(new RDFResource("ex:ConceptA"))]);

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareConceptScheme(null, [new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))]));
    }

    [TestMethod]
    public void ShouldDeclareConcept()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareConcept(new OWLNamedIndividual(new RDFResource("ex:Concept")), [
            new OWLLiteral(new RDFPlainLiteral("This is a concept")),
            new OWLLiteral(new RDFPlainLiteral("This is a concept", "en-US"))]);

        Assert.HasCount(1, ontology.DeclarationAxioms);
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(2, ontology.GetAnnotationAxiomsOfType<OWLAnnotationAssertion>());

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareConcept(null));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareConcept(new OWLNamedIndividual(new RDFResource("ex:Concept")), [
            new OWLLiteral(new RDFPlainLiteral("This is a concept")), new OWLLiteral(new RDFPlainLiteral("This is the same concept"))]));
    }

    [TestMethod]
    public void ShouldDeclareConceptInScheme()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareConcept(new OWLNamedIndividual(new RDFResource("ex:Concept")), [
            new OWLLiteral(new RDFPlainLiteral("This is a concept")),
            new OWLLiteral(new RDFPlainLiteral("This is a concept", "en-US"))], new OWLNamedIndividual(new RDFResource("ex:ConceptScheme")));

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(2, ontology.GetAnnotationAxiomsOfType<OWLAnnotationAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
    }

    [TestMethod]
    public void ShouldDeclareCollection()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareCollection(new OWLNamedIndividual(new RDFResource("ex:Collection")),
            [new OWLNamedIndividual(new RDFResource("ex:ConceptA")), new OWLNamedIndividual(new RDFResource("ex:ConceptB"))],
            [new OWLLiteral(new RDFPlainLiteral("This is a collection")), new OWLLiteral(new RDFPlainLiteral("This is a collection", "en-US"))]);

        Assert.HasCount(3, ontology.DeclarationAxioms);
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(2, ontology.GetAnnotationAxiomsOfType<OWLAnnotationAssertion>());

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareCollection(null, [new OWLNamedIndividual(new RDFResource("ex:ConceptA"))]));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareCollection(new OWLNamedIndividual(new RDFResource("ex:Collection")), null));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareCollection(new OWLNamedIndividual(new RDFResource("ex:Collection")), []));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareCollection(new OWLNamedIndividual(new RDFResource("ex:Collection")),
            [new OWLNamedIndividual(new RDFResource("ex:ConceptA")), new OWLNamedIndividual(new RDFResource("ex:ConceptB"))],
            [new OWLLiteral(new RDFPlainLiteral("This is a collection", "en-US")), new OWLLiteral(new RDFPlainLiteral("This is the same collection", "en-US"))]));
    }

    [TestMethod]
    public void ShouldDeclareCollectionInScheme()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareCollection(new OWLNamedIndividual(new RDFResource("ex:Collection")),
            [new OWLNamedIndividual(new RDFResource("ex:ConceptA")), new OWLNamedIndividual(new RDFResource("ex:ConceptB"))],
            [new OWLLiteral(new RDFPlainLiteral("This is a collection")), new OWLLiteral(new RDFPlainLiteral("This is a collection", "en-US"))],
            new OWLNamedIndividual(new RDFResource("ex:ConceptScheme")));

        Assert.HasCount(4, ontology.DeclarationAxioms);
        Assert.HasCount(4, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(2, ontology.GetAnnotationAxiomsOfType<OWLAnnotationAssertion>());
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
    }

    [TestMethod]
    public void ShouldDeclareOrderedCollection()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareCollection(new OWLNamedIndividual(new RDFResource("ex:Collection")),
            [new OWLNamedIndividual(new RDFResource("ex:ConceptA"))], ordered: true);

        OWLClassAssertion collectionTypeAsn = ontology.GetAssertionAxiomsOfType<OWLClassAssertion>()
            .Single(asn => asn.IndividualExpression.GetIRI().Equals(new RDFResource("ex:Collection")));
        Assert.IsTrue(collectionTypeAsn.ClassExpression.GetIRI().Equals(RDFVocabulary.SKOS.ORDERED_COLLECTION));
    }

    [TestMethod]
    public void ShouldCheckConceptScheme()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>());
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:conceptScheme2")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme2"))));

        Assert.IsTrue(ontology.CheckHasConceptScheme(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        Assert.IsFalse(ontology.CheckHasConceptScheme(new OWLNamedIndividual(new RDFResource("ex:conceptScheme3"))));
        Assert.IsFalse((null as OWLOntology).CheckHasConceptScheme(new OWLNamedIndividual(new RDFResource("ex:conceptScheme3"))));
        Assert.IsFalse(ontology.CheckHasConceptScheme(null));
    }

    [TestMethod]
    public void ShouldCheckAndGetConceptsInScheme()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME));
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.HAS_TOP_CONCEPT));
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.TOP_CONCEPT_OF));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:conceptScheme2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.TOP_CONCEPT_OF),
            new OWLNamedIndividual(new RDFResource("ex:concept4")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.HAS_TOP_CONCEPT),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme2"))));

        List<RDFResource> cs1Concepts = ontology.GetConceptsInScheme(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")));

        Assert.HasCount(4, cs1Concepts);
        Assert.IsTrue(ontology.CheckHasConcept(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsTrue(ontology.CheckHasConcept(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsTrue(ontology.CheckHasConcept(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:concept4")))); //via skos:topConceptOf
        Assert.IsTrue(ontology.CheckHasConcept(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:concept5")))); //via skos:hasTopConcept

        Assert.IsEmpty((null as OWLOntology).GetConceptsInScheme(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsEmpty(ontology.GetConceptsInScheme(null));
        Assert.IsFalse((null as OWLOntology).CheckHasConcept(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetCollectionsInScheme()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.COLLECTION.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.ORDERED_COLLECTION.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:collection1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:collection2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.COLLECTION.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:collection1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.ORDERED_COLLECTION.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:collection2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept4")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:concept5")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:collection1")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
            new OWLNamedIndividual(new RDFResource("ex:collection2")),
            new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));

        List<RDFResource> cs1Collections = ontology.GetCollectionsInScheme(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")));

        Assert.HasCount(2, cs1Collections);
        Assert.IsTrue(ontology.CheckHasCollection(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:collection1"))));
        Assert.IsTrue(ontology.CheckHasCollection(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:collection2"))));

        Assert.IsEmpty((null as OWLOntology).GetCollectionsInScheme(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        Assert.IsEmpty(ontology.GetCollectionsInScheme(null));
        Assert.IsFalse((null as OWLOntology).CheckHasCollection(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasCollection(null, new OWLNamedIndividual(new RDFResource("ex:conceptScheme1"))));
        Assert.IsFalse(ontology.CheckHasCollection(new OWLNamedIndividual(new RDFResource("ex:conceptScheme1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetBroaderConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.BROADER));
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.BROADER),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.BROADER),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));

        List<RDFResource> c1BroaderConcepts = ontology.GetBroaderConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(3, c1BroaderConcepts);
        Assert.IsTrue(ontology.CheckHasBroaderConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasBroaderConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept5")))); //skos:broader is not transitive
        Assert.IsTrue(ontology.CheckHasBroaderConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        Assert.IsTrue(ontology.CheckHasBroaderConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept4")))); //inference

        Assert.IsEmpty(ontology.GetBroaderConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetBroaderConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasBroaderConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasBroaderConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasBroaderConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetNarrowerConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER));
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER_TRANSITIVE));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER_TRANSITIVE),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER_TRANSITIVE),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));

        List<RDFResource> c1NarrowerConcepts = ontology.GetNarrowerConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(3, c1NarrowerConcepts);
        Assert.IsTrue(ontology.CheckHasNarrowerConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasNarrowerConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept5")))); //skos:narrower is not transitive
        Assert.IsTrue(ontology.CheckHasNarrowerConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        Assert.IsTrue(ontology.CheckHasNarrowerConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept4")))); //inference

        Assert.IsEmpty(ontology.GetNarrowerConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetNarrowerConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasNarrowerConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasNarrowerConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasNarrowerConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetRelatedConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>());
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.RELATED.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));

        List<RDFResource> c1RelatedConcepts = ontology.GetRelatedConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(2, c1RelatedConcepts);
        Assert.IsTrue(ontology.CheckHasRelatedConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsTrue(ontology.CheckHasRelatedConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3")))); //inference (via simmetry)

        Assert.IsEmpty(ontology.GetRelatedConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetRelatedConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasRelatedConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasRelatedConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasRelatedConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetBroadMatchConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>());
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));

        List<RDFResource> c1BroadMatchConcepts = ontology.GetBroadMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(2, c1BroadMatchConcepts);
        Assert.IsTrue(ontology.CheckHasBroadMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsTrue(ontology.CheckHasBroadMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3")))); //inference (via inverse)

        Assert.IsEmpty(ontology.GetBroadMatchConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetBroadMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasBroadMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasBroadMatchConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasBroadMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetNarrowMatchConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>());
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.BROAD_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.NARROW_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));

        List<RDFResource> c1NarrowMatchConcepts = ontology.GetNarrowMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(2, c1NarrowMatchConcepts);
        Assert.IsTrue(ontology.CheckHasNarrowMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsTrue(ontology.CheckHasNarrowMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3")))); //inference (via inverse)

        Assert.IsEmpty(ontology.GetNarrowMatchConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetNarrowMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasNarrowMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasNarrowMatchConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasNarrowMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetCloseMatchConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.CLOSE_MATCH.ToEntity<OWLObjectProperty>());
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.CLOSE_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.CLOSE_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.CLOSE_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.CLOSE_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));

        List<RDFResource> c1CloseMatchConcepts = ontology.GetCloseMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(2, c1CloseMatchConcepts);
        Assert.IsTrue(ontology.CheckHasCloseMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsTrue(ontology.CheckHasCloseMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3")))); //inference (via simmetry)

        Assert.IsEmpty(ontology.GetCloseMatchConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetCloseMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasCloseMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasCloseMatchConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasCloseMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetExactMatchConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(RDFVocabulary.SKOS.EXACT_MATCH.ToEntity<OWLObjectProperty>());
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.EXACT_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.EXACT_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.EXACT_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            RDFVocabulary.SKOS.EXACT_MATCH.ToEntity<OWLObjectProperty>(),
            new OWLNamedIndividual(new RDFResource("ex:concept5")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));

        List<RDFResource> c1ExactMatchConcepts = ontology.GetExactMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(4, c1ExactMatchConcepts);
        Assert.IsTrue(ontology.CheckHasExactMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsTrue(ontology.CheckHasExactMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3")))); //inference
        Assert.IsTrue(ontology.CheckHasExactMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept4")))); //inference
        Assert.IsTrue(ontology.CheckHasExactMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept5")))); //inference (via simmetry)

        Assert.IsEmpty(ontology.GetExactMatchConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetExactMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasExactMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasExactMatchConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasExactMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    [TestMethod]
    public void ShouldCheckAndGetRelatedMatchConcepts()
    {
        OWLOntology ontology = new OWLOntology(new Uri("ex:ontology"));
        ontology.DeclareEntity(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>());
        ontology.DeclareEntity(new OWLObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept1")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept2")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept3")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept4")));
        ontology.DeclareEntity(new OWLNamedIndividual(new RDFResource("ex:concept5")));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept3"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLClassAssertion(
            RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH),
            new OWLNamedIndividual(new RDFResource("ex:concept1")),
            new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH),
            new OWLNamedIndividual(new RDFResource("ex:concept2")),
            new OWLNamedIndividual(new RDFResource("ex:concept4"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH),
            new OWLNamedIndividual(new RDFResource("ex:concept3")),
            new OWLNamedIndividual(new RDFResource("ex:concept5"))));

        List<RDFResource> c1RelatedMatchConcepts = ontology.GetRelatedMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1")));

        Assert.HasCount(2, c1RelatedMatchConcepts);
        Assert.IsTrue(ontology.CheckHasRelatedMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsTrue(ontology.CheckHasRelatedMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept3")))); //inference (via simmetry)

        Assert.IsEmpty(ontology.GetRelatedMatchConcepts(null));
        Assert.IsEmpty((null as OWLOntology).GetRelatedMatchConcepts(new OWLNamedIndividual(new RDFResource("ex:concept1"))));
        Assert.IsFalse((null as OWLOntology).CheckHasRelatedMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), new OWLNamedIndividual(new RDFResource("ex:concept2"))));
        Assert.IsFalse(ontology.CheckHasRelatedMatchConcept(null, new OWLNamedIndividual(new RDFResource("ex:concept5"))));
        Assert.IsFalse(ontology.CheckHasRelatedMatchConcept(new OWLNamedIndividual(new RDFResource("ex:concept1")), null));
    }

    // --- New Declare methods (v5) ---

    [TestMethod]
    public void ShouldDeclareBroaderConcept()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual child = new OWLNamedIndividual(new RDFResource("ex:child"));
        OWLNamedIndividual parent = new OWLNamedIndividual(new RDFResource("ex:parent"));
        ontology.DeclareBroaderConcept(child, parent);

        List<OWLObjectPropertyAssertion> asns = ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>();
        OWLObjectPropertyAssertion broaderAsn = asns.Single(a => a.ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.SKOS.BROADER));
        OWLObjectPropertyAssertion narrowerAsn = asns.Single(a => a.ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.SKOS.NARROWER));

        Assert.IsTrue(broaderAsn.SourceIndividualExpression.GetIRI().Equals(child.GetIRI()));
        Assert.IsTrue(broaderAsn.TargetIndividualExpression.GetIRI().Equals(parent.GetIRI()));
        Assert.IsFalse(broaderAsn.IsInference);

        Assert.IsTrue(narrowerAsn.SourceIndividualExpression.GetIRI().Equals(parent.GetIRI()));
        Assert.IsTrue(narrowerAsn.TargetIndividualExpression.GetIRI().Equals(child.GetIRI()));
        Assert.IsTrue(narrowerAsn.IsInference);

        Assert.IsTrue(ontology.CheckHasBroaderConcept(child, parent));
        Assert.IsTrue(ontology.CheckHasNarrowerConcept(parent, child));

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareBroaderConcept(null, parent));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareBroaderConcept(child, null));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareBroaderConcept(child, child));
    }

    [TestMethod]
    public void ShouldDeclareBroaderConceptTransitive()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual c1 = new OWLNamedIndividual(new RDFResource("ex:c1"));
        OWLNamedIndividual c2 = new OWLNamedIndividual(new RDFResource("ex:c2"));
        OWLNamedIndividual c3 = new OWLNamedIndividual(new RDFResource("ex:c3"));
        ontology.DeclareBroaderConcept(c1, c2, transitive: true);
        ontology.DeclareBroaderConcept(c2, c3, transitive: true);

        List<RDFResource> c1Broader = ontology.GetBroaderConcepts(c1);
        Assert.HasCount(2, c1Broader);
        Assert.IsTrue(ontology.CheckHasBroaderConcept(c1, c3)); //closure over 3+ nodes
    }

    [TestMethod]
    public void ShouldDeclareNarrowerConcept()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual parent = new OWLNamedIndividual(new RDFResource("ex:parent"));
        OWLNamedIndividual child = new OWLNamedIndividual(new RDFResource("ex:child"));
        ontology.DeclareNarrowerConcept(parent, child);

        List<OWLObjectPropertyAssertion> asns = ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>();
        OWLObjectPropertyAssertion narrowerAsn = asns.Single(a => a.ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.SKOS.NARROWER));
        OWLObjectPropertyAssertion broaderAsn = asns.Single(a => a.ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.SKOS.BROADER));

        Assert.IsFalse(narrowerAsn.IsInference);
        Assert.IsTrue(broaderAsn.IsInference);
        Assert.IsTrue(ontology.CheckHasNarrowerConcept(parent, child));
        Assert.IsTrue(ontology.CheckHasBroaderConcept(child, parent));

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareNarrowerConcept(null, child));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareNarrowerConcept(parent, null));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareNarrowerConcept(parent, parent));
    }

    [TestMethod]
    public void ShouldDeclareRelatedConcept()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual a = new OWLNamedIndividual(new RDFResource("ex:a"));
        OWLNamedIndividual b = new OWLNamedIndividual(new RDFResource("ex:b"));
        ontology.DeclareRelatedConcept(a, b);

        List<OWLObjectPropertyAssertion> asns = ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>();
        Assert.HasCount(2, asns);
        Assert.IsTrue(ontology.CheckHasRelatedConcept(a, b));
        Assert.IsTrue(ontology.CheckHasRelatedConcept(b, a));
        Assert.IsFalse(asns.Single(x => x.SourceIndividualExpression.GetIRI().Equals(a.GetIRI())).IsInference);
        Assert.IsTrue(asns.Single(x => x.SourceIndividualExpression.GetIRI().Equals(b.GetIRI())).IsInference);

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareRelatedConcept(null, b));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareRelatedConcept(a, null));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareRelatedConcept(a, a));
    }

    [TestMethod]
    [DataRow(SKOSEnums.SKOSMappingRelation.ExactMatch, "http://www.w3.org/2004/02/skos/core#exactMatch", "http://www.w3.org/2004/02/skos/core#exactMatch")]
    [DataRow(SKOSEnums.SKOSMappingRelation.CloseMatch, "http://www.w3.org/2004/02/skos/core#closeMatch", "http://www.w3.org/2004/02/skos/core#closeMatch")]
    [DataRow(SKOSEnums.SKOSMappingRelation.RelatedMatch, "http://www.w3.org/2004/02/skos/core#relatedMatch", "http://www.w3.org/2004/02/skos/core#relatedMatch")]
    [DataRow(SKOSEnums.SKOSMappingRelation.BroadMatch, "http://www.w3.org/2004/02/skos/core#broadMatch", "http://www.w3.org/2004/02/skos/core#narrowMatch")]
    [DataRow(SKOSEnums.SKOSMappingRelation.NarrowMatch, "http://www.w3.org/2004/02/skos/core#narrowMatch", "http://www.w3.org/2004/02/skos/core#broadMatch")]
    public void ShouldDeclareMappingConcept(SKOSEnums.SKOSMappingRelation relation, string directPropertyUri, string inversePropertyUri)
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual left = new OWLNamedIndividual(new RDFResource("ex:left"));
        OWLNamedIndividual right = new OWLNamedIndividual(new RDFResource("ex:right"));
        ontology.DeclareMappingConcept(left, right, relation);

        List<OWLObjectPropertyAssertion> asns = ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>();
        OWLObjectPropertyAssertion directAsn = asns.Single(a => a.ObjectPropertyExpression.GetIRI().Equals(new RDFResource(directPropertyUri))
                                                                  && a.SourceIndividualExpression.GetIRI().Equals(left.GetIRI()));
        OWLObjectPropertyAssertion inverseAsn = asns.Single(a => a.ObjectPropertyExpression.GetIRI().Equals(new RDFResource(inversePropertyUri))
                                                                    && a.SourceIndividualExpression.GetIRI().Equals(right.GetIRI()));

        Assert.IsFalse(directAsn.IsInference);
        Assert.IsTrue(inverseAsn.IsInference);

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareMappingConcept(null, right, relation));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareMappingConcept(left, null, relation));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareMappingConcept(left, left, relation));
    }

    [TestMethod]
    public void ShouldDeclareNotation()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual concept = new OWLNamedIndividual(new RDFResource("ex:concept"));
        OWLLiteral notation = new OWLLiteral(new RDFTypedLiteral("42N", RDFModelEnums.RDFDatatypes.XSD_STRING));
        ontology.DeclareNotation(concept, notation);

        List<OWLDataPropertyAssertion> asns = ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>();
        OWLDataPropertyAssertion notationAsn = asns.Single(a => a.DataProperty.GetIRI().Equals(RDFVocabulary.SKOS.NOTATION));
        Assert.IsTrue(notationAsn.IndividualExpression.GetIRI().Equals(concept.GetIRI()));

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareNotation(null, notation));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareNotation(concept, null));
    }

    [TestMethod]
    public void ShouldDeclareLabel()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual label = new OWLNamedIndividual(new RDFResource("ex:label"));
        OWLLiteral literalForm = new OWLLiteral(new RDFPlainLiteral("Label Text", "en"));
        ontology.DeclareLabel(label, literalForm);

        Assert.IsTrue(ontology.CheckIsIndividualOf(RDFVocabulary.SKOS.SKOSXL.LABEL.ToEntity<OWLClass>(), label));
        OWLDataPropertyAssertion literalFormAsn = ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>()
            .Single(a => a.DataProperty.GetIRI().Equals(RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM));
        Assert.IsTrue(literalFormAsn.IndividualExpression.GetIRI().Equals(label.GetIRI()));

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareLabel(null, literalForm));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareLabel(label, null));
    }

    [TestMethod]
    public void ShouldDeclarePreferredAlternativeHiddenLabel()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual concept = new OWLNamedIndividual(new RDFResource("ex:concept"));
        OWLNamedIndividual prefLabel = new OWLNamedIndividual(new RDFResource("ex:prefLabel"));
        OWLNamedIndividual altLabel = new OWLNamedIndividual(new RDFResource("ex:altLabel"));
        OWLNamedIndividual hiddenLabel = new OWLNamedIndividual(new RDFResource("ex:hiddenLabel"));
        ontology.DeclarePreferredLabel(concept, prefLabel);
        ontology.DeclareAlternativeLabel(concept, altLabel);
        ontology.DeclareHiddenLabel(concept, hiddenLabel);

        List<OWLObjectPropertyAssertion> asns = ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>();
        Assert.IsTrue(asns.Any(a => a.ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.SKOS.SKOSXL.PREF_LABEL)
                                     && a.TargetIndividualExpression.GetIRI().Equals(prefLabel.GetIRI())));
        Assert.IsTrue(asns.Any(a => a.ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.SKOS.SKOSXL.ALT_LABEL)
                                     && a.TargetIndividualExpression.GetIRI().Equals(altLabel.GetIRI())));
        Assert.IsTrue(asns.Any(a => a.ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL)
                                     && a.TargetIndividualExpression.GetIRI().Equals(hiddenLabel.GetIRI())));

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclarePreferredLabel(null, prefLabel));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclarePreferredLabel(concept, null));
    }

    [TestMethod]
    public void ShouldNotThrowOnCrossRelationalDeclarations()
    {
        //Contract: the Declarer only guards local invariants (null, self-loop); cross-relational
        //clashes (e.g. related(A,B) + broader(A,B)) are NOT detected here, but by SKOSValidator.
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual a = new OWLNamedIndividual(new RDFResource("ex:a"));
        OWLNamedIndividual b = new OWLNamedIndividual(new RDFResource("ex:b"));

        ontology.DeclareRelatedConcept(a, b);
        ontology.DeclareBroaderConcept(a, b); //no exception expected, despite the clash with skos:related

        Assert.IsTrue(ontology.CheckHasRelatedConcept(a, b));
        Assert.IsTrue(ontology.CheckHasBroaderConcept(a, b));
    }

    [TestMethod]
    public void ShouldBeIdempotentOnRepeatedDeclareBroaderConcept()
    {
        OWLOntology ontology = new OWLOntology();
        OWLNamedIndividual child = new OWLNamedIndividual(new RDFResource("ex:child"));
        OWLNamedIndividual parent = new OWLNamedIndividual(new RDFResource("ex:parent"));

        ontology.DeclareBroaderConcept(child, parent);
        ontology.DeclareBroaderConcept(child, parent);

        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
    }
    #endregion
}
