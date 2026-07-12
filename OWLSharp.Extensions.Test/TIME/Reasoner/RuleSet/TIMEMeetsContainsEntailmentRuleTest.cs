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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OWLSharp.Extensions.TIME;
using OWLSharp.Ontology;
using OWLSharp.Reasoner;
using RDFSharp.Model;

namespace OWLSharp.Extensions.Test.TIME;

[TestClass]
public class TIMEMeetsContainsEntailmentRuleTest : TIMETestOntology
{
    #region Tests
    [TestMethod]
    public async Task ShouldExecuteTIMEMeetsContainsEntailmentRule()
    {
        OWLOntology ontology = new OWLOntology(TestOntology);
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature1"), new TIMEInterval(new RDFResource("ex:Interval1")));
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature2"), new TIMEInterval(new RDFResource("ex:Interval2")));
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature3"), new TIMEInterval(new RDFResource("ex:Interval3")));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.TIME.INTERVAL_MEETS),
            new OWLNamedIndividual(new RDFResource("ex:Interval1")),
            new OWLNamedIndividual(new RDFResource("ex:Interval2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.TIME.INTERVAL_CONTAINS),
            new OWLNamedIndividual(new RDFResource("ex:Interval2")),
            new OWLNamedIndividual(new RDFResource("ex:Interval3"))));
        List<OWLInference> inferences = await TIMEMeetsContainsEntailmentRule.ExecuteRuleAsync(ontology);

        Assert.IsNotNull(inferences);
        Assert.HasCount(1, inferences);
        Assert.IsTrue(((OWLObjectPropertyAssertion)inferences[0].Axiom).ObjectPropertyExpression.GetIRI().Equals(RDFVocabulary.TIME.INTERVAL_BEFORE));
        Assert.IsTrue(((OWLObjectPropertyAssertion)inferences[0].Axiom).SourceIndividualExpression.GetIRI().Equals(new RDFResource("ex:Interval1")));
        Assert.IsTrue(((OWLObjectPropertyAssertion)inferences[0].Axiom).TargetIndividualExpression.GetIRI().Equals(new RDFResource("ex:Interval3")));
    }

    [TestMethod]
    public async Task ShouldExecuteTIMEMeetsContainsEntailmentRuleViaReasoner()
    {
        OWLOntology ontology = new OWLOntology(TestOntology);
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature1"), new TIMEInterval(new RDFResource("ex:Interval1")));
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature2"), new TIMEInterval(new RDFResource("ex:Interval2")));
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature3"), new TIMEInterval(new RDFResource("ex:Interval3")));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.TIME.INTERVAL_MEETS),
            new OWLNamedIndividual(new RDFResource("ex:Interval1")),
            new OWLNamedIndividual(new RDFResource("ex:Interval2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.TIME.INTERVAL_CONTAINS),
            new OWLNamedIndividual(new RDFResource("ex:Interval2")),
            new OWLNamedIndividual(new RDFResource("ex:Interval3"))));

        TIMEReasoner reasoner = new TIMEReasoner().AddRule(TIMEEnums.TIMEReasonerRules.MeetsContainsEntailment);
        List<OWLInference> inferences = await reasoner.ApplyToOntologyAsync(ontology);

        Assert.IsNotNull(inferences);
        Assert.HasCount(1, inferences);
    }
    #endregion
}
