using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Socket = nStep.Server.Sockets.Socket;

namespace nStep.Server.Todo
{
	public class StepServer : TcpListener
	{
		private ManualResetEvent serverActive = new ManualResetEvent(false);
		
		public StepServer(int port) : base(port)
		{
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
			// Start the listener
			this.Start();

			// begin accepting clients
			this.BeginAcceptSocket(DoAccept, this);

			// Block
			serverActive.WaitOne();
		}

		void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			// Stop the listener
			this.Stop();

			// Unblock
			serverActive.Set();
		}

		private void DoAccept(IAsyncResult ar)
		{
			// Get the listener
			var l = (TcpListener)ar.AsyncState;

			// Establish the connection
			// Todo: provide a real processor
			new CucumberClient(new Socket(l.EndAcceptSocket(ar)), null);

			// Start listening again
			l.BeginAcceptSocket(DoAccept, l);
		}
	}
}
