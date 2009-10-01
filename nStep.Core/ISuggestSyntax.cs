using nStep.Core.Features;

namespace nStep.Core
{
    public interface ISuggestSyntax
    {
        string TurnFeatureIntoSnippet(FeatureStep step);
    }
}