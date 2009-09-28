using Nucumber.Core.Features;

namespace Nucumber.Core
{
    public interface ISuggestSyntax
    {
        string TurnFeatureIntoSnippet(FeatureStep step);
    }
}