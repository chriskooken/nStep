using System;
using Nucumber.App.CommandLineUtilities;
using NUnit.Framework;
using System.Collections.Generic;

namespace CommandLine
{

    public abstract class ConsoleOptionsTestBase
    {
        protected ConsoleOptions options;
        protected string[] args;

        [TestFixtureSetUp]
        public virtual void SetUp()
        {
            InitializeArguments();
            Assert.DoesNotThrow(() =>
                                options = new ConsoleOptions().Parse<ConsoleOptions>(args));
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
            Assert.Throws(typeof (ArgumentException),
                () => options = new ConsoleOptions().Parse<ConsoleOptions>(args) );
        }

        protected override void InitializeArguments()
        {
            args = new string[] { };
        }
    }

    [TestFixture]
    public class If_Parse_Is_called_with_one_parameter_and_no_flags_ : ConsoleOptionsTestBase
    {
        [Test]
        public void Then_it_should_return_a_ConsoleOptionsBase_of_type_ConsoleOptions()
        {
            options.Should().Be.InstanceOf<ConsoleOptions>();
        }

        [Test]
        public void Then_the_required_attribute_should_be_set()
        {
            options.FeatureFiles.Should().Be.EqualTo(args[0]);
        }

        protected override void InitializeArguments()
        {
            args = new string[] { "C:/Projects/Nucumber/example" };
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
            args = new string[] { "C:/Projects/Nucumber/example", "-v" };
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
            args = new string[] { "C:/Projects/Nucumber/example", "-verbose" };
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
                        typeof(ArgumentException),
                        () => options = new ConsoleOptions().Parse<ConsoleOptions>(args)
                        );
        }

        protected override void InitializeArguments()
        {
            args = new string[] { "C:/Projects/Nucumber/example", "-r" };
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
                        typeof(ArgumentException),
                        () => options = new ConsoleOptions().Parse<ConsoleOptions>(args)
                        );
        }

        protected override void InitializeArguments()
        {
            args = new string[] { "C:/Projects/Nucumber/example", "-require" };
        }
    }


    [TestFixture]
    public class If_Parse_Is_called_with_the_require_flag_and_one_parameter : ConsoleOptionsTestBase
    {
        private string someAssembly = "SomeAssembly";

        [Test]
        public void Then_the_assembly_option_should_be_set()
        {
            options.Assemblies.Should().Be.EqualTo(someAssembly);
        }

        protected override void InitializeArguments()
        {
            args = new string[] { "C:/Projects/Nucumber/example", "-r", someAssembly };
        }
    }
    [TestFixture]
    public class If_Parse_Is_called_with_the_require_option_and_one_parameter : ConsoleOptionsTestBase
    {
        private string someAssembly = "SomeAssembly";

        [Test]
        public void Then_the_assembly_option_should_be_set()
        {
            options.Assemblies.Should().Be.EqualTo(someAssembly);
        }

        protected override void InitializeArguments()
        {
            args = new string[] { "C:/Projects/Nucumber/example", "-require", someAssembly };
        }
    }







        //[TestFixture]
    //public class If_Parse_Is_called_with_type_ConsoleOptions_and_a_directory : ConsoleOptionsTestBase
    //{
    //    [Test]
    //    public void Then_it_should_load_all_features_in_the_given_directory()
    //    {
    //        var filePaths = Directory.GetFiles(args[0]);
    //        options.FeatureFiles.Count.Should().Be.EqualTo(filePaths.Length);
    //        options.FeatureFiles.First().Should().Be.EqualTo(filePaths.First());  
    //    }

    //    protected override void InitializeArguments()
    //    {
    //        args = new string[] { "C:/Projects/Nucumber/example" };
    //    }
    //}

    //[TestFixture]
    //public class A_feature_file_is_specified : ConsoleOptionsTestBase
    //{
    //    [Test]
    //    public void Then_it_should_load_a_single_feature()
    //    {
    //        var filePaths = Directory.GetFiles("C:/Projects/Nucumber/example");
    //        var example = filePaths.Where(x => x.Contains("example.feature")).FirstOrDefault();
    //        options.FeatureFiles.Count.Should().Be.EqualTo(1);
    //        options.FeatureFiles.First().Should().Be.EqualTo(example);    
    //    }

    //    protected override void InitializeArguments()
    //    {
    //        args = new string[] { "C:/Projects/Nucumber/example/example.feature" };
    //    }
    //}


}