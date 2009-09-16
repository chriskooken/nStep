using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Specs
{
    [TestFixture]
    public class TypeCaster
    {

        private Nucumber.Core.TypeCaster cut = new Nucumber.Core.TypeCaster();

        private void AssertItWorks<T>(string sample)
        {
            cut.MakeIntoType(sample, typeof(T)).GetType().Should().Be.EqualTo(typeof(T));
        }

        [Test]
        public void it_should_convert_things_to_strings()
        {
            AssertItWorks<string>("foobar");
        }
        
        [Test]
        public void it_should_convert_things_to_Int32()
        {
            AssertItWorks<Int32>("75");
        }



    }
}
