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
using OWLSharp.Extensions.SKOS;
using OWLSharp.Ontology;
using OWLSharp.Validator;
using RDFSharp.Model;

namespace OWLSharp.Extensions.Test.SKOS;

[TestClass]
public class SKOSHierarchyCycleAnalysisRuleTest
{
    #region Tests
    [TestMethod]
    public async Task ShouldAnalyzeHierarchyCycleAndViolateRule1A()
    {
        OWLOntology ontology = new OWLOntology
        {
            DeclarationAxioms = [
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>()),
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>()),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.BROADER)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptB")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.BROADER),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.BROADER),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA"))) //cycle
            ]
        };
        List<OWLIssue> issues = await SKOSHierarchyCycleAnalysisRule.ExecuteRuleAsync(ontology);

        Assert.IsNotNull(issues);
        Assert.IsTrue(issues.Count >= 1);
        Assert.IsTrue(issues.Exists(i => i.Severity == OWLEnums.OWLIssueSeverity.Error
            && string.Equals(i.RuleName, SKOSHierarchyCycleAnalysisRule.rulename)
            && string.Equals(i.Description, SKOSHierarchyCycleAnalysisRule.rulesugg1A)));
    }

    [TestMethod]
    public async Task ShouldAnalyzeHierarchyCycleAndViolateRule1B()
    {
        OWLOntology ontology = new OWLOntology
        {
            DeclarationAxioms = [
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>()),
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>()),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptB")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA"))) //cycle
            ]
        };
        List<OWLIssue> issues = await SKOSHierarchyCycleAnalysisRule.ExecuteRuleAsync(ontology);

        Assert.IsNotNull(issues);
        Assert.IsTrue(issues.Count >= 1);
        Assert.IsTrue(issues.Exists(i => i.Severity == OWLEnums.OWLIssueSeverity.Error
            && string.Equals(i.RuleName, SKOSHierarchyCycleAnalysisRule.rulename)
            && string.Equals(i.Description, SKOSHierarchyCycleAnalysisRule.rulesugg1B)));
    }

    [TestMethod]
    public async Task ShouldAnalyzeHierarchyCycleAndViolateRule2A()
    {
        OWLOntology ontology = new OWLOntology
        {
            DeclarationAxioms = [
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>()),
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>()),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptB")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.NARROWER),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA"))) //cycle
            ]
        };
        List<OWLIssue> issues = await SKOSHierarchyCycleAnalysisRule.ExecuteRuleAsync(ontology);

        Assert.IsNotNull(issues);
        Assert.IsTrue(issues.Count >= 1);
        Assert.IsTrue(issues.Exists(i => i.Severity == OWLEnums.OWLIssueSeverity.Error
            && string.Equals(i.RuleName, SKOSHierarchyCycleAnalysisRule.rulename)
            && string.Equals(i.Description, SKOSHierarchyCycleAnalysisRule.rulesugg2A)));
    }

    [TestMethod]
    public async Task ShouldAnalyzeHierarchyCycleAndNotViolate()
    {
        OWLOntology ontology = new OWLOntology
        {
            DeclarationAxioms = [
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>()),
                new OWLDeclaration(RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>()),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME)),
                new OWLDeclaration(new OWLObjectProperty(RDFVocabulary.SKOS.BROADER)),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLDeclaration(new OWLNamedIndividual(new RDFResource("ex:ConceptC")))
            ],
            AssertionAxioms = [
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT_SCHEME.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLClassAssertion(
                    RDFVocabulary.SKOS.CONCEPT.ToEntity<OWLClass>(),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptC"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.IN_SCHEME),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptScheme"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.BROADER),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptA")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB"))),
                new OWLObjectPropertyAssertion(
                    new OWLObjectProperty(RDFVocabulary.SKOS.BROADER),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptB")),
                    new OWLNamedIndividual(new RDFResource("ex:ConceptC"))) //no cycle
            ]
        };
        List<OWLIssue> issues = await SKOSHierarchyCycleAnalysisRule.ExecuteRuleAsync(ontology);

        Assert.IsNotNull(issues);
        Assert.IsEmpty(issues);
    }
    #endregion
}
