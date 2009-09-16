using System;
using NUnit.Framework;
using NUnit.Framework.ExtensionsImpl;

namespace StepSetBase
{


    [TestFixture]
    public class Given : Nucumber.Framework.StepSetBase<string>
    {
        [Test]
        public void it_should_allow_no_captures()
        {
            Given("No captures here", () => { return; });
        }

        
    }

}
