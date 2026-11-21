# Third-party notices

OWLSharp.Extensions embeds copies of the following ontologies, so that `InitializeSKOSAsync`, `InitializeGEOAsync`
and `InitializeTIMEAsync` work offline. Each copy is byte-identical to its source unless a modification is stated below;
the SHA-256 hash refers to the embedded file.

## SKOS

**SKOS Simple Knowledge Organization System RDF Schema** (`SKOS/Ontologies/skos.rdf`)
- Source: https://www.w3.org/2009/08/skos-reference/skos.rdf (retrieved 2026-11-21)
- SHA-256: `e79633b8d0564816cee8a99f5c9acf9a0e6fc7257c7209acd684ecad53a89dd6`
- Copyright © 2009 World Wide Web Consortium (MIT, ERCIM, Keio, Beihang). https://www.w3.org/copyright/
- License: W3C Document License, https://www.w3.org/copyright/document-license/

**SKOS eXtension for Labels (SKOS-XL) RDF Schema** (`SKOS/Ontologies/skos-xl.rdf`)
- Source: https://www.w3.org/2009/08/skos-reference/skos-xl.rdf (retrieved 2026-11-21)
- SHA-256: `fb6519c5719250d77f17a84ac28c75660c7de09da7286dda138bbe9d8f834b9a`
- Copyright © 2009 World Wide Web Consortium (MIT, ERCIM, Keio, Beihang). https://www.w3.org/copyright/
- License: W3C Document License, https://www.w3.org/copyright/document-license/

## GEO

**OGC GeoSPARQL 1.1 ontology** (`GEO/Ontologies/geosparql.ttl`)
- Source: https://github.com/opengeospatial/ogc-geosparql, tag `1.1.0-ghpages`, file `geosparql11/geo.ttl` (retrieved 2026-11-21)
- SHA-256 of the source: `25e319e0c30c6cf026a5cb1a693ac74a17e7cebc3dcad520b731f77c49719867`
- SHA-256 of the embedded copy: `109ea2b48dd3c22c0a35658dc26fd06b7627349f44a727563aec50336f602936`
- Copyright © 2021 Open Geospatial Consortium. https://www.ogc.org/legal/
- License: OGC Document License Agreement, https://www.ogc.org/license
- **Modification notice:** this copy includes modifications that have not been approved or adopted by the
  Open Geospatial Consortium. The eight `sdo:email` lines describing the contributors were removed, because their values
  (e-mail addresses typed as `xsd:anyURI` without the `mailto:` scheme) are rejected by the RDFSharp Turtle parser.
  No class, property or axiom of the ontology was changed.

**OGC GeoSPARQL 1.1 Simple Features geometries** (`GEO/Ontologies/sf.ttl`)
- Source: https://github.com/opengeospatial/ogc-geosparql, tag `1.1.0-ghpages`, file `geosparql11/sf_geometries.ttl` (retrieved 2026-11-21)
- SHA-256: `92ad3bb64cc2ab9604774758f8521b1088cd07434d22f61b5833a0c2dc872c08`
- Copyright © 2022 Open Geospatial Consortium. https://www.ogc.org/legal/
- License: OGC Document License Agreement, https://www.ogc.org/license

## TIME

**Time Ontology in OWL (OWL-TIME)** (`TIME/Ontologies/time.ttl`)
- Source: https://www.w3.org/2006/time, Turtle serialization (retrieved 2026-11-21)
- SHA-256: `251bd6970b0d7a5efa97ff681afe81a297df85deab3d860fa4f8eeba72d8f16c`
- Copyright © 2006-2017 W3C, OGC. W3C and OGC liability, trademark and document use rules apply.
- License: Creative Commons Attribution 4.0 International, https://creativecommons.org/licenses/by/4.0/

**OWL-TIME Gregorian calendar** (`TIME/Ontologies/gregorian.ttl`)
- Source: https://www.w3.org/ns/time/gregorian, Turtle serialization (retrieved 2026-11-21)
- SHA-256: `b34c62db953d6c9c5a673a1ed0bcdd0c32844877e303238b564ddcea6b13f553`
- Copyright © W3C, OGC. W3C and OGC liability, trademark and document use rules apply.
- License: Creative Commons Attribution 4.0 International, https://creativecommons.org/licenses/by/4.0/ (as OWL-TIME)

**Temporal Hierarchical Ordinal Reference System (THORS)** (`TIME/Ontologies/thors.ttl`)
- Source: https://github.com/CGI-IUGS/timescale-ont, file `rdf/thors.ttl` (retrieved 2026-11-21)
- SHA-256: `5ae3c41d66cb0bad5616719e6ab2a43f81977e78f085758374a53f0f434a1321`
- Copyright 2012-2020 CSIRO
- License: Creative Commons Zero 1.0 Universal (public domain dedication), https://creativecommons.org/publicdomain/zero/1.0/
