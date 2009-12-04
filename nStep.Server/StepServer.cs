using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace nStep.Server
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
			new CucumberClient(l.EndAcceptSocket(ar));

			// Start listening again
			l.BeginAcceptSocket(DoAccept, l);
		}
	}

	public class CucumberClient
	{
		// A read buffer for this Socket
		private byte[] read_buffer = new byte[1500];

		// The current offset into the read buffer for new data
		private int read_offset = 0;

		public CucumberClient(Socket socket)
		{
			Console.WriteLine("Accepting new connection from {0}", socket.RemoteEndPoint.ToString());
			Console.WriteLine("\tto {0}", socket.LocalEndPoint.ToString());

			// Start waiting for data
			socket.BeginReceive(read_buffer, read_offset, read_buffer.Length - read_offset, SocketFlags.None, DoReceive, socket);
		}

		private void DoReceive(IAsyncResult ar)
		{
			// Get the socket
			var s = (Socket)ar.AsyncState;

			// How much data did I actually get
			int len; 
			try
			{
				len = s.EndReceive(ar);
			}
			catch (SocketException e)
			{
				// Socket is probably already closed, this should probably be a bit better
				len = 0;
			}

			// If no data, then the socket has been disconnected
			if (len == 0)
			{
				Console.WriteLine("Disconnecting {0}", s.RemoteEndPoint.ToString());
				// Make sure to shut down
				s.Shutdown(SocketShutdown.Both);
				// and close the socket
				s.Close();
				return;
			}

			// increment the read_offset
			read_offset += len;

			// Is there a new line?
			var i = Array.IndexOf<byte>(read_buffer, 0x0A);

			if (i != -1)
				read_offset -= (i + 1); // move the read_offset back

			// Start listening again
			s.BeginReceive(read_buffer, read_offset, read_buffer.Length - read_offset, SocketFlags.None, DoReceive, s);

			// If there is a new line
			if(i != -1) {
				// Get the string out of the buffer
				var json = Encoding.ASCII.GetString(read_buffer, 0, i);
				
				// Shift the buffer back, really this should be a ring buffer
				Array.Copy(read_buffer, i + 1, read_buffer, 0, read_buffer.Length - i - 1);
				
				// Parse the request
				var r = Messages.Request.ParseFromJson(json);
				if ((r is Messages.BeginScenarioRequest) || (r is Messages.EndScenarioRequest))
					s.Send(Encoding.ASCII.GetBytes("[\"success\", null]\n"));
				else if (r is Messages.StepMatchesRequest)
					s.Send(Encoding.ASCII.GetBytes("[\"step_matches\",[]]\n"));
			}

		}

	}
}
