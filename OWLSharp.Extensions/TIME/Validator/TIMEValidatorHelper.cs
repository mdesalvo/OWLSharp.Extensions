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
using OWLSharp.Validator;
using RDFSharp.Model;
using System.Collections.Generic;
using System.Linq;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// Shared clash-detection helper backing the OWL-TIME validator rules, replacing per-rule SWRL antecedent/consequent
    /// scaffolding with a direct axiom check: given "?I1 propertyA ?I2", flags an issue for every pair also found in
    /// "?I1 propertyB ?I2" (or "?I2 propertyB ?I1" when <paramref name="invertB"/> is set), I1/I2 distinct.
    /// </summary>
    internal static class TIMEValidatorHelper
    {
        internal static List<OWLIssue> CheckRelationClash(OWLOntology ontology, string ruleName, string ruleSuggestion,
            RDFResource propertyA, RDFResource propertyB, bool invertB, string entityKindPlural, string clashPhrase)
        {
            List<OWLIssue> issues = new List<OWLIssue>();

            List<OWLObjectPropertyAssertion> objPropAsns = OWLAssertionAxiomHelper.CalibrateObjectAssertions(ontology);
            List<OWLObjectPropertyAssertion> aAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, propertyA.ToEntity<OWLObjectProperty>());
            List<OWLObjectPropertyAssertion> bAsns = OWLAssertionAxiomHelper.SelectObjectAssertionsByOPEX(objPropAsns, propertyB.ToEntity<OWLObjectProperty>());

            HashSet<(RDFResource, RDFResource)> bPairs = new HashSet<(RDFResource, RDFResource)>(
                bAsns.Select(asn => invertB
                    ? (asn.TargetIndividualExpression.GetIRI(), asn.SourceIndividualExpression.GetIRI())
                    : (asn.SourceIndividualExpression.GetIRI(), asn.TargetIndividualExpression.GetIRI())));

            foreach (OWLObjectPropertyAssertion aAsn in aAsns)
            {
                RDFResource i1 = aAsn.SourceIndividualExpression.GetIRI();
                RDFResource i2 = aAsn.TargetIndividualExpression.GetIRI();

                if (i1.Equals(i2))
                    continue;

                if (bPairs.Contains((i1, i2)))
                    issues.Add(new OWLIssue(
                        OWLEnums.OWLIssueSeverity.Error,
                        ruleName,
                        ruleSuggestion,
                        $"TIME {entityKindPlural} '{i1}' and '{i2}' should be adjusted to not clash on temporal relations ({clashPhrase})"));
            }

            return issues;
        }
    }
}
