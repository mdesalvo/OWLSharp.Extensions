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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.GML2;
using OWLSharp.Ontology;
using OWLSharp.Reasoner;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using RDFSharp.Model;
using RDFSharp.Query;

namespace OWLSharp.Extensions.GEO
{
    /// <summary>
    /// Spatial engine of OWLSharp: it can declare, analyze and correlate GeoSPARQL features georeferenced as WGS84 geometries
    /// </summary>
    public static class GEOHelper
    {
        // WGS84 uses LON/LAT coordinates
        // LON => X (West/East,  -180->180)
        // LAT => Y (North/South, -90->90)

        internal static readonly ThreadLocal<WKTReader> WKTReaderTLS = new ThreadLocal<WKTReader>(() => new WKTReader());
        internal static readonly ThreadLocal<WKTWriter> WKTWriterTLS = new ThreadLocal<WKTWriter>(() => new WKTWriter());
        internal static readonly ThreadLocal<GMLReader> GMLReaderTLS = new ThreadLocal<GMLReader>(() => new GMLReader());
        internal static readonly ThreadLocal<GMLWriter> GMLWriterTLS = new ThreadLocal<GMLWriter>(() => new GMLWriter());
        internal static WKTReader WKTReader => WKTReaderTLS.Value;
        internal static WKTWriter WKTWriter => WKTWriterTLS.Value;
        internal static GMLReader GMLReader => GMLReaderTLS.Value;
        internal static GMLWriter GMLWriter => GMLWriterTLS.Value;

        #region Methods

        #region Helper (Initializer, Declarer, Getter)
        /// <summary>
        /// Imports GeoSPARQL-related ontologies into the working ontology, enriching it with T-BOX required for spatial modeling and reasoning
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static async Task InitializeGEOAsync(this OWLOntology ontology, int timeoutMilliseconds=20000, int cacheMilliseconds=3600000)
        {
            if (ontology != null)
            {
                await ontology.ImportAsync(new Uri(RDFVocabulary.GEOSPARQL.DEREFERENCE_URI), timeoutMilliseconds, cacheMilliseconds);
                await ontology.ImportAsync(new Uri(RDFVocabulary.GEOSPARQL.SF.DEREFERENCE_URI), timeoutMilliseconds, cacheMilliseconds);
                await ontology.ImportAsync(new Uri(RDFVocabulary.GEOSPARQL.GEOF.DEREFERENCE_URI), timeoutMilliseconds, cacheMilliseconds);
            }
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a geospatial feature having the given name and the given point encoding
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclarePointFeature(this OWLOntology ontology, OWLNamedIndividual featureUri, GEOPoint geoPoint, bool isDefaultGeometry=true)
        {
            #region Guards
            if (featureUri == null)
                throw new OWLException($"Cannot declare point feature because given '{nameof(featureUri)}' parameter is null");
            if (geoPoint == null)
                throw new OWLException($"Cannot declare point feature because given '{nameof(geoPoint)}' parameter is null");
            #endregion

            ontology.DeclareEntity(featureUri);
            ontology.DeclareEntity(new OWLNamedIndividual(geoPoint));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.FEATURE.ToEntity<OWLClass>(),
                featureUri));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.GEOMETRY.ToEntity<OWLClass>(),
                new OWLNamedIndividual(geoPoint)));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.SF.POINT.ToEntity<OWLClass>(),
                new OWLNamedIndividual(geoPoint)));
            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                isDefaultGeometry ? RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY.ToEntity<OWLObjectProperty>()
                                  : new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                featureUri,
                new OWLNamedIndividual(geoPoint)));
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.GEOSPARQL.AS_WKT.ToEntity<OWLDataProperty>(),
                new OWLNamedIndividual(geoPoint),
                new OWLLiteral(new RDFTypedLiteral(geoPoint.ToWKT(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.GEOSPARQL.AS_GML.ToEntity<OWLDataProperty>(),
                new OWLNamedIndividual(geoPoint),
                new OWLLiteral(new RDFTypedLiteral(geoPoint.ToGML(), RDFModelEnums.RDFDatatypes.GEOSPARQL_GML))));

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a geospatial feature having the given name and the given linestring encoding
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareLineFeature(this OWLOntology ontology, OWLNamedIndividual featureUri, GEOLine geoLine, bool isDefaultGeometry=true)
        {
            #region Guards
            if (featureUri == null)
                throw new OWLException($"Cannot declare line feature because given '{nameof(featureUri)}' parameter is null");
            if (geoLine == null)
                throw new OWLException($"Cannot declare line feature because given '{nameof(geoLine)}' parameter is null");
            #endregion

            ontology.DeclareEntity(featureUri);
            ontology.DeclareEntity(new OWLNamedIndividual(geoLine));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.FEATURE.ToEntity<OWLClass>(),
                featureUri));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.GEOMETRY.ToEntity<OWLClass>(),
                new OWLNamedIndividual(geoLine)));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.SF.LINESTRING.ToEntity<OWLClass>(),
                new OWLNamedIndividual(geoLine)));
            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                isDefaultGeometry ? RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY.ToEntity<OWLObjectProperty>()
                                  : new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                featureUri,
                new OWLNamedIndividual(geoLine)));
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.GEOSPARQL.AS_WKT.ToEntity<OWLDataProperty>(),
                new OWLNamedIndividual(geoLine),
                new OWLLiteral(new RDFTypedLiteral(geoLine.ToWKT(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.GEOSPARQL.AS_GML.ToEntity<OWLDataProperty>(),
                new OWLNamedIndividual(geoLine),
                new OWLLiteral(new RDFTypedLiteral(geoLine.ToGML(), RDFModelEnums.RDFDatatypes.GEOSPARQL_GML))));

            return ontology;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a geospatial feature having the given name and the given polygon encoding
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static OWLOntology DeclareAreaFeature(this OWLOntology ontology, OWLNamedIndividual featureUri, GEOArea geoArea, bool isDefaultGeometry=true)
        {
            #region Guards
            if (featureUri == null)
                throw new OWLException($"Cannot declare area feature because given '{nameof(featureUri)}' parameter is null");
            if (geoArea == null)
                throw new OWLException($"Cannot declare area feature because given '{nameof(geoArea)}' parameter is null");
            #endregion

            ontology.DeclareEntity(featureUri);
            ontology.DeclareEntity(new OWLNamedIndividual(geoArea));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.FEATURE.ToEntity<OWLClass>(),
                featureUri));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.GEOMETRY.ToEntity<OWLClass>(),
                new OWLNamedIndividual(geoArea)));
            ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.GEOSPARQL.SF.POLYGON.ToEntity<OWLClass>(),
                new OWLNamedIndividual(geoArea)));
            ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                isDefaultGeometry ? RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY.ToEntity<OWLObjectProperty>()
                                  : new OWLObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY),
                featureUri,
                new OWLNamedIndividual(geoArea)));
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.GEOSPARQL.AS_WKT.ToEntity<OWLDataProperty>(),
                new OWLNamedIndividual(geoArea),
                new OWLLiteral(new RDFTypedLiteral(geoArea.ToWKT(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT))));
            ontology.DeclareAssertionAxiom(new OWLDataPropertyAssertion(
                RDFVocabulary.GEOSPARQL.AS_GML.ToEntity<OWLDataProperty>(),
                new OWLNamedIndividual(geoArea),
                new OWLLiteral(new RDFTypedLiteral(geoArea.ToGML(), RDFModelEnums.RDFDatatypes.GEOSPARQL_GML))));

            return ontology;
        }

        /// <summary>
        /// Gets the spatial dimension of the given GeoSPARQL feature from the working ontology.<br/>
        /// It is usually a single entity encoding the default geometry, but it may also contain the secondary geometries.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<GEOEntity>> GetSpatialFeatureAsync(this OWLOntology ontology, OWLNamedIndividual featureURI)
        {
            #region Guards
            if (featureURI == null)
                throw new OWLException($"Cannot get spatial dimension of feature because given '{nameof(featureURI)}' parameter is null");
            #endregion

            List<GEOEntity> spatialExtentOfFeature = new List<GEOEntity>();
            Dictionary<string,List<(Geometry,Geometry)>> featuresWithGeometry = await GetFeaturesWithGeometriesAsync(ontology, featureURI.GetIRI());
            if (featuresWithGeometry.TryGetValue(featureURI.GetIRI().ToString(), out List<(Geometry wgs84,Geometry laz)> featureGeometries))
            {
                foreach (Geometry wgs84Geom in featureGeometries.Select(fg => fg.wgs84))
                {
                    RDFResource geometryUri = new RDFResource((string)wgs84Geom.UserData);
                    switch (wgs84Geom)
                    {
                        case Point wgs84Point:
                            spatialExtentOfFeature.Add(new GEOPoint(geometryUri, (wgs84Point.Coordinate.X,wgs84Point.Coordinate.Y)));
                            break;
                        case LineString wgs84Line:
                            spatialExtentOfFeature.Add(new GEOLine(geometryUri, wgs84Line.Coordinates.Select(c => (c.X,c.Y)).ToArray()));
                            break;
                        case Polygon wgs84Area:
                            spatialExtentOfFeature.Add(new GEOArea(geometryUri, wgs84Area.Coordinates.Select(c => (c.X,c.Y)).ToArray()));
                            break;
                    }
                }
            }
            return spatialExtentOfFeature;
        }
        #endregion

        #region Analyzer (Distance, Length, Area)
        /// <summary>
        /// Gets the spatial distance, expressed in meters, between the given GeoSPARQL features from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<double?> GetDistanceBetweenFeaturesAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(ontology)}' parameter is null");
            if (fromFeatureUri == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(fromFeatureUri)}' parameter is null");
            if (toFeatureUri == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(toFeatureUri)}' parameter is null");
            #endregion

            //Collect WGS84 geometries of "From" feature
            (Geometry, Geometry) defaultGeometryFrom = await ontology.GetDefaultGeometryOfFeatureAsync(fromFeatureUri.GetIRI());
            List<(Geometry, Geometry)> geometriesFrom = await ontology.GetSecondaryGeometriesOfFeatureAsync(fromFeatureUri.GetIRI());
            if (defaultGeometryFrom.Item1 != null && defaultGeometryFrom.Item2 != null)
                geometriesFrom.Insert(0, defaultGeometryFrom);

            //Collect WGS84 geometries of "To" feature
            (Geometry, Geometry) defaultGeometryTo = await ontology.GetDefaultGeometryOfFeatureAsync(toFeatureUri.GetIRI());
            List<(Geometry, Geometry)> geometriesTo = await ontology.GetSecondaryGeometriesOfFeatureAsync(toFeatureUri.GetIRI());
            if (defaultGeometryTo.Item1 != null && defaultGeometryTo.Item2 != null)
                geometriesTo.Insert(0, defaultGeometryTo);

            //Perform spatial analysis between collected geometries (calibrate minimum distance)
            //using dynamic LAEA centered on first "From" geometry for accurate metric calculation
            double? featuresDistance = double.MaxValue;
            geometriesFrom.ForEach(fromGeom =>
            {
                geometriesTo.ForEach(toGeom =>
                {
                    (Geometry lazFrom, Geometry lazTo) = ProjectToDynamicLAEA(fromGeom.Item1, toGeom.Item1);
                    double tempDistance = lazFrom.Distance(lazTo);
                    if (tempDistance < featuresDistance)
                        featuresDistance = tempDistance;
                });
            });

            //Give null in case distance could not be calculated (no available geometries from any sides)
            return featuresDistance is double.MaxValue ? null : featuresDistance;
        }

        /// <summary>
        /// Gets the spatial distance, expressed in meters, between the given GeoSPARQL feature and the given GeoSPARQL literal from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<double?> GetDistanceBetweenFeaturesAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(ontology)}' parameter is null");
            if (fromFeatureUri == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(fromFeatureUri)}' parameter is null");
            if (toFeatureLiteral == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(toFeatureLiteral)}' parameter is null");
            if (!(toFeatureLiteral.GetLiteral() is RDFTypedLiteral toFeatureTypedLiteral) || !toFeatureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get distance between features because given '{nameof(toFeatureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Collect WGS84 geometries of "From" feature
            (Geometry, Geometry) defaultGeometryFrom = await ontology.GetDefaultGeometryOfFeatureAsync(fromFeatureUri.GetIRI());
            List<(Geometry, Geometry)> geometriesFrom = await ontology.GetSecondaryGeometriesOfFeatureAsync(fromFeatureUri.GetIRI());
            if (defaultGeometryFrom.Item1 != null && defaultGeometryFrom.Item2 != null)
                geometriesFrom.Insert(0, defaultGeometryFrom);

            //Transform "To" feature into WGS84 geometry
            bool isWKT = toFeatureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84GeometryTo = isWKT ? WKTReader.Read(toFeatureTypedLiteral.Value) : GMLReader.Read(toFeatureTypedLiteral.Value);
            wgs84GeometryTo.SRID=4326;

            //Perform spatial analysis using dynamic LAEA centered on "From" geometry
            double? featuresDistance = double.MaxValue;
            geometriesFrom.ForEach(fromGeom =>
            {
                (Geometry lazFrom, Geometry lazTo) = ProjectToDynamicLAEA(fromGeom.Item1, wgs84GeometryTo);
                double tempDistance = lazFrom.Distance(lazTo);
                if (tempDistance < featuresDistance)
                    featuresDistance = tempDistance;
            });

            //Give null in case distance could not be calculated (no available geometries)
            return featuresDistance is double.MaxValue ? null : featuresDistance;
        }

        /// <summary>
        /// Gets the spatial distance, expressed in meters, between the given GeoSPARQL literals.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<double?> GetDistanceBetweenFeaturesAsync(OWLLiteral fromFeatureLiteral, OWLLiteral toFeatureLiteral)
        {
            #region Guards
            if (fromFeatureLiteral == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(fromFeatureLiteral)}' parameter is null");
            if (!(fromFeatureLiteral.GetLiteral() is RDFTypedLiteral fromFeatureTypedLiteral) || !fromFeatureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get distance between features because given '{nameof(fromFeatureLiteral)}' parameter is not a geographic typed literal");
            if (toFeatureLiteral == null)
                throw new OWLException($"Cannot get distance between features because given '{nameof(toFeatureLiteral)}' parameter is null");
            if (!(toFeatureLiteral.GetLiteral() is RDFTypedLiteral toFeatureTypedLiteral) || !toFeatureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get distance between features because given '{nameof(toFeatureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform "From" feature into WGS84 geometry
            bool fromIsWKT = fromFeatureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84GeometryFrom = fromIsWKT ? WKTReader.Read(fromFeatureTypedLiteral.Value) : GMLReader.Read(fromFeatureTypedLiteral.Value);
            wgs84GeometryFrom.SRID=4326;

            //Transform "To" feature into WGS84 geometry
            bool toIsWKT = toFeatureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84GeometryTo = toIsWKT ? WKTReader.Read(toFeatureTypedLiteral.Value) : GMLReader.Read(toFeatureTypedLiteral.Value);
            wgs84GeometryTo.SRID=4326;

            //Perform spatial analysis using dynamic LAEA centered on "From" geometry
            (Geometry lazFrom, Geometry lazTo) = ProjectToDynamicLAEA(wgs84GeometryFrom, wgs84GeometryTo);
            return await Task.FromResult(lazFrom.Distance(lazTo));
        }

        /// <summary>
        /// Gets the spatial length, expressed in meters, of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<double?> GetLengthOfFeatureAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get length of feature because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get length of feature because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Collect WGS84 geometries of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            List<(Geometry, Geometry)> geometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
                geometries.Insert(0, defaultGeometry);

            //Perform spatial analysis using dynamic LAEA (calibrate maximum length)
            double? featureLength = double.MinValue;
            geometries.ForEach(geom =>
            {
                Geometry lazGeom = ProjectToDynamicLAEA(geom.Item1);
                double tempLength = lazGeom.Length;
                if (tempLength > featureLength)
                    featureLength = tempLength;
            });

            //Give null in case length could not be calculated (no available geometries)
            return featureLength is double.MinValue ? null : featureLength;
        }

        /// <summary>
        /// Gets the spatial length, expressed in meters, of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<double?> GetLengthOfFeatureAsync(OWLLiteral featureLiteral)
        {
            #region Guards
            if (featureLiteral == null)
                throw new OWLException($"Cannot get length of feature because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get length of feature because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into WGS84 geometry and project to dynamic LAEA
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = ProjectToDynamicLAEA(wgs84Geometry);

            return await Task.FromResult(lazGeometry.Length);
        }

        /// <summary>
        /// Gets the spatial length, expressed in square meters, of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<double?> GetAreaOfFeatureAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get area of feature because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get area of feature because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Collect WGS84 geometries of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            List<(Geometry, Geometry)> geometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
                geometries.Insert(0, defaultGeometry);

            //Perform spatial analysis using dynamic LAEA (calibrate maximum area)
            double? featureArea = double.MinValue;
            geometries.ForEach(geom =>
            {
                Geometry lazGeom = ProjectToDynamicLAEA(geom.Item1);
                double tempArea = lazGeom.Area;
                if (tempArea > featureArea)
                    featureArea = tempArea;
            });

            //Give null in case area could not be calculated (no available geometries)
            return featureArea is double.MinValue ? null : featureArea;
        }

        /// <summary>
        /// Gets the spatial area, expressed in square meters, of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<double?> GetAreaOfFeatureAsync(OWLLiteral featureLiteral)
        {
            #region Guards
            if (featureLiteral == null)
                throw new OWLException($"Cannot get area of feature because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get area of feature because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into WGS84 geometry and project to dynamic LAEA
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = ProjectToDynamicLAEA(wgs84Geometry);

            return await Task.FromResult(lazGeometry.Area);
        }
        #endregion

        #region Analyzer (Boundary, Buffer, Centroid, ConvexHull, Envelope)
        /// <summary>
        /// Gets the spatial boundary polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetBoundaryOfFeatureAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
            => WrapNullableLiteral(await AnalyzeFeatureAsync(ontology, featureUri?.GetIRI(), GEOEnums.GeoAnalysis.Boundary));

        /// <summary>
        /// Gets the spatial boundary polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetBoundaryOfFeatureAsync(OWLLiteral featureLiteral)
            => WrapNullableLiteral(await AnalyzeLiteralAsync(featureLiteral?.GetLiteral() as RDFTypedLiteral, GEOEnums.GeoAnalysis.Boundary));

        /// <summary>
        /// Gets the spatial buffer polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetBufferAroundFeatureAsync(OWLOntology ontology, OWLNamedIndividual featureUri, double bufferMeters)
            => WrapNullableLiteral(await AnalyzeFeatureAsync(ontology, featureUri?.GetIRI(), GEOEnums.GeoAnalysis.Buffer, bufferMeters));

        /// <summary>
        /// Gets the spatial buffer polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetBufferAroundFeatureAsync(OWLLiteral featureLiteral, double bufferMeters)
            => WrapNullableLiteral(await AnalyzeLiteralAsync(featureLiteral?.GetLiteral() as RDFTypedLiteral, GEOEnums.GeoAnalysis.Buffer, bufferMeters));

        /// <summary>
        /// Gets the spatial centroid point, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetCentroidOfFeatureAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
            => WrapNullableLiteral(await AnalyzeFeatureAsync(ontology, featureUri?.GetIRI(), GEOEnums.GeoAnalysis.Centroid));

        /// <summary>
        /// Gets the spatial centroid point, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetCentroidOfFeatureAsync(OWLLiteral featureLiteral)
            => WrapNullableLiteral(await AnalyzeLiteralAsync(featureLiteral?.GetLiteral() as RDFTypedLiteral, GEOEnums.GeoAnalysis.Centroid));

        /// <summary>
        /// Gets the spatial convex hull polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetConvexHullOfFeatureAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
            => WrapNullableLiteral(await AnalyzeFeatureAsync(ontology, featureUri?.GetIRI(), GEOEnums.GeoAnalysis.ConvexHull));

        /// <summary>
        /// Gets the spatial convex hull polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetConvexHullOfFeatureAsync(OWLLiteral featureLiteral)
            => WrapNullableLiteral(await AnalyzeLiteralAsync(featureLiteral?.GetLiteral() as RDFTypedLiteral, GEOEnums.GeoAnalysis.ConvexHull));

        /// <summary>
        /// Gets the spatial envelope (bounding box) polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetEnvelopeOfFeatureAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
            => WrapNullableLiteral(await AnalyzeFeatureAsync(ontology, featureUri?.GetIRI(), GEOEnums.GeoAnalysis.Envelope));

        /// <summary>
        /// Gets the spatial envelope (bounding box) polygon, expressed as a WGS84-georeferenced WKT literal, of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<OWLLiteral> GetEnvelopeOfFeatureAsync(OWLLiteral featureLiteral)
            => WrapNullableLiteral(await AnalyzeLiteralAsync(featureLiteral?.GetLiteral() as RDFTypedLiteral, GEOEnums.GeoAnalysis.Envelope));
        #endregion

        #region Analyzer (Proximity, Direction, Interaction)
        /// <summary>
        /// Gets the features in proximity of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesNearBy(OWLOntology ontology, OWLNamedIndividual featureUri, double distanceMeters)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features within distance because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get features within distance because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Get centroid of feature
            OWLLiteral centroidOfFeatureLit = await GetCentroidOfFeatureAsync(ontology, featureUri);
            if (centroidOfFeatureLit == null)
                return null;
            RDFTypedLiteral centroidOfFeature = (RDFTypedLiteral)centroidOfFeatureLit.GetLiteral();

            //Create WGS84 geometry from centroid of feature
            bool isWKT = centroidOfFeature.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84CentroidOfFeature = isWKT ? WKTReader.Read(centroidOfFeature.Value) : GMLReader.Read(centroidOfFeature.Value);
            wgs84CentroidOfFeature.SRID=4326;

            //Create dynamic LAEA centered on centroid of feature for accurate distance calculation
            (MathTransform forward, _) = CreateDynamicLAEATransforms(wgs84CentroidOfFeature.Centroid.X, wgs84CentroidOfFeature.Centroid.Y);
            Geometry lazCentroidOfFeature = ApplyTransform(wgs84CentroidOfFeature, forward);

            //Retrieve WKT/GML serialization of features
            Dictionary<string,List<(Geometry,Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries:
            //iterate geometries and collect those within given radius
            List<RDFResource> featuresWithinDistance = new List<RDFResource>();
            foreach (KeyValuePair<string,List<(Geometry,Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                //Obviously exclude the given feature itself
                if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                    continue;

                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    Geometry lazGeometryOfFeature = ApplyTransform(geometryOfFeature.Item1, forward);
                    if (lazGeometryOfFeature.IsWithinDistance(lazCentroidOfFeature, distanceMeters))
                    {
                        featuresWithinDistance.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresWithinDistance).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Gets the features in proximity of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesNearBy(OWLOntology ontology, OWLLiteral featureLiteral, double distanceMeters)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features within distance because given '{nameof(ontology)}' parameter is null");
            if (featureLiteral == null)
                throw new OWLException($"Cannot get features within distance because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get features within distance because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into WGS84 geometry
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;

            //Create dynamic LAEA centered on centroid of feature for accurate distance calculation
            Point wgs84Centroid = wgs84Geometry.Centroid;
            (MathTransform forward, _) = CreateDynamicLAEATransforms(wgs84Centroid.X, wgs84Centroid.Y);
            Geometry lazCentroidOfFeature = ApplyTransform(wgs84Centroid, forward);

            //Retrieve WKT/GML serialization of features
            Dictionary<string,List<(Geometry,Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries:
            //iterate geometries and collect those within given radius
            List<RDFResource> featuresWithinDistance = new List<RDFResource>();
            foreach (KeyValuePair<string,List<(Geometry,Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    Geometry lazGeometryOfFeature = ApplyTransform(geometryOfFeature.Item1, forward);
                    if (lazGeometryOfFeature.IsWithinDistance(lazCentroidOfFeature, distanceMeters))
                        featuresWithinDistance.Add(new RDFResource(featureWithGeometry.Key));
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresWithinDistance).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Gets the features in direction of the given GeoSPARQL feature from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesDirectionAsync(OWLOntology ontology, OWLNamedIndividual featureUri, GEOEnums.GeoDirections geoDirection)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features direction because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get features direction because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Analyze default geometry of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresDirection = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (geometryOfFeature.Item2.Coordinates.Any(c1 => defaultGeometry.Item2.Coordinates.Any(c2 => MatchCoordinates(c1, c2, geoDirection))))
                            featuresDirection.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresDirection).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            //Analyze secondary geometries of feature
            List<(Geometry, Geometry)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (secondaryGeometries.Count > 0)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresDirection = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (geometryOfFeature.Item2.Coordinates.Any(c1 => secondaryGeometries.Any(sg => sg.Item2.Coordinates.Any(c2 => MatchCoordinates(c1, c2, geoDirection)))))
                            featuresDirection.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresDirection).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the features in direction of the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesDirectionAsync(OWLOntology ontology, OWLLiteral featureLiteral, GEOEnums.GeoDirections geoDirection)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features direction because given '{nameof(ontology)}' parameter is null");
            if (featureLiteral == null)
                throw new OWLException($"Cannot get features direction because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get features direction because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into geometry
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);

            //Retrieve WKT/GML serialization of features
            Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries
            List<RDFResource> featuresDirection = new List<RDFResource>();
            foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    if (geometryOfFeature.Item2.Coordinates.Any(c1 => lazGeometry.Coordinates.Any(c2 => MatchCoordinates(c1, c2, geoDirection))))
                        featuresDirection.Add(new RDFResource(featureWithGeometry.Key));
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresDirection).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL feature (crossedBy) from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesCrossedByAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Analyze default geometry of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (defaultGeometry.Item2.Crosses(geometryOfFeature.Item2))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            //Analyze secondary geometries of feature
            List<(Geometry, Geometry)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (secondaryGeometries.Count > 0)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (secondaryGeometries.Any(sg => sg.Item2.Crosses(geometryOfFeature.Item2)))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL literal (crossedBy).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesCrossedByAsync(OWLOntology ontology, OWLLiteral featureLiteral)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureLiteral == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into geometry
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);

            //Retrieve WKT/GML serialization of features
            Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries
            List<RDFResource> featuresDirectionOf = new List<RDFResource>();
            foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    if (lazGeometry.Crosses(geometryOfFeature.Item2))
                        featuresDirectionOf.Add(new RDFResource(featureWithGeometry.Key));
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresDirectionOf).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL feature (touchedBy) from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesTouchedByAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Analyze default geometry of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (defaultGeometry.Item2.Touches(geometryOfFeature.Item2))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            //Analyze secondary geometries of feature
            List<(Geometry, Geometry)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (secondaryGeometries.Count > 0)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (secondaryGeometries.Any(sg => sg.Item2.Touches(geometryOfFeature.Item2)))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL literal (touchedBy).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesTouchedByAsync(OWLOntology ontology, OWLLiteral featureLiteral)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureLiteral == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into geometry
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);

            //Retrieve WKT/GML serialization of features
            Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries
            List<RDFResource> featuresDirectionOf = new List<RDFResource>();
            foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    if (lazGeometry.Touches(geometryOfFeature.Item2))
                        featuresDirectionOf.Add(new RDFResource(featureWithGeometry.Key));
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresDirectionOf).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL feature (overlappedBy) from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesOverlappedByAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Analyze default geometry of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (defaultGeometry.Item2.Overlaps(geometryOfFeature.Item2))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            //Analyze secondary geometries of feature
            List<(Geometry, Geometry)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (secondaryGeometries.Count > 0)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (secondaryGeometries.Any(sg => sg.Item2.Overlaps(geometryOfFeature.Item2)))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL literal (overlappedBy).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesOverlappedByAsync(OWLOntology ontology, OWLLiteral featureLiteral)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureLiteral == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into geometry
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);

            //Retrieve WKT/GML serialization of features
            Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries
            List<RDFResource> featuresDirectionOf = new List<RDFResource>();
            foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    if (lazGeometry.Overlaps(geometryOfFeature.Item2))
                        featuresDirectionOf.Add(new RDFResource(featureWithGeometry.Key));
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresDirectionOf).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL feature (within) from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesWithinAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Analyze default geometry of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (defaultGeometry.Item2.Contains(geometryOfFeature.Item2))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            //Analyze secondary geometries of feature
            List<(Geometry, Geometry)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (secondaryGeometries.Count > 0)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                        if (secondaryGeometries.Any(sg => sg.Item2.Contains(geometryOfFeature.Item2)))
                        {
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                        }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL literal (within).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesWithinAsync(OWLOntology ontology, OWLLiteral featureLiteral)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureLiteral == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into geometry
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);

            //Retrieve WKT/GML serialization of features
            Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries
            List<RDFResource> featuresDirectionOf = new List<RDFResource>();
            foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    if (lazGeometry.Contains(geometryOfFeature.Item2))
                        featuresDirectionOf.Add(new RDFResource(featureWithGeometry.Key));
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresDirectionOf).Select(r => new OWLNamedIndividual(r)).ToList();
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL feature (intersectedBy) from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesIntersectedByAsync(OWLOntology ontology, OWLNamedIndividual featureUri)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureUri)}' parameter is null");
            #endregion

            //Analyze default geometry of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri.GetIRI());
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (defaultGeometry.Item2.Intersects(geometryOfFeature.Item2))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            //Analyze secondary geometries of feature
            List<(Geometry, Geometry)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri.GetIRI());
            if (secondaryGeometries.Count > 0)
            {
                //Retrieve WKT/GML serialization of features
                Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

                //Perform spatial analysis between collected geometries
                List<RDFResource> featuresInteraction = new List<RDFResource>();
                foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
                {
                    //Obviously exclude the given feature itself
                    if (string.Equals(featureWithGeometry.Key, featureUri.GetIRI().ToString()))
                        continue;

                    foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                    {
                        if (secondaryGeometries.Any(sg => sg.Item2.Intersects(geometryOfFeature.Item2)))
                            featuresInteraction.Add(new RDFResource(featureWithGeometry.Key));
                    }
                }

                return RDFQueryUtilities.RemoveDuplicates(featuresInteraction).Select(r => new OWLNamedIndividual(r)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the features interacting with the given GeoSPARQL literal (intersectedBy).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static async Task<List<OWLNamedIndividual>> GetFeaturesIntersectedByAsync(OWLOntology ontology, OWLLiteral featureLiteral)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(ontology)}' parameter is null");
            if (featureLiteral == null)
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is null");
            if (!(featureLiteral.GetLiteral() is RDFTypedLiteral featureTypedLiteral) || !featureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot get features interaction because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Transform feature into geometry
            bool isWKT = featureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureTypedLiteral.Value) : GMLReader.Read(featureTypedLiteral.Value);
            wgs84Geometry.SRID=4326;
            Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);

            //Retrieve WKT/GML serialization of features
            Dictionary<string, List<(Geometry, Geometry)>> featuresWithGeometry = await ontology.GetFeaturesWithGeometriesAsync();

            //Perform spatial analysis between collected geometries
            List<RDFResource> featuresIntersectedBy = new List<RDFResource>();
            foreach (KeyValuePair<string, List<(Geometry, Geometry)>> featureWithGeometry in featuresWithGeometry)
            {
                foreach ((Geometry, Geometry) geometryOfFeature in featureWithGeometry.Value)
                {
                    if (lazGeometry.Intersects(geometryOfFeature.Item2))
                        featuresIntersectedBy.Add(new RDFResource(featureWithGeometry.Key));
                }
            }

            return RDFQueryUtilities.RemoveDuplicates(featuresIntersectedBy).Select(r => new OWLNamedIndividual(r)).ToList();
        }
        #endregion

        #region Analyzer (Spatial Relations)
        /// <summary>
        /// Checks if the given GeoSPARQL features are topologically equal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsEqualToAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Equals);

        /// <summary>
        /// Checks if the given GeoSPARQL feature and the given GeoSPARQL literal are topologically equal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsEqualToAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Equals);

        /// <summary>
        /// Checks if the given GeoSPARQL features are topologically disjoint.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsDisjointFromAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Disjoint);

        /// <summary>
        /// Checks if the given GeoSPARQL feature and the given GeoSPARQL literal are topologically disjoint.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsDisjointFromAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Disjoint);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature is touched by the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsTouchedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Touches);

        /// <summary>
        /// Checks if the given GeoSPARQL feature is touched by the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsTouchedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Touches);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature is crossed by the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsCrossedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Crosses);

        /// <summary>
        /// Checks if the given GeoSPARQL feature is crossed by the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsCrossedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Crosses);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature is overlapped by the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsOverlappedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Overlaps);

        /// <summary>
        /// Checks if the given GeoSPARQL feature is overlapped by the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsOverlappedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Overlaps);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature is intersected by the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsIntersectedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Intersects);

        /// <summary>
        /// Checks if the given GeoSPARQL feature is intersected by the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsIntersectedByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Intersects);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature is spatially within the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsWithinAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Within);

        /// <summary>
        /// Checks if the given GeoSPARQL feature is spatially within the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsWithinAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Within);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature spatially contains the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckContainsAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Contains);

        /// <summary>
        /// Checks if the given GeoSPARQL feature spatially contains the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckContainsAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Contains);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature is covered by the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsCoveredByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.CoveredBy);

        /// <summary>
        /// Checks if the given GeoSPARQL feature is covered by the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckIsCoveredByAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.CoveredBy);

        /// <summary>
        /// Checks if the first given GeoSPARQL feature covers the second given GeoSPARQL feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckCoversAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureUri, GEOEnums.GeoSpatialRelation.Covers);

        /// <summary>
        /// Checks if the given GeoSPARQL feature covers the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static Task<bool> CheckCoversAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral)
            => CheckSpatialRelationAsync(ontology, fromFeatureUri, toFeatureLiteral, GEOEnums.GeoSpatialRelation.Covers);
        #endregion

        #endregion

        #region Utilities
        /// <summary>
        /// Wraps the given WKT/GML typed literal into an OWLLiteral, propagating null (no result) as-is.
        /// </summary>
        internal static OWLLiteral WrapNullableLiteral(RDFTypedLiteral literal)
            => literal == null ? null : new OWLLiteral(literal);

        /// <summary>
        /// Evaluates the given pairwise topological relation between two Azimuthal-projected geometries.
        /// </summary>
        internal static bool EvaluateSpatialRelation(Geometry lazFrom, Geometry lazTo, GEOEnums.GeoSpatialRelation relation)
        {
            switch (relation)
            {
                case GEOEnums.GeoSpatialRelation.Equals: return lazFrom.Equals(lazTo);
                case GEOEnums.GeoSpatialRelation.Disjoint: return lazFrom.Disjoint(lazTo);
                case GEOEnums.GeoSpatialRelation.Touches: return lazFrom.Touches(lazTo);
                case GEOEnums.GeoSpatialRelation.Crosses: return lazFrom.Crosses(lazTo);
                case GEOEnums.GeoSpatialRelation.Overlaps: return lazFrom.Overlaps(lazTo);
                case GEOEnums.GeoSpatialRelation.Intersects: return lazFrom.Intersects(lazTo);
                case GEOEnums.GeoSpatialRelation.Within: return lazFrom.Within(lazTo);
                case GEOEnums.GeoSpatialRelation.Contains: return lazFrom.Contains(lazTo);
                case GEOEnums.GeoSpatialRelation.CoveredBy: return lazFrom.CoveredBy(lazTo);
                case GEOEnums.GeoSpatialRelation.Covers: return lazFrom.Covers(lazTo);
                default: return false;
            }
        }

        /// <summary>
        /// Checks the given pairwise topological relation between the given GeoSPARQL features from the working ontology.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        internal static async Task<bool> CheckSpatialRelationAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLNamedIndividual toFeatureUri, GEOEnums.GeoSpatialRelation relation)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot check spatial relation between features because given '{nameof(ontology)}' parameter is null");
            if (fromFeatureUri == null)
                throw new OWLException($"Cannot check spatial relation between features because given '{nameof(fromFeatureUri)}' parameter is null");
            if (toFeatureUri == null)
                throw new OWLException($"Cannot check spatial relation between features because given '{nameof(toFeatureUri)}' parameter is null");
            #endregion

            //Collect Azimuthal geometries of "From" feature
            (Geometry, Geometry) defaultGeometryFrom = await ontology.GetDefaultGeometryOfFeatureAsync(fromFeatureUri.GetIRI());
            List<(Geometry, Geometry)> geometriesFrom = await ontology.GetSecondaryGeometriesOfFeatureAsync(fromFeatureUri.GetIRI());
            if (defaultGeometryFrom.Item1 != null && defaultGeometryFrom.Item2 != null)
                geometriesFrom.Insert(0, defaultGeometryFrom);

            //Collect Azimuthal geometries of "To" feature
            (Geometry, Geometry) defaultGeometryTo = await ontology.GetDefaultGeometryOfFeatureAsync(toFeatureUri.GetIRI());
            List<(Geometry, Geometry)> geometriesTo = await ontology.GetSecondaryGeometriesOfFeatureAsync(toFeatureUri.GetIRI());
            if (defaultGeometryTo.Item1 != null && defaultGeometryTo.Item2 != null)
                geometriesTo.Insert(0, defaultGeometryTo);

            //A relation holds between the features if it holds between any of their (possibly multiple) geometries
            return geometriesFrom.Any(fromGeom => geometriesTo.Any(toGeom => EvaluateSpatialRelation(fromGeom.Item2, toGeom.Item2, relation)));
        }

        /// <summary>
        /// Checks the given pairwise topological relation between the given GeoSPARQL feature and the given GeoSPARQL literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        internal static async Task<bool> CheckSpatialRelationAsync(OWLOntology ontology, OWLNamedIndividual fromFeatureUri, OWLLiteral toFeatureLiteral, GEOEnums.GeoSpatialRelation relation)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot check spatial relation between features because given '{nameof(ontology)}' parameter is null");
            if (fromFeatureUri == null)
                throw new OWLException($"Cannot check spatial relation between features because given '{nameof(fromFeatureUri)}' parameter is null");
            if (toFeatureLiteral == null)
                throw new OWLException($"Cannot check spatial relation between features because given '{nameof(toFeatureLiteral)}' parameter is null");
            if (!(toFeatureLiteral.GetLiteral() is RDFTypedLiteral toFeatureTypedLiteral) || !toFeatureTypedLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot check spatial relation between features because given '{nameof(toFeatureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            //Collect Azimuthal geometries of "From" feature
            (Geometry, Geometry) defaultGeometryFrom = await ontology.GetDefaultGeometryOfFeatureAsync(fromFeatureUri.GetIRI());
            List<(Geometry, Geometry)> geometriesFrom = await ontology.GetSecondaryGeometriesOfFeatureAsync(fromFeatureUri.GetIRI());
            if (defaultGeometryFrom.Item1 != null && defaultGeometryFrom.Item2 != null)
                geometriesFrom.Insert(0, defaultGeometryFrom);

            //Transform "To" feature into Azimuthal geometry
            bool isWKT = toFeatureTypedLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84GeometryTo = isWKT ? WKTReader.Read(toFeatureTypedLiteral.Value) : GMLReader.Read(toFeatureTypedLiteral.Value);
            wgs84GeometryTo.SRID=4326;
            Geometry lazGeometryTo = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84GeometryTo);

            return geometriesFrom.Any(fromGeom => EvaluateSpatialRelation(fromGeom.Item2, lazGeometryTo, relation));
        }

        /// <summary>
        /// Extracts all the GeoSPARQL features living in the A-BOX of the working ontology, eventually filtering the specified one.<br/>
        /// It gives a dictionary having the detected features as keys and their spatial geometries as corresponding values (both WGS84 and Azimuthal).
        /// </summary>
        internal static async Task<Dictionary<string,List<(Geometry wgs84Geom,Geometry lazGeom)>>> GetFeaturesWithGeometriesAsync(this OWLOntology ontology, RDFResource featureIRI=null)
        {
            Dictionary<string,List<(Geometry,Geometry)>> featuresWithGeometry = new Dictionary<string,List<(Geometry,Geometry)>>();

            #region Facilities
            async Task GetGeometriesAsync(RDFResource featureIdvIRI)
            {
                string featureIRIString = featureIdvIRI.ToString();
                if (!featuresWithGeometry.ContainsKey(featureIRIString))
                    featuresWithGeometry.Add(featureIRIString, new List<(Geometry wgs84Geom, Geometry lazGeom)>());

                //Analyze default geometry of the feature
                (Geometry wgs84Geom, Geometry lazGeom) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureIdvIRI);
                if (defaultGeometry.wgs84Geom != null && defaultGeometry.lazGeom != null)
                    featuresWithGeometry[featureIRIString].Add(defaultGeometry);

                //Analyze secondary geometries of the feature
                List<(Geometry wgs84Geom, Geometry lazGeom)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureIdvIRI);
                if (secondaryGeometries.Count > 0)
                    featuresWithGeometry[featureIRIString].AddRange(secondaryGeometries);
            }
            #endregion

            //If a specific individual of class geosparql:Feature has been requested, we can directly extract its geometries
            if (featureIRI != null
                 && ontology.CheckIsIndividualOf(RDFVocabulary.GEOSPARQL.FEATURE.ToEntity<OWLClass>(), featureIRI.ToEntity<OWLNamedIndividual>()))
            {
                await GetGeometriesAsync(featureIRI);
            }

            //Otherwise we must iterate all individuals of class geosparql:Feature and extract their geometries
            else
            {
                foreach (OWLIndividualExpression featureIdv in ontology.GetIndividualsOf(RDFVocabulary.GEOSPARQL.FEATURE.ToEntity<OWLClass>()))
                    await GetGeometriesAsync(featureIdv.GetIRI());
            }

            return featuresWithGeometry;
        }

        /// <summary>
        /// Extracts the default geometry of the given feature, which is returned both in WGS84 and Azimuthal.
        /// </summary>
        internal static Task<(Geometry wgs84Geom,Geometry lazGeom)> GetDefaultGeometryOfFeatureAsync(this OWLOntology ontology, RDFResource featureUri)
        {
            List<(Geometry, Geometry)> geometries = GetGeometriesOfFeatureByProperty(ontology, featureUri, RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY.ToEntity<OWLObjectProperty>());
            return Task.FromResult(geometries.Count > 0 ? geometries[0] : (null, null));
        }

        /// <summary>
        /// Extracts the secondary geometries of the given feature, which are returned both in WGS84 and Azimuthal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        internal static Task<List<(Geometry wgs84Geom,Geometry lazGeom)>> GetSecondaryGeometriesOfFeatureAsync(this OWLOntology ontology, RDFResource featureUri)
            => Task.FromResult(GetGeometriesOfFeatureByProperty(ontology, featureUri, RDFVocabulary.GEOSPARQL.HAS_GEOMETRY.ToEntity<OWLObjectProperty>()));

        /// <summary>
        /// Extracts, via direct axiom lookup (no SWRL), the WGS84/Azimuthal geometries reachable from the given feature
        /// through the given object property (geosparql:defaultGeometry or geosparql:hasGeometry), preferring WKT over
        /// GML serialization when at least one of the feature's geometries exposes a WKT literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        private static List<(Geometry wgs84Geom, Geometry lazGeom)> GetGeometriesOfFeatureByProperty(OWLOntology ontology, RDFResource featureUri, OWLObjectProperty geometryProperty)
        {
            List<(Geometry, Geometry)> results = new List<(Geometry, Geometry)>();

            //Directly select the geometry individuals reachable from the feature through the given object property
            List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
            List<RDFResource> geometryUris = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, geometryProperty)
                .Where(asn => asn.SourceIndividualExpression.GetIRI().Equals(featureUri))
                .Select(asn => asn.TargetIndividualExpression.GetIRI())
                .ToList();
            if (geometryUris.Count == 0)
                return results;

            //Directly select the asWKT/asGML data assertions of the whole ontology, once
            List<OWLDataPropertyAssertion> dataPropAsns = ontology.GetAssertionAxiomsOfType<OWLDataPropertyAssertion>();
            List<OWLDataPropertyAssertion> wktAsns = OWLAssertionAxiomHelper.SelectDataAssertionsByDPEX(dataPropAsns, RDFVocabulary.GEOSPARQL.AS_WKT.ToEntity<OWLDataProperty>());
            List<OWLDataPropertyAssertion> gmlAsns = OWLAssertionAxiomHelper.SelectDataAssertionsByDPEX(dataPropAsns, RDFVocabulary.GEOSPARQL.AS_GML.ToEntity<OWLDataProperty>());

            bool preferWKT = geometryUris.Any(gUri => wktAsns.Any(asn => asn.IndividualExpression.GetIRI().Equals(gUri)));
            List<OWLDataPropertyAssertion> literalAsns = preferWKT ? wktAsns : gmlAsns;

            foreach (RDFResource geometryUri in geometryUris)
            {
                OWLDataPropertyAssertion literalAsn = literalAsns.FirstOrDefault(asn => asn.IndividualExpression.GetIRI().Equals(geometryUri));
                if (literalAsn == null)
                    continue; //No serialization of the expected kind for this geometry: not an error, just nothing to analyze

                try
                {
                    RDFTypedLiteral literal = (RDFTypedLiteral)literalAsn.Literal.GetLiteral();
                    Geometry wgs84Geometry = preferWKT ? WKTReader.Read(literal.Value) : GMLReader.Read(literal.Value);
                    wgs84Geometry.SRID=4326;
                    wgs84Geometry.UserData = geometryUri.ToString();

                    //Project geometry from WGS84 to Lambert Azimuthal
                    Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);
                    lazGeometry.UserData = geometryUri.ToString();

                    results.Add((wgs84Geometry, lazGeometry));
                }
                catch (Exception ex)
                {
                    throw new OWLException($"Cannot analyze geometry '{geometryUri}' because its {(preferWKT ? "WKT" : "GML")} serialization is malformed", ex);
                }
            }

            return results;
        }

        /// <summary>
        /// Compares the given pair of coordinates according to the specified direction of analysis.
        /// </summary>
        private static bool MatchCoordinates(Coordinate c1, Coordinate c2, GEOEnums.GeoDirections geoDirection)
        {
            switch (geoDirection)
            {
                case GEOEnums.GeoDirections.North:
                    return c1.Y > c2.Y;
                case GEOEnums.GeoDirections.East:
                    return c1.X > c2.X;
                case GEOEnums.GeoDirections.South:
                    return c1.Y < c2.Y;
                case GEOEnums.GeoDirections.West:
                    return c1.X < c2.X;
                case GEOEnums.GeoDirections.NorthEast:
                    return c1.Y > c2.Y && c1.X > c2.X;
                case GEOEnums.GeoDirections.NorthWest:
                    return c1.Y > c2.Y && c1.X < c2.X;
                case GEOEnums.GeoDirections.SouthEast:
                    return c1.Y < c2.Y && c1.X > c2.X;
                case GEOEnums.GeoDirections.SouthWest:
                    return c1.Y < c2.Y && c1.X < c2.X;
            }
            return false;
        }

        /// <summary>
        /// Performs the given type of spatial analysis on the given ontology feature.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        private static async Task<RDFTypedLiteral> AnalyzeFeatureAsync(OWLOntology ontology, RDFResource featureUri, GEOEnums.GeoAnalysis geoAnalysis, double? geoAnalysisParam=null)
        {
            #region Guards
            if (ontology == null)
                throw new OWLException($"Cannot analyze feature because given '{nameof(ontology)}' parameter is null");
            if (featureUri == null)
                throw new OWLException($"Cannot analyze feature because given '{nameof(featureUri)}' parameter is null");
            #endregion

            #region Utilities
            Geometry AnalyzeFeature(Geometry featureToAnalyze)
            {
                switch (geoAnalysis)
                {
                    case GEOEnums.GeoAnalysis.Boundary:
                        return featureToAnalyze.Boundary;
                    case GEOEnums.GeoAnalysis.Buffer:
                        return featureToAnalyze.Buffer(geoAnalysisParam ?? 0);
                    case GEOEnums.GeoAnalysis.Centroid:
                        return featureToAnalyze.Centroid;
                    case GEOEnums.GeoAnalysis.ConvexHull:
                        return featureToAnalyze.ConvexHull();
                    case GEOEnums.GeoAnalysis.Envelope:
                        return featureToAnalyze.Envelope;
                    //Just for compiler purpose...
                    default: return featureToAnalyze;
                }
            }
            #endregion

            //Analyze default geometry of feature
            (Geometry, Geometry) defaultGeometry = await ontology.GetDefaultGeometryOfFeatureAsync(featureUri);
            if (defaultGeometry.Item1 != null && defaultGeometry.Item2 != null)
            {
                Geometry computedGeometryWGS84;
                if (geoAnalysis == GEOEnums.GeoAnalysis.Buffer)
                {
                    //Buffer uses meters: dynamic LAEA for accurate metric distance
                    Point centroid = defaultGeometry.Item1.Centroid;
                    (MathTransform forward, MathTransform inverse) = CreateDynamicLAEATransforms(centroid.X, centroid.Y);
                    Geometry lazGeometry = ApplyTransform(defaultGeometry.Item1, forward);
                    Geometry computedGeometryAZ = AnalyzeFeature(lazGeometry);
                    computedGeometryWGS84 = ApplyTransform(computedGeometryAZ, inverse);
                }
                else
                {
                    //Topological operations: fixed LAEA is adequate
                    Geometry computedGeometryAZ = AnalyzeFeature(defaultGeometry.Item2);
                    computedGeometryWGS84 = RDFGeoConverter.GetWGS84GeometryFromLambertAzimuthal(computedGeometryAZ);
                }
                return new RDFTypedLiteral(WKTWriter.Write(computedGeometryWGS84).Replace("LINEARRING", "LINESTRING"), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT);
            }

            //Analyze secondary geometries of feature
            List<(Geometry, Geometry)> secondaryGeometries = await ontology.GetSecondaryGeometriesOfFeatureAsync(featureUri);
            if (secondaryGeometries.Count > 0)
            {
                Geometry computedGeometryWGS84;
                if (geoAnalysis == GEOEnums.GeoAnalysis.Buffer)
                {
                    Point centroid = secondaryGeometries[0].Item1.Centroid;
                    (MathTransform forward, MathTransform inverse) = CreateDynamicLAEATransforms(centroid.X, centroid.Y);
                    Geometry lazGeometry = ApplyTransform(secondaryGeometries[0].Item1, forward);
                    Geometry computedGeometryAZ = AnalyzeFeature(lazGeometry);
                    computedGeometryWGS84 = ApplyTransform(computedGeometryAZ, inverse);
                }
                else
                {
                    Geometry computedGeometryAZ = AnalyzeFeature(secondaryGeometries[0].Item2);
                    computedGeometryWGS84 = RDFGeoConverter.GetWGS84GeometryFromLambertAzimuthal(computedGeometryAZ);
                }
                return new RDFTypedLiteral(WKTWriter.Write(computedGeometryWGS84).Replace("LINEARRING", "LINESTRING"), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT);
            }

            return null;
        }

        /// <summary>
        /// Performs the given type of spatial analysis on the given geospatial literal.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        private static Task<RDFTypedLiteral> AnalyzeLiteralAsync(RDFTypedLiteral featureLiteral, GEOEnums.GeoAnalysis geoAnalysis, double? geoAnalysisParam=null)
        {
            #region Guards
            if (featureLiteral == null)
                throw new OWLException($"Cannot analyze literal because given '{nameof(featureLiteral)}' parameter is null");
            if (!featureLiteral.HasGeographicDatatype())
                throw new OWLException($"Cannot analyze literal because given '{nameof(featureLiteral)}' parameter is not a geographic typed literal");
            #endregion

            #region Utilities
            Geometry AnalyzeFeature(Geometry featureToAnalyze)
            {
                switch (geoAnalysis)
                {
                    case GEOEnums.GeoAnalysis.Boundary:
                        return featureToAnalyze.Boundary;
                    case GEOEnums.GeoAnalysis.Buffer:
                        return featureToAnalyze.Buffer(geoAnalysisParam ?? 0);
                    case GEOEnums.GeoAnalysis.Centroid:
                        return featureToAnalyze.Centroid;
                    case GEOEnums.GeoAnalysis.ConvexHull:
                        return featureToAnalyze.ConvexHull();
                    case GEOEnums.GeoAnalysis.Envelope:
                        return featureToAnalyze.Envelope;
                    //Just for compiler purpose...
                    default: return featureToAnalyze;
                }
            }
            #endregion

            //Transform feature into WGS84 geometry
            bool isWKT = featureLiteral.Datatype.TargetDatatype == RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT;
            Geometry wgs84Geometry = isWKT ? WKTReader.Read(featureLiteral.Value) : GMLReader.Read(featureLiteral.Value);
            wgs84Geometry.SRID=4326;

            Geometry computedGeometryWGS84;
            if (geoAnalysis == GEOEnums.GeoAnalysis.Buffer)
            {
                //Buffer uses meters: dynamic LAEA for accurate metric distance
                Point centroid = wgs84Geometry.Centroid;
                (MathTransform forward, MathTransform inverse) = CreateDynamicLAEATransforms(centroid.X, centroid.Y);
                Geometry lazGeometry = ApplyTransform(wgs84Geometry, forward);
                Geometry computedGeometryAZ = AnalyzeFeature(lazGeometry);
                computedGeometryWGS84 = ApplyTransform(computedGeometryAZ, inverse);
            }
            else
            {
                //Topological operations: fixed LAEA is adequate
                Geometry lazGeometry = RDFGeoConverter.GetLambertAzimuthalGeometryFromWGS84(wgs84Geometry);
                Geometry computedGeometryAZ = AnalyzeFeature(lazGeometry);
                computedGeometryWGS84 = RDFGeoConverter.GetWGS84GeometryFromLambertAzimuthal(computedGeometryAZ);
            }
            return Task.FromResult(new RDFTypedLiteral(WKTWriter.Write(computedGeometryWGS84).Replace("LINEARRING", "LINESTRING"), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));
        }

        /* DYNAMIC LAEA PROJECTION */

        private static readonly CoordinateSystemFactory CSFactory = new CoordinateSystemFactory();
        private static readonly CoordinateTransformationFactory CTFactory = new CoordinateTransformationFactory();

        /// <summary>
        /// Creates forward (WGS84->LAEA) and inverse (LAEA->WGS84) transforms centered on the given WGS84 point.
        /// </summary>
        private static (MathTransform forward, MathTransform inverse) CreateDynamicLAEATransforms(double centerLon, double centerLat)
        {
            List<ProjectionParameter> parameters = new List<ProjectionParameter>
            {
                new ProjectionParameter("latitude_of_center", centerLat),
                new ProjectionParameter("longitude_of_center", centerLon),
                new ProjectionParameter("false_easting", 0),
                new ProjectionParameter("false_northing", 0)
            };
            IProjection projection = CSFactory.CreateProjection("DynLAEA", "Lambert_Azimuthal_Equal_Area", parameters);
            ProjectedCoordinateSystem projCS = CSFactory.CreateProjectedCoordinateSystem(
                "DynLAEA", GeographicCoordinateSystem.WGS84, projection,
                LinearUnit.Metre,
                new AxisInfo("E", AxisOrientationEnum.East),
                new AxisInfo("N", AxisOrientationEnum.North));
            MathTransform forward = CTFactory.CreateFromCoordinateSystems(GeographicCoordinateSystem.WGS84, projCS).MathTransform;
            MathTransform inverse = CTFactory.CreateFromCoordinateSystems(projCS, GeographicCoordinateSystem.WGS84).MathTransform;
            return (forward, inverse);
        }

        /// <summary>
        /// Applies the given math transform to a copy of the geometry, returning the projected result.
        /// </summary>
        private static Geometry ApplyTransform(Geometry geometry, MathTransform transform)
        {
            Geometry result = geometry.Copy();
            result.Apply(new ProjectionCoordinateFilter(transform));
            result.GeometryChanged();
            return result;
        }

        /// <summary>
        /// Projects a WGS84 geometry to a dynamic LAEA centered on its own centroid, for accurate metric operations.
        /// </summary>
        private static Geometry ProjectToDynamicLAEA(Geometry wgs84Geometry)
        {
            Point centroid = wgs84Geometry.Centroid;
            (MathTransform forward, _) = CreateDynamicLAEATransforms(centroid.X, centroid.Y);
            return ApplyTransform(wgs84Geometry, forward);
        }

        /// <summary>
        /// Projects WGS84 geometries to a shared dynamic LAEA centered on the first geometry's centroid.
        /// </summary>
        private static (Geometry lazA, Geometry lazB) ProjectToDynamicLAEA(Geometry wgs84A, Geometry wgs84B)
        {
            Point centroid = wgs84A.Centroid;
            (MathTransform forward, _) = CreateDynamicLAEATransforms(centroid.X, centroid.Y);
            return (ApplyTransform(wgs84A, forward), ApplyTransform(wgs84B, forward));
        }

        private sealed class ProjectionCoordinateFilter : ICoordinateFilter
        {
            private readonly MathTransform _transform;

            internal ProjectionCoordinateFilter(MathTransform transform)
                => _transform = transform;

            public void Filter(Coordinate coord)
            {
                var (x, y) = _transform.Transform(coord.X, coord.Y);
                coord.X = Math.Round(x, 8);
                coord.Y = Math.Round(y, 8);
            }
        }
        #endregion
    }
}