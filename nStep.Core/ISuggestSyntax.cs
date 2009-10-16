using nStep.Framework.Features;

namespace nStep.Core
{
    public interface ISuggestSyntax
    {
        string TurnFeatureIntoSnippet(Step step);
    }
}