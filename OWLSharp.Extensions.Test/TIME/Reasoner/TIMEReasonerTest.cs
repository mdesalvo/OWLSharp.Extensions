/*
   Copyright 2014-2024 Marco De Salvo

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
public class TIMEReasonerTest : TIMETestOntology
{
    [TestMethod]
    public void ShouldAddRule()
    {
        TIMEReasoner reasoner = new TIMEReasoner();

        Assert.IsNotNull(reasoner);
        Assert.IsNotNull(reasoner.Rules);
        Assert.IsEmpty(reasoner.Rules);

        reasoner.AddRule(TIMEEnums.TIMEReasonerRules.EqualsEntailment);
        Assert.HasCount(1, reasoner.Rules);
    }

    [TestMethod]
    public async Task ShouldNotEntailAnythingFromADisjunctiveAllenComposition()
    {
        //Before∘After is one of the two Allen composition cells with no deterministic outcome at all
        //(all 13 base relations remain possible between the outer intervals): I1 before I2, I2 after I3
        //constrains nothing about I1 vs I3, so no rule in the Default reasoner (which covers exactly the
        //97 deterministic cells of the composition table) should fire on this antecedent pair.
        OWLOntology ontology = new OWLOntology(TestOntology);
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature1"), new TIMEInterval(new RDFResource("ex:Interval1")));
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature2"), new TIMEInterval(new RDFResource("ex:Interval2")));
        ontology.DeclareIntervalFeature(new RDFResource("ex:Feature3"), new TIMEInterval(new RDFResource("ex:Interval3")));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.TIME.INTERVAL_BEFORE),
            new OWLNamedIndividual(new RDFResource("ex:Interval1")),
            new OWLNamedIndividual(new RDFResource("ex:Interval2"))));
        ontology.DeclareAssertionAxiom(new OWLObjectPropertyAssertion(
            new OWLObjectProperty(RDFVocabulary.TIME.INTERVAL_AFTER),
            new OWLNamedIndividual(new RDFResource("ex:Interval2")),
            new OWLNamedIndividual(new RDFResource("ex:Interval3"))));

        List<OWLInference> inferences = await TIMEReasoner.Default.ApplyToOntologyAsync(ontology);

        Assert.IsNotNull(inferences);
        Assert.IsEmpty(inferences);
    }
}