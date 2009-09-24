using System;
using Nucumber.Core;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs.WorldViewDictionary
{
    [TestFixture]
    public class ImportWorldViews
    {
        public class foo : Nucumber.Framework.WorldViewProviderBase<string>
        {
            protected override string InitializeWorldView()
            {
                return "monkey";
            }
        }

        private IWorldViewDictionary cut;

        [Test]
        public void It_should_get_an_instance_from_the_provided_provider()
        {            
            NewCut();
            cut.Import(new foo());
            cut[typeof (string)].Should().Be.EqualTo("monkey");
        }

        private void NewCut()
        {
            cut = new Nucumber.Core.WorldViewDictionary();
        }


        [Test]
        public void it_should_throw_a_OnlyOneWorldViewTypeCanExistAtATimeException_if_duplicates_are_imported()
        {
            NewCut();
            cut.Import(new foo());

            new Action(() => cut.Import(new foo())).Should().Throw<OnlyOneWorldViewTypeCanExistAtATimeException>();
        }


        [Test]
        public void it_should_throw_a_OnlyOneWorldViewTypeCanExistAtATimeException_if_duplicates_are_added()
        {
            NewCut();
            cut.Import(new foo());

            new Action(() => cut.Add(typeof (string), "not monkey!")).Should().Throw
                <OnlyOneWorldViewTypeCanExistAtATimeException>();
        }
    }
}