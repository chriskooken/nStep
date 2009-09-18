using Nucumber.Core;

namespace Nucumber.App.CommandLineUtilities
{
    public interface ISuggestSyntax
    {
        string TurnFeatureIntoSnippet(FeatureStep step);
    }
}