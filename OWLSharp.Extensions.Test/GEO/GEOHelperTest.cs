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
using OWLSharp.Extensions.GEO;
using OWLSharp.Ontology;
using RDFSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWLSharp.Extensions.Test.GEO;

[TestClass]
public class GEOHelperTest
{
    #region Tests (Declarer)
    [TestMethod]
    public void ShouldDeclareGEOPointFeatureWithDefaultGeometry()
    {
        OWLOntology ontology = new OWLOntology();
        GEOPoint geom = new GEOPoint(new RDFResource("ex:MilanGM"), (9.188540, 45.464664));
        ontology.DeclarePointFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), geom);

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>());

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclarePointFeature(null, geom));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclarePointFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), null));
    }

    [TestMethod]
    public void ShouldDeclareGEOPointFeatureWithNotDefaultGeometry()
    {
        OWLOntology ontology = new OWLOntology();
        GEOPoint geom = new GEOPoint(new RDFResource("ex:MilanGM"), (9.188540, 45.464664));
        ontology.DeclarePointFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), geom, false);

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>());
    }

    [TestMethod]
    public void ShouldDeclareGEOLineFeatureWithDefaultGeometry()
    {
        OWLOntology ontology = new OWLOntology();
        GEOLine geom = new GEOLine(new RDFResource("ex:MilanGM"), [(9.188540, 45.464664), (9.198540, 45.474664)]);
        ontology.DeclareLineFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), geom);

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>());

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareLineFeature(null, geom));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareLineFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), null));
    }

    [TestMethod]
    public void ShouldDeclareGEOLineFeatureWithNotDefaultGeometry()
    {
        OWLOntology ontology = new OWLOntology();
        GEOLine geom = new GEOLine(new RDFResource("ex:MilanGM"), [(9.188540, 45.464664), (9.198540, 45.474664)]);
        ontology.DeclareLineFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), geom, false);

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>());
    }

    [TestMethod]
    public void ShouldDeclareGEOAreaFeatureWithDefaultGeometry()
    {
        OWLOntology ontology = new OWLOntology();
        GEOArea geom = new GEOArea(new RDFResource("ex:MilanGM"), [(9.188540, 45.464664), (9.198540, 45.474664), (9.208540, 45.484664)]); //will be automatically closed
        ontology.DeclareAreaFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), geom);

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>());

        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareAreaFeature(null, geom));
        Assert.ThrowsExactly<OWLException>(() => _ = ontology.DeclareAreaFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), null));
    }

    [TestMethod]
    public void ShouldDeclareGEOAreaFeatureWithNotDefaultGeometry()
    {
        OWLOntology ontology = new OWLOntology();
        GEOArea geom = new GEOArea(new RDFResource("ex:MilanGM"), [(9.188540, 45.464664), (9.198540, 45.474664), (9.208540, 45.484664)]); //will be automatically closed
        ontology.DeclareAreaFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), geom, false);

        Assert.HasCount(2, ontology.DeclarationAxioms);
        Assert.HasCount(3, ontology.GetAssertionAxiomsOfType<OWLClassAssertion>());
        Assert.HasCount(1, ontology.GetAssertionAxiomsOfType<OWLObjectPropertyAssertion>());
        Assert.HasCount(2, ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>());
    }
    #endregion

    #region Tests (Analyzer)
    [TestMethod]
    public async Task ShouldThrowExceptionOnGettingSpatialFeatureBecauseNullFeatureUri()
        => await Assert.ThrowsExactlyAsync<OWLException>(async () => await new OWLOntology().GetSpatialFeatureAsync(null));

    [TestMethod]
    public async Task ShouldGetSpatialPointFeatureAsync()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclarePointFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), new GEOPoint(new RDFResource("ex:MilanGM"), (9.188540, 45.464664)));
        List<GEOEntity> geoEntities = await ontology.GetSpatialFeatureAsync(new OWLNamedIndividual(new RDFResource("ex:MilanFT")));

        Assert.IsNotNull(geoEntities);
        Assert.IsTrue(geoEntities.Single() is GEOPoint geoPoint
                      && geoPoint.URI.Equals(new Uri("ex:MilanGM"))
                      && string.Equals(geoPoint.ToWKT(), "POINT (9.18854 45.464664)"));
        Assert.IsEmpty(await ontology.GetSpatialFeatureAsync(new OWLNamedIndividual(new RDFResource("ex:MilanGGGGFT"))));
    }

    [TestMethod]
    public async Task ShouldGetSpatialLineFeatureAsync()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareLineFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), new GEOLine(new RDFResource("ex:MilanGM"), [(9.188540, 45.464664), (9.198540, 45.474664)]));
        List<GEOEntity> geoEntities = await ontology.GetSpatialFeatureAsync(new OWLNamedIndividual(new RDFResource("ex:MilanFT")));

        Assert.IsNotNull(geoEntities);
        Assert.IsTrue(geoEntities.Single() is GEOLine geoLine
                      && geoLine.URI.Equals(new Uri("ex:MilanGM"))
                      && string.Equals(geoLine.ToWKT(), "LINESTRING (9.18854 45.464664, 9.19854 45.474664)"));
    }

    [TestMethod]
    public async Task ShouldGetSpatialAreaFeatureAsync()
    {
        OWLOntology ontology = new OWLOntology();
        ontology.DeclareAreaFeature(new OWLNamedIndividual(new RDFResource("ex:MilanFT")), new GEOArea(new RDFResource("ex:MilanGM"), [(9.188540, 45.464664), (9.198540, 45.474664), (9.208540, 45.484664)]));
        List<GEOEntity> geoEntities = await ontology.GetSpatialFeatureAsync(new OWLNamedIndividual(new RDFResource("ex:MilanFT")));

        Assert.IsNotNull(geoEntities);
        Assert.IsTrue(geoEntities.Single() is GEOArea geoArea
                      && geoArea.URI.Equals(new Uri("ex:MilanGM"))
                      && string.Equals(geoArea.ToWKT(), "POLYGON ((9.18854 45.464664, 9.19854 45.474664, 9.20854 45.484664, 9.18854 45.464664))"));
    }
    #endregion

    #region Tests (Analyzer:Distance)
    [TestMethod]
    public async Task ShouldGetDistanceBetweenFeaturesAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("""<gml:Point xmlns:gml="http://www.opengis.net/gml/3.2"><gml:pos>9.19193456 45.46420722</gml:pos></gml:Point>""", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("""<gml:Point xmlns:gml="http://www.opengis.net/gml/3.2"><gml:pos>12.49221871 41.89033014</gml:pos></gml:Point>""", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)))
            ]
        };
        double? milanRomeDistance = await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), new OWLNamedIndividual(new RDFResource("ex:romeFT")));

        Assert.IsTrue(milanRomeDistance is >= 450000 and <= 4800000); //milan-rome should be between 450km and 480km

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), new OWLNamedIndividual(new RDFResource("ex:romeFT"))));
        Assert.IsNull(await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), new OWLNamedIndividual(new RDFResource("ex:romeFT2"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async() => await GEOHelper.GetDistanceBetweenFeaturesAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), new OWLNamedIndividual(new RDFResource("ex:romeFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            null, new OWLNamedIndividual(new RDFResource("ex:romeFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), null as OWLNamedIndividual));
    }

    [TestMethod]
    public async Task ShouldGetDistanceBetweenFeatureAndLiteralAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("""<gml:Point xmlns:gml="http://www.opengis.net/gml/3.2"><gml:pos>9.19193456 45.46420722</gml:pos></gml:Point>""", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)))
            ]
        };
        double? milanRomeDistance = await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsTrue(milanRomeDistance is >= 450000 and <= 4800000); //milan-rome should be between 450km and 480km

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            null, new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), null as OWLLiteral));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    [TestMethod]
    public async Task ShouldGetDistanceBetweenLiteralsAsync()
    {
        double? milanRomeDistance = await GEOHelper.GetDistanceBetweenFeaturesAsync(
            new OWLLiteral(new RDFTypedLiteral("""<gml:Point xmlns:gml="http://www.opengis.net/gml/3.2"><gml:pos>9.19193456 45.46420722</gml:pos></gml:Point>""", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)),
            new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsTrue(milanRomeDistance is >= 450000 and <= 4800000); //milan-rome should be between 450km and 480km

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(
            null, new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING)),
            new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(
            new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)),
            null));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetDistanceBetweenFeaturesAsync(
            new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)),
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }
    #endregion

    #region Tests (Analyzer:Measure)
    [TestMethod]
    public async Task ShouldGetLengthOfFeatureAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT")),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        double? milanLength = await GEOHelper.GetLengthOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")));
        double? brebemiLength = await GEOHelper.GetLengthOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:brebemiFT")));

        Assert.IsTrue(milanLength is >= 3000 and <= 3300); //Perimeter of milan is about 3KM
        Assert.IsTrue(brebemiLength is >= 90000 and <= 95000); //BreBeMi simplified path is about 90-95KM

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetLengthOfFeatureAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetLengthOfFeatureAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetLengthOfFeatureAsync(geoOntology,
            null));
    }

    [TestMethod]
    public async Task ShouldGetLengthOfLiteralAsync()
    {
        double? milanLength = await GEOHelper.GetLengthOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        double ? brebemiLength = await GEOHelper.GetLengthOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsTrue(milanLength is >= 3000 and <= 3300); //Perimeter of milan is about 3KM
        Assert.IsTrue(brebemiLength is >= 90000 and <= 95000); //BreBeMi simplified path is about 90-95KM

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetLengthOfFeatureAsync(
            null));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetLengthOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    [TestMethod]
    public async Task ShouldGetAreaOfFeatureAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT")),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        double? milanArea = await GEOHelper.GetAreaOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")));
        double? brebemiArea = await GEOHelper.GetAreaOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:brebemiFT")));

        Assert.IsTrue(milanArea is >= 590000 and <= 600000);
        Assert.AreEqual(0, brebemiArea);  //lines have no area

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetAreaOfFeatureAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetAreaOfFeatureAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetAreaOfFeatureAsync(geoOntology,
            null));
    }

    [TestMethod]
    public async Task ShouldGetAreaOfLiteralAsync()
    {
        double? milanArea = await GEOHelper.GetAreaOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        double? brebemiArea = await GEOHelper.GetAreaOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsTrue(milanArea is >= 590000 and <= 600000);
        Assert.AreEqual(0, brebemiArea);  //lines have no area

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetAreaOfFeatureAsync(
            null));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetAreaOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }
    #endregion

    #region Tests (Analyzer:Centroid)
    [TestMethod]
    public async Task ShouldGetCentroidOfFeatureAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT")),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        OWLLiteral milanCentroid = await GEOHelper.GetCentroidOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")));
        OWLLiteral brebemiCentroid = await GEOHelper.GetCentroidOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:brebemiFT")));

        Assert.IsNotNull(milanCentroid);
        Assert.IsTrue(milanCentroid.GetLiteral().Equals(new RDFTypedLiteral("POINT (9.1863596 45.46411504)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiCentroid);
        Assert.IsTrue(brebemiCentroid.GetLiteral().Equals(new RDFTypedLiteral("POINT (9.67171402 45.59539507)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetCentroidOfFeatureAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetCentroidOfFeatureAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetCentroidOfFeatureAsync(geoOntology,
            null));
    }

    [TestMethod]
    public async Task ShouldGetCentroidOfLiteralAsync()
    {
        OWLLiteral milanCentroid = await GEOHelper.GetCentroidOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        OWLLiteral brebemiCentroid = await GEOHelper.GetCentroidOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(milanCentroid);
        Assert.IsTrue(milanCentroid.GetLiteral().Equals(new RDFTypedLiteral("POINT (9.1863596 45.46411504)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiCentroid);
        Assert.IsTrue(brebemiCentroid.GetLiteral().Equals(new RDFTypedLiteral("POINT (9.67171402 45.59539507)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetCentroidOfFeatureAsync(
            null));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetCentroidOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }
    #endregion

    #region Tests (Analyzer:Boundary)
    [TestMethod]
    public async Task ShouldGetBoundaryOfFeatureAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT")),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        OWLLiteral milanBoundary = await GEOHelper.GetBoundaryOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")));
        OWLLiteral brebemiBoundary = await GEOHelper.GetBoundaryOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:brebemiFT")));

        Assert.IsNotNull(milanBoundary);
        Assert.IsTrue(milanBoundary.GetLiteral().Equals(new RDFTypedLiteral("LINESTRING (9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiBoundary);
        Assert.IsTrue(brebemiBoundary.GetLiteral().Equals(new RDFTypedLiteral("MULTIPOINT ((9.16778508 45.46481222), (10.21423284 45.54758259))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetBoundaryOfFeatureAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBoundaryOfFeatureAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBoundaryOfFeatureAsync(geoOntology,
            null));
    }

    [TestMethod]
    public async Task ShouldGetBoundaryOfLiteralAsync()
    {
        OWLLiteral milanBoundary = await GEOHelper.GetBoundaryOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        OWLLiteral brebemiBoundary = await GEOHelper.GetBoundaryOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(milanBoundary);
        Assert.IsTrue(milanBoundary.GetLiteral().Equals(new RDFTypedLiteral("LINESTRING (9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiBoundary);
        Assert.IsTrue(brebemiBoundary.GetLiteral().Equals(new RDFTypedLiteral("MULTIPOINT ((9.16778508 45.46481222), (10.21423284 45.54758259))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBoundaryOfFeatureAsync(
            null));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBoundaryOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }
    #endregion

    #region Tests (Analyzer:Buffer)
    [TestMethod]
    public async Task ShouldGetBufferAroundFeatureAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT")),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        OWLLiteral milanBuffer = await GEOHelper.GetBufferAroundFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), 5000);
        OWLLiteral brebemiBuffer = await GEOHelper.GetBufferAroundFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:brebemiFT")), 5000);

        Assert.IsNotNull(milanBuffer);
        Assert.IsTrue(milanBuffer.Value.StartsWith("POLYGON"));
        Assert.IsNotNull(brebemiBuffer);
        Assert.IsTrue(brebemiBuffer.Value.StartsWith("POLYGON"));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetBufferAroundFeatureAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), 5000));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBufferAroundFeatureAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), 5000));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBufferAroundFeatureAsync(geoOntology,
            null, 5000));
    }

    [TestMethod]
    public async Task ShouldGetBufferAroundLiteralAsync()
    {
        OWLLiteral milanBuffer = await GEOHelper.GetBufferAroundFeatureAsync(new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)), 5000);
        OWLLiteral brebemiBuffer = await GEOHelper.GetBufferAroundFeatureAsync(new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)), 5000);

        Assert.IsNotNull(milanBuffer);
        Assert.IsTrue(milanBuffer.Value.StartsWith("POLYGON"));
        Assert.IsNotNull(brebemiBuffer);
        Assert.IsTrue(brebemiBuffer.Value.StartsWith("POLYGON"));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBufferAroundFeatureAsync(
            null, 5000));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetBufferAroundFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING)), 5000));
    }
    #endregion

    #region Tests (Analyzer:ConvexHull)
    [TestMethod]
    public async Task ShouldGetConvexHullOfFeatureAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT")),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        OWLLiteral milanConvexHull = await GEOHelper.GetConvexHullOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")));
        OWLLiteral brebemiConvexHull = await GEOHelper.GetConvexHullOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:brebemiFT")));

        Assert.IsNotNull(milanConvexHull);
        Assert.IsTrue(milanConvexHull.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.18217536 45.46003666, 9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiConvexHull);
        Assert.IsTrue(brebemiConvexHull.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259, 9.16778508 45.46481222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetConvexHullOfFeatureAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2"))));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetConvexHullOfFeatureAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetConvexHullOfFeatureAsync(geoOntology,
            null));
    }

    [TestMethod]
    public async Task ShouldGetConvexHullOfLiteralAsync()
    {
        OWLLiteral milanConvexHull = await GEOHelper.GetConvexHullOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        OWLLiteral brebemiConvexHull = await GEOHelper.GetConvexHullOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(milanConvexHull);
        Assert.IsTrue(milanConvexHull.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.18217536 45.46003666, 9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiConvexHull);
        Assert.IsTrue(brebemiConvexHull.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259, 9.16778508 45.46481222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetConvexHullOfFeatureAsync(
            null));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetConvexHullOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }
    #endregion

    #region Tests (Analyzer:Envelope)
    [TestMethod]
    public async Task ShouldGetEnvelopeOfFeatureAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:brebemiGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiFT")),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:brebemiGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        OWLLiteral milanEnvelope = await GEOHelper.GetEnvelopeOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")));
        OWLLiteral brebemiEnvelope = await GEOHelper.GetEnvelopeOfFeatureAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:brebemiFT")));

        Assert.IsNotNull(milanEnvelope);
        Assert.IsTrue(milanEnvelope.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.18217536 45.46003666, 9.18217476 45.46819347, 9.19054445 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiEnvelope);
        Assert.IsTrue(brebemiEnvelope.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.16778508 45.46481222, 9.16583437 45.6790123, 10.21547874 45.67891916, 10.21345085 45.46471951, 9.16778508 45.46481222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetEnvelopeOfFeatureAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2"))));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetEnvelopeOfFeatureAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetEnvelopeOfFeatureAsync(geoOntology,
            null));
    }

    [TestMethod]
    public async Task ShouldGetEnvelopeOfLiteralAsync()
    {
        OWLLiteral milanEnvelope = await GEOHelper.GetEnvelopeOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("POLYGON((9.18217536 45.46819347, 9.19054385 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666, 9.18217536 45.46819347))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        OWLLiteral brebemiEnvelope = await GEOHelper.GetEnvelopeOfFeatureAsync(new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.16778508 45.46481222, 9.6118352 45.68014585, 10.21423284 45.54758259)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(milanEnvelope);
        Assert.IsTrue(milanEnvelope.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.18217536 45.46003666, 9.18217476 45.46819347, 9.19054445 45.46819347, 9.19054385 45.46003666, 9.18217536 45.46003666))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));
        Assert.IsNotNull(brebemiEnvelope);
        Assert.IsTrue(brebemiEnvelope.GetLiteral().Equals(new RDFTypedLiteral("POLYGON ((9.16778508 45.46481222, 9.16583437 45.6790123, 10.21547874 45.67891916, 10.21345085 45.46471951, 9.16778508 45.46481222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetEnvelopeOfFeatureAsync(
            null));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetEnvelopeOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }
    #endregion

    #region Tests (Analyzer:NearBy)
    [TestMethod]
    public async Task ShouldGetFeaturesNearByAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanProximityFeatures = await GEOHelper.GetFeaturesNearBy(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), 480000);
        List<OWLNamedIndividual> romeProximityFeatures = await GEOHelper.GetFeaturesNearBy(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), 100000);

        Assert.IsNotNull(milanProximityFeatures);
        Assert.IsTrue(milanProximityFeatures.Single().GetIRI().URI.Equals(new Uri("ex:romeFT")));
        Assert.IsNotNull(romeProximityFeatures);
        Assert.IsTrue(romeProximityFeatures.Single().GetIRI().URI.Equals(new Uri("ex:tivoliFT")));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesNearBy(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), 20000));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesNearBy(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), 20000));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesNearBy(geoOntology,
            null as OWLNamedIndividual, 20000));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesNearByLiteralAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> proximityFeatures = await GEOHelper.GetFeaturesNearBy(geoOntology, new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)), 100000);

        Assert.IsNotNull(proximityFeatures);
        Assert.IsTrue(proximityFeatures.Single().GetIRI().URI.Equals(new Uri("ex:romeFT")));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesNearBy(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), 20000));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesNearBy(null,
            new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)), 20000));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesNearBy(geoOntology,
            null as OWLLiteral, 20000));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesNearBy(geoOntology,
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING)), 20000));
    }
    #endregion

    #region Tests (Analyzer:Direction)
    [TestMethod]
    public async Task ShouldGetFeaturesNorthDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionNorthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.North);
        List<OWLNamedIndividual> romeDirectionNorthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.North);
        List<OWLNamedIndividual> tivoliDirectionNorthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.North);

        Assert.IsNotNull(milanDirectionNorthFeatures);
        Assert.IsEmpty(milanDirectionNorthFeatures);
        Assert.IsNotNull(romeDirectionNorthFeatures);
        Assert.HasCount(2, romeDirectionNorthFeatures);
        Assert.IsNotNull(tivoliDirectionNorthFeatures);
        Assert.HasCount(1, tivoliDirectionNorthFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.North));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.North));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.North));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesNorthEastDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionNorthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.NorthEast);
        List<OWLNamedIndividual> romeDirectionNorthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.NorthEast);
        List<OWLNamedIndividual> tivoliDirectionNorthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.NorthEast);

        Assert.IsNotNull(milanDirectionNorthEastFeatures);
        Assert.IsEmpty(milanDirectionNorthEastFeatures);
        Assert.IsNotNull(romeDirectionNorthEastFeatures);
        Assert.HasCount(1, romeDirectionNorthEastFeatures);
        Assert.IsNotNull(tivoliDirectionNorthEastFeatures);
        Assert.IsEmpty(tivoliDirectionNorthEastFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.NorthEast));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.NorthEast));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.NorthEast));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesNorthWestDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionNorthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.NorthWest);
        List<OWLNamedIndividual> romeDirectionNorthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.NorthWest);
        List<OWLNamedIndividual> tivoliDirectionNorthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.NorthWest);

        Assert.IsNotNull(milanDirectionNorthWestFeatures);
        Assert.IsEmpty(milanDirectionNorthWestFeatures);
        Assert.IsNotNull(romeDirectionNorthWestFeatures);
        Assert.HasCount(1, romeDirectionNorthWestFeatures);
        Assert.IsNotNull(tivoliDirectionNorthWestFeatures);
        Assert.HasCount(1, tivoliDirectionNorthWestFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.NorthWest));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.NorthWest));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.NorthWest));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesEastDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.East);
        List<OWLNamedIndividual> romeDirectionEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.East);
        List<OWLNamedIndividual> tivoliDirectionEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.East);

        Assert.IsNotNull(milanDirectionEastFeatures);
        Assert.HasCount(2, milanDirectionEastFeatures);
        Assert.IsNotNull(romeDirectionEastFeatures);
        Assert.HasCount(1, romeDirectionEastFeatures);
        Assert.IsNotNull(tivoliDirectionEastFeatures);
        Assert.IsEmpty(tivoliDirectionEastFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.East));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.East));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.East));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesSouthEastDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionSouthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.SouthEast);
        List<OWLNamedIndividual> romeDirectionSouthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.SouthEast);
        List<OWLNamedIndividual> tivoliDirectionSouthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.SouthEast);

        Assert.IsNotNull(milanDirectionSouthEastFeatures);
        Assert.HasCount(2, milanDirectionSouthEastFeatures);
        Assert.IsNotNull(romeDirectionSouthEastFeatures);
        Assert.IsEmpty(romeDirectionSouthEastFeatures);
        Assert.IsNotNull(tivoliDirectionSouthEastFeatures);
        Assert.IsEmpty(tivoliDirectionSouthEastFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.SouthEast));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.SouthEast));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.East));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesWestDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.West);
        List<OWLNamedIndividual> romeDirectionWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.West);
        List<OWLNamedIndividual> tivoliDirectionWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.West);

        Assert.IsNotNull(milanDirectionWestFeatures);
        Assert.IsEmpty(milanDirectionWestFeatures);
        Assert.IsNotNull(romeDirectionWestFeatures);
        Assert.HasCount(1, romeDirectionWestFeatures);
        Assert.IsNotNull(tivoliDirectionWestFeatures);
        Assert.HasCount(2, tivoliDirectionWestFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.West));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.West));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.West));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesSouthWestDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionSouthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.SouthWest);
        List<OWLNamedIndividual> romeDirectionSouthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.SouthWest);
        List<OWLNamedIndividual> tivoliDirectionSouthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.SouthWest);

        Assert.IsNotNull(milanDirectionSouthWestFeatures);
        Assert.IsEmpty(milanDirectionSouthWestFeatures);
        Assert.IsNotNull(romeDirectionSouthWestFeatures);
        Assert.IsEmpty(romeDirectionSouthWestFeatures);
        Assert.IsNotNull(tivoliDirectionSouthWestFeatures);
        Assert.HasCount(1, tivoliDirectionSouthWestFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.SouthWest));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.SouthWest));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.SouthWest));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesSouthDirectionOfAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> milanDirectionSouthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.South);
        List<OWLNamedIndividual> romeDirectionSouthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:romeFT")), GEOEnums.GeoDirections.South);
        List<OWLNamedIndividual> tivoliDirectionSouthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:tivoliFT")), GEOEnums.GeoDirections.South);

        Assert.IsNotNull(milanDirectionSouthFeatures);
        Assert.HasCount(2, milanDirectionSouthFeatures);
        Assert.IsNotNull(romeDirectionSouthFeatures);
        Assert.IsEmpty(romeDirectionSouthFeatures);
        Assert.IsNotNull(tivoliDirectionSouthFeatures);
        Assert.HasCount(1, tivoliDirectionSouthFeatures);

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:milanFT2")), GEOEnums.GeoDirections.South));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:milanFT")), GEOEnums.GeoDirections.South));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLNamedIndividual, GEOEnums.GeoDirections.South));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesNorthDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionNorthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.North);
        List<OWLNamedIndividual> romeDirectionNorthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.North);
        List<OWLNamedIndividual> tivoliDirectionNorthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.North);

        Assert.IsNotNull(milanDirectionNorthFeatures);
        Assert.IsEmpty(milanDirectionNorthFeatures);
        Assert.IsNotNull(romeDirectionNorthFeatures);
        Assert.HasCount(2, romeDirectionNorthFeatures);
        Assert.IsNotNull(tivoliDirectionNorthFeatures);
        Assert.HasCount(1, tivoliDirectionNorthFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.North));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.North));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesNorthEastDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionNorthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.NorthEast);
        List<OWLNamedIndividual> romeDirectionNorthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.NorthEast);
        List<OWLNamedIndividual> tivoliDirectionNorthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.NorthEast);

        Assert.IsNotNull(milanDirectionNorthEastFeatures);
        Assert.IsEmpty(milanDirectionNorthEastFeatures);
        Assert.IsNotNull(romeDirectionNorthEastFeatures);
        Assert.HasCount(1, romeDirectionNorthEastFeatures);
        Assert.IsNotNull(tivoliDirectionNorthEastFeatures);
        Assert.IsEmpty(tivoliDirectionNorthEastFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.NorthEast));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.NorthEast));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesNorthWestDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionNorthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.NorthWest);
        List<OWLNamedIndividual> romeDirectionNorthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.NorthWest);
        List<OWLNamedIndividual> tivoliDirectionNorthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.NorthWest);

        Assert.IsNotNull(milanDirectionNorthWestFeatures);
        Assert.IsEmpty(milanDirectionNorthWestFeatures);
        Assert.IsNotNull(romeDirectionNorthWestFeatures);
        Assert.HasCount(1, romeDirectionNorthWestFeatures);
        Assert.IsNotNull(tivoliDirectionNorthWestFeatures);
        Assert.HasCount(1, tivoliDirectionNorthWestFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.NorthWest));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.NorthWest));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesEastDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.East);
        List<OWLNamedIndividual> romeDirectionEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.East);
        List<OWLNamedIndividual> tivoliDirectionEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.East);

        Assert.IsNotNull(milanDirectionEastFeatures);
        Assert.HasCount(2, milanDirectionEastFeatures);
        Assert.IsNotNull(romeDirectionEastFeatures);
        Assert.HasCount(1, romeDirectionEastFeatures);
        Assert.IsNotNull(tivoliDirectionEastFeatures);
        Assert.IsEmpty(tivoliDirectionEastFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.East));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.East));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesSouthEastDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionSouthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.SouthEast);
        List<OWLNamedIndividual> romeDirectionSouthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.SouthEast);
        List<OWLNamedIndividual> tivoliDirectionSouthEastFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.SouthEast);

        Assert.IsNotNull(milanDirectionSouthEastFeatures);
        Assert.HasCount(2, milanDirectionSouthEastFeatures);
        Assert.IsNotNull(romeDirectionSouthEastFeatures);
        Assert.IsEmpty(romeDirectionSouthEastFeatures);
        Assert.IsNotNull(tivoliDirectionSouthEastFeatures);
        Assert.IsEmpty(tivoliDirectionSouthEastFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.SouthEast));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.SouthEast));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesWestDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.West);
        List<OWLNamedIndividual> romeDirectionWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.West);
        List<OWLNamedIndividual> tivoliDirectionWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.West);

        Assert.IsNotNull(milanDirectionWestFeatures);
        Assert.IsEmpty(milanDirectionWestFeatures);
        Assert.IsNotNull(romeDirectionWestFeatures);
        Assert.HasCount(1, romeDirectionWestFeatures);
        Assert.IsNotNull(tivoliDirectionWestFeatures);
        Assert.HasCount(2, tivoliDirectionWestFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.West));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.West));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesSouthWestDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionSouthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.SouthWest);
        List<OWLNamedIndividual> romeDirectionSouthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.SouthWest);
        List<OWLNamedIndividual> tivoliDirectionSouthWestFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.SouthWest);

        Assert.IsNotNull(milanDirectionSouthWestFeatures);
        Assert.IsEmpty(milanDirectionSouthWestFeatures);
        Assert.IsNotNull(romeDirectionSouthWestFeatures);
        Assert.IsEmpty(romeDirectionSouthWestFeatures);
        Assert.IsNotNull(tivoliDirectionSouthWestFeatures);
        Assert.HasCount(1, tivoliDirectionSouthWestFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.SouthWest));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.SouthWest));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesSouthDirectionOfLiteralAsync()
    {
        OWLLiteral milanTL = new OWLLiteral(new RDFTypedLiteral("POINT(9.188540 45.464664)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral romeTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.496365 41.902782)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLLiteral tivoliTL = new OWLLiteral(new RDFTypedLiteral("POINT(12.79938661 41.96217718)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:tivoliGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:milanFT")),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:romeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliFT")),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:milanGM")),
                    milanTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:romeGM")),
                    romeTL),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:tivoliGM")),
                    tivoliTL)
            ]
        };
        List<OWLNamedIndividual> milanDirectionSouthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, milanTL, GEOEnums.GeoDirections.South);
        List<OWLNamedIndividual> romeDirectionSouthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, romeTL, GEOEnums.GeoDirections.South);
        List<OWLNamedIndividual> tivoliDirectionSouthFeatures = await GEOHelper.GetFeaturesDirectionAsync(geoOntology, tivoliTL, GEOEnums.GeoDirections.South);

        Assert.IsNotNull(milanDirectionSouthFeatures);
        Assert.HasCount(2, milanDirectionSouthFeatures);
        Assert.IsNotNull(romeDirectionSouthFeatures);
        Assert.IsEmpty(romeDirectionSouthFeatures);
        Assert.IsNotNull(tivoliDirectionSouthFeatures);
        Assert.HasCount(1, tivoliDirectionSouthFeatures);

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(null,
            milanTL, GEOEnums.GeoDirections.South));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesDirectionAsync(geoOntology,
            null as OWLLiteral, GEOEnums.GeoDirections.South));
    }
    #endregion

    #region Tests (Analyzer:Interaction)
    [TestMethod]
    public async Task ShouldGetFeaturesCrossedByAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:PoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:PoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:PoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:PoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:PoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:PoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT")),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:PoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.001141059265075 45.06554633935097, 11.058819281921325 45.036440377586516, 11.127483832702575 45.05972633195962, 11.262066352233825 45.05002500301712, 11.421368110046325 44.960695556664774, 11.605389106140075 44.89068838827955, 11.814129340515075 44.97624111890936, 12.069561469421325 44.98012685115769)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.492779242858825 45.22633159406854, 11.514751899108825 45.0539057320877, 11.448833930358825 44.86538705476387, 11.289532172546325 44.734811449636325)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.067059028015075 45.17020515864295, 11.794903266296325 45.06554633935097, 11.778423774108825 44.68015498753276, 10.710003363952575 44.97818401794916, 11.067059028015075 45.17020515864295))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.270306098327575 45.4078781070719, 10.992901313171325 45.432939821462234, 10.866558539733825 45.338418378714074, 11.270306098327575 45.4078781070719))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> crossedByPoRiver = await GEOHelper.GetFeaturesCrossedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:PoFT")));

        Assert.IsNotNull(crossedByPoRiver);
        Assert.HasCount(2, crossedByPoRiver);
        Assert.IsTrue(crossedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:MontagnanaCentoFT"))));
        Assert.IsTrue(crossedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:NogaraPortoMaggioreFT"))));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesCrossedByAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:PoFT2"))));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesCrossedByAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:PoFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesCrossedByAsync(geoOntology,
            null as OWLNamedIndividual));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesCrossedByLiteralAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT")),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.492779242858825 45.22633159406854, 11.514751899108825 45.0539057320877, 11.448833930358825 44.86538705476387, 11.289532172546325 44.734811449636325)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.067059028015075 45.17020515864295, 11.794903266296325 45.06554633935097, 11.778423774108825 44.68015498753276, 10.710003363952575 44.97818401794916, 11.067059028015075 45.17020515864295))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.270306098327575 45.4078781070719, 10.992901313171325 45.432939821462234, 10.866558539733825 45.338418378714074, 11.270306098327575 45.4078781070719))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> crossedByPoRiver = await GEOHelper.GetFeaturesCrossedByAsync(geoOntology, new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.001141059265075 45.06554633935097, 11.058819281921325 45.036440377586516, 11.127483832702575 45.05972633195962, 11.262066352233825 45.05002500301712, 11.421368110046325 44.960695556664774, 11.605389106140075 44.89068838827955, 11.814129340515075 44.97624111890936, 12.069561469421325 44.98012685115769)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(crossedByPoRiver);
        Assert.HasCount(2, crossedByPoRiver);
        Assert.IsTrue(crossedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:MontagnanaCentoFT"))));
        Assert.IsTrue(crossedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:NogaraPortoMaggioreFT"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesCrossedByAsync(null,
            new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.001141059265075 45.06554633935097, 11.058819281921325 45.036440377586516, 11.127483832702575 45.05972633195962, 11.262066352233825 45.05002500301712, 11.421368110046325 44.960695556664774, 11.605389106140075 44.89068838827955, 11.814129340515075 44.97624111890936, 12.069561469421325 44.98012685115769)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesCrossedByAsync(geoOntology,
            null as OWLLiteral));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesCrossedByAsync(geoOntology,
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesTouchedByAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoBiennoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(10.090599060058592 45.701863522304734)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(10.090599060058592 45.701863522304734, 10.182609558105467 45.89383147810295, 10.292609558105466 45.93283147810291)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((10.090599060058592 45.701863522304734, 10.182609558105467 45.89383147810295, 10.292609558105466 45.93283147810291, 10.392609558105468 45.73283147810295, 10.090599060058592 45.701863522304734))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.270306098327575 45.4078781070719, 10.992901313171325 45.432939821462234, 10.866558539733825 45.338418378714074, 11.270306098327575 45.4078781070719))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> touchedByIseoFT = await GEOHelper.GetFeaturesTouchedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:IseoFT")));

        Assert.IsNotNull(touchedByIseoFT);
        Assert.HasCount(2, touchedByIseoFT);
        Assert.IsTrue(touchedByIseoFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:IseoBiennoFT"))));
        Assert.IsTrue(touchedByIseoFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:IseoLevrangeFT"))));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesTouchedByAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:IseoFT2"))));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesTouchedByAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:IseoFT2"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesTouchedByAsync(geoOntology,
            null as OWLNamedIndividual));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesTouchedByLiteralAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoBiennoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoBiennoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(10.090599060058592 45.701863522304734, 10.182609558105467 45.89383147810295, 10.292609558105466 45.93283147810291)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoLevrangeGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((10.090599060058592 45.701863522304734, 10.182609558105467 45.89383147810295, 10.292609558105466 45.93283147810291, 10.392609558105468 45.73283147810295, 10.090599060058592 45.701863522304734))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.270306098327575 45.4078781070719, 10.992901313171325 45.432939821462234, 10.866558539733825 45.338418378714074, 11.270306098327575 45.4078781070719))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> touchedByIseoFT = await GEOHelper.GetFeaturesTouchedByAsync(geoOntology, new OWLLiteral(new RDFTypedLiteral("POINT(10.090599060058592 45.701863522304734)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(touchedByIseoFT);
        Assert.HasCount(2, touchedByIseoFT);
        Assert.IsTrue(touchedByIseoFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:IseoBiennoFT"))));
        Assert.IsTrue(touchedByIseoFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:IseoLevrangeFT"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesTouchedByAsync(null,
            new OWLLiteral(new RDFTypedLiteral("POINT(10.090599060058592 45.701863522304734)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesTouchedByAsync(geoOntology,
            null as OWLLiteral));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesTouchedByAsync(geoOntology,
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesOverlappedByAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT")),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.346078615493791 45.828624093492635, 9.455255251235979 45.77932096932273, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.386934023208635 45.87907866204932, 9.421609621353166 45.81283269722657, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.406846742935198 45.855650479509684, 9.435685854263323 45.8271886970881, 9.475854616470354 45.82694946075535)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(10.090599060058592 45.701863522304734)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> overlappedByBallabioCivateFT = await GEOHelper.GetFeaturesOverlappedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT")));

        Assert.IsNotNull(overlappedByBallabioCivateFT);
        Assert.HasCount(1, overlappedByBallabioCivateFT);
        Assert.IsTrue(overlappedByBallabioCivateFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:LaorcaVercuragoFT"))));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesOverlappedByAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT2"))));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesOverlappedByAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT2"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesOverlappedByAsync(geoOntology,
            null as OWLNamedIndividual));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesOverlappedByLiteralAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:LaorcaVercuragoGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.386934023208635 45.87907866204932, 9.421609621353166 45.81283269722657, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:LeccoValseccaGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.406846742935198 45.855650479509684, 9.435685854263323 45.8271886970881, 9.475854616470354 45.82694946075535)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(10.090599060058592 45.701863522304734)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> overlappedByBallabioCivateFT = await GEOHelper.GetFeaturesOverlappedByAsync(geoOntology, new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.346078615493791 45.828624093492635, 9.455255251235979 45.77932096932273, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(overlappedByBallabioCivateFT);
        Assert.HasCount(1, overlappedByBallabioCivateFT);
        Assert.IsTrue(overlappedByBallabioCivateFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:LaorcaVercuragoFT"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesOverlappedByAsync(null,
            new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.346078615493791 45.828624093492635, 9.455255251235979 45.77932096932273, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesOverlappedByAsync(geoOntology,
            null as OWLLiteral));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesOverlappedByAsync(geoOntology,
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesWithinAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioPescateFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:FornaciVillaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT")),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateFT")),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioCivateGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.346078615493791 45.828624093492635, 9.455255251235979 45.77932096932273, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.392083864517229 45.85254191756793, 9.346078615493791 45.828624093492635, 9.393457155532854 45.82814563213719, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.370156172304162 45.83948216157425, 9.390755537538537 45.837807855535225)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(10.090599060058592 45.701863522304734)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> overlappedByBallabioCivateFT = await GEOHelper.GetFeaturesWithinAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT")));

        Assert.IsNotNull(overlappedByBallabioCivateFT);
        Assert.HasCount(2, overlappedByBallabioCivateFT);
        Assert.IsTrue(overlappedByBallabioCivateFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:BallabioPescateFT"))));
        Assert.IsTrue(overlappedByBallabioCivateFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:FornaciVillaFT"))));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesWithinAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT2"))));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesWithinAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:BallabioCivateFT2"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesWithinAsync(geoOntology,
            null as OWLNamedIndividual));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesWithinLiteralAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioPescateFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:FornaciVillaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:IseoGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateFT")),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:IseoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:BallabioPescateGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.392083864517229 45.85254191756793, 9.346078615493791 45.828624093492635, 9.393457155532854 45.82814563213719, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:FornaciVillaGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(9.370156172304162 45.83948216157425, 9.390755537538537 45.837807855535225)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:IseoGM")),
                    new OWLLiteral(new RDFTypedLiteral("POINT(10.090599060058592 45.701863522304734)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> overlappedByBallabioCivateFT = await GEOHelper.GetFeaturesWithinAsync(geoOntology, new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.346078615493791 45.828624093492635, 9.455255251235979 45.77932096932273, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(overlappedByBallabioCivateFT);
        Assert.HasCount(2, overlappedByBallabioCivateFT);
        Assert.IsTrue(overlappedByBallabioCivateFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:BallabioPescateFT"))));
        Assert.IsTrue(overlappedByBallabioCivateFT.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:FornaciVillaFT"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesWithinAsync(null,
            new OWLLiteral(new RDFTypedLiteral("POLYGON((9.425042848892229 45.89413442236222, 9.346078615493791 45.828624093492635, 9.455255251235979 45.77932096932273, 9.425042848892229 45.89413442236222))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesWithinAsync(geoOntology,
            null as OWLLiteral));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesWithinAsync(geoOntology,
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesIntersectedByAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:PoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:PoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:PoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:PoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:PoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:PoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT")),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:PoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.001141059265075 45.06554633935097, 11.058819281921325 45.036440377586516, 11.127483832702575 45.05972633195962, 11.262066352233825 45.05002500301712, 11.421368110046325 44.960695556664774, 11.605389106140075 44.89068838827955, 11.814129340515075 44.97624111890936, 12.069561469421325 44.98012685115769)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.492779242858825 45.22633159406854, 11.514751899108825 45.0539057320877, 11.448833930358825 44.86538705476387, 11.289532172546325 44.734811449636325)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.067059028015075 45.17020515864295, 11.794903266296325 45.06554633935097, 11.778423774108825 44.68015498753276, 10.710003363952575 44.97818401794916, 11.067059028015075 45.17020515864295))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.270306098327575 45.4078781070719, 10.992901313171325 45.432939821462234, 10.866558539733825 45.338418378714074, 11.270306098327575 45.4078781070719))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> intersectedByPoRiver = await GEOHelper.GetFeaturesIntersectedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:PoFT")));

        Assert.IsNotNull(intersectedByPoRiver);
        Assert.HasCount(2, intersectedByPoRiver);
        Assert.IsTrue(intersectedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:MontagnanaCentoFT"))));
        Assert.IsTrue(intersectedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:NogaraPortoMaggioreFT"))));

        //Unexisting features
        Assert.IsNull(await GEOHelper.GetFeaturesIntersectedByAsync(geoOntology,
            new OWLNamedIndividual(new RDFResource("ex:PoFT2"))));
        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesIntersectedByAsync(null,
            new OWLNamedIndividual(new RDFResource("ex:PoFT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesIntersectedByAsync(geoOntology,
            null as OWLNamedIndividual));
    }

    [TestMethod]
    public async Task ShouldGetFeaturesIntersectedByLiteralAsync()
    {
        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms = [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_GML)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLClassAssertion(
                    new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoFT")),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreFT")),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaFT")),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM"))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:MontagnanaCentoGM")),
                    new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.492779242858825 45.22633159406854, 11.514751899108825 45.0539057320877, 11.448833930358825 44.86538705476387, 11.289532172546325 44.734811449636325)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:NogaraPortoMaggioreGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.067059028015075 45.17020515864295, 11.794903266296325 45.06554633935097, 11.778423774108825 44.68015498753276, 10.710003363952575 44.97818401794916, 11.067059028015075 45.17020515864295))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))),
                new OWLDataPropertyAssertion(
                    new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                    new OWLNamedIndividual(new RDFResource("ex:VeronaVillafrancaGM")),
                    new OWLLiteral(new RDFTypedLiteral("POLYGON((11.270306098327575 45.4078781070719, 10.992901313171325 45.432939821462234, 10.866558539733825 45.338418378714074, 11.270306098327575 45.4078781070719))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)))
            ]
        };
        List<OWLNamedIndividual> intersectedByPoRiver = await GEOHelper.GetFeaturesIntersectedByAsync(geoOntology, new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.001141059265075 45.06554633935097, 11.058819281921325 45.036440377586516, 11.127483832702575 45.05972633195962, 11.262066352233825 45.05002500301712, 11.421368110046325 44.960695556664774, 11.605389106140075 44.89068838827955, 11.814129340515075 44.97624111890936, 12.069561469421325 44.98012685115769)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

        Assert.IsNotNull(intersectedByPoRiver);
        Assert.HasCount(2, intersectedByPoRiver);
        Assert.IsTrue(intersectedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:MontagnanaCentoFT"))));
        Assert.IsTrue(intersectedByPoRiver.Any(ft => ft.GetIRI().Equals(new RDFResource("ex:NogaraPortoMaggioreFT"))));

        //Input guards
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesIntersectedByAsync(null,
            new OWLLiteral(new RDFTypedLiteral("LINESTRING(11.001141059265075 45.06554633935097, 11.058819281921325 45.036440377586516, 11.127483832702575 45.05972633195962, 11.262066352233825 45.05002500301712, 11.421368110046325 44.960695556664774, 11.605389106140075 44.89068838827955, 11.814129340515075 44.97624111890936, 12.069561469421325 44.98012685115769)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesIntersectedByAsync(geoOntology,
            null as OWLLiteral));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetFeaturesIntersectedByAsync(geoOntology,
            new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    private static OWLOntology BuildSpatialRelationsOntology()
    {
        //SquareA:  (0,0)-(0,10)-(10,10)-(10,0)          10x10 square at origin
        //SquareA2: same geometry as SquareA              (equals SquareA)
        //SquareB:  (10,0)-(10,10)-(20,10)-(20,0)         adjacent to SquareA, shares edge x=10 (touches SquareA)
        //SquareC:  (5,5)-(5,15)-(15,15)-(15,5)           overlaps SquareA on [5,10]x[5,10]
        //SquareD:  (2,2)-(2,8)-(8,8)-(8,2)               strictly inside SquareA (within/contains)
        //SquareE:  (100,100)-(100,110)-(110,110)-(110,100) disjoint from everything
        //LineF:    (-5,5)-(15,5)                         crosses SquareA
        (string uri, string wkt)[] features =
        [
            ("ex:SquareA", "POLYGON((0 0, 0 10, 10 10, 10 0, 0 0))"),
            ("ex:SquareA2", "POLYGON((0 0, 0 10, 10 10, 10 0, 0 0))"),
            ("ex:SquareB", "POLYGON((10 0, 10 10, 20 10, 20 0, 10 0))"),
            ("ex:SquareC", "POLYGON((5 5, 5 15, 15 15, 15 5, 5 5))"),
            ("ex:SquareD", "POLYGON((2 2, 2 8, 8 8, 8 2, 2 2))"),
            ("ex:SquareE", "POLYGON((100 100, 100 110, 110 110, 110 100, 100 100))"),
            ("ex:LineF", "LINESTRING(-5 5, 15 5)")
        ];

        OWLOntology geoOntology = new OWLOntology(new Uri("ex:geoOnt"))
        {
            DeclarationAxioms =
            [
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE)),
                new OWLDeclaration(new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY)),
                new OWLDeclaration(new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT))
            ],
            AssertionAxioms = []
        };

        foreach ((string uri, string wkt) in features)
        {
            string ftUri = $"{uri}FT", gmUri = $"{uri}GM";
            geoOntology.DeclarationAxioms.Add(new OWLDeclaration(new OWLNamedIndividual(new RDFResource(ftUri))));
            geoOntology.DeclarationAxioms.Add(new OWLDeclaration(new OWLNamedIndividual(new RDFResource(gmUri))));
            geoOntology.AssertionAxioms.Add(new OWLClassAssertion(
                new OWLClass(RDFVocabulary.GEOSPARQL.FEATURE),
                new OWLNamedIndividual(new RDFResource(ftUri))));
            geoOntology.AssertionAxioms.Add(new OWLClassAssertion(
                new OWLClass(RDFVocabulary.GEOSPARQL.GEOMETRY),
                new OWLNamedIndividual(new RDFResource(gmUri))));
            geoOntology.AssertionAxioms.Add(new OWLObjectPropertyAssertion(
                new OWLObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY),
                new OWLNamedIndividual(new RDFResource(ftUri)),
                new OWLNamedIndividual(new RDFResource(gmUri))));
            geoOntology.AssertionAxioms.Add(new OWLDataPropertyAssertion(
                new OWLDataProperty(RDFVocabulary.GEOSPARQL.AS_WKT),
                new OWLNamedIndividual(new RDFResource(gmUri)),
                new OWLLiteral(new RDFTypedLiteral(wkt, RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
        }

        return geoOntology;
    }

    [TestMethod]
    public async Task ShouldCheckIsEqualToAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        Assert.IsTrue(await GEOHelper.CheckIsEqualToAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareA2FT"))));
        Assert.IsTrue(await GEOHelper.CheckIsEqualToAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareA2FT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT")))); //symmetric
        Assert.IsFalse(await GEOHelper.CheckIsEqualToAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareBFT"))));

        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.CheckIsEqualToAsync(null, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareA2FT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.CheckIsEqualToAsync(geoOntology, null, new OWLNamedIndividual(new RDFResource("ex:SquareA2FT"))));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.CheckIsEqualToAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), null as OWLNamedIndividual));
    }

    [TestMethod]
    public async Task ShouldCheckIsDisjointFromAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        Assert.IsTrue(await GEOHelper.CheckIsDisjointFromAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareEFT"))));
        Assert.IsTrue(await GEOHelper.CheckIsDisjointFromAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareEFT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT"))));
        Assert.IsFalse(await GEOHelper.CheckIsDisjointFromAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareBFT"))));
    }

    [TestMethod]
    public async Task ShouldCheckIsTouchedByAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        Assert.IsTrue(await GEOHelper.CheckIsTouchedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareBFT"))));
        Assert.IsTrue(await GEOHelper.CheckIsTouchedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareBFT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT")))); //symmetric
        Assert.IsFalse(await GEOHelper.CheckIsTouchedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareEFT"))));

        //Literal overload
        Assert.IsTrue(await GEOHelper.CheckIsTouchedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")),
            new OWLLiteral(new RDFTypedLiteral("POLYGON((10 0, 10 10, 20 10, 20 0, 10 0))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));

        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.CheckIsTouchedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), null as OWLLiteral));
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.CheckIsTouchedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLLiteral(new RDFTypedLiteral("hello", RDFModelEnums.RDFDatatypes.XSD_STRING))));
    }

    [TestMethod]
    public async Task ShouldCheckIsCrossedByAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        Assert.IsTrue(await GEOHelper.CheckIsCrossedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:LineFFT"))));
        Assert.IsTrue(await GEOHelper.CheckIsCrossedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:LineFFT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT")))); //symmetric
        Assert.IsFalse(await GEOHelper.CheckIsCrossedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareEFT"))));
    }

    [TestMethod]
    public async Task ShouldCheckIsOverlappedByAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        Assert.IsTrue(await GEOHelper.CheckIsOverlappedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareCFT"))));
        Assert.IsTrue(await GEOHelper.CheckIsOverlappedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareCFT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT")))); //symmetric
        Assert.IsFalse(await GEOHelper.CheckIsOverlappedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareDFT")))); //D is within A, not overlapping
    }

    [TestMethod]
    public async Task ShouldCheckIsIntersectedByAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        Assert.IsTrue(await GEOHelper.CheckIsIntersectedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareBFT"))));
        Assert.IsTrue(await GEOHelper.CheckIsIntersectedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareDFT"))));
        Assert.IsFalse(await GEOHelper.CheckIsIntersectedByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareEFT"))));
    }

    [TestMethod]
    public async Task ShouldCheckIsWithinAndContainsAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        Assert.IsTrue(await GEOHelper.CheckIsWithinAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareDFT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT"))));
        Assert.IsFalse(await GEOHelper.CheckIsWithinAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareDFT")))); //not symmetric

        Assert.IsTrue(await GEOHelper.CheckContainsAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareDFT"))));
        Assert.IsFalse(await GEOHelper.CheckContainsAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareDFT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT"))));

        //Literal overload
        Assert.IsTrue(await GEOHelper.CheckIsWithinAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareDFT")),
            new OWLLiteral(new RDFTypedLiteral("POLYGON((0 0, 0 10, 10 10, 10 0, 0 0))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
    }

    [TestMethod]
    public async Task ShouldCheckIsCoveredByAndCoversAsync()
    {
        OWLOntology geoOntology = BuildSpatialRelationsOntology();

        //CoveredBy/Covers are a superset of Within/Contains: whatever is strictly within also counts as coveredBy
        Assert.IsTrue(await GEOHelper.CheckIsCoveredByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareDFT")), new OWLNamedIndividual(new RDFResource("ex:SquareAFT"))));
        Assert.IsTrue(await GEOHelper.CheckCoversAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareDFT"))));
        Assert.IsFalse(await GEOHelper.CheckIsCoveredByAsync(geoOntology, new OWLNamedIndividual(new RDFResource("ex:SquareAFT")), new OWLNamedIndividual(new RDFResource("ex:SquareEFT"))));
    }

    [TestMethod]
    public async Task ShouldRejectNonWGS84WKTLiteralAsync()
    {
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetCentroidOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("SRID=3857;POINT(9.18 45.46)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));

        //WGS84 (EPSG:4326) is explicitly allowed
        Assert.IsNotNull(await GEOHelper.GetCentroidOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("SRID=4326;POINT(9.18 45.46)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
    }

    [TestMethod]
    public async Task ShouldRejectNonWGS84GMLLiteralAsync()
    {
        await Assert.ThrowsExactlyAsync<OWLException>(async () => await GEOHelper.GetCentroidOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("""<gml:Point xmlns:gml="http://www.opengis.net/gml/3.2" srsName="EPSG:3857"><gml:pos>9.18 45.46</gml:pos></gml:Point>""", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML))));

        //WGS84 (EPSG:4326 / CRS84) is explicitly allowed
        Assert.IsNotNull(await GEOHelper.GetCentroidOfFeatureAsync(
            new OWLLiteral(new RDFTypedLiteral("""<gml:Point xmlns:gml="http://www.opengis.net/gml/3.2" srsName="EPSG:4326"><gml:pos>9.18 45.46</gml:pos></gml:Point>""", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML))));
    }
    #endregion
}