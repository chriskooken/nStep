using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs.wire_protocol
{
    [TestFixture]
    public class StepDefinitions
    {
        private Nucumber.Core.WireProtocol.Responses.StepDefinitions cut;

        public class steps : StepSetBase<JunkWorldView>
        {
            public steps()
            {
                Given("foobar",()=>
                    {
                        var s = "";
                    });

                Given("barfoo",()=>
                    {
                        var f = "d";
                    });
            }
        }


        [SetUp]
        public void Setup()
        {
            var message = Nucumber.Core.WireProtocol.Responses.StepDefinitions.FromStepDefinitionsEnumerable(new steps().StepDefinitions.Givens);
            var json = message.ToJson();
            cut =
                JsonMapper.ToObject<Nucumber.Core.WireProtocol.Responses.StepDefinitions>
                (json);
        }

        [Test]
        public void it_should_contain_2_steps()
        {
            cut.step_definitions.Count().Should().Be.EqualTo(2);
        }

        [Test]
        public void it_should_contain_the_regex_for_the_steps()
        {
            cut.step_definitions.First().regexp.Should().Be.EqualTo("foobar");
            cut.step_definitions.Last().regexp.Should().Be.EqualTo("barfoo");
        }
        
    }

    public class JunkWorldView  
    {
    }
}
