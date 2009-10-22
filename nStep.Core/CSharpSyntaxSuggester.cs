using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using nStep.Framework.Features;
using nStep.Framework.StepDefinitions;

namespace nStep.Core
{
    public class CSharpSyntaxSuggester : ISuggestSyntax
    {
        public string TurnFeatureIntoSnippet(Step step)
        {
            var argValues = new[] { "string arg1", "string arg2", "string arg3", "string arg4", "string arg5" };
            const string parameterPattern = "\"[^\"]*\"";
            const string stepPatern = "^(Given|When|Then|And|But)(.*)$";

            var paramCount = new Regex(parameterPattern).Matches(step.FeatureLine).Count;
            string paramText;
            if (step.Table == null)
                paramText = string.Format("({0})", string.Join(", ", argValues, 0, paramCount));
            else
            {
                argValues[paramCount] = "Table table";
                paramText = string.Format("({0})", string.Join(", ", argValues, 0, paramCount + 1));
            }

            var replacedFeatureLine = Regex.Replace(step.FeatureLine, parameterPattern, "\\\"([^\\\"]*)\\\"");

            var matchedTemplate = Regex.Replace(replacedFeatureLine, stepPatern, m =>
                {
                    var keyword = step.Kind.ToStringValue();//m.Groups[1];
                    var text = m.Groups[2].Value.Trim();
         
                    return string.Format("{0}(\"^{1}$\", {2} =>\n{{\n\tPending();\n}});", keyword, text, paramText);
                });

            return matchedTemplate;
        }
    }
}