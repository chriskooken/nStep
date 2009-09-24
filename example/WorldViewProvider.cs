using Nucumber.Framework;

namespace Cucumber
{
    public class WorldViewProvider : WorldViewProviderBase<SeleniumWorldView>
    {
        protected override SeleniumWorldView InitializeWorldView()
        {
            return new SeleniumWorldView();
        }
    }
}