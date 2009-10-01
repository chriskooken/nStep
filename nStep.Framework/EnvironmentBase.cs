
using nStep.Core;

namespace nStep.Framework
{
    public abstract class EnvironmentBase
    {
        public abstract void GlobalBegin(IWorldViewDictionary worldViewDictionary);

        public abstract void GlobalExit(IWorldViewDictionary worldViewDictionary);
    }
}
