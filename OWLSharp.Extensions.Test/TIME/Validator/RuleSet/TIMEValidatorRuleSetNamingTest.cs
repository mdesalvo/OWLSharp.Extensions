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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace OWLSharp.Extensions.Test.TIME;

/// <summary>
/// Guards against the copy-paste naming bug where a rule's internal SWRLRule name (used for log/tracing)
/// did not match the actual rule being executed, by checking it against the containing class name.
/// </summary>
[TestClass]
public class TIMEValidatorRuleSetNamingTest
{
    private static string GetRuleSetDirectory([CallerFilePath] string thisFilePath = null)
        => Path.Combine(Path.GetDirectoryName(thisFilePath)!, "..", "..", "..", "..", "OWLSharp.Extensions", "TIME", "Validator", "RuleSet");

    [TestMethod]
    public void ShouldHaveConsistentSWRLRuleNameForEachValidatorRule()
    {
        string ruleSetDirectory = GetRuleSetDirectory();
        Assert.IsTrue(Directory.Exists(ruleSetDirectory), $"RuleSet directory not found at '{ruleSetDirectory}'");

        string[] ruleFiles = Directory.GetFiles(ruleSetDirectory, "*AnalysisRule.cs");
        Assert.IsGreaterThan(0, ruleFiles.Length);

        foreach (string ruleFile in ruleFiles)
        {
            string expectedClassName = Path.GetFileNameWithoutExtension(ruleFile);
            string source = File.ReadAllText(ruleFile);

            //Every "new SWRLRule(" construction must pass, as first argument, a RDFPlainLiteral wrapping
            //nameof(<the rule's own class>) rather than a name copy-pasted from a different rule
            MatchCollection swrlRuleNameMatches = Regex.Matches(source, @"new SWRLRule\(\s*new RDFPlainLiteral\(nameof\((\w+)\)\)");
            Assert.IsGreaterThan(0, swrlRuleNameMatches.Count, $"No SWRLRule construction found in '{expectedClassName}'");

            foreach (Match match in swrlRuleNameMatches)
                Assert.AreEqual(expectedClassName, match.Groups[1].Value, $"SWRLRule internal name mismatch in '{expectedClassName}'");
        }
    }
}
