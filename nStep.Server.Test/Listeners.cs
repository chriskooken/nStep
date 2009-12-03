using System;
using System.Threading;
using Moq;
using nStep.Server.IO;
using NUnit.Framework;
using nStep.Server.Sockets;

namespace nStep.Server.Test
{
	[TestFixture]
	public class Listeners
	{
		private const string textMessage = "Hello, World";

		[Test]
		public void It_Should_Handle_A_Request_Sent_To_The_Designated_Port()
		{
			var mockProcessor = new Mock<IProcessor>();
			var tcpListener = new SingleMessageTcpListener(textMessage);
			var listener = new Listener(tcpListener);
			listener.MessageHandler += mockProcessor.Object.Process;

			listener.Start();
			Thread.Sleep(500);
			listener.Stop();

			mockProcessor.Verify(p => p.Process(textMessage));
		}
	}

	internal class SingleMessageTcpListener : ITcpListener
	{
		private readonly string message;
		private bool alreadySentMessage = false;

		public SingleMessageTcpListener(string message)
		{
			this.message = message;
		}

		public bool Pending()
		{
			return !alreadySentMessage;
		}

		public ITcpClient AcceptTcpClient()
		{
			if (alreadySentMessage)
				return null;

			var stream = new StringStream(message, true);
			var mockTcpClient = new Mock<ITcpClient>();
			mockTcpClient.Setup(tc => tc.GetStream()).Returns(stream);

			alreadySentMessage = true;
			return mockTcpClient.Object;
		}

		public void Start() { }
		public void Stop() { }
	}
}
