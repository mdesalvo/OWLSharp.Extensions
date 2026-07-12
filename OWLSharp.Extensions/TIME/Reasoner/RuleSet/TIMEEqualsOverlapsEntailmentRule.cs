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
using OWLSharp.Ontology;
using OWLSharp.Reasoner;
using RDFSharp.Model;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// EQUALS(?I1,?I2) ^ OVERLAPS(?I2,?I3) -&gt; OVERLAPS(?I1,?I3)
    /// </summary>
    internal static class TIMEEqualsOverlapsEntailmentRule
    {
        internal static Task<List<OWLInference>> ExecuteRuleAsync(OWLOntology ontology)
            => TIMEReasonerHelper.ComposeRelationsAsync(ontology, nameof(TIMEEqualsOverlapsEntailmentRule),
                RDFVocabulary.TIME.INTERVAL_EQUALS, RDFVocabulary.TIME.INTERVAL_OVERLAPS, RDFVocabulary.TIME.INTERVAL_OVERLAPS);
    }
}
