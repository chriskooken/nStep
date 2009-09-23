using Nucumber.Framework;

namespace Cucumber
{
    public class WorldViewProvider : WorldViewProviderBase<TestWorldView>
    {
        protected override TestWorldView InitializeWorldView()
        {
            return new TestWorldView();
        }
    }
}