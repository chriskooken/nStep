using Nucumber.Framework;

namespace Cucumber
{
    public class Environment : EnvironmentBase
    {
        public override void SessionStart()
        {
            //Maybe create the database? Init StructureMap? Start Selenium? Something else?
        }

        public override void SessionEnd()
        {
            //Destroy the database? Clean up? Stop Selenium?
        }

        public override void AfterScenario()
        {
            //Maybe clean out the database?
        }
    }
}