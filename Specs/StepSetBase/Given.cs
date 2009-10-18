using nStep.Framework;
using nStep.Framework.StepDefinitions;
using NUnit.Framework;
using System.Linq;

namespace Specs.StepSetBase
{
	[TestFixture]
	public class GivenHandling : StepSetBase<string>
	{
		[Test]
		public void it_should_register_a_Given_Step_as_a_Given()
		{
			Given("something", () => { return; });
			StepDefinitions.Givens.First().Kind.Should().Be.EqualTo(StepKinds.Given);
		}

	}
}