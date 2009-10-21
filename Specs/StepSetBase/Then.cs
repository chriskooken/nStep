using nStep.Framework;
using nStep.Framework.StepDefinitions;
using NUnit.Framework;
using System.Linq;

namespace Specs.StepSetBase
{
	[TestFixture]
	public class ThenHandling : StepSetBase<string>
	{
		[Test]
		public void it_should_register_as_a_Then()
		{
			Then("something", () => { return; });
			StepDefinitions.Thens.First().Kind.Should().Be.EqualTo(StepKinds.Then);
		}

	}
}