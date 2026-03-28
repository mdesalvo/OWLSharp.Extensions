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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OWLSharp.Extensions.TIME;
using OWLSharp.Ontology;
using RDFSharp.Model;
using System.Collections.Generic;
using System.Linq;

namespace OWLSharp.Extensions.Test.TIME;

[TestClass]
public class TIMEOrdinalReferenceSystemTest
{
    internal static TIMEOrdinalReferenceSystem TestTRS { get; set; }

    [TestInitialize]
    public void Initialize()
    {
        try
        {
            TestTRS ??= new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors"));
        }
        catch (OWLException oex) when (oex.Message.Contains("operation has timed out", StringComparison.OrdinalIgnoreCase))
        {
            Assert.Inconclusive("THORS ontology site is not reachable at the moment");
        }
    }

    #region Tests
    [TestMethod]
    public void ShouldTestInitialization()
    {
        //Test initialization
        Assert.IsNotNull(TestTRS);
        Assert.IsNotNull(TestTRS.Ontology);
        Assert.IsTrue(TestTRS.Equals(new RDFResource("ex:Thors")));
        Assert.HasCount(3, TestTRS.Ontology.Imports);
        Assert.HasCount(5, TestTRS.Ontology.Prefixes);
        Assert.IsGreaterThan(100, TestTRS.Ontology.DeclarationAxioms.Count);
        Assert.IsGreaterThan(60, TestTRS.Ontology.AssertionAxioms.Count);
        Assert.IsGreaterThan(60, TestTRS.Ontology.ClassAxioms.Count);
        Assert.IsGreaterThan(20, TestTRS.Ontology.DataPropertyAxioms.Count);
        Assert.IsGreaterThan(100, TestTRS.Ontology.ObjectPropertyAxioms.Count);

        //Test copy-ctor
        TIMEOrdinalReferenceSystem newTRS = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        Assert.IsNotNull(newTRS);
        Assert.IsNotNull(newTRS.Ontology);
        Assert.IsTrue(newTRS.Equals(new RDFResource("ex:Thors2")));
        Assert.HasCount(3, newTRS.Ontology.Imports);
        Assert.HasCount(5, newTRS.Ontology.Prefixes);
        Assert.IsGreaterThan(100, newTRS.Ontology.DeclarationAxioms.Count);
        Assert.IsGreaterThan(60, newTRS.Ontology.AssertionAxioms.Count);
        Assert.IsGreaterThan(60, newTRS.Ontology.ClassAxioms.Count);
        Assert.IsGreaterThan(20, newTRS.Ontology.DataPropertyAxioms.Count);
        Assert.IsGreaterThan(100, newTRS.Ontology.ObjectPropertyAxioms.Count);
        Assert.ThrowsExactly<OWLException>(() => _ = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), null));
    }

    [TestMethod]
    public void ShouldDeclareEra()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));

        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:era"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA),
                new OWLNamedIndividual(new RDFResource("ex:era")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.COMPONENT),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")),
                new OWLNamedIndividual(new RDFResource("ex:era")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.SYSTEM),
                new OWLNamedIndividual(new RDFResource("ex:era")),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")))));

        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraBeginning"))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraBeginningPosition"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:eraBeginning")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.BEGIN),
                new OWLNamedIndividual(new RDFResource("ex:era")),
                new OWLNamedIndividual(new RDFResource("ex:eraBeginning")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.NEXT_ERA),
                new OWLNamedIndividual(new RDFResource("ex:eraBeginning")),
                new OWLNamedIndividual(new RDFResource("ex:era")))));

        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraEnd"))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraEndPosition"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:eraEnd")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.END),
                new OWLNamedIndividual(new RDFResource("ex:era")),
                new OWLNamedIndividual(new RDFResource("ex:eraEnd")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.PREVIOUS_ERA),
                new OWLNamedIndividual(new RDFResource("ex:eraEnd")),
                new OWLNamedIndividual(new RDFResource("ex:era")))));
    }

    [TestMethod]
    public void ShouldThrowExceptionOnDeclaringEraBecauseNullEra()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS)
            .DeclareEra(null, new TIMEInstant(new RDFResource("ex:begin")), new TIMEInstant(new RDFResource("ex:end"))));

    [TestMethod]
    public void ShouldDeclareSubEra()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));
        thors.DeclareSubEra(new RDFResource("ex:subEra"), new RDFResource("ex:era"));

        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:era"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA),
                new OWLNamedIndividual(new RDFResource("ex:era")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.COMPONENT),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")),
                new OWLNamedIndividual(new RDFResource("ex:era")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.SYSTEM),
                new OWLNamedIndividual(new RDFResource("ex:era")),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraBeginning"))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraBeginningPosition"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:eraBeginning")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.BEGIN),
                new OWLNamedIndividual(new RDFResource("ex:era")),
                new OWLNamedIndividual(new RDFResource("ex:eraBeginning")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.NEXT_ERA),
                new OWLNamedIndividual(new RDFResource("ex:eraBeginning")),
                new OWLNamedIndividual(new RDFResource("ex:era")))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraEnd"))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:eraEndPosition"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:eraEnd")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.END),
                new OWLNamedIndividual(new RDFResource("ex:era")),
                new OWLNamedIndividual(new RDFResource("ex:eraEnd")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.PREVIOUS_ERA),
                new OWLNamedIndividual(new RDFResource("ex:eraEnd")),
                new OWLNamedIndividual(new RDFResource("ex:era")))));

        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:subEra"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA),
                new OWLNamedIndividual(new RDFResource("ex:subEra")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.COMPONENT),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")),
                new OWLNamedIndividual(new RDFResource("ex:subEra")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.SYSTEM),
                new OWLNamedIndividual(new RDFResource("ex:subEra")),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:subEraBeginning"))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:subEraBeginningPosition"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:subEraBeginning")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.BEGIN),
                new OWLNamedIndividual(new RDFResource("ex:subEra")),
                new OWLNamedIndividual(new RDFResource("ex:subEraBeginning")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.NEXT_ERA),
                new OWLNamedIndividual(new RDFResource("ex:subEraBeginning")),
                new OWLNamedIndividual(new RDFResource("ex:subEra")))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:subEraEnd"))));
        Assert.IsTrue(thors.Ontology.CheckHasEntity(new OWLNamedIndividual(new RDFResource("ex:subEraEndPosition"))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:subEraEnd")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.END),
                new OWLNamedIndividual(new RDFResource("ex:subEra")),
                new OWLNamedIndividual(new RDFResource("ex:subEraEnd")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.PREVIOUS_ERA),
                new OWLNamedIndividual(new RDFResource("ex:subEraEnd")),
                new OWLNamedIndividual(new RDFResource("ex:subEra")))));

        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.MEMBER),
                new OWLNamedIndividual(new RDFResource("ex:era")),
                new OWLNamedIndividual(new RDFResource("ex:subEra")))));

        Assert.ThrowsExactly<OWLException>(() => _ = thors.DeclareSubEra(new RDFResource("ex:era"), new RDFResource("ex:subEra")));
    }

    [TestMethod]
    public void ShouldThrowExceptionOnDeclaringSubEraBecauseNullEra()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS)
            .DeclareSubEra(null, new RDFResource("ex:subEra")));

    [TestMethod]
    public void ShouldThrowExceptionOnDeclaringSubEraBecauseNullSubEra()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS)
            .DeclareSubEra(new RDFResource("ex:era"), null));

    [TestMethod]
    public void ShouldDeclareReferencePoints()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareReferencePoints([
            new TIMEInstant(
                new RDFResource("ex:massExtinctionEventA"),
                new TIMEInstantPosition(new RDFResource("ex:massExtinctionEventPositionA"), TIMEPositionReferenceSystem.Geologic, 111.9)),
            new TIMEInstant(
                new RDFResource("ex:massExtinctionEventB"),
                new TIMEInstantPosition(new RDFResource("ex:massExtinctionEventPositionB"), TIMEPositionReferenceSystem.Geologic, 65.5))
        ]);

        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:massExtinctionEventA")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLClassAssertion(
                new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY),
                new OWLNamedIndividual(new RDFResource("ex:massExtinctionEventB")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.REFERENCE_POINT),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")),
                new OWLNamedIndividual(new RDFResource("ex:massExtinctionEventA")))));
        Assert.IsTrue(thors.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.REFERENCE_POINT),
                new OWLNamedIndividual(new RDFResource("ex:Thors2")),
                new OWLNamedIndividual(new RDFResource("ex:massExtinctionEventB")))));
    }

    [TestMethod]
    public void ShouldThrowExceptionOnDeclaringReferencePointsBecauseNullReferencePoints()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS)
            .DeclareReferencePoints(null));

    [TestMethod]
    public void ShouldThrowExceptionOnDeclaringReferencePointsBecauseContainingNullReferencePoints()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS)
            .DeclareReferencePoints([new TIMEInstant(new RDFResource("ex:massExtinctionEvent")), null]));

    [TestMethod]
    public void ShouldThrowExceptionOnDeclaringReferencePointsBecauseLessThan2ReferencePoints()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS)
            .DeclareReferencePoints([new TIMEInstant(new RDFResource("ex:massExtinctionEvent"))]));

    [TestMethod]
    public void ShouldCheckHasEra()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));

        Assert.IsTrue(thors.CheckHasEra(new RDFResource("ex:era")));
        Assert.IsTrue(thors.CheckHasEra(new RDFResource("ex:subEra")));
        Assert.IsFalse(thors.CheckHasEra(new RDFResource("ex:erazz")));
        Assert.ThrowsExactly<OWLException>(() => _ = thors.CheckHasEra(null));
    }

    [TestMethod]
    public void ShouldCheckHasEraBoundary()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));

        Assert.IsTrue(thors.CheckHasEraBoundary(new RDFResource("ex:eraBeginning")));
        Assert.IsTrue(thors.CheckHasEraBoundary(new RDFResource("ex:eraEnd")));
        Assert.IsTrue(thors.CheckHasEraBoundary(new RDFResource("ex:subEraBeginning")));
        Assert.IsTrue(thors.CheckHasEraBoundary(new RDFResource("ex:subEraEnd")));
        Assert.IsFalse(thors.CheckHasEraBoundary(new RDFResource("ex:erazz")));
        Assert.ThrowsExactly<OWLException>(() => _ = thors.CheckHasEraBoundary(null));
    }

    [TestMethod]
    public void ShouldCheckHasReferencePoint()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareReferencePoints([
            new TIMEInstant(
                new RDFResource("ex:massExtinctionEventA"),
                new TIMEInstantPosition(new RDFResource("ex:massExtinctionEventPositionA"), TIMEPositionReferenceSystem.Geologic, 111.9)),
            new TIMEInstant(
                new RDFResource("ex:massExtinctionEventB"),
                new TIMEInstantPosition(new RDFResource("ex:massExtinctionEventPositionB"), TIMEPositionReferenceSystem.Geologic, 65.5))
        ]);

        Assert.IsTrue(thors.CheckHasReferencePoint(new RDFResource("ex:massExtinctionEventA")));
        Assert.IsTrue(thors.CheckHasReferencePoint(new RDFResource("ex:massExtinctionEventB")));
        Assert.IsFalse(thors.CheckHasReferencePoint(new RDFResource("ex:massExtinctionEventzz")));
        Assert.ThrowsExactly<OWLException>(() => _ = thors.CheckHasReferencePoint(null));
    }

    [TestMethod]
    public void ShouldCheckIsSubEraOf()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));
        thors.DeclareEra(
            new RDFResource("ex:subsubEra"),
            new TIMEInstant(
                new RDFResource("ex:subsubEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subsubEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 184)));
        thors.DeclareSubEra(new RDFResource("ex:subEra"), new RDFResource("ex:era"));
        thors.DeclareSubEra(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra"));

        Assert.IsTrue(thors.CheckIsSubEraOf(new RDFResource("ex:subEra"), new RDFResource("ex:era")));
        Assert.IsTrue(thors.CheckIsSubEraOf(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra")));
        Assert.IsTrue(thors.CheckIsSubEraOf(new RDFResource("ex:subsubEra"), new RDFResource("ex:era")));
        Assert.IsTrue(thors.CheckIsSubEraOf(new RDFResource("ex:subEra"), new RDFResource("ex:era"), false));
        Assert.IsTrue(thors.CheckIsSubEraOf(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra"), false));
        Assert.IsFalse(thors.CheckIsSubEraOf(new RDFResource("ex:subsubEra"), new RDFResource("ex:era"), false));
    }

    [TestMethod]
    public void ShouldGetSubErasOf()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));
        thors.DeclareEra(
            new RDFResource("ex:subsubEra"),
            new TIMEInstant(
                new RDFResource("ex:subsubEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subsubEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 184)));
        thors.DeclareSubEra(new RDFResource("ex:subEra"), new RDFResource("ex:era"));
        thors.DeclareSubEra(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra"));
        List<RDFResource> subErasOfEraWithReasoning = thors.GetSubErasOf(new RDFResource("ex:era"));
        List<RDFResource> subErasOfEraWithoutReasoning = thors.GetSubErasOf(new RDFResource("ex:era"), false);
        List<RDFResource> subErasOfSubEraWithReasoning = thors.GetSubErasOf(new RDFResource("ex:subEra"));
        List<RDFResource> subErasOfSubEraWithoutReasoning = thors.GetSubErasOf(new RDFResource("ex:subEra"), false);
        List<RDFResource> subErasOfSubSubEraWithReasoning = thors.GetSubErasOf(new RDFResource("ex:subsubEra"));
        List<RDFResource> subErasOfSubSubEraWithoutReasoning = thors.GetSubErasOf(new RDFResource("ex:subsubEra"), false);

        Assert.HasCount(2, subErasOfEraWithReasoning);
        Assert.IsTrue(subErasOfEraWithReasoning.Any(x => x.Equals(new RDFResource("ex:subEra"))));
        Assert.IsTrue(subErasOfEraWithReasoning.Any(x => x.Equals(new RDFResource("ex:subsubEra"))));
        Assert.HasCount(1, subErasOfEraWithoutReasoning);
        Assert.IsTrue(subErasOfEraWithoutReasoning.Any(x => x.Equals(new RDFResource("ex:subEra"))));
        Assert.HasCount(1, subErasOfSubEraWithReasoning);
        Assert.IsTrue(subErasOfSubEraWithReasoning.Any(x => x.Equals(new RDFResource("ex:subsubEra"))));
        Assert.HasCount(1, subErasOfSubEraWithoutReasoning);
        Assert.IsTrue(subErasOfSubEraWithoutReasoning.Any(x => x.Equals(new RDFResource("ex:subsubEra"))));
        Assert.IsEmpty(subErasOfSubSubEraWithReasoning);
        Assert.IsEmpty(subErasOfSubSubEraWithoutReasoning);
    }

    [TestMethod]
    public void ShouldCheckIsSuperEraOf()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));
        thors.DeclareEra(
            new RDFResource("ex:subsubEra"),
            new TIMEInstant(
                new RDFResource("ex:subsubEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subsubEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 184)));
        thors.DeclareSubEra(new RDFResource("ex:subEra"), new RDFResource("ex:era"));
        thors.DeclareSubEra(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra"));

        Assert.IsTrue(thors.CheckIsSuperEraOf(new RDFResource("ex:era"), new RDFResource("ex:subEra")));
        Assert.IsTrue(thors.CheckIsSuperEraOf(new RDFResource("ex:subEra"), new RDFResource("ex:subsubEra")));
        Assert.IsTrue(thors.CheckIsSuperEraOf(new RDFResource("ex:era"), new RDFResource("ex:subsubEra")));
        Assert.IsTrue(thors.CheckIsSuperEraOf(new RDFResource("ex:era"), new RDFResource("ex:subEra"), false));
        Assert.IsTrue(thors.CheckIsSuperEraOf(new RDFResource("ex:subEra"), new RDFResource("ex:subsubEra"), false));
        Assert.IsFalse(thors.CheckIsSuperEraOf(new RDFResource("ex:era"), new RDFResource("ex:subsubEra"), false));
    }

    [TestMethod]
    public void ShouldGetSuperErasOf()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));
        thors.DeclareEra(
            new RDFResource("ex:subsubEra"),
            new TIMEInstant(
                new RDFResource("ex:subsubEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subsubEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 184)));
        thors.DeclareSubEra(new RDFResource("ex:subEra"), new RDFResource("ex:era"));
        thors.DeclareSubEra(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra"));
        List<RDFResource> superErasOfEraWithReasoning = thors.GetSuperErasOf(new RDFResource("ex:era"));
        List<RDFResource> superErasOfEraWithoutReasoning = thors.GetSuperErasOf(new RDFResource("ex:era"), false);
        List<RDFResource> superErasOfSubEraWithReasoning = thors.GetSuperErasOf(new RDFResource("ex:subEra"));
        List<RDFResource> superErasOfSubEraWithoutReasoning = thors.GetSuperErasOf(new RDFResource("ex:subEra"), false);
        List<RDFResource> superErasOfSubSubEraWithReasoning = thors.GetSuperErasOf(new RDFResource("ex:subsubEra"));
        List<RDFResource> superErasOfSubSubEraWithoutReasoning = thors.GetSuperErasOf(new RDFResource("ex:subsubEra"), false);

        Assert.IsEmpty(superErasOfEraWithReasoning);
        Assert.IsEmpty(superErasOfEraWithoutReasoning);
        Assert.HasCount(1, superErasOfSubEraWithReasoning);
        Assert.IsTrue(superErasOfSubEraWithReasoning.Any(x => x.Equals(new RDFResource("ex:era"))));
        Assert.HasCount(1, superErasOfSubEraWithoutReasoning);
        Assert.IsTrue(superErasOfSubEraWithoutReasoning.Any(x => x.Equals(new RDFResource("ex:era"))));
        Assert.HasCount(2, superErasOfSubSubEraWithReasoning);
        Assert.IsTrue(superErasOfSubSubEraWithReasoning.Any(x => x.Equals(new RDFResource("ex:subEra"))));
        Assert.IsTrue(superErasOfSubSubEraWithReasoning.Any(x => x.Equals(new RDFResource("ex:era"))));
        Assert.HasCount(1, superErasOfSubSubEraWithoutReasoning);
        Assert.IsTrue(superErasOfSubSubEraWithoutReasoning.Any(x => x.Equals(new RDFResource("ex:subEra"))));
    }

    [TestMethod]
    public void ShouldGetEraCoordinates()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));
        thors.DeclareEra(
            new RDFResource("ex:subsubEra"),
            new TIMEInstant(
                new RDFResource("ex:subsubEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subsubEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 184)));
        thors.DeclareSubEra(new RDFResource("ex:subEra"), new RDFResource("ex:era"));
        thors.DeclareSubEra(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra"));
        (TIMECoordinate, TIMECoordinate) eraCoordinates = thors.GetEraCoordinates(new RDFResource("ex:era"));
        (TIMECoordinate, TIMECoordinate) subEraCoordinates = thors.GetEraCoordinates(new RDFResource("ex:subEra"));
        (TIMECoordinate, TIMECoordinate) subsubEraCoordinates = thors.GetEraCoordinates(new RDFResource("ex:subsubEra"));

        Assert.IsNotNull(eraCoordinates.Item1);
        Assert.IsTrue(eraCoordinates.Item1.Equals(new TIMECoordinate(-185_498_050, 1, 1, 0, 0, 0)));
        Assert.IsNotNull(eraCoordinates.Item2);
        Assert.IsTrue(eraCoordinates.Item2.Equals(new TIMECoordinate(-169_998_050, 1, 1, 0, 0, 0)));
        Assert.IsNotNull(subEraCoordinates.Item1);
        Assert.IsTrue(subEraCoordinates.Item1.Equals(new TIMECoordinate(-185_498_050, 1, 1, 0, 0, 0)));
        Assert.IsNotNull(subEraCoordinates.Item2);
        Assert.IsTrue(subEraCoordinates.Item2.Equals(new TIMECoordinate(-180_498_050, 1, 1, 0, 0, 0)));
        Assert.IsNotNull(subsubEraCoordinates.Item1);
        Assert.IsTrue(subsubEraCoordinates.Item1.Equals(new TIMECoordinate(-185_498_050, 1, 1, 0, 0, 0)));
        Assert.IsNotNull(subsubEraCoordinates.Item2);
        Assert.IsTrue(subsubEraCoordinates.Item2.Equals(new TIMECoordinate(-183_998_050, 1, 1, 0, 0, 0)));
        Assert.ThrowsExactly<OWLException>(() => _ = thors.GetEraCoordinates(new RDFResource("ex:unexistingEra")));
        Assert.ThrowsExactly<OWLException>(() => _ = thors.GetEraCoordinates(null));
    }

    [TestMethod]
    public void ShouldGetEraExtent()
    {
        TIMEOrdinalReferenceSystem thors = new TIMEOrdinalReferenceSystem(new RDFResource("ex:Thors2"), TestTRS);
        thors.DeclareEra(
            new RDFResource("ex:era"),
            new TIMEInstant(
                new RDFResource("ex:eraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:eraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:eraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:eraEndPosition"), TIMEPositionReferenceSystem.Geologic, 170)));
        thors.DeclareEra(
            new RDFResource("ex:subEra"),
            new TIMEInstant(
                new RDFResource("ex:subEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 180.5)));
        thors.DeclareEra(
            new RDFResource("ex:subsubEra"),
            new TIMEInstant(
                new RDFResource("ex:subsubEraBeginning"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraBeginningPosition"), TIMEPositionReferenceSystem.Geologic, 185.5)),
            new TIMEInstant(
                new RDFResource("ex:subsubEraEnd"),
                new TIMEInstantPosition(new RDFResource("ex:subsubEraEndPosition"), TIMEPositionReferenceSystem.Geologic, 184)));
        thors.DeclareSubEra(new RDFResource("ex:subEra"), new RDFResource("ex:era"));
        thors.DeclareSubEra(new RDFResource("ex:subsubEra"), new RDFResource("ex:subEra"));
        TIMEExtent eraExtent = thors.GetEraExtent(new RDFResource("ex:era"));
        TIMEExtent subEraExtent = thors.GetEraExtent(new RDFResource("ex:subEra"));
        TIMEExtent subsubEraExtent = thors.GetEraExtent(new RDFResource("ex:subsubEra"));

        Assert.IsNotNull(eraExtent);
        Assert.AreEqual(5_657_500_000, eraExtent.Days);
        Assert.IsNotNull(subEraExtent);
        Assert.AreEqual(1_825_000_000, subEraExtent.Days);
        Assert.IsNotNull(subsubEraExtent);
        Assert.AreEqual(547_500_000, subsubEraExtent.Days);
        Assert.ThrowsExactly<OWLException>(() => _ = thors.GetEraExtent(new RDFResource("ex:unexistingEra")));
        Assert.ThrowsExactly<OWLException>(() => _ = thors.GetEraExtent(null));
    }
    #endregion

    #region Showcase — ICS International Chronostratigraphic Chart

    #region Constants — ICS URIs
    private const string ICS = "http://resource.geosciml.org/classifier/ics/ischart/";

    // Eons
    private static readonly RDFResource Phanerozoic = new RDFResource(ICS + "Phanerozoic");

    // Eras
    private static readonly RDFResource Paleozoic = new RDFResource(ICS + "Paleozoic");
    private static readonly RDFResource Mesozoic = new RDFResource(ICS + "Mesozoic");
    private static readonly RDFResource Cenozoic = new RDFResource(ICS + "Cenozoic");

    // Periods (Paleozoic)
    private static readonly RDFResource Cambrian = new RDFResource(ICS + "Cambrian");
    private static readonly RDFResource Ordovician = new RDFResource(ICS + "Ordovician");
    private static readonly RDFResource Silurian = new RDFResource(ICS + "Silurian");
    private static readonly RDFResource Devonian = new RDFResource(ICS + "Devonian");
    private static readonly RDFResource Carboniferous = new RDFResource(ICS + "Carboniferous");
    private static readonly RDFResource Permian = new RDFResource(ICS + "Permian");

    // Periods (Mesozoic)
    private static readonly RDFResource Triassic = new RDFResource(ICS + "Triassic");
    private static readonly RDFResource Jurassic = new RDFResource(ICS + "Jurassic");
    private static readonly RDFResource Cretaceous = new RDFResource(ICS + "Cretaceous");

    // Periods (Cenozoic)
    private static readonly RDFResource Paleogene = new RDFResource(ICS + "Paleogene");
    private static readonly RDFResource Neogene = new RDFResource(ICS + "Neogene");
    private static readonly RDFResource Quaternary = new RDFResource(ICS + "Quaternary");

    // Epochs (Jurassic)
    private static readonly RDFResource EarlyJurassic = new RDFResource(ICS + "EarlyJurassic");
    private static readonly RDFResource MiddleJurassic = new RDFResource(ICS + "MiddleJurassic");
    private static readonly RDFResource LateJurassic = new RDFResource(ICS + "LateJurassic");

    // Epochs (Cretaceous)
    private static readonly RDFResource EarlyCretaceous = new RDFResource(ICS + "EarlyCretaceous");
    private static readonly RDFResource LateCretaceous = new RDFResource(ICS + "LateCretaceous");

    // Epochs (Paleogene)
    private static readonly RDFResource Paleocene = new RDFResource(ICS + "Paleocene");
    private static readonly RDFResource Eocene = new RDFResource(ICS + "Eocene");
    private static readonly RDFResource Oligocene = new RDFResource(ICS + "Oligocene");

    // Epochs (Neogene)
    private static readonly RDFResource Miocene = new RDFResource(ICS + "Miocene");
    private static readonly RDFResource Pliocene = new RDFResource(ICS + "Pliocene");

    // Epochs (Quaternary)
    private static readonly RDFResource Pleistocene = new RDFResource(ICS + "Pleistocene");
    private static readonly RDFResource Holocene = new RDFResource(ICS + "Holocene");
    #endregion

    #region Helpers
    private static TIMEInstant GeoBoundary(string localName, double ma, TIMEIntervalDuration uncertainty = null)
    {
        RDFResource instantUri = new RDFResource(ICS + localName);
        RDFResource positionUri = new RDFResource(ICS + localName + "_position");
        TIMEInstantPosition position = new TIMEInstantPosition(positionUri, TIMEPositionReferenceSystem.Geologic, ma);
        if (uncertainty != null)
            position.PositionalUncertainty = uncertainty;
        return new TIMEInstant(instantUri, position);
    }

    private static TIMEIntervalDuration MaUncertainty(string localName, double ma)
        => new TIMEIntervalDuration(new RDFResource(ICS + localName), RDFVocabulary.TIME.UNIT_YEAR, ma * 1_000_000);

    private static TIMEOrdinalReferenceSystem _ics;

    private static TIMEOrdinalReferenceSystem GetICS()
    {
        if (_ics != null)
            return _ics;

        TIMEOrdinalReferenceSystem ics = new TIMEOrdinalReferenceSystem(
            new RDFResource(ICS + "InternationalChronostratigraphicChart"), TestTRS);

        // === Shared boundary instants (reused across adjacent eras) ===
        TIMEInstant phanerozoicBegin = GeoBoundary("Phanerozoic_begin", 538.8, MaUncertainty("Phanerozoic_begin_unc", 0.2));
        TIMEInstant phanerozoicEnd = GeoBoundary("Phanerozoic_end", 0);
        TIMEInstant paleozoicEnd = GeoBoundary("Paleozoic_Mesozoic_boundary", 251.902, MaUncertainty("Paleozoic_Mesozoic_unc", 0.024));
        TIMEInstant mesozoicEnd = GeoBoundary("K_Pg_boundary", 66, MaUncertainty("K_Pg_unc", 0.05));

        // Shared Paleozoic period boundaries
        TIMEInstant cambrianEnd = GeoBoundary("Cambrian_Ordovician_boundary", 485.4, MaUncertainty("Cambrian_Ordovician_unc", 1.9));
        TIMEInstant ordovicianEnd = GeoBoundary("Ordovician_Silurian_boundary", 443.8, MaUncertainty("Ordovician_Silurian_unc", 1.5));
        TIMEInstant silurianEnd = GeoBoundary("Silurian_Devonian_boundary", 419.2, MaUncertainty("Silurian_Devonian_unc", 3.2));
        TIMEInstant devonianEnd = GeoBoundary("Devonian_Carboniferous_boundary", 358.9, MaUncertainty("Devonian_Carboniferous_unc", 0.4));
        TIMEInstant carboniferousEnd = GeoBoundary("Carboniferous_Permian_boundary", 298.9, MaUncertainty("Carboniferous_Permian_unc", 0.15));

        // Shared Mesozoic period boundaries
        TIMEInstant triassicEnd = GeoBoundary("Triassic_Jurassic_boundary", 201.4, MaUncertainty("Triassic_Jurassic_unc", 0.2));
        TIMEInstant jurassicEnd = GeoBoundary("Jurassic_Cretaceous_boundary", 145, MaUncertainty("Jurassic_Cretaceous_unc", 0.8));

        // Shared Cenozoic period boundaries
        TIMEInstant paleogeneEnd = GeoBoundary("Paleogene_Neogene_boundary", 23.03);
        TIMEInstant neogeneEnd = GeoBoundary("Neogene_Quaternary_boundary", 2.58);

        // Shared Jurassic epoch boundaries
        TIMEInstant earlyJurassicEnd = GeoBoundary("EarlyJurassic_MiddleJurassic_boundary", 174.7, MaUncertainty("EarlyJurassic_MiddleJurassic_unc", 1.0));
        TIMEInstant middleJurassicEnd = GeoBoundary("MiddleJurassic_LateJurassic_boundary", 161.5, MaUncertainty("MiddleJurassic_LateJurassic_unc", 1.0));

        // Shared Cretaceous epoch boundaries
        TIMEInstant earlyCretaceousEnd = GeoBoundary("EarlyCretaceous_LateCretaceous_boundary", 100.5, MaUncertainty("EarlyCretaceous_LateCretaceous_unc", 0.9));

        // Shared Paleogene epoch boundaries
        TIMEInstant paleoceneEnd = GeoBoundary("Paleocene_Eocene_boundary", 56, MaUncertainty("Paleocene_Eocene_unc", 0.05));
        TIMEInstant eoceneEnd = GeoBoundary("Eocene_Oligocene_boundary", 33.9, MaUncertainty("Eocene_Oligocene_unc", 0.1));

        // Shared Neogene epoch boundaries
        TIMEInstant mioceneEnd = GeoBoundary("Miocene_Pliocene_boundary", 5.333);

        // Shared Quaternary epoch boundaries
        TIMEInstant pleistoceneEnd = GeoBoundary("Pleistocene_Holocene_boundary", 0.0117);

        // === EON: Phanerozoic ===
        ics.DeclareEra(Phanerozoic, phanerozoicBegin, phanerozoicEnd);

        // === ERAS (shared boundaries between adjacent eras) ===
        ics.DeclareEra(Paleozoic, phanerozoicBegin, paleozoicEnd);
        ics.DeclareEra(Mesozoic, paleozoicEnd, mesozoicEnd);
        ics.DeclareEra(Cenozoic, mesozoicEnd, phanerozoicEnd);
        ics.DeclareSubEra(Paleozoic, Phanerozoic);
        ics.DeclareSubEra(Mesozoic, Phanerozoic);
        ics.DeclareSubEra(Cenozoic, Phanerozoic);

        // === PERIODS (Paleozoic — shared boundaries) ===
        ics.DeclareEra(Cambrian, phanerozoicBegin, cambrianEnd);
        ics.DeclareEra(Ordovician, cambrianEnd, ordovicianEnd);
        ics.DeclareEra(Silurian, ordovicianEnd, silurianEnd);
        ics.DeclareEra(Devonian, silurianEnd, devonianEnd);
        ics.DeclareEra(Carboniferous, devonianEnd, carboniferousEnd);
        ics.DeclareEra(Permian, carboniferousEnd, paleozoicEnd);
        ics.DeclareSubEra(Cambrian, Paleozoic);
        ics.DeclareSubEra(Ordovician, Paleozoic);
        ics.DeclareSubEra(Silurian, Paleozoic);
        ics.DeclareSubEra(Devonian, Paleozoic);
        ics.DeclareSubEra(Carboniferous, Paleozoic);
        ics.DeclareSubEra(Permian, Paleozoic);

        // === PERIODS (Mesozoic — shared boundaries) ===
        ics.DeclareEra(Triassic, paleozoicEnd, triassicEnd);
        ics.DeclareEra(Jurassic, triassicEnd, jurassicEnd);
        ics.DeclareEra(Cretaceous, jurassicEnd, mesozoicEnd);
        ics.DeclareSubEra(Triassic, Mesozoic);
        ics.DeclareSubEra(Jurassic, Mesozoic);
        ics.DeclareSubEra(Cretaceous, Mesozoic);

        // === PERIODS (Cenozoic — shared boundaries) ===
        ics.DeclareEra(Paleogene, mesozoicEnd, paleogeneEnd);
        ics.DeclareEra(Neogene, paleogeneEnd, neogeneEnd);
        ics.DeclareEra(Quaternary, neogeneEnd, phanerozoicEnd);
        ics.DeclareSubEra(Paleogene, Cenozoic);
        ics.DeclareSubEra(Neogene, Cenozoic);
        ics.DeclareSubEra(Quaternary, Cenozoic);

        // === EPOCHS (Jurassic — shared boundaries) ===
        ics.DeclareEra(EarlyJurassic, triassicEnd, earlyJurassicEnd);
        ics.DeclareEra(MiddleJurassic, earlyJurassicEnd, middleJurassicEnd);
        ics.DeclareEra(LateJurassic, middleJurassicEnd, jurassicEnd);
        ics.DeclareSubEra(EarlyJurassic, Jurassic);
        ics.DeclareSubEra(MiddleJurassic, Jurassic);
        ics.DeclareSubEra(LateJurassic, Jurassic);

        // === EPOCHS (Cretaceous) ===
        ics.DeclareEra(EarlyCretaceous, jurassicEnd, earlyCretaceousEnd);
        ics.DeclareEra(LateCretaceous, earlyCretaceousEnd, mesozoicEnd);
        ics.DeclareSubEra(EarlyCretaceous, Cretaceous);
        ics.DeclareSubEra(LateCretaceous, Cretaceous);

        // === EPOCHS (Paleogene) ===
        ics.DeclareEra(Paleocene, mesozoicEnd, paleoceneEnd);
        ics.DeclareEra(Eocene, paleoceneEnd, eoceneEnd);
        ics.DeclareEra(Oligocene, eoceneEnd, paleogeneEnd);
        ics.DeclareSubEra(Paleocene, Paleogene);
        ics.DeclareSubEra(Eocene, Paleogene);
        ics.DeclareSubEra(Oligocene, Paleogene);

        // === EPOCHS (Neogene) ===
        ics.DeclareEra(Miocene, paleogeneEnd, mioceneEnd);
        ics.DeclareEra(Pliocene, mioceneEnd, neogeneEnd);
        ics.DeclareSubEra(Miocene, Neogene);
        ics.DeclareSubEra(Pliocene, Neogene);

        // === EPOCHS (Quaternary) ===
        ics.DeclareEra(Pleistocene, neogeneEnd, pleistoceneEnd);
        ics.DeclareEra(Holocene, pleistoceneEnd, phanerozoicEnd);
        ics.DeclareSubEra(Pleistocene, Quaternary);
        ics.DeclareSubEra(Holocene, Quaternary);

        // === REFERENCE POINTS: Big Five mass extinction events ===
        ics.DeclareReferencePoints(new[]
        {
            GeoBoundary("EndOrdovicianExtinction", 443.8, MaUncertainty("EndOrdovician_unc", 1.5)),
            GeoBoundary("LateDevonianExtinction", 372, MaUncertainty("LateDevonian_unc", 2)),
            GeoBoundary("EndPermianExtinction", 251.902, MaUncertainty("EndPermian_unc", 0.024)),
            GeoBoundary("EndTriassicExtinction", 201.4, MaUncertainty("EndTriassic_unc", 0.2)),
            GeoBoundary("EndCretaceousExtinction", 66, MaUncertainty("KPg_extinction_unc", 0.05))
        });

        _ics = ics;
        return _ics;
    }
    #endregion

    #region Showcase Tests — Ontological layer

    [TestMethod]
    public void ShouldDeclareAllErasInICSChart()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();
        Assert.IsTrue(ics.CheckHasEra(Phanerozoic));
        Assert.IsTrue(ics.CheckHasEra(Paleozoic));
        Assert.IsTrue(ics.CheckHasEra(Mesozoic));
        Assert.IsTrue(ics.CheckHasEra(Cenozoic));
        Assert.IsTrue(ics.CheckHasEra(Cambrian));
        Assert.IsTrue(ics.CheckHasEra(Ordovician));
        Assert.IsTrue(ics.CheckHasEra(Silurian));
        Assert.IsTrue(ics.CheckHasEra(Devonian));
        Assert.IsTrue(ics.CheckHasEra(Carboniferous));
        Assert.IsTrue(ics.CheckHasEra(Permian));
        Assert.IsTrue(ics.CheckHasEra(Triassic));
        Assert.IsTrue(ics.CheckHasEra(Jurassic));
        Assert.IsTrue(ics.CheckHasEra(Cretaceous));
        Assert.IsTrue(ics.CheckHasEra(Paleogene));
        Assert.IsTrue(ics.CheckHasEra(Neogene));
        Assert.IsTrue(ics.CheckHasEra(Quaternary));
        Assert.IsTrue(ics.CheckHasEra(EarlyJurassic));
        Assert.IsTrue(ics.CheckHasEra(MiddleJurassic));
        Assert.IsTrue(ics.CheckHasEra(LateJurassic));
        Assert.IsTrue(ics.CheckHasEra(EarlyCretaceous));
        Assert.IsTrue(ics.CheckHasEra(LateCretaceous));
        Assert.IsTrue(ics.CheckHasEra(Paleocene));
        Assert.IsTrue(ics.CheckHasEra(Eocene));
        Assert.IsTrue(ics.CheckHasEra(Oligocene));
        Assert.IsTrue(ics.CheckHasEra(Miocene));
        Assert.IsTrue(ics.CheckHasEra(Pliocene));
        Assert.IsTrue(ics.CheckHasEra(Pleistocene));
        Assert.IsTrue(ics.CheckHasEra(Holocene));
    }

    [TestMethod]
    public void ShouldDeclareSharedBoundaries()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // K-Pg boundary (66 Ma) is shared between Mesozoic end and Cenozoic begin
        RDFResource kPgBoundary = new RDFResource(ICS + "K_Pg_boundary");
        Assert.IsTrue(ics.CheckHasEraBoundary(kPgBoundary));

        // The same individual appears in both thors:end(Mesozoic, K_Pg) and thors:begin(Cenozoic, K_Pg)
        Assert.IsTrue(ics.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.END),
                new OWLNamedIndividual(Mesozoic),
                new OWLNamedIndividual(kPgBoundary))));
        Assert.IsTrue(ics.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.BEGIN),
                new OWLNamedIndividual(Cenozoic),
                new OWLNamedIndividual(kPgBoundary))));

        // THORS inferences: nextEra(K_Pg, Cenozoic) and previousEra(K_Pg, Mesozoic)
        Assert.IsTrue(ics.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.NEXT_ERA),
                new OWLNamedIndividual(kPgBoundary),
                new OWLNamedIndividual(Cenozoic))));
        Assert.IsTrue(ics.Ontology.CheckHasAssertionAxiom(
            new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.TIME.THORS.PREVIOUS_ERA),
                new OWLNamedIndividual(kPgBoundary),
                new OWLNamedIndividual(Mesozoic))));

        // Shared boundary produces identical coordinates
        (TIMECoordinate _, TIMECoordinate mesozoicEnd) = ics.GetEraCoordinates(Mesozoic);
        (TIMECoordinate cenozoicBegin, TIMECoordinate _) = ics.GetEraCoordinates(Cenozoic);
        Assert.IsNotNull(mesozoicEnd);
        Assert.IsNotNull(cenozoicBegin);
        Assert.IsTrue(mesozoicEnd.Equals(cenozoicBegin));
    }

    [TestMethod]
    public void ShouldDeclareMassExtinctionReferencePoints()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();
        Assert.IsTrue(ics.CheckHasReferencePoint(new RDFResource(ICS + "EndOrdovicianExtinction")));
        Assert.IsTrue(ics.CheckHasReferencePoint(new RDFResource(ICS + "LateDevonianExtinction")));
        Assert.IsTrue(ics.CheckHasReferencePoint(new RDFResource(ICS + "EndPermianExtinction")));
        Assert.IsTrue(ics.CheckHasReferencePoint(new RDFResource(ICS + "EndTriassicExtinction")));
        Assert.IsTrue(ics.CheckHasReferencePoint(new RDFResource(ICS + "EndCretaceousExtinction")));
        Assert.IsFalse(ics.CheckHasReferencePoint(new RDFResource(ICS + "Jurassic_Cretaceous_boundary")));
    }

    [TestMethod]
    public void ShouldModelFourLevelHierarchyWithTransitiveReasoning()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // Direct sub-eras of Phanerozoic: 3 eras
        List<RDFResource> phanerozoicDirectSubs = ics.GetSubErasOf(Phanerozoic, false);
        Assert.HasCount(3, phanerozoicDirectSubs);

        // With reasoning: Phanerozoic contains ALL 27 sub-eras at all levels
        List<RDFResource> phanerozoicAllSubs = ics.GetSubErasOf(Phanerozoic, true);
        Assert.HasCount(27, phanerozoicAllSubs);

        // EarlyJurassic is 3 levels deep — reachable only with reasoning
        Assert.IsTrue(ics.CheckIsSubEraOf(EarlyJurassic, Phanerozoic));
        Assert.IsFalse(ics.CheckIsSubEraOf(EarlyJurassic, Phanerozoic, false));

        // Super-era chain: Holocene → Quaternary → Cenozoic → Phanerozoic
        List<RDFResource> holoceneSuperEras = ics.GetSuperErasOf(Holocene, true);
        Assert.HasCount(3, holoceneSuperEras);
        Assert.IsTrue(holoceneSuperEras.Any(e => e.Equals(Quaternary)));
        Assert.IsTrue(holoceneSuperEras.Any(e => e.Equals(Cenozoic)));
        Assert.IsTrue(holoceneSuperEras.Any(e => e.Equals(Phanerozoic)));
    }

    #endregion

    #region Showcase Tests — Temporal engine

    [TestMethod]
    public void ShouldComputeEraDurationsForMesozoicPeriods()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        TIMEExtent triassicExtent = ics.GetEraExtent(Triassic);
        TIMEExtent jurassicExtent = ics.GetEraExtent(Jurassic);
        TIMEExtent cretaceousExtent = ics.GetEraExtent(Cretaceous);

        Assert.IsNotNull(triassicExtent);
        Assert.IsNotNull(jurassicExtent);
        Assert.IsNotNull(cretaceousExtent);

        // Cretaceous (79 Ma) is the longest Mesozoic period
        Assert.IsTrue(cretaceousExtent.Days > jurassicExtent.Days);
        Assert.IsTrue(cretaceousExtent.Days > triassicExtent.Days);
    }

    [TestMethod]
    public void ShouldVerifySubEpochsSumToParentPeriod()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        TIMEExtent jurassicExtent = ics.GetEraExtent(Jurassic);
        TIMEExtent earlyJ = ics.GetEraExtent(EarlyJurassic);
        TIMEExtent middleJ = ics.GetEraExtent(MiddleJurassic);
        TIMEExtent lateJ = ics.GetEraExtent(LateJurassic);

        double sumDays = (earlyJ.Days ?? 0) + (middleJ.Days ?? 0) + (lateJ.Days ?? 0);
        double parentDays = jurassicExtent.Days ?? 0;

        double ratio = sumDays / parentDays;
        Assert.IsTrue(ratio > 0.99 && ratio < 1.01,
            $"Sub-epoch sum ({sumDays:N0} days) should approximate parent ({parentDays:N0} days), ratio={ratio:F4}");
    }

    [TestMethod]
    public void ShouldProduceLongerDurationForHigherRanks()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        TIMEExtent phanerozoicExtent = ics.GetEraExtent(Phanerozoic);
        TIMEExtent mesozoicExtent = ics.GetEraExtent(Mesozoic);
        TIMEExtent jurassicExtent = ics.GetEraExtent(Jurassic);
        TIMEExtent middleJurassicExtent = ics.GetEraExtent(MiddleJurassic);

        Assert.IsTrue(phanerozoicExtent.Days > mesozoicExtent.Days);
        Assert.IsTrue(mesozoicExtent.Days > jurassicExtent.Days);
        Assert.IsTrue(jurassicExtent.Days > middleJurassicExtent.Days);
    }

    #endregion

    #region Feature Tests — GetEras

    [TestMethod]
    public void ShouldGetAllTopLevelEras()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();
        List<RDFResource> eras = ics.GetEras();

        // All 28 eras are declared as thors:component of this TRS
        Assert.HasCount(28, eras);
        Assert.IsTrue(eras.Any(e => e.Equals(Phanerozoic)));
        Assert.IsTrue(eras.Any(e => e.Equals(Holocene)));
        Assert.IsTrue(eras.Any(e => e.Equals(EarlyJurassic)));
    }

    #endregion

    #region Feature Tests — FindErasAt

    [TestMethod]
    public void ShouldFindErasAtGeologicPosition()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // 150 Ma falls in the Late Jurassic (161.5 → 145 Ma), Jurassic, Mesozoic, Phanerozoic
        List<RDFResource> erasAt150Ma = ics.FindErasAt(150, TIMEPositionReferenceSystem.Geologic);
        Assert.IsTrue(erasAt150Ma.Any(e => e.Equals(Phanerozoic)));
        Assert.IsTrue(erasAt150Ma.Any(e => e.Equals(Mesozoic)));
        Assert.IsTrue(erasAt150Ma.Any(e => e.Equals(Jurassic)));
        Assert.IsTrue(erasAt150Ma.Any(e => e.Equals(LateJurassic)));
        Assert.IsFalse(erasAt150Ma.Any(e => e.Equals(Cenozoic)));
        Assert.IsFalse(erasAt150Ma.Any(e => e.Equals(Cretaceous)));
    }

    [TestMethod]
    public void ShouldFindErasAtRecentPosition()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // 30 Ma falls inside the Oligocene, Paleogene, Cenozoic, Phanerozoic
        List<RDFResource> erasAt30Ma = ics.FindErasAt(30, TIMEPositionReferenceSystem.Geologic);
        Assert.IsTrue(erasAt30Ma.Any(e => e.Equals(Phanerozoic)));
        Assert.IsTrue(erasAt30Ma.Any(e => e.Equals(Cenozoic)));
        Assert.IsTrue(erasAt30Ma.Any(e => e.Equals(Paleogene)));
        Assert.IsTrue(erasAt30Ma.Any(e => e.Equals(Oligocene)));
        Assert.IsFalse(erasAt30Ma.Any(e => e.Equals(Mesozoic)));
    }

    [TestMethod]
    public void ShouldThrowExceptionOnFindErasAtBecauseNullTRS()
        => Assert.ThrowsExactly<OWLException>(() => _ = GetICS().FindErasAt(150, null));

    #endregion

    #region Feature Tests — Chronological ordering

    [TestMethod]
    public void ShouldGetSubErasInChronologicalOrder()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // Mesozoic sub-periods ordered: Triassic (251.9 Ma), Jurassic (201.4 Ma), Cretaceous (145 Ma)
        List<RDFResource> mesozoicPeriods = ics.GetSubErasOf(Mesozoic, false, true);
        Assert.HasCount(3, mesozoicPeriods);
        Assert.IsTrue(mesozoicPeriods[0].Equals(Triassic));
        Assert.IsTrue(mesozoicPeriods[1].Equals(Jurassic));
        Assert.IsTrue(mesozoicPeriods[2].Equals(Cretaceous));

        // Jurassic sub-epochs ordered: Early, Middle, Late
        List<RDFResource> jurassicEpochs = ics.GetSubErasOf(Jurassic, false, true);
        Assert.HasCount(3, jurassicEpochs);
        Assert.IsTrue(jurassicEpochs[0].Equals(EarlyJurassic));
        Assert.IsTrue(jurassicEpochs[1].Equals(MiddleJurassic));
        Assert.IsTrue(jurassicEpochs[2].Equals(LateJurassic));

        // Paleozoic periods ordered: Cambrian through Permian
        List<RDFResource> paleozoicPeriods = ics.GetSubErasOf(Paleozoic, false, true);
        Assert.HasCount(6, paleozoicPeriods);
        Assert.IsTrue(paleozoicPeriods[0].Equals(Cambrian));
        Assert.IsTrue(paleozoicPeriods[5].Equals(Permian));
    }

    #endregion

    #region Feature Tests — GetEraUncertainties

    [TestMethod]
    public void ShouldGetEraUncertainties()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // Phanerozoic: begin has ±0.2 Ma uncertainty, end is 0 Ma (no uncertainty)
        (TIMEIntervalDuration beginUnc, TIMEIntervalDuration endUnc) = ics.GetEraUncertainties(Phanerozoic);
        Assert.IsNotNull(beginUnc);
        Assert.AreEqual(200_000, beginUnc.Value); // 0.2 Ma = 200,000 years
        Assert.IsNull(endUnc); // 0 Ma has no uncertainty

        // Jurassic: begin ±0.2 Ma, end ±0.8 Ma
        (TIMEIntervalDuration jBeginUnc, TIMEIntervalDuration jEndUnc) = ics.GetEraUncertainties(Jurassic);
        Assert.IsNotNull(jBeginUnc);
        Assert.IsNotNull(jEndUnc);
        Assert.AreEqual(200_000, jBeginUnc.Value);
        Assert.AreEqual(800_000, jEndUnc.Value);
    }

    [TestMethod]
    public void ShouldThrowExceptionOnGetEraUncertaintiesBecauseNullEra()
        => Assert.ThrowsExactly<OWLException>(() => _ = GetICS().GetEraUncertainties(null));

    [TestMethod]
    public void ShouldThrowExceptionOnGetEraUncertaintiesBecauseUnknownEra()
        => Assert.ThrowsExactly<OWLException>(() => _ = GetICS().GetEraUncertainties(new RDFResource("ex:unknownEra")));

    #endregion

    #region Feature Tests — Uncertainty-aware FindErasAt

    [TestMethod]
    public void ShouldFindErasAtBoundaryWithUncertainty()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // 66.03 Ma is just outside the exact K-Pg boundary (66 Ma) but within its ±0.05 Ma uncertainty.
        // Without uncertainty: should match Mesozoic (251.9→66 Ma contains 66.03) and Phanerozoic, Cretaceous
        // but NOT Cenozoic (begin=66 Ma, 66.03 > 66 in geological time = older = outside)
        List<TIMEEraMatch> withoutUnc = ics.FindErasAtDetailed(66.03, TIMEPositionReferenceSystem.Geologic, false);
        List<TIMEEraMatch> withUnc = ics.FindErasAtDetailed(66.03, TIMEPositionReferenceSystem.Geologic, true);

        // Without uncertainty: 66.03 Ma is inside Mesozoic/Cretaceous but outside Cenozoic
        Assert.IsTrue(withoutUnc.Any(m => m.Era.Equals(Phanerozoic) && m.IsExact));
        Assert.IsTrue(withoutUnc.Any(m => m.Era.Equals(Mesozoic) && m.IsExact));
        Assert.IsTrue(withoutUnc.Any(m => m.Era.Equals(Cretaceous) && m.IsExact));
        Assert.IsFalse(withoutUnc.Any(m => m.Era.Equals(Cenozoic)));

        // With uncertainty: Cenozoic (begin=66 ±0.05 Ma, effective begin=66.05 Ma) now includes 66.03 Ma
        Assert.IsTrue(withUnc.Count > withoutUnc.Count);
        Assert.IsTrue(withUnc.Any(m => m.Era.Equals(Cenozoic) && !m.IsExact));
    }

    [TestMethod]
    public void ShouldFindErasAtDeepInsideWithExactMatch()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // 150 Ma is deep inside the Late Jurassic — all matches should be exact
        List<TIMEEraMatch> matches = ics.FindErasAtDetailed(150, TIMEPositionReferenceSystem.Geologic, true);
        Assert.IsTrue(matches.All(m => m.IsExact));
        Assert.IsTrue(matches.Any(m => m.Era.Equals(LateJurassic)));
        Assert.IsTrue(matches.Any(m => m.Era.Equals(Jurassic)));
        Assert.IsTrue(matches.Any(m => m.Era.Equals(Mesozoic)));
        Assert.IsTrue(matches.Any(m => m.Era.Equals(Phanerozoic)));
    }

    [TestMethod]
    public void ShouldFindErasAtWithUncertaintyConsistentWithExact()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // Uncertainty-aware search must be a superset of exact search
        List<RDFResource> exact = ics.FindErasAt(30, TIMEPositionReferenceSystem.Geologic, false);
        List<RDFResource> withUnc = ics.FindErasAt(30, TIMEPositionReferenceSystem.Geologic, true);

        // Every exact match must also appear in uncertainty-aware results
        foreach (RDFResource era in exact)
            Assert.IsTrue(withUnc.Any(e => e.Equals(era)));
    }

    [TestMethod]
    public void ShouldFindErasAtDetailedDistinguishesExactFromUncertain()
    {
        TIMEOrdinalReferenceSystem ics = GetICS();

        // Query at 251.92 Ma — just inside the Paleozoic (end=251.902 ±0.024 Ma)
        // This is 0.018 Ma above the boundary, within the ±0.024 Ma uncertainty band of the Mesozoic begin.
        // Without uncertainty: should be in Paleozoic only (not Mesozoic)
        // With uncertainty: should also find Mesozoic as an uncertain match
        List<TIMEEraMatch> detailed = ics.FindErasAtDetailed(251.92, TIMEPositionReferenceSystem.Geologic, true);

        Assert.IsTrue(detailed.Any(m => m.Era.Equals(Paleozoic) && m.IsExact));
        Assert.IsTrue(detailed.Any(m => m.Era.Equals(Phanerozoic) && m.IsExact));
        // Mesozoic begin is 251.902 Ma ±0.024 → effective begin at 251.926 Ma
        // 251.92 < 251.926, so it falls within the uncertainty band
        Assert.IsTrue(detailed.Any(m => m.Era.Equals(Mesozoic) && !m.IsExact));
    }

    [TestMethod]
    public void ShouldThrowExceptionOnFindErasAtDetailedBecauseNullTRS()
        => Assert.ThrowsExactly<OWLException>(() => _ = GetICS().FindErasAtDetailed(150, null));

    #endregion

    #endregion
}