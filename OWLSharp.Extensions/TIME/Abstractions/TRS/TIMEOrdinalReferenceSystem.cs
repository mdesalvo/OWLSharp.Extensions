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
using System.Collections.Generic;
using System.Linq;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// Represents an ordinal temporal reference system that organizes time through ordered, named temporal eras or periods
    /// rather than numeric coordinates, following the THORS (Temporal Hierarchical Ordinal Reference System) ontology.
    /// It defines sequences of discrete temporal intervals (epochs, geological eras, dynasties, reigns)
    /// where positions are identified by their ordinal placement and hierarchical relationships within the sequence
    /// rather than by numeric measurement. THORS provides the semantic framework for expressing these ordered temporal
    /// structures and their inter-relationships.
    /// </summary>
    public sealed class TIMEOrdinalReferenceSystem : TIMEReferenceSystem
    {
        #region Properties
        /// <summary>
        /// Singleton instance of the THORS ontology, to be available whenever
        /// any new instance of an ordinal TRS is going to be created
        /// </summary>
        private static OWLOntology THORSOntology { get; set; }

        /// <summary>
        /// The ontology mapping the T-BOX and A-BOX of this ordinal TRS
        /// </summary>
        public OWLOntology Ontology { get; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an ordinal TRS with the given name. It automatically
        /// includes the T-BOX and A-BOX of the THORS ontology.
        /// </summary>
        public TIMEOrdinalReferenceSystem(RDFResource trsIRI) : base(trsIRI)
        {
            #region Initialize
            if (THORSOntology == null)
            {
                THORSOntology = new OWLOntology();
                THORSOntology.InitializeTIMEAsync(30000).GetAwaiter().GetResult();
            }
            #endregion

            Ontology = new OWLOntology(THORSOntology) { IRI = trsIRI.ToString()};
        }

        /// <summary>
        /// Builds an ordinal TRS with the given name and cloning the ontology knowledge from the given one
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEOrdinalReferenceSystem(RDFResource trsIRI, TIMEOrdinalReferenceSystem ordinalTRS) : base(trsIRI)
            => Ontology = ordinalTRS?.Ontology ?? throw new OWLException($"Cannot create ordinal TRS because given '{nameof(ordinalTRS)}' parameter is null");
        #endregion

        #region Methods

        #region Declarer
        /// <summary>
        /// Injects the A-BOX axioms for declaring the existence of a thors:Era individual with the given name
        /// and the given time:Instant individuals being the temporal coordinates indicating its formal begin/end
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEOrdinalReferenceSystem DeclareEra(RDFResource era, TIMEInstant eraBeginning, TIMEInstant eraEnd)
        {
            #region Guards
            if (era == null)
                throw new OWLException($"Cannot declare era to ordinal TRS because given '{nameof(era)}' parameter is null");
            if (eraBeginning == null)
                throw new OWLException($"Cannot declare era to ordinal TRS because given '{nameof(eraBeginning)}'parameter is null");
            if (eraEnd == null)
                throw new OWLException($"Cannot declare era to ordinal TRS because given '{nameof(eraEnd)}' parameter is null");
            #endregion

            //Add knowledge to the A-BOX (era)
            Ontology.DeclareEntity(new OWLNamedIndividual(era));
            Ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.TIME.THORS.ERA.ToEntity<OWLClass>(),
                new OWLNamedIndividual(era)));
            Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                RDFVocabulary.TIME.THORS.COMPONENT.ToEntity<OWLObjectProperty>(),
                new OWLNamedIndividual(this),
                new OWLNamedIndividual(era)));
            Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                RDFVocabulary.TIME.THORS.SYSTEM.ToEntity<OWLObjectProperty>(),
                new OWLNamedIndividual(era),
                new OWLNamedIndividual(this))); //inference

            //Add knowledge to the A-BOX (begin)
            Ontology.DeclareInstantFeatureInternal(eraBeginning);
            Ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.TIME.THORS.ERA_BOUNDARY.ToEntity<OWLClass>(),
                new OWLNamedIndividual(eraBeginning)));
            Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                RDFVocabulary.TIME.THORS.BEGIN.ToEntity<OWLObjectProperty>(),
                new OWLNamedIndividual(era),
                new OWLNamedIndividual(eraBeginning)));
            Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                RDFVocabulary.TIME.THORS.NEXT_ERA.ToEntity<OWLObjectProperty>(),
                new OWLNamedIndividual(eraBeginning),
                new OWLNamedIndividual(era))); //inference

            //Add knowledge to the A-BOX (end)
            Ontology.DeclareInstantFeatureInternal(eraEnd);
            Ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.TIME.THORS.ERA_BOUNDARY.ToEntity<OWLClass>(),
                new OWLNamedIndividual(eraEnd)));
            Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                RDFVocabulary.TIME.THORS.END.ToEntity<OWLObjectProperty>(),
                new OWLNamedIndividual(era),
                new OWLNamedIndividual(eraEnd)));
            Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                RDFVocabulary.TIME.THORS.PREVIOUS_ERA.ToEntity<OWLObjectProperty>(),
                new OWLNamedIndividual(eraEnd),
                new OWLNamedIndividual(era))); //inference

            return this;
        }

        /// <summary>
        /// Injects the A-BOX axioms for declaring the formal thors:Member hierarchical relationship
        /// between the given thors:Era individuals of this ordinal TRS
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEOrdinalReferenceSystem DeclareSubEra(RDFResource subEra, RDFResource superEra)
        {
            #region Guards
            if (subEra == null)
                throw new OWLException($"Cannot declare sub-era to ordinal TRS because given '{nameof(subEra)}' parameter is null");
            if (superEra == null)
                throw new OWLException($"Cannot declare sub-era to ordinal TRS because given '{nameof(superEra)}' parameter is null");
            if (CheckIsSubEraOf(superEra, subEra))
                throw new OWLException($"Cannot declare sub-era to ordinal TRS because given '{nameof(superEra)}' parameter is already defined as sub-era of the given '{nameof(subEra)}' parameter!");
            #endregion

            //Add knowledge to the A-BOX
            Ontology.DeclareEntity(new OWLNamedIndividual(subEra));
            Ontology.DeclareEntity(new OWLNamedIndividual(superEra));
            Ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.TIME.THORS.ERA.ToEntity<OWLClass>(),
                new OWLNamedIndividual(subEra)));
            Ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                RDFVocabulary.TIME.THORS.ERA.ToEntity<OWLClass>(),
                new OWLNamedIndividual(superEra)));
            Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                RDFVocabulary.TIME.THORS.MEMBER.ToEntity<OWLObjectProperty>(),
                new OWLNamedIndividual(superEra),
                new OWLNamedIndividual(subEra)));

            return this;
        }

        /// <summary>
        /// Injects the A-BOX axioms for giving a set of thors:EraBoundary individuals (at least 2) to this ordinal TRS.
        /// Reference points enable ordinal systems to be anchored to absolute time (via numeric dates) while maintaining
        /// their hierarchical ordinal structure. They bridge the gap between ordinal sequences (named eras)
        /// and positional/calendar systems (numeric dates).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEOrdinalReferenceSystem DeclareReferencePoints(TIMEInstant[] referencePoints)
        {
            #region Guards
            if (referencePoints == null)
                throw new OWLException($"Cannot declare reference points to ordinal TRS because given '{nameof(referencePoints)}' parameter is null");
            if (referencePoints.Any(rp => rp == null))
                throw new OWLException($"Cannot declare reference points to ordinal TRS because given '{nameof(referencePoints)}' parameter contains null elements");
            if (referencePoints.Length < 2)
                throw new OWLException($"Cannot declare reference points to ordinal TRS because given '{nameof(referencePoints)}' parameter must contain at least 2 elements");
            #endregion

            //Add knowledge to the A-BOX
            foreach (TIMEInstant referencePoint in referencePoints)
            {
                Ontology.DeclareInstantFeatureInternal(referencePoint);
                Ontology.DeclareAssertionAxiom(new OWLClassAssertion(
                    RDFVocabulary.TIME.THORS.ERA_BOUNDARY.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(referencePoint)));
                Ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
                    RDFVocabulary.TIME.THORS.REFERENCE_POINT.ToEntity<OWLObjectProperty>(),
                    new OWLNamedIndividual(this),
                    new OWLNamedIndividual(referencePoint)));
            }

            return this;
        }
        #endregion

        #region Analyzer
        /// <summary>
        /// Checks if this ordinal TRS contains a thors:Era individual with the given name
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public bool CheckHasEra(RDFResource era)
        {
            #region Guards
            if (era == null)
                throw new OWLException($"Cannot check if ordinal TRS has era because given '{nameof(era)}' parameter is null");
            #endregion

            return Ontology.GetIndividualsOf(new OWLClass(RDFVocabulary.TIME.THORS.ERA))
                           .Any(idv => idv.GetIRI().Equals(era));
        }

        /// <summary>
        /// Checks if this ordinal TRS contains a thors:EraBoundary individual with the given name
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public bool CheckHasEraBoundary(RDFResource eraBoundary)
        {
            #region Guards
            if (eraBoundary == null)
                throw new OWLException($"Cannot check if ordinal TRS has era boundary because given '{nameof(eraBoundary)}' parameter is null");
            #endregion

            return Ontology.GetIndividualsOf(new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY))
                           .Any(idv => idv.GetIRI().Equals(eraBoundary));
        }

        /// <summary>
        /// Checks if this ordinal TRS contains a thors:EraBoundary individual with the given name,
        /// which is so notable to be also declared as one of its thors:referencePoint instants
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public bool CheckHasReferencePoint(RDFResource referencePoint)
        {
            #region Guards
            if (referencePoint == null)
                throw new OWLException($"Cannot check if ordinal TRS has reference point because given '{nameof(referencePoint)}' parameter is null");
            #endregion

            return Ontology.GetIndividualsOf(new OWLClass(RDFVocabulary.TIME.THORS.ERA_BOUNDARY))
                           .Any(idv => Ontology.CheckHasAssertionAxiom(new OWLObjectPropertyAssertion(new OWLObjectProperty(RDFVocabulary.TIME.THORS.REFERENCE_POINT), new OWLNamedIndividual(this), idv))
                                        && idv.GetIRI().Equals(referencePoint));
        }

        /// <summary>
        /// Checks if this ordinal TRS contains the given thors:Era individuals and that
        /// the have a formal thors:Member hierarchical relationship
        /// </summary>
        public bool CheckIsSubEraOf(RDFResource subEra, RDFResource superEra, bool enableReasoning=true)
            => subEra != null && superEra != null && GetSubErasOf(superEra, enableReasoning).Any(e => e.Equals(subEra));

        /// <summary>
        /// Checks if this ordinal TRS contains the given thors:Era individuals and that
        /// the have a formal thors:Member hierarchical relationship
        /// </summary>
        public bool CheckIsSuperEraOf(RDFResource superEra, RDFResource subEra, bool enableReasoning=true)
            => superEra != null && subEra != null && GetSuperErasOf(subEra, enableReasoning).Any(e => e.Equals(superEra));

        /// <summary>
        /// Enlists the thors:Era individuals having a formal thors:Member hierarchical relationship with the given one
        /// in this ordinal TRS (in this case they must be sub-eras of it)
        /// </summary>
        public List<RDFResource> GetSubErasOf(RDFResource era, bool enableReasoning=true)
        {
            List<RDFResource> subEras = new List<RDFResource>();

            if (era != null)
            {
                //Temporary working variables
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(Ontology);
                List<OWLObjectPropertyAssertion> thorsMemberObjPropAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.TIME.THORS.MEMBER.ToEntity<OWLObjectProperty>());

                //Reason on the given era
                subEras.AddRange(FindSubErasOf(era, thorsMemberObjPropAsns, new Dictionary<long, RDFResource>(), enableReasoning));

                //We don't want to also enlist the given thors:Era
                subEras.RemoveAll(cls => cls.Equals(era));
            }

            return RDFQueryUtilities.RemoveDuplicates(subEras);
        }
        internal List<RDFResource> FindSubErasOf(RDFResource era, List<OWLObjectPropertyAssertion> thorsMemberObjPropAsns, Dictionary<long, RDFResource> visitContext, bool enableReasoning)
        {
            List<RDFResource> subEras = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(era.PatternMemberID))
                visitContext.Add(era.PatternMemberID, era);
            else
                return subEras;
            #endregion

            // DIRECT: thors:member(A,B)
            subEras.AddRange(thorsMemberObjPropAsns.Where(asn  => asn.SourceIndividualExpression.GetIRI().Equals(era))
                                                   .Select(asn => asn.TargetIndividualExpression.GetIRI()));

            // INDIRECT: thors:member(A,B) ^ thors:member(B,C) -> thors:member(A,C)
            if (enableReasoning)
            {
                foreach (RDFResource subEra in subEras.ToList())
                    subEras.AddRange(FindSubErasOf(subEra, thorsMemberObjPropAsns, visitContext, true));
            }

            return subEras;
        }

        /// <summary>
        /// Enlists the thors:Era individuals having a formal thors:Member hierarchical relationship with the given one
        /// in this ordinal TRS (in this case they must be super-eras of it)
        /// </summary>
        public List<RDFResource> GetSuperErasOf(RDFResource era, bool enableReasoning=true)
        {
            List<RDFResource> superEras = new List<RDFResource>();

            if (era != null)
            {
                //Temporary working variables
                List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(Ontology);
                List<OWLObjectPropertyAssertion> thorsMemberObjPropAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.TIME.THORS.MEMBER.ToEntity<OWLObjectProperty>());

                //Reason on the given era
                superEras.AddRange(FindSuperErasOf(era, thorsMemberObjPropAsns, new Dictionary<long, RDFResource>(), enableReasoning));

                //We don't want to also enlist the given thors:Era
                superEras.RemoveAll(cls => cls.Equals(era));
            }

            return RDFQueryUtilities.RemoveDuplicates(superEras);
        }
        internal List<RDFResource> FindSuperErasOf(RDFResource era, List<OWLObjectPropertyAssertion> thorsMemberObjPropAsns, Dictionary<long, RDFResource> visitContext, bool enableReasoning)
        {
            List<RDFResource> superEras = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(era.PatternMemberID))
                visitContext.Add(era.PatternMemberID, era);
            else
                return superEras;
            #endregion

            // DIRECT: thors:member(A,B)
            superEras.AddRange(thorsMemberObjPropAsns.Where(asn  => asn.TargetIndividualExpression.GetIRI().Equals(era))
                                                     .Select(asn => asn.SourceIndividualExpression.GetIRI()));

            // INDIRECT: thors:member(A,B) ^ thors:member(B,C) -> thors:member(A,C)
            if (enableReasoning)
            {
                foreach (RDFResource superEra in superEras.ToList())
                    superEras.AddRange(FindSuperErasOf(superEra, thorsMemberObjPropAsns, visitContext, true));
            }

            return superEras;
        }

        /// <summary>
        /// Gets the temporal coordinates of the begin/end instants of the given thors:Era individual in this ordinal TRS
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public (TIMECoordinate,TIMECoordinate) GetEraCoordinates(RDFResource era, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (era == null)
                throw new OWLException($"Cannot get coordinates of era because given '{nameof(era)}' parameter is null");
            if (!CheckHasEra(era))
                throw new OWLException($"Cannot get coordinates of era because given '{nameof(era)}' parameter is not declared as era of this ordinal TRS");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            //Temporary working variables
            List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(Ontology);
            List<OWLObjectPropertyAssertion> thorsBeginObjPropAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.TIME.THORS.BEGIN.ToEntity<OWLObjectProperty>());
            List<OWLObjectPropertyAssertion> thorsEndObjPropAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, RDFVocabulary.TIME.THORS.END.ToEntity<OWLObjectProperty>());

            //Get begin boundary of era (if correctly declared to the ordinal TRS through THORS semantics)
            TIMECoordinate eraBeginBoundaryCoordinate = null;
            OWLObjectPropertyAssertion thorsBeginEraAsn = thorsBeginObjPropAsns.FirstOrDefault(asn => asn.SourceIndividualExpression.GetIRI().Equals(era)
                                                           && CheckHasEraBoundary(asn.TargetIndividualExpression.GetIRI()));
            if (thorsBeginEraAsn != null)
                eraBeginBoundaryCoordinate = Ontology.GetCoordinateOfInstant(thorsBeginEraAsn.TargetIndividualExpression.GetIRI(), calendarTRS);

            //Get end boundary of era (if correctly declared to the ordinal TRS through THORS semantics)
            TIMECoordinate eraEndBoundaryCoordinate = null;
            OWLObjectPropertyAssertion thorsEndEraAsn = thorsEndObjPropAsns.FirstOrDefault(asn => asn.SourceIndividualExpression.GetIRI().Equals(era)
                                                         && CheckHasEraBoundary(asn.TargetIndividualExpression.GetIRI()));
            if (thorsEndEraAsn != null)
                eraEndBoundaryCoordinate = Ontology.GetCoordinateOfInstant(thorsEndEraAsn.TargetIndividualExpression.GetIRI(), calendarTRS);

            return (eraBeginBoundaryCoordinate, eraEndBoundaryCoordinate);
        }

        /// <summary>
        /// Gets the temporal extent of the given thors:Era individual in this ordinal TRS.
        /// It is based on the computation of its coordinates (see GetEraCoordinates).
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public TIMEExtent GetEraExtent(RDFResource era, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (era == null)
                throw new OWLException($"Cannot get extent of era because given '{nameof(era)}' parameter is null");
            if (!CheckHasEra(era))
                throw new OWLException($"Cannot get extent of era because given '{nameof(era)}' parameter is not declared as era of this ordinal TRS");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            //Get coordinates of era (if correctly declared to the ordinal TRS with THORS semantics)
            (TIMECoordinate,TIMECoordinate) eraCoordinates = GetEraCoordinates(era, calendarTRS);

            //Get extent of era
            if (eraCoordinates.Item1 != null && eraCoordinates.Item2 != null)
                return TIMEConverter.ExtentBetweenCoordinates(eraCoordinates.Item1, eraCoordinates.Item2, calendarTRS);

            return null;
        }
        #endregion

        #endregion
    }
}