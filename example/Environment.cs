using System;
using Nucumber.Core;
using Nucumber.Framework;
using Selenium;

namespace Cucumber
{
	public class Environment : EnvironmentBase
	{
	    public override void GlobalBegin(IWorldViewDictionary worldViewDictionary)
	    {
	        
	        Console.WriteLine("Begin");
	    }

	    public override void GlobalExit(IWorldViewDictionary worldViewDictionary)
	    {
            var world = worldViewDictionary.GetWorldViewOfType<TestWorldView>();

            world.Browser.Stop();
            Console.WriteLine("End");
	    }
	}
}
