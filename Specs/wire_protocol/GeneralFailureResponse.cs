using System;
using LitJson;
using Nucumber.Core.WireProtocol.Responses;
using NUnit.Framework;

namespace Specs.wire_protocol
{
    [TestFixture]
    public class GeneralFailureResponse
    {
        private GeneralFailure cut;

        [SetUp]
        public void setup()
        {
            try
            {
                throw new Exception("foobar");
            }
            catch (Exception exception)
            {
                cut = JsonMapper.ToObject<GeneralFailure>(GeneralFailure.FromException(exception).ToJson());
            }
        }

        [Test]
        public void it_should_specify_version_1_dot_0()
        {
            cut.version.Should().Be.EqualTo("1.0");
        }

        [Test]
        public void it_should_contain_a_failed_object_with_the_full_name_of_the_exception()
        {
            cut.failed.exception.Should().Be.EqualTo("System.Exception");
        }

        [Test]
        public void it_should_contain_a_failed_object_with_the_exception_message()
        {
            cut.failed.message.Should().Be.EqualTo("foobar");
        }

        [Test]
        public void it_should_contain_a_failed_object_with_a_backtrace()
        {
            cut.failed.backtrace.Should().Not.Be.Empty();
        }
    }
}
