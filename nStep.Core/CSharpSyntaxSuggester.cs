using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using nStep.Core.Features;
using nStep.Framework;

namespace nStep.Core
{
    public class CSharpSyntaxSuggester : ISuggestSyntax
    {
        public string TurnFeatureIntoSnippet(FeatureStep step)
        {
            var argValues = new[] { "string arg1", "string arg2", "string arg3", "string arg4" };
            const string parameterPattern = "\"[^\"]*\"";
            const string stepPatern = "^(Given|When|Then|And|But)(.*)$";

            var paramCount = new Regex(parameterPattern).Matches(step.FeatureLine).Count;
            var paramText = string.Format("({0})", string.Join(", ", argValues, 0, paramCount));

            var replacedFeatureLine = Regex.Replace(step.FeatureLine, parameterPattern, "\\\"([^\\\"]*)\\\"");

            var matchedTemplate = Regex.Replace(replacedFeatureLine, stepPatern, m =>
                {
                    var keyword = step.Kind.ToStringValue();//m.Groups[1];
                    var text = m.Groups[2].Value.Trim();
         
                    const string template = "{0}(\"^{1}$\", {2} =>\n{{\n\tPending();\n}});";

                    return string.Format(template, keyword, text, paramText);
                });

            return matchedTemplate;
        }
    }
}