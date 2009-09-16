using System;
using NUnit.Framework;
using NUnit.Framework.ExtensionsImpl;

namespace StepSetBase
{

    public class bar
    {
        public string value;
    }

    [TestFixture]
    public class Given : Nucumber.Framework.StepSetBase<string>
    {
        [Test]
        public void it_should_allow_no_captures()
        {
            Given("No captures here", () => { return; });

            Given("blah (.*) blah (.*) blah",new {userName = null as bar, foo = ""}, parms =>
                {
                    Console.Write(parms.foo);
                    Console.Write(parms.userName.value);
                } );

            Given("blah (.*) blah (.*) blah", new {foo = "", bar = ""} , parms =>
            {
                Console.Write(parms.foo);
                Console.Write(parms.bar);
            });

        }

        
    }

}
