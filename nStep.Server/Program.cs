﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Server.Todo;

namespace nStep.Server
{
	internal class Program
	{
		internal static void Main(string[] args)
		{
			(new StepServer(9898)).Start();
		}
	}
}
