﻿using System;
using System.Linq;
using System.Linq.Expressions;
using nStep.App.CommandLineUtilities;
using nStep.App.CommandLineUtilities;
using NUnit.Framework;
using System.Collections.Generic;

namespace Specs.CommandLine
{
	public abstract class ConsoleOptionsTestBase
	{
		protected TestOptions options;
		protected string[] args;

		[TestFixtureSetUp]
		public virtual void SetUp()
		{
			InitializeArguments();

			Assert.DoesNotThrow(() =>
			                    options = new TestOptions().Parse<TestOptions>(args));
		}

		protected abstract void InitializeArguments(); 
	}

	[TestFixture]
	public class If_Parse_Is_called_no_parameters : ConsoleOptionsTestBase
	{
		[TestFixtureSetUp]
		public override void SetUp()
		{
			InitializeArguments();
		}

		[Test]
		public void Then_It_Should_Error()
		{
			Assert.Throws(typeof (ConsoleOptionsException),
			              () => options = new TestOptions().Parse<TestOptions>(args) );
		}

		protected override void InitializeArguments()
		{
			args = new string[] { };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_NULL_parameters : ConsoleOptionsTestBase
	{
		[TestFixtureSetUp]
		public override void SetUp()
		{
			InitializeArguments();
		}

		[Test]
		public void Then_It_Should_Error()
		{
			Assert.Throws(typeof(ConsoleOptionsException),
			              () => options = new TestOptions().Parse<TestOptions>(args));
		}

		protected override void InitializeArguments()
		{
			args = null;
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_one_parameter_and_no_flags_ : ConsoleOptionsTestBase
	{
		[Test]
		public void Then_it_should_return_a_ConsoleOptionsBase_of_type_ConsoleOptions()
		{
			options.Should().Be.InstanceOf<TestOptions>();
		}

		[Test]
		public void Then_the_required_attribute_should_be_set()
		{
			options.FeatureFiles.Should().Be.EqualTo(args[0]);
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example" };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_verbose_flag_ : ConsoleOptionsTestBase
	{
		[Test]
		public void Then_the_verbose_option_should_be_set()
		{
			options.Verbose.Should().Be.True();
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-v" };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_verbose_option : ConsoleOptionsTestBase
	{
		[Test]
		public void Then_the_verbose_option_should_be_set()
		{
			options.Verbose.Should().Be.True();
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-verbose" };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_require_flag_and_no_parameters : ConsoleOptionsTestBase
	{
		[TestFixtureSetUp]
		public override void SetUp()
		{
			InitializeArguments();
		}

		[Test]
		public void Then_it_should_error()
		{
			Assert.Throws(
				typeof(ConsoleOptionsException),
				() => options = new TestOptions().Parse<TestOptions>(args)
				);
		}

		protected override void InitializeArguments()
		{
			args = new string[] {"C:/Projects/nStep/example", "-r" };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_require_option_and_no_parameters : ConsoleOptionsTestBase
	{
		[TestFixtureSetUp]
		public override void SetUp()
		{
			InitializeArguments();
		}

		[Test]
		public void Then_it_should_error()
		{
			Assert.Throws(
				typeof(ConsoleOptionsException),
				() => options = new TestOptions().Parse<TestOptions>(args)
				);
		}

		protected override void InitializeArguments()
		{
			args = new string[] {"C:/Projects/nStep/example", "-require" };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_require_flag_and_one_parameter : ConsoleOptionsTestBase
	{
		private string someAssembly = "SomeAssembly";

		[Test]
		public void Then_the_assembly_option_should_be_set()
		{
			options.Assemblies.Count.Should().Be.EqualTo(1);
			options.Assemblies.First().Should().Be.EqualTo(someAssembly);
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-r", someAssembly };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_require_option_and_one_parameter : ConsoleOptionsTestBase
	{
		private string someAssembly = "SomeAssembly";

		[Test]
		public void Then_the_assembly_option_should_be_set()
		{
			options.Assemblies.Count.Should().Be.EqualTo(1);
			options.Assemblies.First().Should().Be.EqualTo(someAssembly);
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-require", someAssembly };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_require_flag_and_more_than_one_parameter : ConsoleOptionsTestBase
	{
		private IList<string> someAssembly = new List<string>(){"SomeAssembly","someotherassembly"};

		[Test]
		public void Then_the_assembly_option_should_be_set()
		{
			options.Assemblies.SequenceEqual(someAssembly).Should().Be.True();
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-r", someAssembly[0], someAssembly[1] };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_require_option_and_more_than_one_parameter : ConsoleOptionsTestBase
	{
		private IList<string> someAssembly = new List<string>(){"SomeAssembly","someotherassembly"};

		[Test]
		public void Then_the_assembly_option_should_be_set()
		{
			options.Assemblies.SequenceEqual(someAssembly).Should().Be.True();
		}

		protected override void InitializeArguments()
		{
			args = new string[] {"C:/Projects/nStep/example", "-require", someAssembly[0],someAssembly[1]};

		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_format_flag_and_no_parameters : ConsoleOptionsTestBase
	{

		[TestFixtureSetUp]
		public override void SetUp()
		{
			InitializeArguments();
		}

		[Test]
		public void Then_it_should_error()
		{
			Assert.Throws(
				typeof(ConsoleOptionsException),
				() => options = new TestOptions().Parse<TestOptions>(args)
				);
		}

		protected override void InitializeArguments()
		{
			args = new string[] {"C:/Projects/nStep/example", "-f" };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_format_option_and_no_parameters : ConsoleOptionsTestBase
	{

		[TestFixtureSetUp]
		public override void SetUp()
		{
			InitializeArguments();
		}

		[Test]
		public void Then_it_should_error()
		{
			Assert.Throws(
				typeof(ConsoleOptionsException),
				() => options = new TestOptions().Parse<TestOptions>(args)
				);
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-format" };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_format_flag_and_one_parameter : ConsoleOptionsTestBase
	{
		private Format format = Format.Html;

		[Test]
		public void Then_the_format_option_should_be_set()
		{
			options.Format.Should().Be.EqualTo(format);
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-f", format.ToString() };
		}
	}

	[TestFixture]
	public class If_Parse_Is_called_with_the_format_option_and_one_parameter : ConsoleOptionsTestBase
	{
		private Format format = Format.Html;

		[Test]
		public void Then_the_format_option_should_be_set()
		{
			options.Format.Should().Be.EqualTo(format);
		}

		protected override void InitializeArguments()
		{
			args = new string[] { "C:/Projects/nStep/example", "-format", format.ToString() };
		}
	}
}