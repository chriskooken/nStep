using System;
using nStep.Core;
using nStep.Core.Exceptions;
using nStep.Framework;
using NUnit.Framework;

namespace Specs.WorldViewDictionary
{
    public class TestWorldView : IAmWorldView
    {

    }
    public class StepSet : StepSetBase<TestWorldView>
    {
        public string providedName { get; private set; }
        public string Before { get; set; }
        public string After { get; set; }

        public StepSet()
        {
            Given("^My Name is \"([^\"]*)\"$", name =>
            {
                providedName = name;
            });
        }
    }

    [TestFixture]
    public class ImportWorldViews
    {

        private IWorldViewDictionary cut;


        public class StringWorldView : IAmWorldView
        {
            public string Foo = "monkey";

            public StringWorldView()
            {
                
            }
            public StringWorldView(string foo)
            {
                Foo = foo;
            }
        }


        public class foo : WorldViewProviderBase<StringWorldView>
        {
            protected override StringWorldView InitializeWorldView()
            {
                return new StringWorldView();
            }
        }


        private void NewCut()
        {
            cut = new nStep.Core.WorldViewDictionary();
        }

        [Test]
        public void It_should_get_an_instance_from_the_provided_provider()
        {            
            NewCut();
            cut.Import(new foo());
            ((StringWorldView)cut[typeof(StringWorldView)]).Foo.Should().Be.EqualTo("monkey");
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

            new Action(() => cut.Add(typeof (StringWorldView), new StringWorldView("not monkey!"))).Should().Throw
                <OnlyOneWorldViewTypeCanExistAtATimeException>();
        }
    }

    [TestFixture]
    public class InitializeWorldViews
    {
        public class WorldViewProvider : WorldViewProviderBase<TestWorldView>
        {
            protected override TestWorldView InitializeWorldView()
            {
                return new TestWorldView();
            }
        }
        [Test]
        public void It_Should_not_Error_if_world_view_is_initialized()
        {
            var set = new StepSet();
            var worldViewDictionary = new nStep.Core.WorldViewDictionary();
            worldViewDictionary.Add(typeof(TestWorldView),new TestWorldView());
            var mother = new nStep.Core.StepMother(worldViewDictionary, null);
            
            Assert.DoesNotThrow(() => mother.AdoptSteps(set));
        }
    }

    [TestFixture]
    public class NotInitializeWorldViews
    {
        [Test]
        public void It_Should_Error_if_world_view_is_not_initialized()
        {
            var set = new StepSet();
            var mother = new nStep.Core.StepMother(new nStep.Core.WorldViewDictionary(),null);
            Assert.Throws<UnInitializedWorldViewException>(() => mother.AdoptSteps(set));
        }
    }
}