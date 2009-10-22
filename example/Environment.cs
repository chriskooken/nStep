using System;
using nStep.Framework;
using nStep.Framework.Execution.Results;
using nStep.Framework.WorldViews;
using Selenium;

namespace nStep
{
	public class Environment : EnvironmentBase
	{
	    public override void GlobalBegin()
	    {
	    }

	    public override void GlobalExit(IWorldViewDictionary worldViewDictionary, RunResult result)
	    {
            var world = worldViewDictionary.GetWorldViewOfType<SeleniumWorldView>();

            world.Browser.Stop();
         
	    }
	}
}
