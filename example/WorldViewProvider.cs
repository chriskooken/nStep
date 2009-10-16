using nStep.Framework;

namespace nStep
{
    public sealed class WorldViewProvider : WorldViewProviderBase<SeleniumWorldView>
    {
        protected override SeleniumWorldView InitializeWorldView()
        {
            return new SeleniumWorldView();
        }
    }
}