using nStep.Framework;
using nStep.Framework.StepDefinitions;
using NUnit.Framework;
using System.Linq;

namespace Specs.StepSetBase
{
	[TestFixture]
	public class WhenHandling : StepSetBase<string>
	{
		[Test]
		public void it_should_register_as_a_When()
		{
			When("something", () => { return; });
			StepDefinitions.Whens.First().Kind.Should().Be.EqualTo(StepKinds.When);
		}

	}
}