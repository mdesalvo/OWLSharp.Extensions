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
using RDFSharp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// Shared entailment helper backing the OWL-TIME reasoner rules, replacing per-rule SWRL antecedent/consequent
    /// scaffolding with a direct axiom join: given "?I1 propertyR1 ?I2" and "?I2 propertyR2 ?I3" (I1,I2,I3 pairwise
    /// distinct), it entails "?I1 propertyResult ?I3". This covers both Allen composition (propertyR1 != propertyR2)
    /// and self-transitivity (propertyR1 == propertyR2 == propertyResult) of a single relation.
    /// </summary>
    internal static class TIMEReasonerHelper
    {
        internal static Task<List<OWLInference>> ComposeRelationsAsync(OWLOntology ontology, string ruleName,
            RDFResource propertyR1, RDFResource propertyR2, RDFResource propertyResult, bool alsoInverse = false)
        {
            List<OWLInference> inferences = new List<OWLInference>();

            List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
            List<OWLObjectPropertyAssertion> r1Asns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, propertyR1.ToEntity<OWLObjectProperty>());
            List<OWLObjectPropertyAssertion> r2Asns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, propertyR2.ToEntity<OWLObjectProperty>());

            //Index the R2 assertions by their source individual, to join them against R1 assertions on the shared middle individual (?I2)
            ILookup<RDFResource, RDFResource> r2BySource = r2Asns.ToLookup(
                asn => asn.SourceIndividualExpression.GetIRI(),
                asn => asn.TargetIndividualExpression.GetIRI());

            foreach (OWLObjectPropertyAssertion r1Asn in r1Asns)
            {
                RDFResource i1 = r1Asn.SourceIndividualExpression.GetIRI();
                RDFResource i2 = r1Asn.TargetIndividualExpression.GetIRI();

                foreach (RDFResource i3 in r2BySource[i2])
                {
                    if (i1.Equals(i2) || i1.Equals(i3) || i2.Equals(i3))
                        continue;

                    inferences.Add(new OWLInference(ruleName,
                        new OWLObjectPropertyAssertion(
                            new OWLObjectProperty(propertyResult),
                            new OWLNamedIndividual(i1),
                            new OWLNamedIndividual(i3))));

                    if (alsoInverse)
                        inferences.Add(new OWLInference(ruleName,
                            new OWLObjectPropertyAssertion(
                                new OWLObjectProperty(propertyResult),
                                new OWLNamedIndividual(i3),
                                new OWLNamedIndividual(i1))));
                }
            }

            return Task.FromResult(inferences);
        }
    }
}
