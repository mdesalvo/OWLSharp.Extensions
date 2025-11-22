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

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NetTopologySuite.Geometries;
using RDFSharp.Model;

namespace OWLSharp.Extensions.GEO
{
    /// <summary>
    /// Represents the spatial dimension of a GeoSPARQL feature
    /// </summary>
    public class GEOEntity : RDFResource
    {
        #region Properties
        internal Geometry WGS84Geometry { get;set; }
        #endregion

        #region Ctors
        internal GEOEntity(RDFResource geoEntityUri)
            : base(geoEntityUri?.ToString()) { }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the WKT representation of the spatial entity
        /// </summary>
        public string ToWKT()
            => GEOHelper.WKTWriter.Write(WGS84Geometry);

        /// <summary>
        /// Gets the GML representation of the spatial entity
        /// </summary>
        public string ToGML()
        {
            using (XmlReader gmlReader = GEOHelper.GMLWriter.Write(WGS84Geometry))
            {
                XmlDocument gmlDocument = new XmlDocument();
                gmlDocument.Load(gmlReader);
                return gmlDocument.InnerXml;
            }
        }
        #endregion
    }

    /// <summary>
    /// Represents a spatial entity specialized at encoding a WGS84 point
    /// </summary>
    public sealed class GEOPoint : GEOEntity
    {
        #region Ctors
        /// <summary>
        /// Builds a spatial entity encoding a point having the given name and WGS84 coordinates
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public GEOPoint(RDFResource geoEntityUri, (double longitude, double latitude) wgs84Coordinate) : base(geoEntityUri)
        {
            #region Guards
            if (wgs84Coordinate.longitude < -180 || wgs84Coordinate.longitude > 180)
                throw new OWLException($"Cannot declare point entity because given '{nameof(wgs84Coordinate)}' parameter has not a valid WGS84 longitude");
            if (wgs84Coordinate.latitude < -90 || wgs84Coordinate.latitude > 90)
                throw new OWLException($"Cannot declare point entity because given '{nameof(wgs84Coordinate)}' parameter has not a valid WGS84 latitude");
            #endregion

            WGS84Geometry = new Point(wgs84Coordinate.longitude, wgs84Coordinate.latitude) { SRID=4326 };
        }
        #endregion
    }

    /// <summary>
    /// Represents a spatial entity specialized at encoding a WGS84 linestring
    /// </summary>
    public sealed class GEOLine : GEOEntity
    {
        #region Ctors
        /// <summary>
        /// Builds a spatial entity encoding a linestring having the given name and WGS84 coordinates (at least 2 required)
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public GEOLine(RDFResource geoEntityUri, (double longitude, double latitude)[] wgs84Coordinates) : base(geoEntityUri)
        {
            #region Guards
            if (wgs84Coordinates == null)
                throw new OWLException($"Cannot declare line entity because given '{nameof(wgs84Coordinates)}' parameter is null");
            if (wgs84Coordinates.Length < 2)
                throw new OWLException($"Cannot declare line entity because given '{nameof(wgs84Coordinates)}' parameter contains less than 2 points");
            if (wgs84Coordinates.Any(pt => pt.longitude < -180 || pt.longitude > 180))
                throw new OWLException($"Cannot declare line entity because given '{nameof(wgs84Coordinates)}' parameter contains a point with invalid WGS84 longitude");
            if (wgs84Coordinates.Any(pt => pt.latitude < -90 || pt.latitude > 90))
                throw new OWLException($"Cannot declare line entity because given '{nameof(wgs84Coordinates)}' parameter contains a point with invalid WGS84 latitude");
            #endregion

            WGS84Geometry = new LineString(wgs84Coordinates.Select(wgs84Point => new Coordinate(wgs84Point.longitude, wgs84Point.latitude)).ToArray()) { SRID=4326 };
        }
        #endregion
    }

    /// <summary>
    /// Represents a spatial entity specialized at encoding a WGS84 polygon
    /// </summary>
    public sealed class GEOArea : GEOEntity
    {
        #region Ctors
        /// <summary>
        /// Builds a spatial entity encoding a polygon having the given name and WGS84 coordinates (at least 3 required)
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public GEOArea(RDFResource geoEntityUri, (double longitude, double latitude)[] wgs84Coordinates) : base(geoEntityUri)
        {
            #region Guards
            if (wgs84Coordinates == null)
                throw new OWLException($"Cannot declare area entity because given '{nameof(wgs84Coordinates)}' parameter is null");
            if (wgs84Coordinates.Length < 3)
                throw new OWLException($"Cannot declare area entity because given '{nameof(wgs84Coordinates)}' parameter contains less than 3 points");
            if (wgs84Coordinates.Any(pt => pt.longitude < -180 || pt.longitude > 180))
                throw new OWLException($"Cannot declare area entity because given '{nameof(wgs84Coordinates)}' parameter contains a point with invalid WGS84 longitude");
            if (wgs84Coordinates.Any(pt => pt.latitude < -90 || pt.latitude > 90))
                throw new OWLException($"Cannot declare area entity because given '{nameof(wgs84Coordinates)}' parameter contains a point with invalid WGS84 latitude");
            #endregion

            //Automatically close polygon (if the last coordinate does not match with the first one)
            Point openingPoint = new Point(new Coordinate(wgs84Coordinates[0].longitude, wgs84Coordinates[0].latitude)) { SRID=4326 };
            Point closingPoint = new Point(new Coordinate(wgs84Coordinates[wgs84Coordinates.Length - 1].longitude, wgs84Coordinates[wgs84Coordinates.Length - 1].latitude)) { SRID=4326 };
            if (!openingPoint.EqualsExact(closingPoint))
            {
                List<(double, double)> wgs84CoordinatesList = wgs84Coordinates.ToList();
                wgs84CoordinatesList.Add(wgs84Coordinates[0]);
                wgs84Coordinates = wgs84CoordinatesList.ToArray();
            }

            WGS84Geometry = new Polygon(new LinearRing(wgs84Coordinates.Select(wgs84Point => new Coordinate(wgs84Point.longitude, wgs84Point.latitude)).ToArray())) { SRID=4326 };
        }
        #endregion
    }
}