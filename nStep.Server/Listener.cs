using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using nStep.Server.Sockets;

namespace nStep.Server
{
	public delegate string MessageHandler(string message);

	public class Listener
	{
		private readonly ITcpListener tcpListener;

		private bool stopping;
		private readonly Thread thread;
		private AutoResetEvent started = new AutoResetEvent(false);
		private AutoResetEvent stopped = new AutoResetEvent(false);

		public event MessageHandler MessageHandler;
		
		public Listener(ITcpListener tcpListener)
		{
			this.tcpListener = tcpListener;

			thread = new Thread(Run) { Name = "AsynchListener" };
		}

		public void Start()
		{
			thread.Start();
			started.WaitOne();
		}

		public void Stop()
		{
			stopping = true;
			stopped.WaitOne();
			thread.Join();
		}

		public void Run()
		{
			tcpListener.Start();

			started.Set();

			while (!stopping)
			{
				var client = WaitForClientToConnect();
				if (!stopping)
				{
					Process(client);
					client.Close();
				}
			}

			tcpListener.Stop();

			stopped.Set();
		}

		private ITcpClient WaitForClientToConnect()
		{
			while (!stopping)
				if (tcpListener.Pending())
					return tcpListener.AcceptTcpClient();
				else
					Thread.Sleep(100);

			return null;
		}

		private void Process(ITcpClient client)
		{
			if (client == null)
				return;

			var stream = client.GetStream();
			var reader = new StreamReader(stream, Encoding.Unicode);
			var writer = new StreamWriter(stream, Encoding.Unicode);

			while (!stopping)
			{
				var request = reader.ReadLine();
				if (request == null)
					break;

				var response = MessageHandler(request);
				writer.WriteLine(response);
			}
		}
	}
}
