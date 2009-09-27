using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Core.Features;
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
        public void it_should_convert_things_to_Int64()
        {
            AssertItWorks<Int64>("75");
        }
        [Test]
        public void it_should_convert_things_to_Int32()
        {
            AssertItWorks<Int32>("75");
        }
        [Test]
        public void it_should_convert_things_to_double()
        {
            AssertItWorks<double>("75.54");
        }
        [Test]
        public void it_should_convert_things_to_decimal()
        {
            AssertItWorks<decimal>("75.0124");
        }
        [Test]
        public void it_should_convert_things_to_float()
        {
            AssertItWorks<float>(".11075");
        }
        [Test]
        public void it_should_convert_things_to_DateTime()
        {
            AssertItWorks<DateTime>("5/11/2009");
        }   
        
        [Test]
        public void it_should_convert_things_to_Bool()
        {
            AssertItWorks<bool>("true");
        } 
        
        [Test]
        public void it_should_convert_things_to_Guid()
        {
            AssertItWorks<Guid>("83f32761-4794-470e-9d88-161a130488d4");
        }

        [Test]
        public void it_should_convert_a_table_to_a_table_object()
        {
            string tableText = "|ID | Name | Age| \r\n | 1 | Chris | 25 | \r\n | 2 | Sam | 29 | \r\n | 3 | Brendan | 31 | \r\n";

            var table = Table.Parse(tableText);

            table.Rows.Count.Should().Be.EqualTo(4);
            table.Rows.First().Columns.Count.Should().Be.EqualTo(3);
        }

        [Test]
        public void it_should_throw_an_exception_if_the_table_is_missing_any_leading_pipes()
        {
            var tableText = "bob|";
            Assert.Throws<FormatException>(()=> Table.Parse(tableText));
        }
        [Test]
        public void it_should_throw_an_exception_if_the_table_is_missing_any_trailing_pipes()
        {
            var tableText = "|bob";
            Assert.Throws<FormatException>(() => Table.Parse(tableText));
        }
        [Test]
        public void it_should_throw_an_exception_if_the_table_has_any_mismatched_columns()
        {
            string tableText = "|ID | Name | Age| \r\n | 1 | Chris | 25 | \r\n | 2 | Sam | 29 | \r\n | 3 | Brendan | \r\n";
            Assert.Throws<FormatException>(() => Table.Parse(tableText));
        }

    }
}
