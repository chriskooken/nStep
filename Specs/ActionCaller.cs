using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs
{
    [TestFixture]
	public class ActionCaller
	{
        [Test]
        public void it_should_call_an_action_with_no_params()
        {
            string enclosed = "hello";
            Action action = () => { enclosed = "goodbye"; };
            var caller = new Nucumber.Core.ActionCaller(action);
            caller.Call();
            enclosed.Should().Be.EqualTo("goodbye");
        }

        [Test]
        public void it_should_call_an_action_with_1_param()
        {
            string enclosed = "hello";
            Action<string> action = (foo) => { enclosed = foo; }; 
            var caller = new Nucumber.Core.ActionCaller(action,"bob");
            caller.Call();
            enclosed.Should().Be.EqualTo("bob");
        }


        [Test]
        public void it_should_call_an_action_with_2_params()
        {
            string enclosed = "hello";
            Action<string,string> action = (foo, bar) => { enclosed = foo + bar; };
            var caller = new Nucumber.Core.ActionCaller(action, "bob", "o");
            caller.Call();
            enclosed.Should().Be.EqualTo("bobo");
        }

        [Test]
        public void it_should_throw_the_internal_exception()
        {
            Action action = () => { throw new InvalidTimeZoneException(); };
            var caller = new Nucumber.Core.ActionCaller(action);
            new Action(() => caller.Call()).Should().Throw<InvalidTimeZoneException>();
        }
	}
}
