using nStep.Framework;

namespace Cucumber
{
    public sealed class WorldViewProvider : WorldViewProviderBase<SeleniumWorldView>
    {
        protected override SeleniumWorldView InitializeWorldView()
        {
            return new SeleniumWorldView();
        }
    }
}