using Nucumber.Framework;
using NUnit.Framework;
using System.Linq;

namespace StepSetBase
{
    [TestFixture]
    public class GivenHandling : StepSetBase<string>
    {
        [Test]
        public void it_should_register_a_Given_Step_as_a_Given()
        {
            Given("something", () => { return; });
            GivenStepDefinitions.First().Kind.Should().Be.EqualTo(StepKinds.Given);
        }

    }

}
