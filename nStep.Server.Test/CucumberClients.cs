using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Moq;
using nStep.Server.Messages;
using nStep.Server.Sockets;
using NUnit.Framework;

namespace nStep.Server.Test
{
	[TestFixture]
	public class CucumberClients
	{
		[Test]
		public void It_Receives_A_Request_Sent_All_At_Once()
		{
            var request = new BeginScenarioRequest();
			var mockSocket = new Mock<ISocket>();
			// Todo: Prepare socket to transmit message

			var mockProcessor = new Mock<IProcessor>();
			var cucumberClient = new CucumberClient(mockSocket.Object, mockProcessor.Object);

			var asyncResult = new Mock<IAsyncResult>();
			asyncResult.Setup(ar => ar.AsyncState).Returns(mockSocket.Object);

			cucumberClient.DoReceive(asyncResult.Object);

			mockProcessor.Verify(p => p.Process(request));
		}
	}
}
